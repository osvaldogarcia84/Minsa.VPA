using Android.Locations;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Grantland.Widget;
using Minsa.VPA.Activities;
using Minsa.VPA.Adaptadores;
using Minsa.VPA.Modelos;
using Minsa.VPA.Proveedores;
using Minsa.VPA.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace Minsa.VPA.Fragments
{
    public class CarritoFragment : ListFragment
    {        
        ProveedorDeClientes proveedorDeCliente = new ProveedorDeClientes();
        public ResultadoDeOperacionGenerico<Vendedor> vendedor;
        public ResultadoDeOperacionGenerico<List<InformacionDeVenta>> informacionDeVenta;
        public ResultadoDeOperacionGenerico<List<MetodoDePago>> metodoDePago;
        private readonly List<ArticuloMovil> _articulosDeVista = new List<ArticuloMovil>();
        public const string LlaveDeCarrito = "Carrito";
        private AdaptadorSpinnerArticulos adapter;
        public AdaptadorMetodoDePago adapterMetodo;
        private string FechaActual;
        private Spinner sp;
        private Spinner MetodoSP;
        public LineaMovil lineaMovil;
        public TextView PrecioArticulo;
        public string FormaDePago;
        public Precio txtprecio;
        public List<LineaMovil> _lineas = new List<LineaMovil>();
        public decimal ArticuloaValidar;
        public decimal CantidadIngresadaaValidar;
        Button btnComprar;
        ProveedorDeVenta proveedorDeVenta = new ProveedorDeVenta();        
        Cliente cliente;
        TextView txtFacturaResultado;
        TextView txtOrdenResultado;
        public TextView Descuento;
        public const string InformacionDeTipoDeOrden = "VPA";
        public string Almacen = String.Empty;
        private ProveedorDeLocacion proveedorDeLocacion;
        Button agregar;
        ResultadoDeOperacionGenerico<string> OrdenId;
        ResultadoDeOperacionGenerico<string> ResultadoDeLinea;
        ResultadoDeOperacionGenerico<string> ObtenerFactura;
        AutofitTextView NombreDeCliente;
        decimal Latitud, Longitud;
        public decimal DescuentoXOrden;
        decimal SumaCantidadAdd;
        ResultadoDeOperacionGenerico<PromocionBultos> ResultadoPromocionBultosAdd;
        ResultadoDeOperacionGenerico<InformacionDeInventario> ArticuloSeleccionado;
        ProveedorDeEstrategia proveedorDeEstrategia = new ProveedorDeEstrategia();
        decimal OperacionPromoAdd;
        double ConversionToneladaAdd = 0.02;
        decimal DescuentoTotalAdd;
        ResultadoDeOperacionGenerico<ArticulosSinDescuentos> resultadoArticulosSinDescuento;
        ResultadoDeOperacionGenerico<PromocionBultos> ResultadoPromocionBultos;
        decimal SumaCantidad = 0;
        decimal Impuesto = 0;
        decimal OperacionPromo = 0;        
        decimal DescuentoTotal = 0;
        decimal CantidadCredito = 0;
        decimal CantidadCreditoNLineas = 0;
        decimal SumaLineasCredito = 0;
        Timer timer;
        string MetodoDePago;
        ResultadoDeOperacionGenerico<ValidaCreditoCtes> resultadoValidaCred;
        string FormaPago = String.Empty;
        public decimal Cantidad
        {
            get { return View.FindViewById<EditText>(Resource.Id.Cantidad).Text.ObtenerCantidad(); }
            set { View.FindViewById<EditText>(Resource.Id.Cantidad).Text = value.ToString("##,###.##"); }
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            cliente =  ProveedorGlobal.Cliente;
            vendedor = ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Arguments.Obtener<string>(MainActivity.LlaveUsuario));             
          
            try
            {
                //OperacionPromo = 0;
                ProveedorGlobal.OrdenId = null;
                ProveedorGlobal.DescuentoTotalView = 0;
                ProveedorGlobal.DescuentoXOrden = 0;
                //proveedorDeLocacion = new ProveedorDeLocacion(Activity, 15); // 900000 15 minutos
                //proveedorDeLocacion.LocacionEncontrada += ActualizarLocacion;
                // ProveedorGlobal.Lan = new Lan(0, 0);

                var info = Task.Run(async () => {
                    informacionDeVenta = await proveedorDeCliente.ObtenerInformacionDeVenta(new DataVendedor(vendedor.Valor.Id));
                });
                info.Wait();


                Almacen = vendedor.Valor.Almacen;
                if (informacionDeVenta.Tipo == TipoDeResultado.Exito)
                {

                    foreach(var articuloMovil in informacionDeVenta.Valor)
                    {
                            _articulosDeVista.Add(new ArticuloMovil(articuloMovil.ArticuloId, articuloMovil.NombreArticulo, articuloMovil.Monto, articuloMovil.Sitio,
                                                                    articuloMovil.Grupo, Activity, articuloMovil.Impuesto));
                    }                 
                }
                var metodo = Task.Run(async () => {
                    metodoDePago = await proveedorDeCliente.MetodoDePago();
                });
                metodo.Wait();
                
                if(metodoDePago.Tipo != TipoDeResultado.Exito)
                    Snackbar.Make(View, "Ocurrio un error comunicate con el departamento de Sistemas", Snackbar.LengthLong)
                      .Show();

            }
            catch(Exception ex)
            {
                Activity.MostrarMensaje(ex.Message);
            }
        }
        public static CarritoFragment NewInstance()
        {
            var frag1 = new CarritoFragment { Arguments = new Bundle() };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
           View view;         
           var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            
            FechaActual = DateTime.Now.ToString(InformacionGeneral.FormatoDeFecha);

            if(_articulosDeVista != null)
            {
               view = ConfigurarVista(inflater, container);
            }
            else
            {
                view = inflater.Inflate(Resource.Layout.Error, container, false);
                view.FindViewById<TextView>(Resource.Id.ErrorTexto).Text =
                   "No tienes cargada tu lista de articulos comunicate con el departamento de INE.";
            }
           
            return view;
        }
        //private void ActualizarLocacion(object sender, Location location)
        //{
        //    try
        //    {
        //        ProveedorGlobal.Lan = new Lan(location.Longitude, location.Latitude);
        //        if(ProveedorGlobal.Lan == null)
        //        {
        //            ProveedorGlobal.Lan = new Lan(0, 0);
        //        }

        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        private View ConfigurarVista(LayoutInflater inflater, ViewGroup container)
        {

             var view = inflater.Inflate(Resource.Layout.Carrito, null);
            Descuento = view.FindViewById<TextView>(Resource.Id.Descuento);
            MetodoSP = view.FindViewById<Spinner>(Resource.Id.FormasDePago);
            adapterMetodo = new AdaptadorMetodoDePago(Activity, metodoDePago.Valor.ToList());
            MetodoSP.Adapter = adapterMetodo;
            MetodoSP.ItemSelected += MetodoSP_ItemSelected;

            sp = view.FindViewById<Spinner>(Resource.Id.sp);
            adapter = new AdaptadorSpinnerArticulos(Activity, _articulosDeVista, this);
            sp.Adapter = adapter;
            sp.ItemSelected += Sp_ItemSelected;

            PrecioArticulo = view.FindViewById<TextView>(Resource.Id.PrecioArticulo);

           

            agregar = view.FindViewById<Button>(Resource.Id.Agregar);
            agregar.Click += Agregar_Click;

            btnComprar = view.FindViewById<Button>(Resource.Id.Comprar);
            btnComprar.Enabled = false;
            btnComprar.Click += Comprar_Click;

            txtOrdenResultado = view.FindViewById<TextView>(Resource.Id.OrdenResultado);
            //txtOrdenResultado.Visibility = ViewStates.Invisible;

            txtFacturaResultado = view.FindViewById<TextView>(Resource.Id.FacturaResultado);
            //txtFacturaResultado.Visibility = ViewStates.Invisible;

            NombreDeCliente = view.FindViewById<AutofitTextView>(Resource.Id.NombreCliente);
            NombreDeCliente.Text = cliente.ClienteId+  " - "  + cliente.Nombre;

            if (cliente.MicroCredito == true)
            {
                MetodoSP.Enabled = false;
            }

            //Descuento.Text = "";
            return view;
        }
        private void MetodoSP_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                FormaDePago = metodoDePago.Valor[e.Position].PAYMMODE;
            }
            catch(Exception ex)
            {
                Activity.MostrarMensaje("Ocurrio un error" + ex, ToastLength.Long);
            }
        }

        private void Sp_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {

                
                PrecioArticulo.Text = Convert.ToString(String.Format("{0:C}", _articulosDeVista[e.Position].Monto != 0 ? _articulosDeVista[e.Position].Monto : 0));
                lineaMovil = new LineaMovil(_articulosDeVista[e.Position].ArticuloId,
                                            _articulosDeVista[e.Position].NombreArticulo,
                                             Cantidad,
                                             _articulosDeVista[e.Position].Monto,
                                            0,
                                            0,
                                            vendedor.Valor.Almacen,
                                            FechaActual,
                                            FechaActual,
                                            "",
                                            FormaDePago != null ? FormaDePago : "01",
                                            vendedor.Valor.Id,
                                            //Convert.ToDecimal(ProveedorGlobal.Lan != 0 ? ProveedorGlobal.Lan.Longitud : 0),
                                            //Convert.ToDecimal(ProveedorGlobal.Lan != 0 ? ProveedorGlobal.Lan.Latitud : 0),
                                            Convert.ToDecimal(0),
                                            Convert.ToDecimal(0),
                                            _articulosDeVista[e.Position].Impuesto,
                                            _articulosDeVista[e.Position].Grupo,
                                            Activity                                           
                                            );
            }
            catch (Exception ex)
            {
                Activity.MostrarMensaje("Ocurrio un error: " + ex, ToastLength.Long);
            }

        }
        private async void Agregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lineaMovil == null)
                {
                    Snackbar.Make(View, "No haz seleccionado un producto.", Snackbar.LengthLong)
                    .Show();
                    return;
                }
                if (Cantidad <= 0)
                {                   
                    Snackbar.Make(View, "La cantidad debe ser mayor a 0.", Snackbar.LengthLong)
                   .Show();
                    return;
                }

                //========================================================== Consulta de inventario =============================================================//

                ArticuloSeleccionado = await proveedorDeCliente.ConsultaDisponiblePorArticulo(new DisponiblePorArticulo(lineaMovil.AlmacenId, lineaMovil.ArticuloId));

                if (ArticuloSeleccionado.Tipo == TipoDeResultado.Exito)
                    ArticuloaValidar = Convert.ToDecimal(ArticuloSeleccionado.Valor.DisponiblePorSaco);
                else
                    ArticuloaValidar = 0;

                if (Cantidad > ArticuloaValidar)
                {
                    Snackbar.Make(View, "Productos solicitados: " + Cantidad + "\nProductos disponibles: " + ArticuloaValidar + "\nEstas excediendo tu inventario.", Snackbar.LengthLong)
                  .Show();
                    return;
                }
                //========================================================== Agregar Lineas al carrito ========================================================//

                var validarSiExiste = _lineas.Where(t => t.ArticuloId == lineaMovil.ArticuloId).FirstOrDefault();
                ///=========================================================  1 Linea =======================================================================//
                if(validarSiExiste == null)
                {
                    if (Cantidad != 0)
                    {                        

                        lineaMovil.Cantidad = Cantidad;
                        if (cliente.MicroCredito == true)
                        {
                            lineaMovil.ReferenciaDeCompra = "99";
                            FormaDePago = "99";
                        }
                        else
                        {
                            lineaMovil.ReferenciaDeCompra = FormaDePago;
                        }
                            

                        if (ProveedorGlobal.Lan != null)
                        {
                            lineaMovil.CoordenadaX = Convert.ToDecimal(ProveedorGlobal.Lan.Longitud);
                            lineaMovil.CoordenadaY = Convert.ToDecimal(ProveedorGlobal.Lan.Latitud);
                        }
                        else
                        {
                            lineaMovil.CoordenadaX = 1;
                            lineaMovil.CoordenadaY = 1;
                        }                   
                        _lineas.Add(lineaMovil);     
                        // ======================================================= Descuento x bulto =======================================================================//
                        Promocion(cliente, _lineas);
                        // ======================================================== Micro Creditos ========================================================================//                        
                        Cantidad = 0;
                        if (cliente.MicroCredito == true)
                        {
                           
                            var respuesta = MicroCreditosMinsa(_lineas);
                            if (respuesta.Resultado == "ExcedeCredito")
                            {
                                new Android.Support.V7.App.AlertDialog.Builder(View.Context)
                               .SetTitle(View.Context.GetString(Resource.String.main_dialog_simple_title))
                               .SetMessage("El monto de la venta es: $" + respuesta.Monto.ToString("##,###.##") + ".\nEs mayor a su limite de credito: $" + resultadoValidaCred.Valor.Saldo.ToString("##,###.##") +
                                           ".\nExcede su credito.\nNota: Tu cliente pertence al segmento de MICROCREDITOS MINSA")
                                   .SetPositiveButton(View.Context.GetString(Resource.String.dialog_ok), (sender, args) =>
                                   {
                                       try
                                       {
                                           _lineas.Clear();
                                          // MetodoSP.Enabled = true;
                                           return;
                                       }
                                       catch (Exception ex)
                                       {
                                           Snackbar.Make(View, "Ocurrio un problema: " + ex, Snackbar.LengthLong)
                                          .Show();
                                       }
                                   }).Show();
                                return;
                            }
                        }
                        
                        // ===============================================================================================================================================
                        ListAdapter = new LineasAdapter(Activity, _lineas, this, cliente, vendedor.Valor);
                        ((BaseAdapter)ListAdapter).NotifyDataSetChanged();
                        CalcularTamaño();
                        //Cantidad = 0;
                        Snackbar.Make(View, "Articulo agregado correctamente.\n" + lineaMovil.ArticuloNombre, Snackbar.LengthLong)
                          .Show();
                        btnComprar.Enabled = true;
                    }
                    else
                    {
                        Snackbar.Make(View, "Debes seleccionar una fecha y una cantidad", Snackbar.LengthLong)
                            .Show();
                        return;
                    }
                }
                else
                {
                    // SumaCantidadAdd = 0;
                  
                    var CantidadaLineaObtenida = _lineas.Where(r => r.ArticuloId == lineaMovil.ArticuloId).Select(n => n.Cantidad).FirstOrDefault();
                     CantidadIngresadaaValidar = Cantidad + CantidadaLineaObtenida;
                    if (CantidadIngresadaaValidar > ArticuloaValidar)
                    {
                        Snackbar.Make(View, "Productos solicitados: " + CantidadIngresadaaValidar + "\nProductos disponibles: " + ArticuloaValidar + "\nEstas excediendo tu inventario.", Snackbar.LengthLong)
                        .Show();
                        return;
                    }
                    else
                    {
                        string IdArticuloActualizado = String.Empty;
                        string ArticuloActualizado = String.Empty;
                        for (var x = 0; x < _lineas.Count; x++)
                        {

                            if (_lineas[x].ArticuloId == lineaMovil.ArticuloId)
                            {
                                _lineas[x].Cantidad = _lineas[x].Cantidad + Cantidad;
                                ArticuloActualizado = _lineas[x].ArticuloNombre;
                                IdArticuloActualizado = _lineas[x].ArticuloId;
                                SumaCantidadAdd = SumaCantidadAdd + _lineas[x].Cantidad;
                            }                             
                        }
                        CantidadCreditoNLineas = Cantidad;
                       // ======================================================= Descuento x bulto =======================================================================//
                        Promocion(cliente, _lineas);
                        // ======================================================== Micro Creditos ========================================================================//                        
                        if (cliente.MicroCredito == true)
                        {
                            var respuesta = MicroCreditosMinsa(_lineas);
                            if (respuesta.Resultado == "ExcedeCredito")
                            {
                                new Android.Support.V7.App.AlertDialog.Builder(View.Context)
                               .SetTitle(View.Context.GetString(Resource.String.main_dialog_simple_title))
                               .SetMessage("El monto de la venta es: $" + respuesta.Monto.ToString("##,###.##") + ".\nEs mayor a su limite de credito: $" + resultadoValidaCred.Valor.Saldo.ToString("##,###.##") +
                                           ".\nExcede su credito.\nNota: Tu cliente pertence al segmento de MICROCREDITOS MINSA")
                                   .SetPositiveButton(View.Context.GetString(Resource.String.dialog_ok), (sender, args) =>
                                   {
                                       try
                                       {
                                           //  _lineas.Clear();
                                           for (var x = 0; x < _lineas.Count; x++)
                                           {

                                               if (_lineas[x].ArticuloId == lineaMovil.ArticuloId)
                                               {
                                                   _lineas[x].Cantidad = _lineas[x].Cantidad - Cantidad;
                                               }
                                           }
                                           Cantidad = 0;
                                           return;
                                       }
                                       catch (Exception ex)
                                       {
                                           Snackbar.Make(View, "Ocurrio un problema: " + ex, Snackbar.LengthLong)
                                          .Show();
                                       }
                                   }).Show();
                                return;
                            }
                        }
                        // =====================================================================================================================================//                              
                        ListAdapter = new LineasAdapter(Activity, _lineas, this, cliente, vendedor.Valor);
                        ((BaseAdapter)ListAdapter).NotifyDataSetChanged();
                        Cantidad = 0;
                        Snackbar.Make(View, "Articulo actualizado correctamente.\n" + ArticuloActualizado, Snackbar.LengthLong)
                            .Show();
                    }
                }
                Descuento.Text = "Descuento: " + Convert.ToString(ProveedorGlobal.DescuentoTotalView);
            }
            catch (Exception ex)
            {
                Activity.MostrarMensaje("Ocurrio un error: " + ex, ToastLength.Long);
            }
        }

        private void Comprar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_lineas.Count == 0)
                {
                    Snackbar.Make(View, "Tu carrito esta vacio, selecciona un producto.", Snackbar.LengthLong)
                       .Show();
                    return;
                }
                // ======================================================== Valida insumos  ======================================================================//
                    
                // =================================================================================================================================================//    
                Activity.RunOnUiThread(() =>
                {
                    Snackbar.Make(View, "Procesando tu orden... Espera porfavor", Snackbar.LengthLong)
                    .Show();
                });
                DeshabilitarVista();

                if (ProveedorGlobal.Lan == null)
                {
                    Latitud = 1;
                    Longitud = 1;
                }
                else
                {
                    Latitud = Convert.ToDecimal(ProveedorGlobal.Lan.Latitud);
                    Longitud = Convert.ToDecimal(ProveedorGlobal.Lan.Longitud);
                }
                if (cliente.MicroCredito == true)
                {
                    FormaPago = "PPD";
                    MetodoDePago = "PPD";
                }
                else
                {
                    FormaPago = "CONTADO";
                    MetodoDePago = "PUE";
                }

                    // DescuentoXOrden = ProveedorGlobal.DescuentoXOrden ? null : 0 
                    Activity.RunOnUiThread(async () =>
                {
                    OrdenId = await proveedorDeVenta.CrearOrdenVPA(new Orden(cliente.ClienteId, InformacionDeTipoDeOrden, InformacionGeneral.Compañia ,vendedor.Valor.Id, QuitAccents(vendedor.Valor.Nombre), Longitud,
                                                                             Latitud, FormaPago, "", ""));
                    if (OrdenId.Tipo == TipoDeResultado.Exito)
                    {
                        //==== Poner textview con la factura                  
                        txtOrdenResultado.Text =  OrdenId.Valor;
                        foreach (var lineasProcesar in _lineas)
                        {
                            var linea = new Linea(OrdenId.Valor, vendedor.Valor.Usuario, Almacen, lineasProcesar.ArticuloId,
                                                  lineasProcesar.FechaDeEnvio, lineasProcesar.FechaDeEntrega, lineasProcesar.Cantidad, "", "", 0, "", "", MetodoDePago, Math.Abs(ProveedorGlobal.DescuentoXOrden), FormaDePago);
                            ResultadoDeLinea =  await proveedorDeVenta.CrearLineaVPA(linea);
                            if (ResultadoDeLinea.Tipo == TipoDeResultado.Exito)
                            {
                                Snackbar.Make(View, "Registrando líneas.", Snackbar.LengthLong)
                                .Show();
                            }
                            else
                            {
                                Snackbar.Make(View, "Ocurrio un problema al generar la lineas", Snackbar.LengthLong)
                               .Show();
                            }
                        }
                        var Factura = new Factura(OrdenId.Valor);
                        Snackbar.Make(View, "Registrando factura.", Snackbar.LengthLong)
                               .Show();
                        ObtenerFactura = await proveedorDeVenta.ObtenerFactura(Factura);
                        //    txtFacturaResultado.Visibility = ViewStates.Visible;
                        if (ObtenerFactura.Tipo == TipoDeResultado.Exito)
                        {

                            txtFacturaResultado.Text = "Factura: " + ObtenerFactura.Valor;
                            var factura = ObtenerFactura.Valor.Substring(0,1);
                            if (factura == "F")
                            {
                                ProveedorGlobal.OrdenId = ObtenerFactura.Valor;
                                if (FormaDePago == "01" || FormaDePago == "02" || FormaDePago == "99")
                                {
                                    var activity = new Android.Content.Intent(Activity, typeof(ReciboValoresImpresionActivity));
                                    activity.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(vendedor));
                                    //   activity.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(CarritoFragment.LlaveDeCarrito));
                                    activity.PutExtra(ReciboValoresImpresionActivity.LlaveFactura, ProveedorDeSerializado.Generar(ObtenerFactura.Valor));
                                    activity.PutExtra(ReciboValoresImpresionActivity.LlaveFormaDePago, ProveedorDeSerializado.Generar(FormaDePago));
                                    //   activity.PutExtra(ReciboValoresActivity.LlaveSitio, ProveedorDeSerializado.Generar(Sitio));
                                    StartActivity(activity);

                                }
                                else
                                {
                                    Snackbar.Make(View, "Los tickets que se puede imprimir son pago en efectivo, cheque nominativo o sin definir", Snackbar.LengthLong).Show();
                                }
                            }
                            
                        }
                        else
                        {
                            txtFacturaResultado.Text = "Ocurrio un problema al Facturar";
                        }
                    }
                    else
                    {
                        txtOrdenResultado.Text = "Ocurrio un problema al generar la orden";
                        Snackbar.Make(View, "No se genero la OC contacta a un administrador", Snackbar.LengthLong)
                        .Show();
                    }
                });
            }
            catch (Exception ex)
            {
                Activity.RunOnUiThread(() =>
                {
                    Activity.MostrarMensaje(String.Format("Error en el servidor: {0}",
                   ex.Message));
                });
            }
        }

        public string QuitAccents(string inputString)
        {
            Regex a = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            Regex e = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            Regex i = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            Regex o = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            Regex u = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            Regex n = new Regex("[ñ|Ñ]", RegexOptions.Compiled);
            inputString = a.Replace(inputString, "a");
            inputString = e.Replace(inputString, "e");
            inputString = i.Replace(inputString, "i");
            inputString = o.Replace(inputString, "o");
            inputString = u.Replace(inputString, "u");
            inputString = n.Replace(inputString, "n");
            return inputString;
        }
        public void DeshabilitarVista()
        {
            btnComprar.Enabled = false;
            agregar.Enabled = false;
            sp.Enabled = false;
            //MetodoSP.Enabled = false;
            //LineasAdapter.ServiceViewHolder delete = new LineasAdapter.ServiceViewHolder();
            //delete.DeleteButton.Enabled = false;
        }
        public void CalcularTamaño()
        {
            if (!_lineas.Any() || View == null)
                return;
            View item = ListAdapter.GetView(0, null, ListView);
            item.Measure(0, 0);
            var size = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent,
                item.MeasuredHeight + 70);
            ListView.LayoutParameters = size;
        }
        //public void Impuestos(List<LineaMovil> lineasImpuestos)
        //{
        //    Impuesto = 0;
        //    foreach(var LineaImpuestos in lineasImpuestos)
        //    {
        //        Impuesto = (LineaImpuestos.PrecioNeto * LineaImpuestos.Cantidad) * LineaImpuestos.Impuesto;

        //    }
        //}
        public void Promocion(Cliente cliente, List<LineaMovil> lineasPromocion)
        {

            SumaCantidad = 0;
            foreach (var lineapromocion in lineasPromocion)
            {               
                var ArtiSinDescuento = Task.Run(async () =>
                {
                    resultadoArticulosSinDescuento = await proveedorDeEstrategia.ArticulosSinDescuentos(new DataArticulosSinDescuentos(lineapromocion.ArticuloId));
                });
                ArtiSinDescuento.Wait();

                if (resultadoArticulosSinDescuento.Valor == null)
                {
                    SumaCantidad = SumaCantidad + lineapromocion.Cantidad;
                }
                var removeDescuento = Task.Run(async () =>
                {
                    ResultadoPromocionBultos = await proveedorDeEstrategia.PromocionBultos(new DataPromocionBultos(cliente.ClienteId, SumaCantidad,vendedor.Valor.Almacen, vendedor.Valor.Usuario));
                });
                removeDescuento.Wait();
                if (ResultadoPromocionBultos.Tipo == TipoDeResultado.Exito)
                {

                    OperacionPromo = ResultadoPromocionBultos.Valor.Descuento;
                    DescuentoTotal = OperacionPromo * (SumaCantidad * Convert.ToDecimal(InformacionGeneral.ConversionTonelada));
                    ProveedorGlobal.DescuentoTotalView = DescuentoTotal;
                    ProveedorGlobal.DescuentoXOrden = ResultadoPromocionBultos.Valor.Descuento;
                }
                else
                {
                    DescuentoTotal = 0;
                    ProveedorGlobal.DescuentoTotalView = 0;
                    ProveedorGlobal.DescuentoXOrden = 0;
                }
                //if (ResultadoPromocionBultos.Tipo == TipoDeResultado.Exito)
                //{   
                //        OperacionPromo = ResultadoPromocionBultos.Valor.Descuento;
                //        if (lineapromocion.GrupoArticulo == "109" || lineapromocion.GrupoArticulo == "110")
                //        {
                //            DescuentoTotal = OperacionPromo * (SumaCantidad * Convert.ToDecimal(InformacionGeneral.ConversionTonelada));
                //        }
                //        else if (lineapromocion.GrupoArticulo == "101")
                //        {
                //            DescuentoTotal = OperacionPromo * (SumaCantidad * Convert.ToDecimal(InformacionGeneral.Sacos25KG));
                //        }
                //        else
                //        {
                //            DescuentoTotal = OperacionPromo * (SumaCantidad * Convert.ToDecimal(InformacionGeneral.Pieza));
                //        }
                //        ProveedorGlobal.DescuentoTotalView = DescuentoTotal;
                //        ProveedorGlobal.DescuentoXOrden = ResultadoPromocionBultos.Valor.Descuento;                                      
                //}
                //else
                //{
                //    DescuentoTotal = 0;
                //    ProveedorGlobal.DescuentoTotalView = 0;
                //    ProveedorGlobal.DescuentoXOrden = 0;
                //}

            }
        }
        public CalculoMicroCreditos MicroCreditosMinsa(List<LineaMovil> lineasCredito)
        {
            CalculoMicroCreditos calculo = new CalculoMicroCreditos();
            SumaLineasCredito = 0;
            if (cliente.MicroCredito == true)
            {
                var validaCtes = Task.Run(async () => {
                    resultadoValidaCred = await proveedorDeCliente.ValidaCreditoCtes(new DataCreditoCtes(Configuracion.Compania, cliente.ClienteId));
                });
                validaCtes.Wait();

                if (resultadoValidaCred.Tipo == TipoDeResultado.Exito)
                {
                    foreach(var lineacredito in lineasCredito)
                    {
                        SumaLineasCredito = SumaLineasCredito + lineacredito.Cantidad;
                    }
                    var precioArticulo = Convert.ToDecimal(ArticuloSeleccionado.Valor.PrecioPorSaco);
                    var Monto = (SumaLineasCredito * precioArticulo) - OperacionPromo;
                    calculo.Monto = Monto;
                    if (resultadoValidaCred.Valor.Saldo < Monto)
                    {
                        calculo.Resultado = "ExcedeCredito";

                    }                
                }
                else
                {
                    calculo.Resultado = resultadoValidaCred.Mensaje;
                }             
            }
            return calculo;
        }
        public static bool IsWithinTime(string stringNowTime, string stringStartTime, string stringEndTime)
        {

            var nowTime = DateTime.Parse(stringNowTime);
            var startTime = DateTime.Parse(stringStartTime);
            var endTime = DateTime.Parse(stringEndTime);

            if ((nowTime <= endTime) && (nowTime >= startTime))
            {
                return true;
            }

            return false;
        }
        public override void OnResume()
        {
            base.OnResume();

            if (timer != null)
            {
                timer.Elapsed -= OnTimerElapsed;
                timer.Enabled = false;
                timer.Stop();
            }
        }

        public override void OnPause()
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