using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Com.Karumi.Dexter;
using Android;
using Com.Karumi.Dexter.Listener.Multi;
using Com.Karumi.Dexter.Listener;
using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text.pdf.draw;
using Minsa.VPA.Common;
using Android.Util;
using Android.Print;
using Android.Content;
using Android.Content.PM;
using Minsa.VPA.Modelos;
using Minsa.VPA.Servicios;
using Minsa.VPA.Proveedores;
using System.Collections;
using System.Threading.Tasks;
using Com.Karumi.Dexter.Listener.Single;
using System.Collections.Generic;
using Android.Support.V4.App;
using QRCoder;
using System.Timers;
namespace Minsa.VPA.Activities
{
    [Activity(Label = "Recibo valores impresion", LaunchMode = LaunchMode.SingleTop, ScreenOrientation = ScreenOrientation.Portrait)]
    public class ReciboValoresImpresionActivity : AppCompatActivity, IPermissionListener
    {

        private ResultadoDeOperacionGenerico<Vendedor> vendedor;
        public static string file_name = "Order.pdf";
        public static string LlaveReciboValores = "LlaveReciboValores";
        public static string LlaveFormaDePago = "LlaveFormaDePago";
        public static string LlaveFactura = "LlaveSitio";
        public static string LLaveIdRV = "LlaveIdRV";
        public static string LLaveMonto = "LLaveMonto";
        //public Transacciones transacciones;
        static string Factura;
        static string MontoQr;
        public string ObtieneFactura;
        public string FormaDepago;
        public int IdRV;
        public decimal Monto;

        ProveedorDeClientes proveedorDeCliente = new ProveedorDeClientes();
        private ResultadoDeOperacionGenerico<Ticket> ticket;
        ResultadoDeOperacionGenerico<List<CuentasBancarias>> cuentasBancarias;
        ProveedorDeCXC proveedorDeCXC = new ProveedorDeCXC();
        //private ResultadoDeOperacionGenerico<int> digitoVerificador;
        ResultadoDeOperacionGenerico<string> reimpresion;
        Timer timer;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            vendedor = Servicios.ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Intent.Obtener<string>(MainActivity.LlaveUsuario));

            FormaDepago = ProveedorDeSerializado.Obtener<string>(Intent.Obtener<string>(LlaveFormaDePago));
            ObtieneFactura = ProveedorDeSerializado.Obtener<string>(Intent.Obtener<string>(LlaveFactura));
            IdRV = ProveedorDeSerializado.Obtener<int>(Intent.Obtener<string>(LLaveIdRV));
            Monto = ProveedorDeSerializado.Obtener<decimal>(Intent.Obtener<string>(LLaveMonto));

            if (ObtieneFactura == null || ObtieneFactura == "") 
            {
                SetContentView(Resource.Layout.Error);
                FindViewById<TextView>(Resource.Id.ErrorTexto).Text =
                   "Error al generar el ticket, no se genero la factura";
            }
            //// ============================================== Valida Cuentas Bancarias ============================================////
            var ConsultaCtaBancarias = Task.Run(async () => {
                cuentasBancarias = await proveedorDeCliente.CuentasBancarias();
            });
            ConsultaCtaBancarias.Wait();

            if (cuentasBancarias.Tipo == TipoDeResultado.Fallo)
            {
                SetContentView(Resource.Layout.Error);
                FindViewById<TextView>(Resource.Id.ErrorTexto).Text = "No se han configurado las cuentas bancarias:";
            }

            //// ============================================== Valida Cuentas Bancarias ============================================////

            if (FormaDepago == "01" || FormaDepago == "02" && Monto > 0)
            {
                FormaDePagoContado();
            }
            else if (FormaDepago == "99" )
            {
                FormaDePagoMicroCreditos();
            }
            else
            {
                SetContentView(Resource.Layout.Error);
                FindViewById<TextView>(Resource.Id.ErrorTexto).Text =
                   "Solo se pueden generar tickets con forma de pago Efectivo, Cheque Nominativo o sin definir";
            }
        }
        public async void FormaDePagoContado()
        {
            var descargaTicket = Task.Run(async () =>
            {
                ticket = await proveedorDeCliente.Ticket(new DatosTicket(vendedor.Valor.Almacen, ObtieneFactura));
            });
            descargaTicket.Wait();
            Factura = ObtieneFactura;           

            if (ticket.Tipo == TipoDeResultado.Exito)
            {
                MontoQr = String.Format("{0:C}", ticket.Valor.Importe);
                SetContentView(Resource.Layout.ReciboValores);
                await Common.Common.WriteFileToStorageAsync(this, BaseFont.HELVETICA);
                Dexter.WithActivity(this)
                    .WithPermission(Manifest.Permission.WriteExternalStorage)
                    .WithListener(this)
                    .Check();
                FindViewById<Button>(Resource.Id.RegresarRegistroValores).Click += ReciboValoresActivity_Click;
            }
            else
            {

                var reimpresion = await proveedorDeCliente.ReimprimeTicketVPA(new DataReimpresion(ObtieneFactura));
                if (reimpresion.Tipo == TipoDeResultado.Exito)
                {
                    var Reimprimeticket = Task.Run(async () =>
                    {
                        ticket = await proveedorDeCliente.Ticket(new DatosTicket(vendedor.Valor.Almacen, ObtieneFactura));
                    });

                    Reimprimeticket.Wait();
                    MontoQr = String.Format("{0:C}", ticket.Valor.Importe);
                    SetContentView(Resource.Layout.ReciboValores);
                    await Common.Common.WriteFileToStorageAsync(this, BaseFont.HELVETICA);
                    Dexter.WithActivity(this)
                        .WithPermission(Manifest.Permission.WriteExternalStorage)
                        .WithListener(this)
                        .Check();
                    FindViewById<Button>(Resource.Id.RegresarRegistroValores).Click += ReciboValoresActivity_Click;
                }
                else
                {
                    SetContentView(Resource.Layout.Error);
                    FindViewById<TextView>(Resource.Id.ErrorTexto).Text =
                       "Error al generar el ticket";
                }
            }
        }
        public async void FormaDePagoMicroCreditos()
        {
            if(IdRV > 0)
            {
                var descargaTicketOVPagado = Task.Run(async () =>
                {
                    ticket = await proveedorDeCXC.TicketOVPagado(new DataTicketOVPago(IdRV));
                });
                descargaTicketOVPagado.Wait();
                
            }
            else
            {
                var descargaTicketOV = Task.Run(async () =>
                {
                    ticket = await proveedorDeCliente.TicketOV(new DatosTicket(vendedor.Valor.Almacen, ObtieneFactura));
                });
                descargaTicketOV.Wait();
            }            

            Factura = ObtieneFactura;           

            if (ticket.Tipo == TipoDeResultado.Exito)
            {
                MontoQr = String.Format("{0:C}", ticket.Valor.Importe);
                SetContentView(Resource.Layout.ReciboValores);
                await Common.Common.WriteFileToStorageAsync(this, BaseFont.HELVETICA);
                Dexter.WithActivity(this)
                    .WithPermission(Manifest.Permission.WriteExternalStorage)
                    .WithListener(this)
                    .Check();
                FindViewById<Button>(Resource.Id.RegresarRegistroValores).Click += ReciboValoresActivity_Click;
            }
            else
            {
                if(IdRV > 0)
                {                     
                    reimpresion = await proveedorDeCliente.ReimprimeTicketOVVPA(new DataReimpresion(ObtieneFactura));
                    await proveedorDeCXC.CreaTicketVPAAP(new DataCreaTicket(ProveedorGlobal.ClienteCXC.Factura, Convert.ToDecimal(Monto), FormaDepago));
                }
                else
                {
                    reimpresion = await proveedorDeCliente.ReimprimeTicketVPA(new DataReimpresion(ObtieneFactura));
                }
                 

                if (reimpresion.Tipo == TipoDeResultado.Exito)
                {
                    var ReimprimeticketOV = Task.Run(async () =>
                    {
                        ticket = await proveedorDeCliente.TicketOV(new DatosTicket(vendedor.Valor.Almacen, ObtieneFactura));
                    });
                    ReimprimeticketOV.Wait();
                    MontoQr = String.Format("{0:C}", ticket.Valor.Importe);
                    SetContentView(Resource.Layout.ReciboValores);
                    await Common.Common.WriteFileToStorageAsync(this, BaseFont.HELVETICA);
                    Dexter.WithActivity(this)
                        .WithPermission(Manifest.Permission.WriteExternalStorage)
                        .WithListener(this)
                        .Check();
                    FindViewById<Button>(Resource.Id.RegresarRegistroValores).Click += ReciboValoresActivity_Click;
                }
                else
                {
                    SetContentView(Resource.Layout.Error);
                    FindViewById<TextView>(Resource.Id.ErrorTexto).Text =
                       "Error al generar el ticket";
                }
            }
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
        private void ReciboValoresActivity_Click(object sender, EventArgs e)
        {
            var activity = new Android.Content.Intent(this, typeof(ContenidoActivity));
            activity.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(vendedor));
            //activity.PutExtra(LlaveVentaCompleta, ProveedorDeSerializado.Generar(ClientesProspecto));
            SetResult(Result.Ok, activity);
            Finish();
        }

        public void OnPermissionGranted(PermissionGrantedResponse p0)
        {
            TicketPDF ticketPDF = new TicketPDF();
            ticketPDF.AddHeaderLine("");
            ticketPDF.AddHeaderLine("                 MINSA COMERCIAL S.A. DE C.V.");
            ticketPDF.AddHeaderLine("                  PROLONGACION TOLTECAS 4");
            ticketPDF.AddHeaderLine("                  COL. LOS REYES IXTACALA");
            ticketPDF.AddHeaderLine("              TLALNEPANTLA, EDO DE MEX 54090");            
            ticketPDF.AddHeaderLine("                               " + ticket.Valor.Folio);
            ticketPDF.AddHeaderLine("");

            ticketPDF.AddHeaderLine("BODEGA: " + ticket.Valor.Bodega);
            ticketPDF.AddHeaderLine("SITIO: " + ticket.Valor.Almacen);
            ticketPDF.AddHeaderLine("RUTA: " + ticket.Valor.Ruta);
            var cuentafactuacion = ticket.Valor.CuentaFacturacion.Split("-");
            ticketPDF.AddHeaderLine("REFERENCIA 1: " + cuentafactuacion[0] + ticket.Valor.DigitoVerificador);
            ticketPDF.AddHeaderLine("REFERENCIA 2: " + ticket.Valor.Factura + "XXXX");
            ticketPDF.AddHeaderLine("CLIENTE: " + ticket.Valor.Nombre);
            ticketPDF.AddHeaderLine("FECHA: " + ticket.Valor.FechaFactura);
            ticketPDF.AddHeaderLine("HORA: " + ticket.Valor.Hora);

            //====================================================================================
            ticketPDF.AddSubHeaderLine("=============================================== ");
            ticketPDF.AddSubHeaderLine("                CONVENIOS BANCARIOS");
            foreach (var cuentas in cuentasBancarias.Valor)
            {
                ticketPDF.AddSubHeaderLine("                " + cuentas.NombreCuenta);
                ticketPDF.AddSubHeaderLine("                " + cuentas.Referencia);
            }
            ticketPDF.AddSubHeaderLine("=============================================== ");

            //Importe
            var importe = String.Format("{0:C}", ticket.Valor.ImporteTotal);
            if (FormaDepago == "01" || FormaDepago == "02" || Monto > 0)
            {
                ticketPDF.AddTotal("IMPORTE DE PAGO: ", importe);
            }
            else if(FormaDepago == "99")
            {
                ticketPDF.AddTotal("IMPORTE DE VENTA: ", importe);
            }
            ticketPDF.AddTotal(ticket.Valor.CantidadLetras, "");
            ticketPDF.AddTotal("FORMA DE PAGO: ", ticket.Valor.FormaPago);
            ticketPDF.AddTotal("NUMERO DE CHEQUE", "");

            if (FormaDepago == "99")
            {
                ticketPDF.AddLeyendas("             ESTE DOCUMENTO NO AMPARA");
                ticketPDF.AddLeyendas("             LA RECEPCION DE EFECTIVO");
            }
            //Informacion            

            ticketPDF.AddFooterLine("        GRACIAS POR UTILIZAR ESTE SERVICIO!!!");
            ticketPDF.AddFooterLine("          PARA DUDAS O ACLARACIONES DE ESTA");
            ticketPDF.AddFooterLine("            OPERACIÓN, FAVOR DE LLAMAR AL");
            ticketPDF.AddFooterLine("            CENTRO DE SERVICIO AL CLIENTE");
            ticketPDF.AddFooterLine("             800 712 5050 LADA SIN COSTO");
            ticketPDF.AddFooterLine("                          COPIA CLIENTE");


            //========================================================= COPIA VENDEDOR ==========================================
            var path = Common.Common.GetAppPath(this);
            ticketPDF.Print(Common.Common.GetAppPath(this) + file_name, FormaDepago);
            PrintPDF();
        }

        public void OnPermissionRationaleShouldBeShown(PermissionRequest p0, IPermissionToken p1)
        {
            throw new System.NotImplementedException();
        }


        public class TicketPDF
        {
            public TicketPDF()
            {
                myDocument.AddAuthor("Osvaldo Garcia");
                myDocument.AddCreator("Osvaldo Garcia");
                myDocument.AddTitle("Ticket de Venta");
            }

            PdfWriter writer = null;
            PdfContentByte cb = null;
            ArrayList headerLines = new ArrayList();
            ArrayList subHeaderLines = new ArrayList();
            ArrayList items = new ArrayList();
            ArrayList totales = new ArrayList();
            ArrayList leyenda = new ArrayList();
            ArrayList footerLines = new ArrayList();
            private string headerImage = "";
            bool _DrawItemHeaders = true;
            int count = 0;
            string path = "";

            int maxChar = 50;
            int maxCharDescription = 20;
            int imageHeight = 0;
            float leftMargin = 0;
            float rightMargin = 0;
            float topMargin = 0;
            static int fontSize = 17;

            static BaseFont bfCourier =
                BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);


            static Font font = new Font(bfCourier, fontSize, Font.NORMAL, Color.BLACK);

            Document myDocument = new Document(PageSize.LEGAL); //Aqui se ponen todos los objetos

            string line = "";

            #region Properties

            public String Path
            {
                get { return path; }
                set { path = value; }
            }

            public String FileName
            {
                get { return file_name; }
                set { file_name = value; }
            }
            //wmjul30
            public String FullFileName
            {
                get { return (String.Format("{0}{1}", path, file_name)); }
            }
            public String HeaderImage
            {
                get { return headerImage; }
                set { if (headerImage != value) headerImage = value; }
            }

            public int MaxChar
            {
                get { return maxChar; }
                set { if (value != maxChar) maxChar = value; }
            }

            public bool DrawItemHeaders
            {
                set { _DrawItemHeaders = value; }
            }

            public int MaxCharDescription
            {
                get { return maxCharDescription; }
                set { if (value != maxCharDescription) maxCharDescription = value; }
            }

            public int FontSize
            {
                get { return fontSize; }
                set { if (value != fontSize) fontSize = value; }
            }

            //public Font FontName
            //{
            //    get { return font; }
            //    set { if (value != font) font = value; }
            //}

            #endregion

            public void AddHeaderLine(string line)
            {
                headerLines.Add(line);
            }

            public void AddSubHeaderLine(string line)
            {
                subHeaderLines.Add(line);
            }

            public void AddItem(string cantidad, string item, string price)
            {
                TicketOrderItem newItem = new TicketOrderItem('?');
                items.Add(newItem.GenerateItem(cantidad, item, price));
            }

            public void AddTotal(string name, string price)
            {
                TicketOrderTotal newTotal = new TicketOrderTotal('?');
                totales.Add(newTotal.GenerateTotal(name, price));
            }
            public void AddLeyendas(string line)
            {
                leyenda.Add(line);
            }
            public void AddFooterLine(string line)
            {
                footerLines.Add(line);
            }

            private string AlignRightText(int lenght)
            {
                string espacios = "";
                int spaces = maxChar - lenght;
                for (int x = 0; x < spaces; x++)
                    espacios += " ";
                return espacios;
            }

            private string DottedLine()
            {
                string dotted = "";
                for (int x = 0; x < maxChar; x++)
                    dotted += "=";
                return dotted;
            }

            public bool Print(string v, string FormaDepago)
            {
                if (new Java.IO.File(v).Exists())
                    new Java.IO.File(v).Delete();
                try
                {
                    //aqui para generar el PDF
                    writer = PdfWriter.GetInstance(myDocument,
                        new FileStream(v, FileMode.Create));
                    myDocument.Open();
                    cb = writer.DirectContent;
                    writer.PageEvent = new PdfWriteEvents();
                    cb.SetFontAndSize(font.BaseFont, fontSize);
                    cb.BeginText();
                    DrawHeader();
                    DrawSubHeader();
                    DrawTotales();        
                    DrawLeyendas();
                    DrawFooter();
                    DrawImage();
                    cb.EndText();
                    myDocument.Close();

                    return true;
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }

            private float YPosition()
            {
                return (myDocument.PageSize.Height -
                    (topMargin + (count * 20)));
            }
            public class PdfWriteEvents : IPdfPageEvent
            {

                public void OnChapter(PdfWriter writer, Document document, float paragraphPosition, Paragraph title)
                {
                    //throw new NotImplementedException();
                }

                public void OnChapterEnd(PdfWriter writer, Document document, float paragraphPosition)
                {
                    //throw new NotImplementedException();
                }

                public void OnCloseDocument(PdfWriter writer, Document document)
                {
                    //throw new NotImplementedException();
                }

                public void OnEndPage(PdfWriter writer, Document document)
                {
                    //string marcaagua = "Reimpresión";
                    //float positionX = writer.PageSize.Right / 2;
                    //float positionY = writer.PageSize.Top / 2;
                    //float fontSize = 80f;
                    //float rotation = 45f;
                    PdfContentByte cbM = writer.DirectContentUnder;
                    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

                    cbM.BeginText();
                    //cbM.SetColorFill(Color.LIGHT_GRAY);
                    cbM.SetFontAndSize(bf, fontSize);
                   // cbM.ShowTextAligned(PdfContentByte.ALIGN_CENTER, marcaagua, 250, 650, rotation);
                    cbM.EndText();
                }

                public void OnGenericTag(PdfWriter writer, Document document, Rectangle rect, string text)
                {
                    //throw new NotImplementedException();
                }

                public void OnOpenDocument(PdfWriter writer, Document document)
                {
                    //throw new NotImplementedException();
                }

                public void OnParagraph(PdfWriter writer, Document document, float paragraphPosition)
                {
                    //throw new NotImplementedException();
                }

                public void OnParagraphEnd(PdfWriter writer, Document document, float paragraphPosition)
                {
                    //throw new NotImplementedException();
                }

                public void OnSection(PdfWriter writer, Document document, float paragraphPosition, int depth, Paragraph title)
                {
                    //throw new NotImplementedException();
                }

                public void OnSectionEnd(PdfWriter writer, Document document, float paragraphPosition)
                {
                    //throw new NotImplementedException();
                }

                public void OnStartPage(PdfWriter writer, Document document)
                {
                    //throw new NotImplementedException();
                }
            }
            private void DrawImage()
            {
                try
                {
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(Factura + " " + MontoQr, QRCodeGenerator.ECCLevel.L);
                    PngByteQRCode qRCode = new PngByteQRCode(qrCodeData);
                    byte[] qrCodeBytes = qRCode.GetGraphic(4);
                    string base64ImageRepresentation = Convert.ToBase64String(qrCodeBytes);

                    Image logo = Image.GetInstance(qrCodeBytes);
                    double height = ((double)logo.Height / 58) * 15;
                    imageHeight = (int)Math.Round(height) + 3;
                    logo.SetAbsolutePosition(145,65);
                    logo.ScaleToFit(150, 150);

                    myDocument.Add(logo);

                }
                catch (Exception ex) { throw (ex); }
            }

            private void DrawHeader()
            {
                try
                {
                    foreach (string header in headerLines)
                    {
                        if (header.Length > maxChar)
                        {
                            int currentChar = 0;
                            int headerLenght = header.Length;
                            while (headerLenght > maxChar)
                            {
                                line = header.Substring(currentChar, maxChar);
                                cb.SetTextMatrix(leftMargin, YPosition());
                                cb.ShowText(line);
                                count++;
                                currentChar += maxChar;
                                headerLenght -= maxChar;
                            }
                            line = header;
                            cb.SetTextMatrix(leftMargin, YPosition());
                            cb.ShowText(line.Substring(currentChar,
                                line.Length - currentChar));
                            count++;
                        }
                        else
                        {
                            line = header;
                            cb.SetTextMatrix(leftMargin, YPosition());
                            cb.ShowText(line);
                            // cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, line, leftMargin, rightMargin,0);
                            count++;
                        }
                    }
                    DrawEspacio();
                }
                catch (Exception ex) { throw (ex); }
            }

            private void DrawSubHeader()
            {
                try
                {
                    //    DottedLine();
                    foreach (string header in subHeaderLines)
                    {
                        if (header.Length > maxChar)
                        {
                            int currentChar = 0;
                            int headerLenght = header.Length;
                            while (headerLenght > maxChar)
                            {
                                line = header.Substring(currentChar, maxChar);
                                cb.SetTextMatrix(leftMargin, YPosition());
                                cb.ShowText(line);
                                count++;
                                currentChar += maxChar;
                                headerLenght -= maxChar;
                            }
                            line = header;
                            cb.SetTextMatrix(leftMargin, YPosition());
                            cb.ShowText(line.Substring(currentChar,
                                line.Length - currentChar));
                            count++;
                        }
                        else
                        {
                            line = header;
                            cb.SetTextMatrix(leftMargin, YPosition());
                            cb.ShowText(line);
                            // cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, line, leftMargin, rightMargin,0);
                            count++;
                        }
                    }
                    //     DottedLine();
                    DrawEspacio();
                }
                catch (Exception ex) { throw (ex); }
            }
            private void DrawLeyendas()
            {
                try
                {
                    foreach (string _leyenda in leyenda)
                    {
                        if (_leyenda.Length > maxChar)
                        {
                            int currentChar = 0;
                            int headerLenght = _leyenda.Length;
                            while (headerLenght > maxChar)
                            {
                                line = _leyenda.Substring(currentChar, maxChar);
                                cb.SetTextMatrix(leftMargin, YPosition());
                                cb.ShowText(line);
                                count++;
                                currentChar += maxChar;
                                headerLenght -= maxChar;
                            }
                            line = _leyenda;
                            cb.SetTextMatrix(leftMargin, YPosition());
                            cb.ShowText(line.Substring(currentChar,
                                line.Length - currentChar));
                            count++;
                        }
                        else
                        {
                            line = _leyenda;
                            cb.SetTextMatrix(leftMargin, YPosition());
                            cb.ShowText(line);
                            // cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, line, leftMargin, rightMargin,0);
                            count++;
                        }
                    }
                    DrawEspacio();
                }
                catch (Exception ex) { throw (ex); }
            }
            //private void DrawSubHeader()
            //{
            //    try
            //    {
            //        line = DottedLine();
            //        cb.SetTextMatrix(leftMargin, YPosition());
            //        cb.ShowText(line);
            //        DrawEspacio();
            //        foreach (string subHeader in subHeaderLines)
            //        {
            //            if (subHeader.Length > maxChar)
            //            {
            //                int currentChar = 0;
            //                int subHeaderLenght = subHeader.Length;
            //                while (subHeaderLenght > maxChar)
            //                {
            //                    line = subHeader;
            //                    cb.SetTextMatrix(leftMargin, YPosition());
            //                    cb.ShowText(line.Substring(currentChar, maxChar));
            //                    count++;
            //                    currentChar += maxChar;
            //                    subHeaderLenght -= maxChar;
            //                }
            //                line = subHeader;
            //                cb.SetTextMatrix(leftMargin, YPosition());
            //                cb.ShowText(line.Substring(currentChar,
            //                    line.Length - currentChar));
            //                count++;
            //              //  line = DottedLine();
            //                cb.SetTextMatrix(leftMargin, YPosition());
            //                cb.ShowText(line);
            //               DrawEspacio();
            //            }
            //            else
            //            {
            //                line = subHeader;
            //                cb.SetTextMatrix(leftMargin, YPosition());
            //                cb.ShowText(line);
            //                count++;
            //                //line = DottedLine();
            //                cb.SetTextMatrix(leftMargin, YPosition());
            //                cb.ShowText(line);
            //                count++;
            //            }
            //        }
            //        DrawEspacio();
            //    }
            //    catch (Exception ex) { throw (ex); }
            //}

            private void DrawItems()
            {
                TicketOrderItem ordIt = new TicketOrderItem('?');
                if (_DrawItemHeaders)
                {
                    cb.SetTextMatrix(leftMargin, YPosition());
                    cb.ShowText("CANT  DESCRIPCION                IMPORTE");
                }
                count++;
                DrawEspacio();
                foreach (string item in items)
                {
                    line = ordIt.GetItemCantidad(item);
                    cb.SetTextMatrix(leftMargin, YPosition());
                    cb.ShowText(line);
                    line = ordIt.GetItemPrice(item);
                    line = AlignRightText(line.Length) + line;
                    cb.SetTextMatrix(leftMargin, YPosition());
                    cb.ShowText(line);
                    string name = ordIt.GetItemName(item);
                    leftMargin = 0;
                    if (name.Length > maxCharDescription)
                    {
                        int currentChar = 0;
                        int itemLenght = name.Length;
                        while (itemLenght > maxCharDescription)
                        {
                            line = ordIt.GetItemName(item);
                            cb.SetTextMatrix(leftMargin, YPosition());
                            cb.ShowText("      " + line.Substring(currentChar,
                                maxCharDescription));
                            count++;
                            currentChar += maxCharDescription;
                            itemLenght -= maxCharDescription;
                        }
                        line = ordIt.GetItemName(item);
                        cb.SetTextMatrix(leftMargin, YPosition());
                        cb.ShowText("      " + line.Substring(currentChar,
                            maxCharDescription));
                        count++;
                    }
                    else
                    {
                        cb.SetTextMatrix(leftMargin, YPosition());
                        cb.ShowText("      " + ordIt.GetItemName(item));
                        count++;
                    }
                }

                leftMargin = 0;
                DrawEspacio();
                line = DottedLine();
                cb.SetTextMatrix(leftMargin, YPosition());
                cb.ShowText(line);
                count++;
                DrawEspacio();
            }

            private void DrawTotales()
            {
                TicketOrderTotal ordTot = new TicketOrderTotal('?');
                foreach (string total in totales)
                {
                    line = ordTot.GetTotalCantidad(total);
                    line = AlignRightText(line.Length) + line;
                    cb.SetTextMatrix(leftMargin, YPosition());
                    cb.ShowText(line);
                    leftMargin = 0;
                    line = "" + ordTot.GetTotalName(total);
                    cb.SetTextMatrix(leftMargin, YPosition());
                    cb.ShowText(line);
                    count++;
                }
                leftMargin = 0;
                DrawEspacio();
                DrawEspacio();
            }

            private void DrawFooter()
            {
                foreach (string footer in footerLines)
                {
                    if (footer.Length > maxChar)
                    {
                        int currentChar = 0;
                        int footerLenght = footer.Length;
                        while (footerLenght > maxChar)
                        {
                            line = footer;
                            cb.SetTextMatrix(leftMargin, YPosition());
                            cb.ShowText(line.Substring(currentChar, maxChar));
                            count++;
                            currentChar += maxChar;
                            footerLenght -= maxChar;
                        }
                        line = footer;
                        cb.SetTextMatrix(leftMargin, YPosition());
                        cb.ShowText(line.Substring(currentChar, maxChar));
                        count++;
                    }
                    else
                    {
                        line = footer;
                        cb.SetTextMatrix(leftMargin, YPosition());
                        cb.ShowText(line);

                        count++;
                    }
                }
                leftMargin = 0;
                DrawEspacio();
            }

            private void DrawEspacio()
            {
                line = "";
                cb.SetTextMatrix(leftMargin, YPosition());
                //cb.SetFontAndSize(20, fontSize);
                cb.ShowText(line);
                count++;
            }
        }

        public class TicketOrderItem
        {
            char[] delimitador = new char[] { '?' };
            public TicketOrderItem(char delimit)
            {
                delimitador = new char[] { delimit };
            }

            public string GetItemCantidad(string TicketOrderItem)
            {
                string[] delimitado = TicketOrderItem.Split(delimitador);
                return delimitado[0];
            }

            public string GetItemName(string TicketOrderItem)
            {
                string[] delimitado = TicketOrderItem.Split(delimitador);
                return delimitado[1];
            }

            public string GetItemPrice(string TicketOrderItem)
            {
                string[] delimitado = TicketOrderItem.Split(delimitador);
                return delimitado[2];
            }

            public string GenerateItem(string cantidad,
                string itemName, string price)
            {
                return cantidad + delimitador[0] +
                    itemName + delimitador[0] + price;
            }
        }

        public class TicketOrderTotal
        {
            char[] delimitador = new char[] { '?' };
            public TicketOrderTotal(char delimit)
            {
                delimitador = new char[] { delimit };
            }

            public string GetTotalName(string totalItem)
            {
                string[] delimitado = totalItem.Split(delimitador);
                return delimitado[0];
            }

            public string GetTotalCantidad(string totalItem)
            {
                string[] delimitado = totalItem.Split(delimitador);
                return delimitado[1];
            }

            public string GenerateTotal(string totalName,
                string price)
            {
                return totalName + delimitador[0] + price;
            }
        }

        private void PrintPDF()
        {
            PrintManager printManager = (PrintManager)GetSystemService(Context.PrintService);
            try
            {
                PrintDocumentAdapter adapter = new PrintPDFAdapter(this, Common.Common.GetAppPath(this) + file_name);

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