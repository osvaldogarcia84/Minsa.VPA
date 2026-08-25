using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Print;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Widget;
using Com.Karumi.Dexter;
using Com.Karumi.Dexter.Listener;
using Com.Karumi.Dexter.Listener.Single;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Minsa.VPA.Common;
using Minsa.VPA.Modelos;
using Minsa.VPA.Proveedores;
using Minsa.VPA.Servicios;

namespace Minsa.VPA.Activities
{
    [Activity(Label = "Corte de caja", LaunchMode = LaunchMode.SingleTop, ScreenOrientation = ScreenOrientation.Portrait)]
    public class CorteCajaActivity : AppCompatActivity, IPermissionListener
    {
        private ResultadoDeOperacionGenerico<Vendedor> vendedor;
        
        public static string LlaveSitioCorteCaja = "LlaveSitioCorteCaja";
      //  private string sitio;
        public static string LlaveTransaccionesCorteCaja = "LlaveTransaccionesCorteCaja";
        ProveedorDeEstrategia proveedorDeEstrategia = new ProveedorDeEstrategia();
        private ResultadoDeOperacionGenerico<CorteCaja> cortecaja;
        public static string fileName = "CorteCaja.pdf";
        public ResultadoDeOperacionGenerico<List<Transacciones>> transacciones;
        ProveedorDeClientes proveedorDeCliente = new ProveedorDeClientes();
        Timer timer;
        ProveedorDeCXC proveedorDeCxC = new ProveedorDeCXC();
        public ResultadoDeOperacionGenerico<List<Transacciones>> reciboCdtoCorteCaja;
        public decimal ImpTotal = 0;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            vendedor = ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Intent.Obtener<string>(MainActivity.LlaveUsuario));
            //  sitio = ProveedorDeSerializado.Obtener<string>(Intent.Obtener<string>(LlaveSitioCorteCaja));
            //transacciones = ProveedorDeSerializado.Obtener<List<Transacciones>>(Intent.Obtener<string>(LlaveTransaccionesCorteCaja));
            var obtieneTransacciones = Task.Run(async () => {
                transacciones = await proveedorDeCliente.ObtenerTransacciones(new Sitio(vendedor.Valor.Almacen));
            });
            obtieneTransacciones.Wait();
            var obtieneReciboCredito = Task.Run(async () => {
                reciboCdtoCorteCaja = await proveedorDeCxC.RecibosCredito(new Sitio(vendedor.Valor.Almacen));
            });
            obtieneReciboCredito.Wait();
            transacciones.Valor.AddRange(reciboCdtoCorteCaja.Valor);

            cortecaja = await proveedorDeEstrategia.CorteCaja(new Sitio(vendedor.Valor.Almacen));
            //if(cortecaja.Tipo == TipoDeResultado.Exito)
            //{
                SetContentView(Resource.Layout.CorteCaja);
                await Common.Common.WriteFileToStorageAsync(this, "cour.ttf");
                Dexter.WithActivity(this)
                    .WithPermission(Manifest.Permission.WriteExternalStorage)
                    .WithListener(this)
                    .Check();
            FindViewById<Button>(Resource.Id.RegresarCaja).Click += CorteCajaActivity_Click;
            //}
            //else
            //{
            //    SetContentView(Resource.Layout.Error);
            //    FindViewById<TextView>(Resource.Id.ErrorTexto).Text =
            //       "Error al generar el ticket";
            //}
            // Create your application here
        }

        private void CorteCajaActivity_Click(object sender, EventArgs e)
        {
            var activity = new Android.Content.Intent(this, typeof(ContenidoActivity));
            activity.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(vendedor));
            //activity.PutExtra(LlaveVentaCompleta, ProveedorDeSerializado.Generar(ClientesProspecto));
            SetResult(Result.FirstUser, activity);
            Finish();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public void OnPermissionDenied(PermissionDeniedResponse p0)
        {
            Toast.MakeText(this, "Acepta los permisos", ToastLength.Long).Show();
        }

        public void OnPermissionGranted(PermissionGrantedResponse p0)
        {
            CreatePDFFile(Common.Common.GetAppPath(this) + fileName);
        }

        public void OnPermissionRationaleShouldBeShown(PermissionRequest p0, IPermissionToken p1)
        {
            throw new NotImplementedException();
        }

        private void CreatePDFFile(string v)
        {
            if (new Java.IO.File(v).Exists())
                new Java.IO.File(v).Delete();
            try
            {
                Document document = new Document();
                // Save
                PdfWriter.GetInstance(document, new FileStream(v, FileMode.Create));
                // Open
                document.Open();
                //Setting
                Rectangle one = new Rectangle(612, 861);
                Rectangle pagesize = new Rectangle(360.0f, 60400.0f);
                document.SetPageSize(PageSize.LETTER);
                //document.SetMargins(4, 2, 4, 2);

                document.AddCreationDate();
                document.AddAuthor("Minsa");
                document.AddCreator("Minsa Vpa");

                //Font Setting
              //  Color colorAccent = new Color(0, 153, 204, 255);

                float fontSize = 18, fontbody = 18;
                BaseFont fontName = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252,
                                    false);

                Font titleFont = new Font(Font.NORMAL, fontSize, Font.NORMAL, Color.BLACK);
                Font bodyFont = new Font(Font.NORMAL, fontbody, Font.NORMAL, Color.BLACK);
                string checkin = String.Empty;
                string checkout = String.Empty;
                string kminicial = String.Empty;
                string kmfinal = String.Empty;
                if (cortecaja.Valor != null)
                {
                    checkin = cortecaja.Valor.CheckIn == null ? "" : cortecaja.Valor.CheckIn;
                    checkout = cortecaja.Valor.CheckOut == null ? "" : cortecaja.Valor.CheckOut;
                    kminicial = cortecaja.Valor.KmInicial == null ? "" : cortecaja.Valor.KmInicial;
                    kmfinal = cortecaja.Valor.KmFinal == null ? "" : cortecaja.Valor.KmFinal;
                }
                else
                {

                    checkin = "N/A";
                    checkout = "N/A";
                    kminicial = "N/A";
                    kmfinal = "N/A";
                }
               
                //Encabezado                
                AddNewItem(document, "CORTE DE CAJA", Element.ALIGN_CENTER, titleFont);
                AddNewItem(document, "Fecha: " + Convert.ToString(DateTime.Now), Element.ALIGN_LEFT, titleFont);
                AddNewItem(document, "Sitio: " + vendedor.Valor.Almacen, Element.ALIGN_LEFT, titleFont);
                AddNewItem(document, "Nombre del vendedor: " + vendedor.Valor.Nombre, Element.ALIGN_LEFT, titleFont);
                AddNewItem(document, "CheckIn: " + checkin, Element.ALIGN_LEFT, titleFont);
                AddNewItem(document, "CheckOut: " + checkout, Element.ALIGN_LEFT, titleFont);
                AddNewItem(document, "Km Inicial: " + kminicial, Element.ALIGN_LEFT, titleFont);
                AddNewItem(document, "Km Final: " + kmfinal, Element.ALIGN_LEFT, titleFont);

                AddLineSeparator(document);
                AddNewItemWithLeftAndRight(document, "Factura", "Monto", titleFont, bodyFont);
                decimal sumaCantidad = transacciones.Valor.Where(t => t.FormaPago != "99").Select(e => e.Bultos).Sum();
                foreach (var items in transacciones.Valor.Where(t => t.FormaPago != "99"))
                {   
                    
                    AddNewItemWithLeftAndRight(document, items.Factura, Convert.ToString(String.Format("{0:C}", items.Importe)), titleFont, bodyFont);
                    ImpTotal = ImpTotal + items.Importe;
                }
                AddLineSeparator(document);
                
           
                AddNewItem(document, "Total del monto: " + String.Format("{0:C}", ImpTotal), Element.ALIGN_LEFT, titleFont);
                AddNewItem(document, "Total cantidad en sacos: " + Convert.ToString(sumaCantidad), Element.ALIGN_LEFT, titleFont);

                // Datos de la bodega
                //Font orderBodega = new Font(fontName, fontSize, Font.NORMAL, colorAccent);

                //AddNewItemWithLeftAndRight(document, "BODEGA", ticket.Valor.Bodega, titleFont, bodyFont);
                //AddNewItemWithLeftAndRight(document, "SITIO", ticket.Valor.Almacen, titleFont, bodyFont);
                //AddNewItemWithLeftAndRight(document, "RUTA", ticket.Valor.Ruta, titleFont, bodyFont);
                //AddNewItemWithLeftAndRight(document, "REFERENCIA 1", trnsacciones.Cliente + transacciones.DigitoVerificador + "(CTA DE DEPÓSITO)", titleFont, bodyFont);
                //AddNewItemWithLeftAndRight(document, "REFERENCIA 2", transacciones.Factura + "XXXX", titleFont, bodyFont);
                //AddNewItemWithLeftAndRight(document, "NOMBRE DEL CLIENTE", ticket.Valor.Nombre, titleFont, bodyFont);
                //AddNewItemWithLeftAndRight(document, "FECHA", transacciones.FechaFactura, titleFont, bodyFont);
                //AddNewItemWithLeftAndRight(document, "HORA", DateTime.Now.ToString("HH:mm:ss"), titleFont, bodyFont);

                //// Datos bancario
                //AddLineSeparator(document);
                //AddNewItem(document, "CONVENIOS BANCARIOS", Element.ALIGN_LEFT, bodyFont);
                //AddNewItem(document, "CONVENIO BANCOMER:", Element.ALIGN_CENTER, bodyFont);
                //AddNewItem(document, "0077348", Element.ALIGN_CENTER, bodyFont);
                //AddNewItem(document, "RAP HSBC:", Element.ALIGN_CENTER, bodyFont);
                //AddNewItem(document, "7621", Element.ALIGN_CENTER, bodyFont);
                //AddNewItem(document, "CTA REFERENCIADA BANAMEX", Element.ALIGN_CENTER, bodyFont);
                //AddNewItem(document, "06337968889", Element.ALIGN_CENTER, bodyFont);
                //AddNewItem(document, "BANCO AZTECA:", Element.ALIGN_CENTER, bodyFont);
                //AddNewItem(document, "Pago de Servicio a Minsa S.A. de C.V.", Element.ALIGN_CENTER, bodyFont);
                //AddLineSeparator(document);

                ////Importe
                //AddLineSpace(document);
                //AddNewItem(document, "IMPORTE DE PAGO: $" + transacciones.Importe, Element.ALIGN_LEFT, titleFont);
                //AddNewItem(document, ticket.Valor.CantidadLetras, Element.ALIGN_LEFT, titleFont);
                //AddNewItem(document, "FORMA DE PAGO: " + ticket.Valor.FormaPago, Element.ALIGN_LEFT, titleFont);
                //AddNewItem(document, "NUMERO DE CHEQUE", Element.ALIGN_LEFT, titleFont);


                ////Informacion
                //AddLineSpace(document);
                //AddLineSpace(document);
                //AddNewItem(document, "GRACIAS POR UTILIZAR ESTE SERVICIO!!!", Element.ALIGN_LEFT, bodyFont);
                //AddNewItem(document, "PARA DUDAS O ACLARACIONES DE ESTA", Element.ALIGN_LEFT, bodyFont);
                //AddNewItem(document, "OPERACIÓN, FAVOR DE LLAMAR AL", Element.ALIGN_LEFT, bodyFont);
                //AddNewItem(document, "CENTRO DE SERVICIO AL CLIENTE", Element.ALIGN_LEFT, bodyFont);
                //AddNewItem(document, "01 800 712 5050 LADA SIN COSTO", Element.ALIGN_LEFT, bodyFont);

                //AddNewItem(document, "COPIA CLIENTE", Element.ALIGN_CENTER, bodyFont);
                // AddNewItem(document, Convert.ToString(DateTime.Now), Element.ALIGN_CENTER, bodyFont);
                // AddNewItem(document, "BODEGA REYES", Element.ALIGN_CENTER, bodyFont);

                document.Close();
                Toast.MakeText(this, "Se genero correctamente el corte de caja", ToastLength.Short).Show();

                PrintPDF();
            }
            catch (FileNotFoundException e)
            {
                Log.Debug("Sistemas", "" + e.Message);
            }
            catch (DocumentException e)
            {
                Log.Debug("Sistemas", "" + e.Message);
            }
            catch (IOException e)
            {
                Log.Debug("SIstemas", "" + e.Message);
            }
        }

        private void PrintPDF()
        {
            PrintManager printManager = (PrintManager)GetSystemService(Context.PrintService);
            try
            {
                PrintDocumentAdapter adapter = new PrintPDFAdapter(this, Common.Common.GetAppPath(this) + fileName);

                printManager.Print("Document", adapter, new PrintAttributes.Builder().Build());
                //var activity = new Android.Content.Intent(this, typeof(ContenidoActivity));
                //activity.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(vendedor));
                ////activity.PutExtra(LlaveVentaCompleta, ProveedorDeSerializado.Generar(ClientesProspecto));
                //SetResult(Result.Ok, activity);
                //Finish();
            }
            catch (Exception e)
            {
                Log.Error("Sistemas", "" + e.Message);
            }
        }

        private void AddNewItemWithLeftAndRight(Document document, string leftText, string rigthText, Font leftFont, Font rightFont)
        {
            Chunk chunkLeft = new Chunk(leftText, leftFont);
            Chunk chunkRight = new Chunk(rigthText, rightFont);
            Paragraph p = new Paragraph(chunkLeft);
            p.Add(new Chunk(new VerticalPositionMark()));
            p.Add(chunkRight);
            document.Add(p);
        }

        private void AddLineSeparator(Document document)
        {
            LineSeparator lineSeparator = new LineSeparator();
            lineSeparator.LineColor = Color.BLACK;
            AddLineSpace(document);
            document.Add(new Chunk(lineSeparator));
            AddLineSpace(document);
        }

        private void AddLineSpace(Document document)
        {
            document.Add(new Paragraph(""));
        }
        private void AddNewItem(Document document, string text, int align, Font font)
        {
            Chunk chunk = new Chunk(text, font);
            Paragraph p = new Paragraph(chunk);
            p.Alignment = align;
            document.Add(p);
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (timer != null)
            {
                timer.Elapsed -= OnTimerElapsed;
                timer.Enabled = false;
                timer.Stop();
            }
        }

        protected override void OnPause()
        {
            base.OnPause();


            DateTime horaActual = DateTime.Now;
            horaActual.ToString("HH:mm:ss tt");
            DateTime HoraDeCierre = DateTime.Parse(InformacionGeneral.HoraCierre);
            DateTime HoraDeApertura = DateTime.Parse(InformacionGeneral.HoraApertura);
            if (horaActual >= HoraDeCierre && horaActual <= HoraDeApertura)
            {
                timer = new Timer();
                //timer.AutoReset = false;
                //timer.Interval = 20000;
                timer.Elapsed += OnTimerElapsed;
                timer.Start();
            }

        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            //Wipe your valuable data here
            Java.Lang.JavaSystem.Exit(0);
        }
    }
}