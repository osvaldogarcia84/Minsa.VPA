using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
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
using System.Threading.Tasks;
using System.Timers;
namespace Minsa.VPA.Fragments
{
    [Obsolete]
    public class YALOFragment : Fragment
    {
        ProveedorDeYalo proveedorDeYalo = new ProveedorDeYalo();
        public ResultadoDeOperacionGenerico<Vendedor> vendedor;
        ResultadoDeOperacionGenerico<List<OrdenesYalo>> OrdenesYalo;
        public OrdenesYaloAdapter adapter;
        public ExpandableListView expandableListView;
        Cliente cliente;        
        public static string LlaveYALO = "LlaveYALO";       
        ProveedorDeVenta proveedorDeVenta = new ProveedorDeVenta();
        ResultadoDeOperacionGenerico<string> ObtenerFactura;
        ResultadoDeOperacionGenerico<InformacionDeInventario> articulosDisponibles;
        ProveedorDeClientes proveedorDeCliente = new ProveedorDeClientes();
        List<OrdenesYaloMovil> ordenesMovil = new List<OrdenesYaloMovil>();
        ResultadoDeOperacionGenerico<List<MetodoDePago>> metodoDePagoYalo;
        Spinner SPFormaDePagoYalo;
        public AdaptadorMetodoDePago adapterMetodoYalo;
        string FormaDePagoSelected;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            cliente = ProveedorGlobal.Cliente;
            vendedor = ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Arguments.Obtener<string>(MainActivity.LlaveUsuario));
           
            var metodo = Task.Run(async () => {
                metodoDePagoYalo = await proveedorDeCliente.MetodoDePago();
            });
            metodo.Wait();
       
        }
        public static YALOFragment NewInstance()
        {
            var frag1 = new YALOFragment { Arguments = new Bundle() };
            return frag1;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            // Progress de primera carga
            var progress = new Android.App.ProgressDialog(this.Context, Resource.Style.MyAlertDialogStyle);
            progress.SetMessage("Cargando órdenes...");
            progress.SetCancelable(false);
            progress.Show();

            try
            {
                // Ejecuta la primera consulta y valida
                bool hayOrdenes = Task.Run(async () => await CargarOrdenesInicialAsync()).GetAwaiter().GetResult();

                if (hayOrdenes)
                {
                    // Continúa con la vista normal
                    var view = ConfigurarVista(inflater, container);
                    return view;
                }
                else
                {
                    // No hay órdenes → Vista de Error
                    var view = inflater.Inflate(Resource.Layout.Error, container, false);
                    view.FindViewById<TextView>(Resource.Id.ErrorTexto).Text = "No hay órdenes Yalo.";
                    return view;
                }
            }
            catch (Exception ex)
            {
                var view = inflater.Inflate(Resource.Layout.Error, container, false);
                view.FindViewById<TextView>(Resource.Id.ErrorTexto).Text = "Ocurrió un error al cargar: " + ex.Message;
                return view;
            }
            finally
            {
                progress.Dismiss();
            }

        }
        private async Task<bool> CargarOrdenesInicialAsync()
        {
            OrdenesYalo = await proveedorDeYalo.OrdenesYalo(
                new DataOrdenesYalo(vendedor.Valor.Almacen)
            );

            // true si vino OK y con elementos
            return (OrdenesYalo?.Tipo == TipoDeResultado.Exito)
                   && (OrdenesYalo?.Valor != null)
                   && (OrdenesYalo.Valor.Count > 0);
        }
        public View ConfigurarVista(LayoutInflater inflater, ViewGroup container)
        {
            var view = inflater.Inflate(Resource.Layout.OrdenesYALO, container, false);
            expandableListView = view.FindViewById<ExpandableListView>(Resource.Id.expandableListViewOrdenes);
            Task.Run(async () => await Ordenes(recargar: false));

            return view;
        }
        public async Task Ordenes(bool recargar = false)
        {
            Android.App.ProgressDialog progressDialog = null;

            try
            {
                Activity.RunOnUiThread(() =>
                {
                    progressDialog = new Android.App.ProgressDialog(this.Context, Resource.Style.MyAlertDialogStyle);
                    progressDialog.SetMessage(recargar ? "Actualizando órdenes..." : "Preparando órdenes...");
                    progressDialog.SetCancelable(false);
                    progressDialog.Show();
                });

                // Si es refresco, vuelve a consultar
                if (recargar)
                {
                    OrdenesYalo = await proveedorDeYalo.OrdenesYalo(
                        new DataOrdenesYalo(vendedor.Valor.Almacen)
                    );
                }

                // Si falló o vino vacío, muestra Error en caliente
                if (!(OrdenesYalo?.Tipo == TipoDeResultado.Exito) || OrdenesYalo?.Valor == null || OrdenesYalo.Valor.Count == 0)
                {
                    Activity.RunOnUiThread(() =>
                    {
                        Snackbar.Make(View, "No hay órdenes Yalo.", Snackbar.LengthLong).Show();
                    });
                    return;
                }

                // Reconstruye la lista sin duplicados
                ordenesMovil.Clear();

                foreach (var ordenmovil in OrdenesYalo.Valor)
                {
                    if (ordenmovil?.LINEAS == null) continue;

                    var lineasMovil = new List<LineasYaloMovil>();

                    foreach (var lineasmovil in ordenmovil.LINEAS)
                    {
                        if (lineasmovil == null) continue;

                        var articulosDisponibles = await proveedorDeCliente.ConsultaDisponiblePorArticulo(
                            new DisponiblePorArticulo(vendedor.Valor.Almacen, lineasmovil.IDPRODUCTO));

                        lineasMovil.Add(new LineasYaloMovil
                        {
                            ORDERIDLINEA = lineasmovil.ORDERIDLINEA,
                            IDPRODUCTO = lineasmovil.IDPRODUCTO,
                            NOMBREPRODUCTO = lineasmovil.NOMBREPRODUCTO,
                            CANTIDAD = lineasmovil.CANTIDAD,
                            PRECIOUNITARIO = lineasmovil.PRECIOUNITARIO,
                            DESCUENTO = lineasmovil.DESCUENTO,
                            IMPORTENETO = lineasmovil.IMPORTENETO,
                            DISPONIBLE = articulosDisponibles?.Valor?.DisponiblePorTM != null
                                               ? Convert.ToDecimal(articulosDisponibles.Valor.DisponiblePorTM) : 0,
                            BLOQUEADO = lineasmovil.BLOQUEADO
                        });
                    }

                    bool tieneDisponible = lineasMovil.All(l => l.DISPONIBLE >= l.CANTIDAD);
                    bool estaBloqueada = lineasMovil.All(t => t.BLOQUEADO);
                    decimal totalOrden = lineasMovil.Sum(x => x.IMPORTENETO);

                    ordenesMovil.Add(new OrdenesYaloMovil
                    {
                        ORDERID = ordenmovil.ORDERID,
                        CTACLIENTE = ordenmovil.CTACLIENTE,
                        NOMBRECLIENTE = ordenmovil.NOMBRECLIENTE,
                        ESTATUSBANCO = ordenmovil.ESTATUSBANCO,
                        ESTADOORDEN = ordenmovil.ESTADOORDEN,
                        FORMADEPAGO = ordenmovil.FORMADEPAGO,
                        FECHACREACION = ordenmovil.FECHACREACION,
                        TIENEDISPONIBLE = tieneDisponible,
                        BLOQUEADA = estaBloqueada,
                        TOTALAPAGAR = totalOrden,
                        LINEAS = lineasMovil
                    });
                }

                // Pinta
                Activity.RunOnUiThread(() =>
                {
                    adapter = new OrdenesYaloAdapter(Activity, ordenesMovil.ToList(), this);
                    expandableListView.SetAdapter(adapter);
                    adapter.NotifyDataSetChanged();
                });
            }
            catch (Exception ex)
            {
                Activity.RunOnUiThread(() =>
                {
                    Toast.MakeText(Activity, "Error: " + ex.Message, ToastLength.Long).Show();
                });
            }
            finally
            {
                Activity.RunOnUiThread(() => progressDialog?.Dismiss());
            }
        }

        private void SPFormaDePagoYalo_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                FormaDePagoSelected = metodoDePagoYalo.Valor[e.Position].PAYMMODE;
            }
            catch (Exception ex)
            {
                Activity.MostrarMensaje("Ocurrio un error" + ex, ToastLength.Long);
            }
        }
        public async void callyalofragment(OrdenesYaloMovil ordenesYaloMovil)
        {
            try
            {

                if (ordenesYaloMovil.TIENEDISPONIBLE)
                {
                    if (ordenesYaloMovil.FORMADEPAGO == "0")
                    {
                        var vistaDePreguntaYalo = LayoutInflater.Inflate(Resource.Layout.PreguntaFormaDePagoYalo, null);

                        vistaDePreguntaYalo.FindViewById<TextView>(Resource.Id.PreguntatxtFormaPagoCXC).Text =
                                   "Selecciona la forma de pago";

                        SPFormaDePagoYalo = vistaDePreguntaYalo.FindViewById<Spinner>(Resource.Id.SPFormaDePagoYalo);

                        adapterMetodoYalo = new AdaptadorMetodoDePago(Activity, metodoDePagoYalo.Valor.ToList());
                        SPFormaDePagoYalo.Adapter = adapterMetodoYalo;

                        SPFormaDePagoYalo.ItemSelected += SPFormaDePagoYalo_ItemSelected;

                        _ = new Android.Support.V7.App.AlertDialog.Builder(Context)
                        .SetTitle(this.GetString(Resource.String.dialogInformacionYalo))
                        .SetView(vistaDePreguntaYalo)
                        .SetPositiveButton(this.GetString(Resource.String.dialog_ok), async (sender, args) =>
                        {

                            FacturacionConFormaDePago(ordenesYaloMovil);

                        }).SetNegativeButton(this.GetString(Resource.String.dialog_cancel), (sender, args) => { })
                       // .SetNeutralButton(View.Context.GetString(Resource.String.dialog_neutral), (sender, args) => { })
                       .Show();                       

                    }
                    else
                    {
                            FacturacionDirecta(ordenesYaloMovil);
                    }
                }
                else
                {                    
                    MensajeInformativo("No cuentas con el inventario para realizar la factura",false);
                }
               

                
            }
            catch(Exception ex)
            {                
                MensajeInformativo("Ocurrio un error" + ex, false);
            }
            

        }
        public async void FacturacionDirecta(OrdenesYaloMovil ordenesYaloMovil)
        {
            Android.App.ProgressDialog progressDialog = new Android.App.ProgressDialog(this.Context, Resource.Style.MyAlertDialogStyle);
            progressDialog.SetMessage(this.Context.GetString(Resource.String.main_dialog_progress_factura));
            progressDialog.SetCancelable(false);
            progressDialog.Show();
            bool estaBloqueada = ordenesYaloMovil.LINEAS.All(t => t.BLOQUEADO);
            if (estaBloqueada)
            {
                var desbloqueaLineas = await proveedorDeYalo.LiberaLineasPedido(new DataLiberaLineas(ordenesYaloMovil.ORDERID));
                if (desbloqueaLineas.Tipo == TipoDeResultado.Fallo)
                {
                    progressDialog.Dismiss();
                    Snackbar.Make(View, "Ocurrio un error al desbloquear las lineas. Mensaje: " + desbloqueaLineas.Mensaje, Snackbar.LengthLong).Show();
                    return;
                }
            }
            var actualizaFormaPago = await proveedorDeYalo.ActualizaFormaPagoYalo(new DataActFormaPagoYalo(ordenesYaloMovil.FORMADEPAGO, ordenesYaloMovil.ORDERID));
            
            if (actualizaFormaPago.Tipo == TipoDeResultado.Error)
            {
                progressDialog.Dismiss();
                Snackbar.Make(View, "Ocurrio un error al actualizar.", Snackbar.LengthLong).Show();
                return;
            }

            ObtenerFactura = await proveedorDeVenta.ObtenerFactura(new Factura(ordenesYaloMovil.ORDERID));

            if (ObtenerFactura.Tipo == TipoDeResultado.Exito)
            {
                var factura = ObtenerFactura.Valor.Substring(0, 1);
                if (factura == "F")
                {
                    if (ordenesYaloMovil.FORMADEPAGO == "01" || ordenesYaloMovil.FORMADEPAGO == "02" || ordenesYaloMovil.FORMADEPAGO == "99")
                    {
                        progressDialog.Dismiss();
                        //MensajeInformativo("Factura: " + ObtenerFactura.Valor);
                        var activity = new Android.Content.Intent(Activity, typeof(ReciboValoresImpresionActivity));
                        activity.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(vendedor));
                        activity.PutExtra(ReciboValoresImpresionActivity.LlaveFactura, ProveedorDeSerializado.Generar(ObtenerFactura.Valor));
                        activity.PutExtra(ReciboValoresImpresionActivity.LlaveFormaDePago, ProveedorDeSerializado.Generar(ordenesYaloMovil.FORMADEPAGO));
                        activity.PutExtra(ReciboValoresImpresionActivity.LLaveMonto, ProveedorDeSerializado.Generar(ordenesYaloMovil.TOTALAPAGAR));
                        StartActivity(activity);
                    }
                    else
                    {
                        progressDialog.Dismiss();
                        MensajeInformativo("Factura: " + ObtenerFactura.Valor, true);
                        Snackbar.Make(View, "Los tickets que se puede imprimir son pago en efectivo, cheque nominativo o sin definir", Snackbar.LengthLong).Show();
                    }
                }
                else
                {
                    progressDialog.Dismiss();
                    MensajeInformativo(ObtenerFactura.Valor,false);
                }



            }
            else
            {
                progressDialog.Dismiss();
                MensajeInformativo(ObtenerFactura.Valor,false);
            }
        }
       public async void FacturacionConFormaDePago(OrdenesYaloMovil ordenesYaloMovil)
        {
            Android.App.ProgressDialog progressDialog = new Android.App.ProgressDialog(this.Context, Resource.Style.MyAlertDialogStyle);
            progressDialog.SetMessage(this.Context.GetString(Resource.String.main_dialog_progress_factura));
            progressDialog.SetCancelable(false);
            progressDialog.Show();

            FormaDePagoSelected = FormaDePagoSelected != null ? FormaDePagoSelected : "01";
            if (FormaDePagoSelected != null || FormaDePagoSelected != "" || FormaDePagoSelected != "0")
            {
                if(FormaDePagoSelected == "01" || FormaDePagoSelected == "04" || FormaDePagoSelected == "28")
                {
                    bool estaBloqueada = ordenesYaloMovil.LINEAS.All(t => t.BLOQUEADO);
                    if (estaBloqueada)
                    {
                        var desbloqueaLineas = await proveedorDeYalo.LiberaLineasPedido(new DataLiberaLineas(ordenesYaloMovil.ORDERID));
                        if (desbloqueaLineas.Tipo == TipoDeResultado.Fallo)
                        {
                            progressDialog.Dismiss();
                            Snackbar.Make(View, "Ocurrio un error al desbloquear las lineas. Mensaje: " + desbloqueaLineas.Mensaje, Snackbar.LengthLong).Show();
                            return;
                        }
                    }
                    var actualizaFormaPago = await proveedorDeYalo.ActualizaFormaPagoYalo(new DataActFormaPagoYalo(FormaDePagoSelected, ordenesYaloMovil.ORDERID));
                    if (actualizaFormaPago.Tipo == TipoDeResultado.Error)
                    {
                        progressDialog.Dismiss();
                        Snackbar.Make(View, "Ocurrio un error al actualizar.", Snackbar.LengthLong).Show();
                        return;
                    }
                    ObtenerFactura = await proveedorDeVenta.ObtenerFactura(new Factura(ordenesYaloMovil.ORDERID));

                    if (ObtenerFactura.Tipo == TipoDeResultado.Exito)
                    {
                        var factura = ObtenerFactura.Valor.Substring(0, 1);
                        if (factura == "F")
                        {
                            if (FormaDePagoSelected == "01" || FormaDePagoSelected == "02" || FormaDePagoSelected == "99")
                            {
                                progressDialog.Dismiss();
                                //MensajeInformativo("Factura: " + ObtenerFactura.Valor);
                                var activity = new Android.Content.Intent(Activity, typeof(ReciboValoresImpresionActivity));
                                activity.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(vendedor));
                                activity.PutExtra(ReciboValoresImpresionActivity.LlaveFactura, ProveedorDeSerializado.Generar(ObtenerFactura.Valor));
                                activity.PutExtra(ReciboValoresImpresionActivity.LlaveFormaDePago, ProveedorDeSerializado.Generar(FormaDePagoSelected));
                                activity.PutExtra(ReciboValoresImpresionActivity.LLaveMonto, ProveedorDeSerializado.Generar(ordenesYaloMovil.TOTALAPAGAR));
                                StartActivity(activity);
                            }
                            else
                            {
                                progressDialog.Dismiss();
                                MensajeInformativo("Factura: " + ObtenerFactura.Valor, true);
                            }
                        }                     
                    }
                    else
                    {
                        Snackbar.Make(View, "Ocurrio un error al Factura.", Snackbar.LengthLong).Show();
                        return;
                    }
                }else
                {
                    Snackbar.Make(View, "Para poder facturar solo esta disponible Efectivo o pago con tarjeta de debito o credito.", Snackbar.LengthLong)
                             .Show();
                    return;
                }
                
              
            }
            else
            {
                Snackbar.Make(View, "No haz seleccionado la forma de pago.", Snackbar.LengthLong)
                .Show();
                return;
            }
        }

        public void MensajeInformativo(string valor, bool accion)
        {
            var vistaDePregunta = LayoutInflater.Inflate(Resource.Layout.PreguntaFlujo, null);
            vistaDePregunta.FindViewById<TextView>(Resource.Id.PreguntaFlujo).Text = valor;

            new Android.Support.V7.App.AlertDialog.Builder(Context)
                .SetTitle(this.GetString(Resource.String.dialogFlujo))
                .SetView(vistaDePregunta)
                .SetPositiveButton(this.GetString(Resource.String.dialog_ok), async (sender, args) =>
                {
                    // Refresca desde servidor y vuelve a pintar la lista
                    if (accion)
                    {
                        var progress = new Android.App.ProgressDialog(this.Context, Resource.Style.MyAlertDialogStyle);
                        progress.SetMessage("Actualizando órdenes...");
                        progress.SetCancelable(false);
                        progress.Show();
                        try
                        {
                            await Ordenes(recargar: true);
                        }
                        finally { progress.Dismiss(); }
                    }
                    
                })
                .Show();
        }



    }
}