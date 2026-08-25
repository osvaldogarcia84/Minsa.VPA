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
    public class ReciboFragment : ListFragment        
    {
        public static string LlaveRecibo = "LlaveRecibo";
        public ResultadoDeOperacionGenerico<Vendedor> vendedor;
        Cliente cliente;
        ProveedorDeClientes proveedorDeCliente = new ProveedorDeClientes();
        ProveedorDeCXC proveedorDeCxC = new ProveedorDeCXC();
        public ResultadoDeOperacionGenerico<List<Transacciones>> transacciones;
        public ResultadoDeOperacionGenerico<List<Transacciones>> reciboCredito;
        private const int GET_THREE = 2;
        private const int GET_SECOND = 1;
        private const int GET_FIRST = 0;
        Button cortecaja;
        Timer timer;
        public decimal ImpTotal = 0;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            cliente = ProveedorGlobal.Cliente;
            vendedor = ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Arguments.Obtener<string>(MainActivity.LlaveUsuario));
            
            var obtieneTransacciones = Task.Run(async () => {
                transacciones = await proveedorDeCliente.ObtenerTransacciones(new Sitio(vendedor.Valor.Almacen));
            });
            obtieneTransacciones.Wait();

            var obtieneReciboCredito = Task.Run(async () => {
                reciboCredito = await proveedorDeCxC.RecibosCredito(new Sitio(vendedor.Valor.Almacen));
            });
            obtieneReciboCredito.Wait();
            transacciones.Valor.AddRange(reciboCredito.Valor);
        }
        public static ReciboFragment NewInstance()
        {
            var frag1 = new ReciboFragment { Arguments = new Bundle() };
            return frag1;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view;
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            //var ObtenerInfoInvt = proveedorDeCliente.ObtenerInformacionDeInventario(new LlaveVendedor(vendedor.Valor.Id, vendedor.Valor.Usuario, vendedor.Valor.Nombre, InformacionGeneral.InformacionDeTipoDeOrden));
           // Sitio = vendedor.Valor.Almacen;
            
            if (transacciones.Valor.Count != 0)
            {
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
            var view = inflater.Inflate(Resource.Layout.Recibo, container, false);
            foreach(var recibos in transacciones.Valor.Where(t => t.FormaPago != "99"))
            {
                ImpTotal = ImpTotal + recibos.Importe;
            }
            AsignarTransacciones();            
            var ConImporte = view.FindViewById<AutofitTextView>(Resource.Id.ImporteDepto);
            cortecaja = view.FindViewById<Button>(Resource.Id.CorteCaja);
            cortecaja.Click += Cortecaja_Click;
            ConImporte.Text = "$ " + ImpTotal.ToString("##,###.##");
           
            return view;
        }

        private void Cortecaja_Click(object sender, EventArgs e)
        {
            var activity = new Android.Content.Intent(Activity, typeof(CorteCajaActivity));
            activity.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(vendedor));
            
            activity.PutExtra(CorteCajaActivity.LlaveTransaccionesCorteCaja, ProveedorDeSerializado.Generar(transacciones));
         //   activity.PutExtra(CorteCajaActivity.LlaveSitioCorteCaja, ProveedorDeSerializado.Generar(Sitio));
            //StartActivity(activity);
            StartActivityForResult(activity, GET_FIRST);
        }

        private void AsignarTransacciones()
        {
            Activity.RunOnUiThread(() =>
            {
                ListAdapter = new TransaccionesAdapter(Activity, transacciones.Valor.ToList(),this, vendedor.Valor.Almacen);
                ((BaseAdapter)ListAdapter).NotifyDataSetChanged();
            });
        }
        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            Transacciones transaccion = ((TransaccionesAdapter)ListAdapter).Items.ElementAt(position);
            ((TransaccionesAdapter)ListAdapter).setSelectedIndex(position);
            ((BaseAdapter)ListAdapter).NotifyDataSetChanged();
            ProveedorGlobal.Cliente = cliente;

            Snackbar.Make(View, "Factura: " + transaccion.Factura, Snackbar.LengthLong)
            .Show();

            //var activity = new Android.Content.Intent(Activity, typeof(ReciboValoresActivity));
            //activity.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(vendedor));
            //activity.PutExtra(ReciboValoresActivity.LlaveReciboValores, ProveedorDeSerializado.Generar(transaccion));
            //activity.PutExtra(ReciboValoresActivity.LlaveSitio, ProveedorDeSerializado.Generar(Sitio));
            ////StartActivity(activity);
            //StartActivityForResult(activity, GET_FIRST);
        }

        //public Android.Content.Intent ImprimirCliente()
        //{
        //    var activity = new Android.Content.Intent(Activity, typeof(ReciboValoresActivity));
        //    activity.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(vendedor));
        //    activity.PutExtra(ReciboValoresActivity.LlaveReciboValores, ProveedorDeSerializado.Generar(transaccion));
        //    activity.PutExtra(ReciboValoresActivity.LlaveSitio, ProveedorDeSerializado.Generar(Sitio));
        //    //StartActivity(activity);
        //    StartActivityForResult(activity, GET_FIRST);
        //}

        //public void callimprimir(int method)
        //{
        //    switch (method)
        //    {
        //        case 1:
        //            RegistraVisita();
        //            break;
        //        case 2:
        //            Degustacion();
        //            break;
        //        case 3:
        //            Carrito();
        //            break;
        //        case 4:
        //            SeguimientoVenta();
        //            break;
        //        case 5:
        //            SeguimientoVisita();
        //            break;
        //        case 6:
        //            SeguimientoCierre();
        //            break;

        //    }
        //    FragmentManager.BeginTransaction()
        //       .Replace(Resource.Id.content_frame, fragment)
        //       .Commit();
        //}
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
