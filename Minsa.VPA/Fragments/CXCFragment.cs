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
    [Obsolete]
    public class CXCFragment : ListFragment
    {
        public static string LlaveCXC = "LlaveCXC";
        Cliente cliente;
        ProveedorDeCXC proveedorDeCXC = new ProveedorDeCXC();
        ProveedorDeClientes proveedorDeCliente = new ProveedorDeClientes();
        public ResultadoDeOperacionGenerico<Vendedor> vendedor;
        ResultadoDeOperacionGenerico<List<DashboardCXC>> dashboardCXC;
        Timer timer;
        List<Transacciones> transacciones = new List<Transacciones>();
        ResultadoDeOperacionGenerico<int> resultadoCreaTicketCXC;
        Spinner FormaDePagoCXC;
        ResultadoDeOperacionGenerico<List<MetodoDePago>> metodoDePagoCXC;
        public AdaptadorMetodoDePago adapterMetodoCXC;
        string SPFormaDePagoCXC;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            cliente = ProveedorGlobal.Cliente;
            vendedor = ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Arguments.Obtener<string>(MainActivity.LlaveUsuario));
            // Create your fragment here
            var obtieneFacturas = Task.Run(async () =>
            {
                dashboardCXC = await proveedorDeCXC.DashboardCXC(new DataDashboardCXC(vendedor.Valor.Almacen, cliente.ClienteId));
            });
            obtieneFacturas.Wait();
            var metodo = Task.Run(async () => {
                metodoDePagoCXC = await proveedorDeCliente.MetodoDePago();
            });
            metodo.Wait();
        }
        public static CXCFragment NewInstance()
        {
            var frag1 = new CXCFragment { Arguments = new Bundle() };
            return frag1;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
             View view;
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            //var ObtenerInfoInvt = proveedorDeCliente.ObtenerInformacionDeInventario(new LlaveVendedor(vendedor.Valor.Id, vendedor.Valor.Usuario, vendedor.Valor.Nombre, InformacionGeneral.InformacionDeTipoDeOrden));
            // Sitio = vendedor.Valor.Almacen;           
            if (dashboardCXC.Valor.Count > 0)
            {
                Transacciones();
                AsignarCXC();
                view = ConfigurarVista(inflater, container);
            }
            else
            {
                view = inflater.Inflate(Resource.Layout.Error, container, false);
                view.FindViewById<TextView>(Resource.Id.ErrorTexto).Text =
                   "No tienes transacciones el dia de hoy.";
            }
            return view;
        }
        private View ConfigurarVista(LayoutInflater inflater, ViewGroup container)
        {
            var view = inflater.Inflate(Resource.Layout.CXC, container, false);           
            return view;
        }
        public void Transacciones()
        {           
           // transacciones = null;
            if(dashboardCXC != null)
            {
                foreach (var trans in dashboardCXC.Valor)
                {
                    Transacciones transacion = new Transacciones()
                    {
                        FechaFactura = trans.FechaDocumento,
                        Cliente = trans.Cliente,
                        Nombre = trans.NombreCliente,
                        //CuentaFacturacion
                        Factura = trans.Factura,
                        //DigitoVerificador = trans
                        MontoxFactura = trans.MontoOriginal,
                        Importe = trans.MontoOriginal,
                        ImporteTotal = trans.MontoAbierto,

                        FormaPago = trans.FormaDePago
                    };
                    transacciones.Add(transacion);
                }
            }
            
        }
        private void AsignarCXC()
        {
            Activity.RunOnUiThread(() =>
            {
                ListAdapter = new TransaccionesAdapterCXC(Activity, dashboardCXC.Valor.ToList(), this, vendedor.Valor.Almacen, transacciones);
                ((BaseAdapter)ListAdapter).NotifyDataSetChanged();
            });
        }
        public void callfragmentCXC(int method)
        {
            switch (method)
            {
                case 1:
                    if (ProveedorGlobal.ClienteCXC == null)
                    {
                        Snackbar.Make(View, "Debes seleccionar una factura.", Snackbar.LengthLong).Show();
                        return;
                        
                    }

                    var vistaDePreguntaCXC = LayoutInflater.Inflate(Resource.Layout.PreguntaMontoCXC, null);

                    vistaDePreguntaCXC.FindViewById<TextView>(Resource.Id.PreguntatxtMontoCXC).Text =
                               "Ingresa el monto a depositar";

                    vistaDePreguntaCXC.FindViewById<TextView>(Resource.Id.PreguntatxtFormaPagoCXC).Text =
                               "Forma de pago";

                    FormaDePagoCXC = vistaDePreguntaCXC.FindViewById<Spinner>(Resource.Id.SPFormaDePagoCXC);

                    adapterMetodoCXC = new AdaptadorMetodoDePago(Activity, metodoDePagoCXC.Valor.ToList());
                    FormaDePagoCXC.Adapter = adapterMetodoCXC;

                    FormaDePagoCXC.ItemSelected += FormaDePagoCXC_ItemSelected;

                    new Android.Support.V7.App.AlertDialog.Builder(Context)
                    .SetTitle(this.GetString(Resource.String.dialogRegistroMonto))
                    .SetView(vistaDePreguntaCXC)
                    .SetPositiveButton(this.GetString(Resource.String.dialog_ok), (sender, args) =>
                    {
                        string montoApagar = vistaDePreguntaCXC.FindViewById<EditText>(Resource.Id.PreguntaMontoCXC).Text;                       

                        if (montoApagar != null || montoApagar != "")
                        {
                            var creaTicket = Task.Run(async () => {
                                resultadoCreaTicketCXC = await proveedorDeCXC.CreaTicketVPAAP(new DataCreaTicket(ProveedorGlobal.ClienteCXC.Factura , Convert.ToDecimal(montoApagar), SPFormaDePagoCXC));
                                
                            });
                            creaTicket.Wait();
                        
                            if(resultadoCreaTicketCXC != null)
                            {
                                var activity = new Android.Content.Intent(Activity, typeof(ReciboValoresImpresionActivity));
                                activity.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(vendedor));
                                //   activity.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(CarritoFragment.LlaveDeCarrito));
                                activity.PutExtra(ReciboValoresImpresionActivity.LlaveFactura, ProveedorDeSerializado.Generar(ProveedorGlobal.ClienteCXC.Factura));
                                activity.PutExtra(ReciboValoresImpresionActivity.LlaveFormaDePago, ProveedorDeSerializado.Generar(SPFormaDePagoCXC));
                                activity.PutExtra(ReciboValoresImpresionActivity.LLaveIdRV, ProveedorDeSerializado.Generar(resultadoCreaTicketCXC.Valor));
                                activity.PutExtra(ReciboValoresImpresionActivity.LLaveMonto, ProveedorDeSerializado.Generar(Convert.ToDecimal(montoApagar)));
                                StartActivity(activity);
                            }
                            else
                            {
                                Snackbar.Make(View, "Ocurrio un error al generarl el ticket.", Snackbar.LengthLong).Show();
                                return;
                            }
                        }
                        else
                        {
                            Snackbar.Make(View, "No haz ingresado el monto a pagar.", Snackbar.LengthLong)
                            .Show();
                            return;
                        }                     

                    }).SetNegativeButton(this.GetString(Resource.String.dialog_cancel), (sender, args) => { })
                   // .SetNeutralButton(View.Context.GetString(Resource.String.dialog_neutral), (sender, args) => { })
                   .Show();
                    break;
            }
        }

        private void FormaDePagoCXC_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {           
            try
            {
                SPFormaDePagoCXC = metodoDePagoCXC.Valor[e.Position].PAYMMODE;
            }
            catch (Exception ex)
            {
                Activity.MostrarMensaje("Ocurrio un error" + ex, ToastLength.Long);
            }
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            DashboardCXC clienteCXC = ((TransaccionesAdapterCXC)ListAdapter).Items.ElementAt(position);
            ((TransaccionesAdapterCXC)ListAdapter).setSelectedIndex(position);
            ((BaseAdapter)ListAdapter).NotifyDataSetChanged();
            ProveedorGlobal.ClienteCXC = clienteCXC;
            Snackbar.Make(View, "Factura elegida: " + clienteCXC.Factura , Snackbar.LengthLong)
            .Show();
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