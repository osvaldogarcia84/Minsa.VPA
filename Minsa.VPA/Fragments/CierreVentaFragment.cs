using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Minsa.VPA.Activities;
using Minsa.VPA.Adaptadores;
using Minsa.VPA.Modelos;
using Minsa.VPA.Proveedores;
using Minsa.VPA.Repositorio;
using Minsa.VPA.Servicios;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Reactive.Linq;
using System.Timers;

namespace Minsa.VPA.Fragments
{
    public class CierreVentaFragment : ListFragment
    {
        public const string LlaveListadoCierreVenta = "LlaveListadoCierreVenta";
        private ResultadoDeOperacionGenerico<Vendedor> vendedor;
        ProveedorDeEstrategia proveedorDeEstrategia = new ProveedorDeEstrategia();
        ResultadoDeOperacionGenerico<List<ClientesProspecto>> ClientesProspecto;
        Android.Support.V4.App.Fragment fragment = null;
        private const int GET_FIRST = 0;
        Timer timer;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            vendedor = Servicios.ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Arguments.Obtener<string>(MainActivity.LlaveUsuario));
            var prospectos = proveedorDeEstrategia.ClientesProspecto(new ClienteId(vendedor.Valor.Id));
            if(prospectos.Tipo == TipoDeResultado.Exito)
            {
                ClientesProspecto = prospectos;
            }
            else
            {
                Snackbar.Make(View, "Ocurrio un error comunicate con el departamento de Sistemas", Snackbar.LengthLong)
                .Show();
            }
        }
        public static CierreVentaFragment NewInstance()
        {
            var frag1 = new CierreVentaFragment { Arguments = new Bundle() };
            return frag1;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                View view;
                var ignored = base.OnCreateView(inflater, container, savedInstanceState);
                if (ClientesProspecto != null)
                {
                    view = ConfigurarVista(inflater, container);
                    //ConfigurarModelo(view);
                }
                else
                {
                    view = inflater.Inflate(Resource.Layout.Error, container, false);
                    view.FindViewById<TextView>(Resource.Id.ErrorTexto).Text =
                        "No se encontraron registros para el cliente.";
                }
                return view;
            }
            catch (Exception ex)
            {
                Toast.MakeText(this.Activity, "No estas conectado a internet y/o ocurrio un problema " + ex, ToastLength.Short).Show();
                return null;
            }
        }

        private View ConfigurarVista(LayoutInflater inflater, ViewGroup container)
        {
            View view = inflater.Inflate(Resource.Layout.CierreVentas, null);
            AsignarProspectos();
            return view;
        }    
        private void AsignarProspectos()
        {
            Activity.RunOnUiThread(() =>
            {
                //ListAdapter = filtro != null
                //       ? new ClientesAdapter(Activity, clientes.Where(filtro).ToList(), this)
                //       : new ClientesAdapter(Activity, clientes.ToList(), this);
                ListAdapter = new ProspectosAdapter(Activity, ClientesProspecto.Valor, this);
                ((BaseAdapter)ListAdapter).NotifyDataSetChanged();
            });
        }
        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            ClientesProspecto cliente = ((ProspectosAdapter)ListAdapter).Items.ElementAt(position);
            ((ProspectosAdapter)ListAdapter).setSelectedIndex(position);
            ((BaseAdapter)ListAdapter).NotifyDataSetChanged();
            ProveedorGlobal.ClientesProspecto = cliente;
            Snackbar.Make(View, "Cliente elegido: " + cliente.IdCliente, Snackbar.LengthLong)
            .Show();

            //Intent cerrarPago = new Intent(Activity, typeof(CerrarVentaCompletaActivity));
            //cerrarPago.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(vendedor));
            //cerrarPago.PutExtra(CerrarVentaCompletaActivity.LlaveVentaCompleta, ProveedorDeSerializado.Generar(cliente));
            //StartActivity(cerrarPago);      
            var activity = new Android.Content.Intent(Activity, typeof(CerrarVentaCompletaActivity));
            activity.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(vendedor));
            activity.PutExtra(CerrarVentaCompletaActivity.LlaveVentaCompleta, ProveedorDeSerializado.Generar(cliente));
            StartActivityForResult(activity, GET_FIRST);
        }

        public Android.Support.V4.App.Fragment Cierre()
        {
            var arguments = new Bundle();
            Arguments.PutString(SeguimientoCierreVentaFragment.LlaveCierreVentas, ProveedorDeManipulacionDeDatos.Generar(vendedor));
            // Arguments.PutString(ProveedorGlobal.InformacionOrdenId, ProveedorGlobal.ObtenerOrdenDeCliente());
            arguments = Arguments;
            fragment = SeguimientoCierreVentaFragment.NewInstance();
            fragment.Arguments = arguments;
            return new SeguimientoCierreVentaFragment();
        }
        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            try
            {
                base.OnActivityResult(requestCode, resultCode, data);
                if (requestCode == GET_FIRST && resultCode == -1) // Agregar
                {
                    var clientes  = ProveedorDeSerializado.Obtener<ClientesProspecto>(data.Obtener<string>(CerrarVentaCompletaActivity.LlaveVentaCompleta));
                    List<ClientesProspecto> info = new List<ClientesProspecto>();
                    //foreach(var updateCliente in ClientesProspecto.Valor)
                    //{
                    //if(updateCliente.IdCliente == clientes.IdCliente)
                    //{
                    //     ClientesProspecto.Valor.Select(c => { c.Cerrada = true; return c; }).Where(t => t.IdCliente == clientes.IdCliente);
                    // }
                    //}

                    ClientesProspecto.Valor.Where(c => c.IdCliente == clientes.IdCliente).Select(c => { c.Cerrada = true; return c; }).ToList();

                    ListAdapter = new ProspectosAdapter(Activity, ClientesProspecto.Valor, this);
                    ((BaseAdapter)ListAdapter).NotifyDataSetChanged();
                }
            }
            catch (Exception ex)
            {
                Snackbar.Make(View, "Error: " + ex, Snackbar.LengthLong)
                        .Show();

            }
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