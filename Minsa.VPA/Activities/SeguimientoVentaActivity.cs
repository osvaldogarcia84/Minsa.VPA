using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Grantland.Widget;
using Minsa.VPA.Adaptadores;
using Minsa.VPA.Enums;
using Minsa.VPA.Fragments;
using Minsa.VPA.Modelos;
using Minsa.VPA.Proveedores;
using Minsa.VPA.Repositorio;
using Minsa.VPA.Servicios;
using Android.Support.V4.Widget;

using Android.Support.Design.Widget;
using Minsa.VPA.Extensiones;
using System.Globalization;
using Android.Locations;



namespace Minsa.VPA.Activities
{
    [Activity(Label = "Causas de SI/NO venta", LaunchMode = LaunchMode.SingleTop, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    
    public class SeguimientoVentaActivity : Activity
    {
     
        public const string SeguimientoVenta = "SeguimientoVenta";
        public ResultadoDeOperacionGenerico<List<Respuesta>> ResultadoDeListaDeVenta;
        private ResultadoDeOperacionGenerico<Vendedor> vendedor;
        private string ordenId;
        public ResultadoDeVentaMovil resultadoDeVentaMovil;
        private TipoDeCausa _tipoDeCausa;
        private List<Respuesta> _respuestas;
        private SeguimientoDeVenta seguimientoDeVenta;// resultadoDeVenta;
        private ResultadoDeVenta resultadoDeVenta;
        //private List<SeguimientoDeVenta> ListaresultadoDeVenta = new List<SeguimientoDeVenta>();
        private List<Respuesta> _razones;
        private List<Harinera> _harineras;
        Button Enviar;
        Button BtnRegresarAlMenu;
        Cliente cliente;
        AutofitTextView NombreVendedorVenta;
        ProveedorDeEstrategia proveedorDeEstrategia = new ProveedorDeEstrategia();
        public const string InformacionDeTipoDeCausa = "TipoDeCausa";
        public const string InformacionDeHarineas = "Harineras";
        public const string InformacionDeRazones = "Razones";
        //ResultadoDeOperacionGenerico<string> resultadoProcesaVenta;
        ResultadoDeOperacionGenerico<string> resultadoProcesaVenta2;
        ResultadoDeOperacionGenerico<ResultadoRegistroAddCausasNoVenta> resultadoAddCausasNoVenta;
        ResultadoDeOperacionGenerico<ResultadoSeguimiento> result1;
        ResultadoDeOperacionGenerico<List<PrecioSeguimientoVenta>> resultadoPrecio;
        List<PrecioSeguimientoVenta> precioList;
        ResultadoDeOperacionGenerico<List<Desharinizacion>> resultadoHarinizacion;
        ResultadoDeOperacionGenerico<List<FrecuenciaCompra>> resultadoFrecuenciaCompra;
        ResultadoDeOperacionGenerico<List<Cliente>> resultadoCliente;
        ProveedorDeClientes proveedorDeClientes = new ProveedorDeClientes();
       
        List<ItemGenerico> itemsGenericos;
        List<ItemGenerico> itemsApoyosDados;
        List<ItemGenerico> itemsNegocioCerrado;
        Timer timer;
        bool RegresarAlMenu;
        Spinner SPPrecio;
        Spinner SPFrecuencia;
        Spinner SPApoyos;
        Spinner SPDesharinizacion;
        Spinner SPQuienvende;
        Spinner SPApoyoDado;
        Spinner SPTipoCierre;
        ResultadoDeOperacionGenerico<List<Apoyos>> resultadoApoyos;
        public AdapterPrecioSeguimiento adapterPrecio;
        public AdapterFrecuenciaCompra adapterFrecuenciaCompra;
        public AdapterApoyos adapterApoyos;
        public AdapterDesharinizacion adapterDesharinizacion;

        TextView txtPrecioVentaSaco;
        EditText Precio;
        
        TextInputLayout PrecioSacoinput;
        TextView txtPromocionApoyoCompetencia;
        CheckBox CheckPromocionApoyoCompetencia;
        TextView txtCantidadTM;
        EditText CantidadXTM;
        EditText NoCteCompra;
        EditText NombreCteCompra;
        Button FechaProximaCompra;
        EditText VigenciaPromocion;
        EditText NombreComercializador;
        TextInputLayout CantidadXTMinput;
        TextView txtPlazoVigencia;
        TextView txtSacosConsumeDiario;
        EditText SacosConsumeDiario;
        TextView txtFrecuenciaCompra;
        TextView txtDesharinizacion;
        TextView txtPrecioCompra;
        EditText PrecioCompra;

        TextView txtQuienVendiendo;
        TextView txtNombreComercializador;
        TextView TxtMotivoRazon;
        TextView txtApoyoDieron;
        TextView txtQueInsumoes;
        TextView txtNoCteCompra;
        TextView txtNombreCteCompra;
        TextView txtFechaProximaCompra;
        TextView txtTipoCierre;
        TextView FechaProximaCompraDisplay;
        TextInputLayout VigenciaPromocioninput;
        TextInputLayout PrecioComprainput;
        TextInputLayout SacosConsumeDiarioinput;
        TextInputLayout NombreComercializadorinput;
        TextInputLayout txtQueInsumoesinput;
        TextInputLayout NoCteComprainput;
        TextInputLayout NombreCteComprainput;
        //TextInputLayout FechaProximaComprainput;
        List<ItemGenerico> quienVendio;
        List<ItemGenerico> ApoyoDieron;
        bool PromocionApoyoCompetencia;
        string IdApoyo;
        string TipoApoyoCB;
        string FrecuenciaCompra;
        string Desharinizacion;
        string QuienAtiende;
        string TipoCierre;
        string FechaProximacompra;
        string MotivoPrecio;

        //private RutaFragment RutaFragment;
        private ContenidoActivity contenidoActivity;
        public decimal CantidadxTM
        {
            get { return FindViewById<EditText>(Resource.Id.CantidadXTM).Text.ObtenerCantidad(); }
            set { FindViewById<EditText>(Resource.Id.CantidadXTM).Text = value.ToString("##,###.##"); }
        }
        public decimal SacosConsumeDiarios
        {
            get { return FindViewById<EditText>(Resource.Id.SacosConsumeDiario).Text.ObtenerCantidad(); }
            set { FindViewById<EditText>(Resource.Id.SacosConsumeDiario).Text = value.ToString("##,###.##"); }
        }
        public string VigenciaPromocionApoyo
        {
            get { return FindViewById<EditText>(Resource.Id.VigenciaPromocion).Text; }
            set { FindViewById<EditText>(Resource.Id.SacosConsumeDiario).Text = value.ToString(); }
        }
        public decimal Preciocompra
        {
            get { return FindViewById<EditText>(Resource.Id.PrecioCompra).Text.ObtenerCantidad(); }
            set { FindViewById<EditText>(Resource.Id.PrecioCompra).Text = value.ToString("##,###.##"); }
        }
        public string Nombrecomercializador
        {
            get { return FindViewById<EditText>(Resource.Id.NombreComercializador).Text; }
            set { FindViewById<EditText>(Resource.Id.NombreComercializador).Text = value.ToString(); }
        }
        public string NumeroCteCompra
        {
            get { return FindViewById<EditText>(Resource.Id.NoCteCompra).Text; }
            set { FindViewById<EditText>(Resource.Id.NoCteCompra).Text = value.ToString(); }
        }
        public string NombreClienteCompra
        {
            get { return FindViewById<EditText>(Resource.Id.NombreCteCompra).Text; }
            set { FindViewById<EditText>(Resource.Id.NombreCteCompra).Text = value.ToString(); }
        }
        public decimal PrecioVentaSaco
        {
            get { return FindViewById<TextView>(Resource.Id.PrecioVentaSaco).Text.ObtenerCantidad(); ; }
            set { FindViewById<EditText>(Resource.Id.PrecioVentaSaco).Text = value.ToString(); }
        }
        public string QueInsumoes
        {
            get { return FindViewById<EditText>(Resource.Id.QueInsumoes).Text;  }
            set { FindViewById<EditText>(Resource.Id.QueInsumoes).Text = value.ToString(); }
        }
      
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            try
            {                
                vendedor = Servicios.ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Intent.Obtener<string>(MainActivity.LlaveUsuario));
                cliente = ProveedorGlobal.Cliente;
                // Create your application here
                _tipoDeCausa = ordenId.EsValido() ? TipoDeCausa.Venta : TipoDeCausa.NoVenta;

                _harineras = ProveedorDeManipulacionDeDatos.ObtenerTodos<Harinera>(
                        Intent.Obtener<string[]>(InformacionDeHarineas)).ConvertirLista();

                _razones = ProveedorDeManipulacionDeDatos.ObtenerTodos<Respuesta>(
                        Intent.Obtener<string[]>(InformacionDeRazones)).ConvertirLista();

                seguimientoDeVenta = new SeguimientoDeVenta(ProveedorGlobal.Cliente.ClienteId, 0, 1, 0);

                resultadoDeVentaMovil = new ResultadoDeVentaMovil(0, ProveedorGlobal.Cliente.ClienteId, DateTime.Now, null, null);

                if(vendedor.Valor.Almacen == "Supervisor")
                {
                    resultadoDeVenta = new ResultadoDeVenta(ProveedorGlobal.Cliente.ClienteId,
                                                        0,
                                                        vendedor.Valor.Usuario
                                                        );
                }
                else
                {
                    resultadoDeVenta = new ResultadoDeVenta(ProveedorGlobal.Cliente.ClienteId,
                                                       0,
                                                       vendedor.Valor.Id
                                                       );
                }
                
                if (_razones == null)
                {
                    var razon = Task.Run(async () =>
                    {
                        ResultadoDeListaDeVenta = await proveedorDeEstrategia.ObtenerCausasDeResultadoDeVenta();
                    });
                    razon.Wait();

                    if (ResultadoDeListaDeVenta.Tipo == TipoDeResultado.Exito)
                    {
                        _razones = ResultadoDeListaDeVenta.Valor;
                        Intent.PutStringArrayListExtra(InformacionDeRazones,
                            ProveedorDeManipulacionDeDatos.GenerarTodos(_razones));
                    }
                    else

                        Toast.MakeText(this, "Error: " + ResultadoDeListaDeVenta.Mensaje, ToastLength.Long).Show();
                }
                if (_harineras == null)
                {
                    var resultadoDeHarineras = proveedorDeEstrategia.ObtenerHarineras();
                    if (resultadoDeHarineras.Tipo == TipoDeResultado.Exito)
                    {
                        _harineras = resultadoDeHarineras.Valor;
                        Intent.PutStringArrayListExtra(InformacionDeHarineas,
                            ProveedorDeManipulacionDeDatos.GenerarTodos(_harineras));
                    }
                    else
                        Toast.MakeText(this, "Error: " + resultadoDeHarineras.Valor, ToastLength.Long).Show();
                }

                // ===========================================================================================================//

                var spprecios = Task.Run(async () => {
                    resultadoPrecio = await proveedorDeEstrategia.Precio();
                });
                spprecios.Wait();
                precioList = resultadoPrecio.Valor;

                var spHarinizacion = Task.Run(async () => {
                    resultadoHarinizacion = await proveedorDeEstrategia.Desharinizacion();
                });
                spHarinizacion.Wait();

                var spFrecuenciaCompra = Task.Run(async () => {
                    resultadoFrecuenciaCompra = await proveedorDeEstrategia.FrecuenciaCompra();
                });
                spFrecuenciaCompra.Wait();

                var spApoyos = Task.Run(async () => {
                    resultadoApoyos = await proveedorDeEstrategia.Apoyos();
                });
                spApoyos.Wait();

                itemsGenericos = CargaQuienVendio();
                itemsApoyosDados = CargaApoyoCompetencia();
                itemsNegocioCerrado = CargaNegocioCerrado(); 
                // ===========================================================================================================//

                ordenId = String.Empty;
                //ProveedorGlobal.OrdenId = "OC-52125405";            
                SetContentView(Resource.Layout.SeguimientoVenta);

                _tipoDeCausa = TipoDeCausa.NoVenta;

                NombreVendedorVenta = FindViewById<AutofitTextView>(Resource.Id.NombreVendedorVenta);
                NombreVendedorVenta.Text = "Cliente: " + cliente.ClienteId + " - " + cliente.Nombre;

                // ===========================================================================================================//

                SPPrecio = FindViewById<Spinner>(Resource.Id.SPPrecio);
                adapterPrecio = new AdapterPrecioSeguimiento(this, resultadoPrecio.Valor);
                SPPrecio.Adapter = adapterPrecio;
                SPPrecio.Visibility = ViewStates.Invisible;
                //SPPrecio.Enabled = false;

                SPFrecuencia = FindViewById<Spinner>(Resource.Id.SPFrecuencia);
                adapterFrecuenciaCompra = new AdapterFrecuenciaCompra(this, resultadoFrecuenciaCompra.Valor);
                SPFrecuencia.Adapter = adapterFrecuenciaCompra;
                SPFrecuencia.Visibility = ViewStates.Invisible;
                //SPFrecuencia.Enabled = false;

                SPApoyos = FindViewById<Spinner>(Resource.Id.SPApoyos);
                adapterApoyos = new AdapterApoyos(this, resultadoApoyos.Valor);
                SPApoyos.Adapter = adapterApoyos;
                SPApoyos.Visibility = ViewStates.Invisible;
                //SPApoyos.Enabled = false;

                SPDesharinizacion = FindViewById<Spinner>(Resource.Id.SPDesHarinizacion);
                adapterDesharinizacion = new AdapterDesharinizacion(this, resultadoHarinizacion.Valor);
                SPDesharinizacion.Adapter = adapterDesharinizacion;
                SPDesharinizacion.Visibility = ViewStates.Invisible;
                //SPDesharinizacion.Enabled = false;

                SPApoyoDado = FindViewById<Spinner>(Resource.Id.SPApoyoDado);
                SPApoyoDado.Adapter = new ObjetoAdapter<ItemGenerico>(this, itemsApoyosDados);
                SPApoyoDado.Visibility = ViewStates.Invisible;

                SPTipoCierre = FindViewById<Spinner>(Resource.Id.SPTipoCierre);
                SPTipoCierre.Adapter = new ObjetoAdapter<ItemGenerico>(this, itemsNegocioCerrado);
                SPTipoCierre.Visibility = ViewStates.Invisible;

                SPQuienvende = FindViewById<Spinner>(Resource.Id.SPQuienVende);
                SPQuienvende.Adapter = new ObjetoAdapter<ItemGenerico>(this, itemsGenericos);
                SPQuienvende.Visibility = ViewStates.Invisible;
                TxtMotivoRazon = FindViewById<TextView>(Resource.Id.TxtMotivoRazon);
                txtPrecioVentaSaco = FindViewById<TextView>(Resource.Id.txtPrecioVentaSaco);
                txtPromocionApoyoCompetencia = FindViewById<TextView>(Resource.Id.txtPromocionApoyoCompetencia);
                CheckPromocionApoyoCompetencia = FindViewById<CheckBox>(Resource.Id.CheckPromocionApoyoCompetencia);
                txtCantidadTM = FindViewById<TextView>(Resource.Id.txtCantidadTM);
                txtPlazoVigencia = FindViewById<TextView>(Resource.Id.txtPlazoVigencia);
                txtSacosConsumeDiario = FindViewById<TextView>(Resource.Id.txtSacosConsumeDiario);
                txtFrecuenciaCompra = FindViewById<TextView>(Resource.Id.txtFrecuenciaCompra);
                txtPrecioCompra = FindViewById<TextView>(Resource.Id.txtPrecioCompra);
                txtDesharinizacion = FindViewById<TextView>(Resource.Id.txtDesharinizacion);
                txtQuienVendiendo = FindViewById<TextView>(Resource.Id.txtQuienVendiendo);
                txtNombreComercializador = FindViewById<TextView>(Resource.Id.txtNombreComercializador);
                txtApoyoDieron = FindViewById<TextView>(Resource.Id.txtApoyoDieron);
                PrecioSacoinput = FindViewById<TextInputLayout>(Resource.Id.PrecioSacoinput);              
                Precio = FindViewById<EditText>(Resource.Id.Precio);
                txtQueInsumoes = FindViewById<TextView>(Resource.Id.txtQueInsumoes);
               // NoCteCompra = FindViewById<EditText>(Resource.Id.NoCteCompra);
                NombreCteCompra = FindViewById<EditText>(Resource.Id.NombreCteCompra);
                txtTipoCierre = FindViewById<TextView>(Resource.Id.txtTipoCierre);

                FechaProximaCompra = FindViewById<Button>(Resource.Id.FechaProximaCompra);
                txtFechaProximaCompra = FindViewById<TextView>(Resource.Id.txtFechaProximaCompra);
                FechaProximaCompraDisplay = FindViewById<TextView>(Resource.Id.FechaProximaCompraDisplay);

                //FechaProximaComprainput = FindViewById<TextInputLayout>(Resource.Id.FechaProximaComprainput);

                txtNoCteCompra = FindViewById<TextView>(Resource.Id.txtNoCteCompra);
                NoCteComprainput = FindViewById<TextInputLayout>(Resource.Id.NoCteComprainput);
                
                txtNombreCteCompra = FindViewById<TextView>(Resource.Id.txtNombreCteCompra);
                NombreCteComprainput = FindViewById<TextInputLayout>(Resource.Id.NombreCteComprainput);

                CantidadXTM = FindViewById<EditText>(Resource.Id.CantidadXTM);
                CantidadXTMinput = FindViewById<TextInputLayout>(Resource.Id.CantidadXTMinput);
                
                SacosConsumeDiario = FindViewById<EditText>(Resource.Id.SacosConsumeDiario);
                SacosConsumeDiarioinput = FindViewById<TextInputLayout>(Resource.Id.SacosConsumeDiarioinput);
                
                PrecioComprainput = FindViewById<TextInputLayout>(Resource.Id.PrecioComprainput);
                PrecioCompra = FindViewById<EditText>(Resource.Id.PrecioCompra);               

                VigenciaPromocioninput = FindViewById<TextInputLayout>(Resource.Id.VigenciaPromocioninput);
                VigenciaPromocion = FindViewById<EditText>(Resource.Id.VigenciaPromocion);

                NombreComercializadorinput = FindViewById<TextInputLayout>(Resource.Id.NombreComercializadorinput);
                NombreComercializador = FindViewById<EditText>(Resource.Id.NombreComercializador);
                txtQueInsumoesinput = FindViewById<TextInputLayout>(Resource.Id.txtQueInsumoesinput);

                TxtMotivoRazon.Visibility = ViewStates.Gone;
                txtPrecioVentaSaco.Visibility = ViewStates.Gone;
                PrecioSacoinput.Visibility = ViewStates.Gone;
                txtPromocionApoyoCompetencia.Visibility = ViewStates.Gone;
                CheckPromocionApoyoCompetencia.Visibility = ViewStates.Gone;
                txtCantidadTM.Visibility = ViewStates.Gone;
                CantidadXTMinput.Visibility = ViewStates.Gone;
                //CantidadXTM.Visibility = ViewStates.Gone;
                txtPlazoVigencia.Visibility = ViewStates.Gone;
                txtSacosConsumeDiario.Visibility = ViewStates.Gone;
                //SacosConsumeDiario.Visibility = ViewStates.Gone;
                txtFrecuenciaCompra.Visibility = ViewStates.Gone;
                txtPrecioCompra.Visibility = ViewStates.Gone;
                txtDesharinizacion.Visibility = ViewStates.Gone;
                VigenciaPromocioninput.Visibility = ViewStates.Gone;
                txtQuienVendiendo.Visibility = ViewStates.Gone;
                PrecioComprainput.Visibility = ViewStates.Gone;
                NombreComercializadorinput.Visibility = ViewStates.Gone;
                SacosConsumeDiarioinput.Visibility = ViewStates.Gone;
                txtNombreComercializador.Visibility = ViewStates.Gone;
                txtApoyoDieron.Visibility = ViewStates.Gone;
                txtQueInsumoesinput.Visibility = ViewStates.Gone;
                txtQueInsumoes.Visibility = ViewStates.Gone;
                txtNoCteCompra.Visibility = ViewStates.Gone;
                txtNombreCteCompra.Visibility = ViewStates.Gone;
                NoCteComprainput.Visibility = ViewStates.Gone;
                NombreCteComprainput.Visibility = ViewStates.Gone;
                txtFechaProximaCompra.Visibility = ViewStates.Gone;
                FechaProximaCompra.Visibility = ViewStates.Gone;
                //FechaProximaComprainput.Visibility = ViewStates.Gone;
                txtTipoCierre.Visibility = ViewStates.Gone;
                // ==========================================================================================================//

                FindViewById<TextView>(Resource.Id.TxtResultadoRazon).Text =
                 _tipoDeCausa == TipoDeCausa.Venta
                     ? "¿Cuál es la razón por la cual Sí compra?"
                     : "¿Cuál es la razón por la cual No compra?";
                
                //========================================= Carga informacion ===============================================//

                FindViewById<Spinner>(Resource.Id.SeguimientoDeVentaCompetencia).Visibility =
                   _tipoDeCausa == TipoDeCausa.NoVenta ? ViewStates.Visible : ViewStates.Gone;

                FindViewById<Spinner>(Resource.Id.SeguimientoDeVentaCompetencia).Adapter =
                new ObjetoAdapter<Harinera>(this, _harineras);

                FindViewById<Spinner>(Resource.Id.SeguimientoDeVentaCompetencia).ItemSelected += (sender, args) =>
                {
                    var spinner = ((Spinner)sender);
                    long posicion = spinner.SelectedItemId;
                    seguimientoDeVenta.CompetenciaId = (int)((ObjetoAdapter<Harinera>)spinner.Adapter).ObtenerSeleccion(posicion);
                };



                FindViewById<CheckBox>(Resource.Id.SeguimientoDeVentaCompetenciaRespuesta).CheckedChange +=
                (sender, args) =>
                {
                    FindViewById<Spinner>(Resource.Id.SeguimientoDeVentaCompetencia).Visibility = args.IsChecked
                ? ViewStates.Visible
                : ViewStates.Gone;
                    seguimientoDeVenta.CompetenciaId = args.IsChecked ? seguimientoDeVenta.CompetenciaId : 4;
                };

                _respuestas = _razones.Where(e => e.Tipo == _tipoDeCausa).ToList();

                FindViewById<Spinner>(Resource.Id.SPRazon).Adapter = new ObjetoAdapter<Respuesta>(this, _respuestas);

                FindViewById<CheckBox>(Resource.Id.SeguimientoDeVentaCompetenciaRespuesta).Checked =
                    _tipoDeCausa == TipoDeCausa.NoVenta;

                FindViewById<Spinner>(Resource.Id.SPRazon).ItemSelected += (sender, args) =>
                {
                    var spinner = ((Spinner)sender);
                    long posicion = spinner.SelectedItemId;
                    resultadoDeVentaMovil.Respuesta = ((ObjetoAdapter<Respuesta>)spinner.Adapter).ObtenerSeleccion(posicion);
                    seguimientoDeVenta.CuestionarioId = resultadoDeVentaMovil.Respuesta.Id;
                    resultadoDeVenta.RespuestaId = resultadoDeVentaMovil.Respuesta.Id;
                    FechaProximaCompraDisplay.Text = "";

                    if (resultadoDeVentaMovil.Respuesta.Id == 253) // Precio
                    {
                        TxtMotivoRazon.Visibility = ViewStates.Visible;
                        SPPrecio.Visibility = ViewStates.Visible;
                        txtPrecioVentaSaco.Visibility = ViewStates.Visible;
                        PrecioSacoinput.Visibility = ViewStates.Visible;
                        txtPromocionApoyoCompetencia.Visibility = ViewStates.Visible;
                        CheckPromocionApoyoCompetencia.Visibility = ViewStates.Visible;
                        CheckPromocionApoyoCompetencia.CheckedChange += (sender, args) =>
                        {
                            if (args.IsChecked)
                            {
                                PromocionApoyoCompetencia = true;
                                SPApoyoDado.Visibility = ViewStates.Visible;
                                SPApoyoDado.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
                                {
                                       IdApoyo = Convert.ToString(itemsApoyosDados[e.Position]);
                                    if (IdApoyo == "Insumo")
                                    {
                                        txtApoyoDieron.Visibility = ViewStates.Gone;
                                        SPApoyos.Visibility = ViewStates.Gone;

                                        txtCantidadTM.Visibility = ViewStates.Visible;
                                        CantidadXTMinput.Visibility = ViewStates.Visible;
                                        txtQueInsumoes.Visibility = ViewStates.Visible;
                                        txtQueInsumoesinput.Visibility = ViewStates.Visible;
                                    }else if (IdApoyo == "Sacos")
                                    {
                                        txtApoyoDieron.Visibility = ViewStates.Gone;
                                        SPApoyos.Visibility = ViewStates.Gone;

                                        txtQueInsumoes.Visibility = ViewStates.Visible;
                                        txtQueInsumoesinput.Visibility = ViewStates.Visible;
                                    }
                                    else
                                    {
                                        txtApoyoDieron.Visibility = ViewStates.Visible;
                                        SPApoyos.Visibility = ViewStates.Visible;

                                        txtCantidadTM.Visibility = ViewStates.Gone;
                                        CantidadXTMinput.Visibility = ViewStates.Gone;
                                        txtQueInsumoes.Visibility = ViewStates.Gone;
                                        txtQueInsumoesinput.Visibility = ViewStates.Gone;
                                    }
                                };
                            }
                            else
                            {
                                PromocionApoyoCompetencia = false;
                                SPApoyoDado.Visibility = ViewStates.Gone;
                                txtCantidadTM.Visibility = ViewStates.Gone;
                                CantidadXTMinput.Visibility = ViewStates.Gone;
                                txtQueInsumoes.Visibility = ViewStates.Gone;
                                txtQueInsumoesinput.Visibility = ViewStates.Gone;
                                txtApoyoDieron.Visibility = ViewStates.Gone;
                                SPApoyos.Visibility = ViewStates.Gone;
                            }
                        };
                        txtPlazoVigencia.Visibility = ViewStates.Visible;
                        VigenciaPromocioninput.Visibility = ViewStates.Visible;

                        txtSacosConsumeDiario.Visibility = ViewStates.Visible;
                        SacosConsumeDiarioinput.Visibility = ViewStates.Visible;
                        
                        SPFrecuencia.Visibility = ViewStates.Visible;
                        txtFrecuenciaCompra.Visibility = ViewStates.Visible;
                       
                    }else if(resultadoDeVentaMovil.Respuesta.Id == 198 ||
                        resultadoDeVentaMovil.Respuesta.Id == 287 || resultadoDeVentaMovil.Respuesta.Id == 196 ||
                        resultadoDeVentaMovil.Respuesta.Id == 201 || resultadoDeVentaMovil.Respuesta.Id == 288 ||
                        resultadoDeVentaMovil.Respuesta.Id == 202 || resultadoDeVentaMovil.Respuesta.Id == 195 ||
                        resultadoDeVentaMovil.Respuesta.Id == 290 || resultadoDeVentaMovil.Respuesta.Id == 197 ||
                        resultadoDeVentaMovil.Respuesta.Id == 199 || resultadoDeVentaMovil.Respuesta.Id == 192)
                    {
                            TxtMotivoRazon.Visibility = ViewStates.Gone;
                            SPPrecio.Visibility = ViewStates.Gone;
                            txtPrecioVentaSaco.Visibility = ViewStates.Visible;
                            PrecioSacoinput.Visibility = ViewStates.Visible;
                            txtPromocionApoyoCompetencia.Visibility = ViewStates.Visible;
                            CheckPromocionApoyoCompetencia.Visibility = ViewStates.Visible;
                            CheckPromocionApoyoCompetencia.CheckedChange += (sender, args) =>
                            {
                                if (args.IsChecked)
                                {
                                    PromocionApoyoCompetencia = true;
                                    SPApoyoDado.Visibility = ViewStates.Visible;
                                    SPApoyoDado.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
                                    {
                                        IdApoyo = Convert.ToString(itemsApoyosDados[e.Position]);
                                        if (IdApoyo == "Insumo")
                                        {
                                            txtApoyoDieron.Visibility = ViewStates.Gone;
                                            SPApoyos.Visibility = ViewStates.Gone;

                                            txtCantidadTM.Visibility = ViewStates.Visible;
                                            CantidadXTMinput.Visibility = ViewStates.Visible;
                                            txtQueInsumoes.Visibility = ViewStates.Visible;
                                            txtQueInsumoesinput.Visibility = ViewStates.Visible;
                                        }
                                        else if (IdApoyo == "Sacos")
                                        {
                                            txtApoyoDieron.Visibility = ViewStates.Gone;
                                            SPApoyos.Visibility = ViewStates.Gone;

                                            txtQueInsumoes.Visibility = ViewStates.Visible;
                                            txtQueInsumoesinput.Visibility = ViewStates.Visible;
                                        }
                                        else
                                        {
                                            txtApoyoDieron.Visibility = ViewStates.Visible;
                                            SPApoyos.Visibility = ViewStates.Visible;

                                            txtCantidadTM.Visibility = ViewStates.Gone;
                                            CantidadXTMinput.Visibility = ViewStates.Gone;
                                            txtQueInsumoes.Visibility = ViewStates.Gone;
                                            txtQueInsumoesinput.Visibility = ViewStates.Gone;
                                        }
                                    };
                                }
                                else
                                {
                                    PromocionApoyoCompetencia = false;
                                    SPApoyoDado.Visibility = ViewStates.Gone;
                                    txtCantidadTM.Visibility = ViewStates.Gone;
                                    CantidadXTMinput.Visibility = ViewStates.Gone;
                                    txtQueInsumoes.Visibility = ViewStates.Gone;
                                    txtQueInsumoesinput.Visibility = ViewStates.Gone;
                                    txtApoyoDieron.Visibility = ViewStates.Gone;
                                    SPApoyos.Visibility = ViewStates.Gone;
                                }
                            };
                            txtPlazoVigencia.Visibility = ViewStates.Visible;
                            VigenciaPromocioninput.Visibility = ViewStates.Visible;

                            txtSacosConsumeDiario.Visibility = ViewStates.Visible;
                            SacosConsumeDiarioinput.Visibility = ViewStates.Visible;

                            SPFrecuencia.Visibility = ViewStates.Visible;
                            txtFrecuenciaCompra.Visibility = ViewStates.Visible;
                    }
                    else 
                    {
                        TxtMotivoRazon.Visibility = ViewStates.Gone;
                        SPPrecio.Visibility = ViewStates.Gone;
                        txtPrecioVentaSaco.Visibility = ViewStates.Gone;
                        PrecioSacoinput.Visibility = ViewStates.Gone;
                        txtPromocionApoyoCompetencia.Visibility = ViewStates.Gone;
                        CheckPromocionApoyoCompetencia.Visibility = ViewStates.Gone;
                        SPApoyoDado.Visibility = ViewStates.Gone;
                        txtCantidadTM.Visibility = ViewStates.Gone;
                        CantidadXTMinput.Visibility = ViewStates.Gone;
                        txtQueInsumoes.Visibility = ViewStates.Gone;
                        txtQueInsumoesinput.Visibility = ViewStates.Gone;
                        txtApoyoDieron.Visibility = ViewStates.Gone;
                        SPApoyos.Visibility = ViewStates.Gone;

                        txtPlazoVigencia.Visibility = ViewStates.Gone;
                        VigenciaPromocioninput.Visibility = ViewStates.Gone;

                        txtSacosConsumeDiario.Visibility = ViewStates.Gone;
                        SacosConsumeDiarioinput.Visibility = ViewStates.Gone;

                        SPFrecuencia.Visibility = ViewStates.Gone;
                        txtFrecuenciaCompra.Visibility = ViewStates.Gone;
                    }

                    if (resultadoDeVentaMovil.Respuesta.Id == 214) // Desharinizacion
                    {
                        txtDesharinizacion.Visibility = ViewStates.Visible;
                        SPDesharinizacion.Visibility = ViewStates.Visible;
                        
                        txtPrecioCompra.Visibility = ViewStates.Visible;
                        PrecioComprainput.Visibility = ViewStates.Visible;
                        
                        txtSacosConsumeDiario.Visibility = ViewStates.Visible;
                        SacosConsumeDiarioinput.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        txtDesharinizacion.Visibility = ViewStates.Gone;
                        SPDesharinizacion.Visibility = ViewStates.Gone;
                        txtPrecioCompra.Visibility = ViewStates.Gone;
                        PrecioComprainput.Visibility = ViewStates.Gone;
                        txtSacosConsumeDiario.Visibility = ViewStates.Gone;
                        SacosConsumeDiarioinput.Visibility = ViewStates.Gone;
                    }
                    if(resultadoDeVentaMovil.Respuesta.Id == 295) // Atencion con otra figura
                    {
                        txtQuienVendiendo.Visibility = ViewStates.Visible;
                        SPQuienvende.Visibility = ViewStates.Visible;
                        txtNombreComercializador.Visibility = ViewStates.Visible;
                        NombreComercializadorinput.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        txtQuienVendiendo.Visibility = ViewStates.Gone;
                        SPQuienvende.Visibility = ViewStates.Gone;
                        txtNombreComercializador.Visibility = ViewStates.Gone;
                        NombreComercializadorinput.Visibility = ViewStates.Gone;
                    }

                    if (resultadoDeVentaMovil.Respuesta.Id == 190) // razon social
                    {
                        txtNoCteCompra.Visibility = ViewStates.Visible;
                        txtNombreCteCompra.Visibility = ViewStates.Visible;
                        NoCteComprainput.Visibility = ViewStates.Visible;
                        NombreCteComprainput.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        txtNoCteCompra.Visibility = ViewStates.Gone;
                        txtNombreCteCompra.Visibility = ViewStates.Gone;
                        NoCteComprainput.Visibility = ViewStates.Gone;
                        NombreCteComprainput.Visibility = ViewStates.Gone;
                    }

                    if (resultadoDeVentaMovil.Respuesta.Id == 266) // Inventario
                    {
                        txtFechaProximaCompra.Visibility = ViewStates.Visible;
                        FechaProximaCompra.Visibility = ViewStates.Visible;
                        //FechaProximaComprainput.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        txtFechaProximaCompra.Visibility = ViewStates.Gone;
                        FechaProximaCompra.Visibility = ViewStates.Gone;
                        //FechaProximaComprainput.Visibility = ViewStates.Gone;
                    }

                    if (resultadoDeVentaMovil.Respuesta.Id == 296) // Negocio
                    {
                        txtTipoCierre.Visibility = ViewStates.Visible;
                        SPTipoCierre.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        txtTipoCierre.Visibility = ViewStates.Gone;
                        SPTipoCierre.Visibility = ViewStates.Gone;
                    }
                   

                    // OnRespuestaSeleccionada(_resultadoDeVenta.Respuesta);
                };
                SPPrecio.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
                {
                    MotivoPrecio = Convert.ToString(resultadoPrecio.Valor[e.Position].Descripcion);
                };
                SPApoyos.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
                {
                    TipoApoyoCB = Convert.ToString(resultadoApoyos.Valor[e.Position].Descripcion);                   
                };
                SPFrecuencia.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
                {
                    FrecuenciaCompra = Convert.ToString(resultadoFrecuenciaCompra.Valor[e.Position].Descripcion);
                };
                SPDesharinizacion.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
                {
                    Desharinizacion = Convert.ToString(resultadoHarinizacion.Valor[e.Position].Descripcion);
                };
                SPQuienvende.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
                {
                    QuienAtiende = Convert.ToString(itemsGenericos[e.Position].Nombre);
                };
                SPTipoCierre.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
                {
                    TipoCierre = Convert.ToString(itemsNegocioCerrado[e.Position].Nombre);
                };

                //CantidadxTM = Convert.ToDecimal(CantidadXTM.Text);
                // SacosConsumeDiarios = Convert.ToDecimal(SacosConsumeDiario.Text);
                //VigenciaPromocionApoyo = VigenciaPromocion.Text;
                //Preciocompra = Convert.ToDecimal(PrecioCompra.Text);
                //Nombrecomercializador = NombreComercializador.Text;
                //NumeroCteCompra = NoCteCompra.Text;
                //NombreClienteCompra = NombreCteCompra.Text;
                //FechaProximacompra = FechaProximaCompra.Text;
                FechaProximaCompra.Click += FechaProximaCompra_Click;
                Enviar = FindViewById<Button>(Resource.Id.EnviarSeguimientoVenta);
                Enviar.Click += SeguimientoVentaFragment_Click;

                BtnRegresarAlMenu = FindViewById<Button>(Resource.Id.RegresaMenu);
                BtnRegresarAlMenu.Visibility = ViewStates.Invisible;

                BtnRegresarAlMenu.Click += BtnRegresarAlMenu_Click;
                
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Error: " + ex, ToastLength.Long).Show();
            }
        }

        [Obsolete]
        private void FechaProximaCompra_Click(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                FechaProximaCompraDisplay.Text = time.ToString("yyyy-MM-dd");
                FechaProximacompra = FechaProximaCompraDisplay.Text;

            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private async void BtnRegresarAlMenu_Click(object sender, EventArgs e)
        {
       
                //Bundle mybundle = new Bundle();
                //resultadoCliente = await proveedorDeClientes.ObtenerTodosPorRuta(new DataVendedor(vendedor.Valor.Id));
                //var clientes = resultadoCliente.Valor;
                //mybundle.PutStringArray(RutaFragment.LlaveDeRuta, ProveedorDeManipulacionDeDatos.GenerarTodos(clientes.OrderBy(e => e.Secuencia)));

                //FragmentTransaction fragmentTransaction = FragmentManager.BeginTransaction();
                //var myFragment = new RutaFragment();
                //myFragment.Arguments = mybundle;
                //fragmentTransaction.Replace(Resource.Id.content_frame,myFragment).Commit();

                //var activity = new Android.Content.Intent(this, typeof(ContenidoActivity));
                //activity.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(vendedor));
                ////activity.PutExtra(LlaveVentaCompleta, ProveedorDeSerializado.Generar(ClientesProspecto));
                //SetResult(Result.Ok, activity);
                //Finish();

                var myActivity = new Intent(this, typeof(ContenidoActivity));
                myActivity.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(vendedor));
                // LLave
                StartActivity(myActivity);          
        }
        //public async void ListItemClicked(int position)
        //{

        //    //var arguments = new Bundle();
        //    //var fragmentTransaction = this.FragmentManager.BeginTransaction();
        //    //fragmentTransaction.Replace(Resource.Id.content_frame, fragment).Commit();
        //    Bundle mybundle = new Bundle();
        //    resultadoCliente = await proveedorDeClientes.ObtenerTodosPorRuta(new DataVendedor(vendedor.Valor.Id));
        //    var clientes = resultadoCliente.Valor;            
        //    mybundle.PutStringArray(RutaFragment.LlaveDeRuta, ProveedorDeManipulacionDeDatos.GenerarTodos(clientes.OrderBy(e => e.Secuencia)));

        //    FragmentTransaction fragmentTransaction = FragmentManager.BeginTransaction();
        //    var myFragment = new RutaFragment();
        //    myFragment.Arguments = mybundle;
        //}
        private void SeguimientoVentaFragment_Click(object sender, EventArgs e)
        {
            // ========================================== Validaciones =======================================================
            if(seguimientoDeVenta.CompetenciaId == 0 || seguimientoDeVenta.CompetenciaId == null)
            {
                Toast.MakeText(this, "Debes seleccionar una harinera: ", ToastLength.Long).Show();
                return;
            }
            if (resultadoDeVentaMovil.Respuesta.Id == null || resultadoDeVentaMovil.Respuesta.Id == 0)
            {
                Toast.MakeText(this, "Debes seleccionar una causa de no venta: ", ToastLength.Long).Show();
                return;
            }

            if (resultadoDeVentaMovil.Respuesta.Id == 253)   // Precio 253
            {
                if(resultadoDeVentaMovil.Respuesta.Id == 253)
                {
                    if (MotivoPrecio == "Selecciona una opción")
                    {
                        Toast.MakeText(this, "Debes seleccionar un motivo", ToastLength.Long).Show();
                        return;
                    }
                }
                
                if(PrecioVentaSaco == null || PrecioVentaSaco == 0)
                {
                    Toast.MakeText(this, "Debes ingresar el precio de venta por saco", ToastLength.Long).Show();
                    return;
                }

               if (PromocionApoyoCompetencia)
               {
                        if(IdApoyo == "Selecciona una opción" || IdApoyo == null || IdApoyo == "")
                        {
                            Toast.MakeText(this, "Debes ingresar un tipo de promocion o apoyo", ToastLength.Long).Show();
                            return;
                        }

                    if (IdApoyo == "Insumo")
                    {
                        if (CantidadxTM == null || CantidadxTM == 0)
                        {
                            Toast.MakeText(this, "Debes ingresar la cantidad por TM", ToastLength.Long).Show();
                            return;
                        }
                        if (QueInsumoes == null || QueInsumoes == "")
                        {
                            Toast.MakeText(this, "Debes ingresar que el insumo", ToastLength.Long).Show();
                            return;
                        }
                    }
                    else if (IdApoyo == "Sacos")
                    {
                        if (QueInsumoes == null || QueInsumoes == "")
                        {
                            Toast.MakeText(this, "Debes ingresar que el insumo", ToastLength.Long).Show();
                            return;
                        }
                    }                                            
                }                
                if (VigenciaPromocionApoyo == null || VigenciaPromocionApoyo == "")
                {
                    Toast.MakeText(this, "Debes ingresar la vigencia", ToastLength.Long).Show();
                    return;
                }
               
                if (FrecuenciaCompra == "Selecciona una opción")
                {
                    Toast.MakeText(this, "Debes seleccionar una frecuencia de compra", ToastLength.Long).Show();
                    return;
                }

            }
            else if (resultadoDeVentaMovil.Respuesta.Id == 214) // Desharinizacion
            {
                if(Desharinizacion == "Selecciona una opción")
                {
                    Toast.MakeText(this, "Debes seleccionar una opcion desharinzación", ToastLength.Long).Show();
                    return;
                }
                if(Preciocompra == null || Preciocompra  == 0)
                {
                    Toast.MakeText(this, "Debes ingresa el precio de compra", ToastLength.Long).Show();
                    return;
                }
                if(SacosConsumeDiarios == null || SacosConsumeDiarios == 0)
                {
                    Toast.MakeText(this, "Debes ingresa cuantos sacos o kilos de masa consume diario", ToastLength.Long).Show();
                    return;
                }
            }else if(resultadoDeVentaMovil.Respuesta.Id == 198 ||
                        resultadoDeVentaMovil.Respuesta.Id == 287 || resultadoDeVentaMovil.Respuesta.Id == 196 ||
                        resultadoDeVentaMovil.Respuesta.Id == 201 || resultadoDeVentaMovil.Respuesta.Id == 288 ||
                        resultadoDeVentaMovil.Respuesta.Id == 202 || resultadoDeVentaMovil.Respuesta.Id == 195 ||
                        resultadoDeVentaMovil.Respuesta.Id == 290 || resultadoDeVentaMovil.Respuesta.Id == 197 ||
                        resultadoDeVentaMovil.Respuesta.Id == 199 || resultadoDeVentaMovil.Respuesta.Id == 192)
            {
                if (PrecioVentaSaco == null || PrecioVentaSaco == 0)
                {
                    Toast.MakeText(this, "Debes ingresar el precio de venta por saco", ToastLength.Long).Show();
                    return;
                }
                if (FrecuenciaCompra == "Selecciona una opción")
                {
                    Toast.MakeText(this, "Debes seleccionar una frecuencia de compra", ToastLength.Long).Show();
                    return;
                }
            }else if(resultadoDeVentaMovil.Respuesta.Id == 295) // Atencion con otra figura Minsa
            {
                if (QuienAtiende == "Selecciona una opción" || QuienAtiende == null || QuienAtiende == "")
                {
                    Toast.MakeText(this, "Debes seleccionar quien le esta atendiendo", ToastLength.Long).Show();
                    return;
                }
                if(Nombrecomercializador == null || Nombrecomercializador == "")
                {
                    Toast.MakeText(this, "Debes ingresar el nombre del comercializador.", ToastLength.Long).Show();
                    return;
                }

            }else if(resultadoDeVentaMovil.Respuesta.Id == 190)
            {
               if( NumeroCteCompra == null || NumeroCteCompra == "")
                {
                    Toast.MakeText(this, "Debes ingresar el nuevo numero de cliente con el que compra", ToastLength.Long).Show();
                    return;
                }
               if(NombreClienteCompra == null || NombreClienteCompra == "")
                {
                    Toast.MakeText(this, "Debes ingresar el nombre del nuevo cliente con el que compra", ToastLength.Long).Show();
                    return;
                }
            }else if(resultadoDeVentaMovil.Respuesta.Id == 266)
            {
                if(FechaProximacompra == null || FechaProximacompra == "")
                {
                    Toast.MakeText(this, "Debes ingresar la fecha de la proxima compra", ToastLength.Long).Show();
                    return;
                }
            }else if(resultadoDeVentaMovil.Respuesta.Id == 296)
            {
                if (TipoCierre == null || TipoCierre == "" || TipoCierre == "Selecciona un opción")
                {
                    Toast.MakeText(this, "Debes ingresar el tipo de cierre.", ToastLength.Long).Show();
                    return;
                }
            }

                //=================================================================================================================//
           if (seguimientoDeVenta.CompetenciaId != 0)
            {
                var info = Task.Run(async () => {
                    result1 = await proveedorDeEstrategia.RegistrarSeguimientoDeVenta(seguimientoDeVenta);
                });
                info.Wait();
                if (result1.Tipo == TipoDeResultado.Exito)
                {
                    Enviar.Enabled = false;
                    RegresarAlMenu = true;
                    Toast.MakeText(this, result1.Valor.Respuesta, ToastLength.Long).Show();                    
                }
                else
                {
                    Toast.MakeText(this, result1.Mensaje, ToastLength.Long).Show();                   
                }
            }
            else
            {
                Toast.MakeText(this, "Ocurrio un problema con el cuestionario. ", ToastLength.Long).Show();           
            }
            //================================================== Add Causas No Venta ============================================//
                AddCausasNoVenta addCausasNoVenta = new AddCausasNoVenta();

                addCausasNoVenta.ClienteId = ProveedorGlobal.Cliente.ClienteId;
                addCausasNoVenta.VendedorId = vendedor.Valor.Id; // vendedor.Valor.Usuario;
                addCausasNoVenta.Usuario = vendedor.Valor.Usuario;
                addCausasNoVenta.PreguntaId = resultadoDeVentaMovil.Respuesta.Id;
                addCausasNoVenta.Pregunta = "NoVenta";
                addCausasNoVenta.Causa = resultadoDeVentaMovil.Respuesta.Texto;
                addCausasNoVenta.Motivo = MotivoPrecio;
                addCausasNoVenta.PrecioVentaxSaco = PrecioVentaSaco;
                addCausasNoVenta.PromocionApoyoCompetencia = PromocionApoyoCompetencia;
                addCausasNoVenta.PromocionApoyoCB = IdApoyo;
                addCausasNoVenta.Insumo = QueInsumoes;  
                addCausasNoVenta.CantidadXTM = CantidadxTM;
                addCausasNoVenta.TipoApoyoCB = TipoApoyoCB;
                addCausasNoVenta.VigenciaPromocionApoyo = VigenciaPromocionApoyo;
                addCausasNoVenta.SacosDiario = SacosConsumeDiarios;
                addCausasNoVenta.FrecuenciaCompra = FrecuenciaCompra;
                addCausasNoVenta.Desharinizacion = Desharinizacion;
                addCausasNoVenta.PrecioCompra = Preciocompra;
                addCausasNoVenta.SacosKilosMasaConsume = SacosConsumeDiarios;
                addCausasNoVenta.QuienAtiende = QuienAtiende;
                addCausasNoVenta.NombreComercializador = Nombrecomercializador;
                addCausasNoVenta.NuevoNoCteCompra = NumeroCteCompra;
                addCausasNoVenta.NombreCteCompra = NombreClienteCompra;
                addCausasNoVenta.FechaProximaCompra = FechaProximacompra;
                addCausasNoVenta.TipoCierre = TipoCierre;

                var execAddCausasNoVenta = Task.Run(async () => {
                    resultadoAddCausasNoVenta = await proveedorDeEstrategia.InsertAddCausasNoVenta(addCausasNoVenta);
                });
                execAddCausasNoVenta.Wait();
                if (resultadoAddCausasNoVenta.Tipo == TipoDeResultado.Fallo)
                {
                    Toast.MakeText(this, resultadoAddCausasNoVenta.Mensaje, ToastLength.Long).Show();
                }

            //=================================================================================================================//
            var exec = Task.Run(async () => {
                resultadoProcesaVenta2 = await proveedorDeEstrategia.RegistrarResultadoDeVenta(resultadoDeVenta);
            });
            exec.Wait();

            if (resultadoProcesaVenta2.Tipo == TipoDeResultado.Exito)
            {
                //EnviarSiNo.Enabled = false;
                Toast.MakeText(this, resultadoProcesaVenta2.Mensaje, ToastLength.Long).Show();
                
            }
            else
            {
                Toast.MakeText(this, resultadoProcesaVenta2.Mensaje, ToastLength.Long).Show();
            }

            //=================================================================================================================//
            if (RegresarAlMenu)
            {
                BtnRegresarAlMenu.Visibility = ViewStates.Visible;
            }
        }

        //private void ReciboValoresActivity_Click(object sender, EventArgs e)
        //{
        //    var activity = new Android.Content.Intent(this, typeof(ContenidoActivity));
        //    activity.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(vendedor));
        //    //activity.PutExtra(LlaveVentaCompleta, ProveedorDeSerializado.Generar(ClientesProspecto));
        //    SetResult(Result.Ok, activity);
        //    Finish();
        //}
        public List<ItemGenerico> CargaQuienVendio()
        {
            quienVendio = new List<ItemGenerico>();
            quienVendio.Add(new ItemGenerico
            {
                Id = 0,
                Nombre = "Selecciona una opción"
            });
            quienVendio.Add(new ItemGenerico
            {
                Id = 1,
                Nombre = "IMT"
            });
            quienVendio.Add(new ItemGenerico
            {
                Id = 2,
                Nombre = "Comercializador"
            });
            quienVendio.Add(new ItemGenerico
            {
                Id = 3,
                Nombre = "Revendedor"
            });
            return quienVendio;

        }
        public List<ItemGenerico> CargaApoyoCompetencia()
        {
            ApoyoDieron = new List<ItemGenerico>();
            ApoyoDieron.Add(new ItemGenerico
            {
                Id = 0,
                Nombre = "Selecciona una opción"
            });
            ApoyoDieron.Add(new ItemGenerico
            {
                Id = 1,
                Nombre = "Insumo"
            });
            ApoyoDieron.Add(new ItemGenerico
            {
                Id = 2,
                Nombre = "Sacos"
            });
            ApoyoDieron.Add(new ItemGenerico
            {
                Id = 3,
                Nombre = "Apoyo"
            });
            return ApoyoDieron;

        }
        public List<ItemGenerico> CargaNegocioCerrado()
        {
            itemsNegocioCerrado = new List<ItemGenerico>();
            itemsNegocioCerrado.Add(new ItemGenerico
            {
                Id = 0,
                Nombre = "Selecciona un opción"
            });
            itemsNegocioCerrado.Add(new ItemGenerico
            {
                Id = 1,
                Nombre = "Definitivo"
            });
            itemsNegocioCerrado.Add(new ItemGenerico
            {
                Id = 2,
                Nombre = "Temporal"
            });
           
            return itemsNegocioCerrado;

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
        public override void OnBackPressed()
        {
            var myActivity = new Intent(this, typeof(SeguimientoVentaActivity));
            myActivity.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(vendedor));
           // LLave
            StartActivity(myActivity);
        }
    }
}