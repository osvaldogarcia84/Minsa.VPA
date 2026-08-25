using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
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
    public class InventarioClienteFragment : ListFragment
    {
        public const string LLaveInventarioClienteFragment = "InventarioClienteFragment";
        public ResultadoDeOperacionGenerico<Vendedor> vendedor;
        Cliente cliente;
        List<InformacionDeInventario> ListaInventario;
        public ResultadoDeOperacionGenerico<List<InformacionDeInventario>> ObtenerListaInventario;
        Timer timer;
        ProveedorDeClientes proveedorDeCliente = new ProveedorDeClientes();
        View view;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            cliente = ProveedorGlobal.Cliente;
            vendedor = ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Arguments.Obtener<string>(MainActivity.LlaveUsuario));
            var inventario = Task.Run(async () =>
            {
                ObtenerListaInventario = await proveedorDeCliente.ObtenerConsultaDisponible(new DataSitio(vendedor.Valor.Almacen));
            });
            inventario.Wait();

        }
        public static InventarioClienteFragment NewInstance()
        {
            var frag1 = new InventarioClienteFragment { Arguments = new Bundle() };
            return frag1;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {           
                    
            if(ObtenerListaInventario.Valor != null)
            {
                view = ConfigurarVista(inflater, container);
            }
            else
            {
                view = inflater.Inflate(Resource.Layout.Error, container, false);
                view.FindViewById<TextView>(Resource.Id.ErrorTexto).Text =
                   "No tienes inventario cargado en tu unidad. Comunicate con tu jefe inmediato o tu bodega.";
            }
            
            return view;
        }

        private View ConfigurarVista(LayoutInflater inflater, ViewGroup container)
        {
            var view = inflater.Inflate(Resource.Layout.InventarioCliente, container, false);
            view.FindViewById<TextView>(Resource.Id.Disponible).Text = "Disponible en Sitio: " + vendedor.Valor.Almacen;
            ListaInventario = ObtenerListaInventario.Valor.Select(e => new InformacionDeInventario(e.CodigoArticulo, e.NombreArticulo, e.DisponiblePorSaco, e.DisponiblePorTM, e.PrecioPorSaco, e.PrecioPorTM, e.UnidadDeMedida)).ToList();
            AsignarInventario();
            return view;
        }
        private void AsignarInventario()
        {
            Activity.RunOnUiThread(() =>
            {
                ListAdapter = new InventarioAdapter(Activity, ListaInventario.ToList());

                ((BaseAdapter)ListAdapter).NotifyDataSetChanged();
            });
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