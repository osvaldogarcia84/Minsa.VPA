using Android.Locations;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Text;
using Android.Views;
using Android.Widget;
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
    public class RutaFragment : ListFragment
    {
        public ResultadoDeOperacionGenerico<Vendedor> vendedor;
        public List<Cliente> clientes;
        public List<Cliente> BusquedaCliente;
        public const string LlaveDeRuta = "Ruta";
        private ProveedorDeLocacion proveedorDeLocacion;
        Android.Support.V4.App.Fragment fragment = null;
        private IObservable<string> filtros;
        Timer timer;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            vendedor = ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Arguments.Obtener<string>(MainActivity.LlaveUsuario));
            //proveedorDeLocacion = new ProveedorDeLocacion(Activity, 0); 
            //proveedorDeLocacion.LocacionEncontrada += ActualizarLocacion;
            try
            {
                Activity.MostrarMensaje("Cargando información de los clientes");
                if(ProveedorGlobal.Cliente == null)
                ProveedorGlobal.Cliente = null;
                clientes = ProveedorDeManipulacionDeDatos.ObtenerTodos<Cliente>(Arguments.Obtener<string[]>(LlaveDeRuta))
                .ConvertirLista();
                AsignarClientes(null);
            }
            catch(Exception ex)
            {

                Activity.MostrarMensaje(ex.Message);
            }            
        }
        public static RutaFragment NewInstance()
        {
            var frag1 = new RutaFragment { Arguments = new Bundle() };
            return frag1;
        }
        //private void ActualizarLocacion(object sender, Location location)
        //{
        //    try
        //    {              
        //            ProveedorGlobal.Lan = new Lan(location.Longitude, location.Latitude);               
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view;
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            if(clientes.Count > 0)
            {
                view = ConfigurarVista(inflater, container);
            }
            else
            {
                view = inflater.Inflate(Resource.Layout.Error, container, false);
                view.FindViewById<TextView>(Resource.Id.ErrorTexto).Text =
                   "No tienes ruta cargada comunicate con tu jefe inmediato o con el departamento de INE.";
            }
            return view;
        }
        private View ConfigurarVista(LayoutInflater inflater, ViewGroup container)
        {
            var view = inflater.Inflate(Resource.Layout.Ruta, null);           
            AsignarClientes(null);
            AsignarBusqueda(view);
            return view;
        }
        private void AsignarBusqueda(View view)
        {
            if (filtros != null)
                return;

            var clientesSearch = view.FindViewById<EditText>(Resource.Id.ClientesBusqueda);
            filtros = Observable.FromEventPattern<TextChangedEventArgs>(
                h => clientesSearch.TextChanged += h,
                h => clientesSearch.TextChanged -= h)
                .Select(e => ((EditText)e.Sender).Text).Throttle(TimeSpan.FromSeconds(0.5)).DistinctUntilChanged();
            filtros.Subscribe(text =>
            {
                Func<Cliente, bool> filter;
                if (clientes != null && !String.IsNullOrEmpty(text))
                {
                    //filter = x => x.ClienteId.EsValido() && x.Nombre.Contains(text.ToUpper());
                    filter = (x => x.Nombre.ToUpper().Contains(text.ToUpper()) || x.ClienteId.Contains(text));
                }
                else
                {
                    filter = null;
                }
                    
                AsignarClientes(filter);
            });
        }
        private void AsignarClientes(Func<Cliente, bool> filtro)
        {
            Activity.RunOnUiThread(() =>
            {
                ListAdapter = filtro != null
                       ? new ClientesAdapter(Activity, clientes .Where(filtro).ToList(),this)
                       : new ClientesAdapter(Activity, clientes.ToList(),this);
                ((BaseAdapter)ListAdapter).NotifyDataSetChanged();
            });
        }
        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            Cliente cliente = ((ClientesAdapter)ListAdapter).Items.ElementAt(position);
            ((ClientesAdapter)ListAdapter).setSelectedIndex(position);
            ((BaseAdapter)ListAdapter).NotifyDataSetChanged();
            ProveedorGlobal.Cliente = cliente;
            Snackbar.Make(View, "Cliente elegido: " + cliente.Nombre, Snackbar.LengthLong)
            .Show();
        }

        //public Android.Support.V4.App.Fragment RegistraVisita()
        //{
        //    var arguments = new Bundle();
        //    Arguments.PutString(RegistrarVisitaFragment.LLaveRegistrarVisita, ProveedorDeManipulacionDeDatos.Generar(vendedor));
        //    arguments = Arguments;
        //    fragment = RegistrarVisitaFragment.NewInstance();
        //    fragment.Arguments = Arguments;            
        //    return new RegistrarVisitaFragment();
        //}
        public Android.Support.V4.App.Fragment Degustacion()
        {
            if(vendedor.Valor.Almacen != "Supervisor")
            {
                var arguments = new Bundle();
                Arguments.PutString(DegustacionesFragment.LLaveDegustaciones, ProveedorDeManipulacionDeDatos.Generar(vendedor));
                arguments = Arguments;
                fragment = DegustacionesFragment.NewInstance();
                fragment.Arguments = Arguments;
                return new DegustacionesFragment();
            }
            else
            {
                Activity.MostrarMensaje("No tienes acceso a este menu");
                return null;
            }
            
        }
        public Android.Support.V4.App.Fragment Carrito()
        {
            var arguments = new Bundle();
            Arguments.PutString(CarritoFragment.LlaveDeCarrito, ProveedorDeManipulacionDeDatos.Generar(vendedor));
            arguments = Arguments;
            fragment = CarritoFragment.NewInstance();
            fragment.Arguments = arguments;
            return new CarritoFragment();      
        }
        public Android.Support.V4.App.Fragment SeguimientoVenta()
        {
            var arguments = new Bundle();           
            Arguments.PutString(SeguimientoVentaFragment.SeguimientoVenta, ProveedorDeManipulacionDeDatos.Generar(vendedor));
           // Arguments.PutString(ProveedorGlobal.InformacionOrdenId, ProveedorGlobal.ObtenerOrdenDeCliente());
            arguments = Arguments;
            fragment = SeguimientoVentaFragment.NewInstance();
            fragment.Arguments = arguments;
            return new SeguimientoVentaFragment();
        }
        //public Android.Support.V4.App.Fragment SeguimientoVentaSi()
        //{
        //    var arguments = new Bundle();
        //    Arguments.PutString(SeguimientoVentaSiFragment.SeguimientoVentaSi, ProveedorDeManipulacionDeDatos.Generar(vendedor));
        //    // Arguments.PutString(ProveedorGlobal.InformacionOrdenId, ProveedorGlobal.ObtenerOrdenDeCliente());
        //    arguments = Arguments;
        //    fragment = SeguimientoVentaSiFragment.NewInstance();
        //    fragment.Arguments = arguments;
        //    return new SeguimientoVentaSiFragment();
        //}
            public Android.Support.V4.App.Fragment SeguimientoVisita()
        {
            var arguments = new Bundle();
            Arguments.PutString(SeguimientoVisitaFragment.LLaveSeguimientoVisita, ProveedorDeManipulacionDeDatos.Generar(vendedor));
            arguments = Arguments;
            fragment = SeguimientoVisitaFragment.NewInstance();
            fragment.Arguments = arguments;
            return new SeguimientoVisitaFragment();
        }
        public Android.Support.V4.App.Fragment SeguimientoCierre()
        {
            var arguments = new Bundle();
            Arguments.PutString(SeguimientoCierreVentaFragment.LlaveCierreVentas, ProveedorDeManipulacionDeDatos.Generar(vendedor));
            // Arguments.PutString(ProveedorGlobal.InformacionOrdenId, ProveedorGlobal.ObtenerOrdenDeCliente());
            arguments = Arguments;
            fragment = SeguimientoCierreVentaFragment.NewInstance();
            fragment.Arguments = arguments;
            return new SeguimientoCierreVentaFragment();
        }
        public Android.Support.V4.App.Fragment BajaCliente()
        {
            var arguments = new Bundle();
            Arguments.PutString(BajaClienteFragment.LlaveDeBaja, ProveedorDeManipulacionDeDatos.Generar(vendedor));
            arguments = Arguments;
            fragment = BajaClienteFragment.NewInstance();
            fragment.Arguments = Arguments;
            return new BajaClienteFragment();
        }
        public Android.Support.V4.App.Fragment RegistraVisita()
        {
            var arguments = new Bundle();
            Arguments.PutString(RegistrarVisitaFragment.LLaveRegistrarVisita, ProveedorDeManipulacionDeDatos.Generar(vendedor));
            arguments = Arguments;
            fragment = RegistrarVisitaFragment.NewInstance();
            fragment.Arguments = Arguments;
            return new RegistrarVisitaFragment();
        }

        public Android.Support.V4.App.Fragment CXC()
        {
            var arguments = new Bundle();
            Arguments.PutString(CXCFragment.LlaveCXC, ProveedorDeManipulacionDeDatos.Generar(vendedor));
            arguments = Arguments;
            fragment = CXCFragment.NewInstance();
            fragment.Arguments = Arguments;
            return new CXCFragment();
        }

        public void callfragment(int method)
        {
            switch (method)
            {
                case 1:
                    BajaCliente();
                    break;
                case 2:
                    Degustacion();
                    break;
                case 3:
                    RegistraVisita();
                    // Carrito();
                    break;
                case 4:
                    CXC();                    
                    break;
                    //case 5:
                    //    //SeguimientoVisita();
                    //    break;
                    //case 6:
                    //    //BajaCliente();
                    //    //  SeguimientoCierre();
                    //    break;

            }
            FragmentManager.BeginTransaction()
               .Replace(Resource.Id.content_frame, fragment)
               .Commit();
        }
        //public override void OnResume()
        //{
        //    base.OnResume();
        //    proveedorDeLocacion.Obtener();
        //}

        //public override void OnPause()
        //{
        //    base.OnPause();
        //    proveedorDeLocacion.Remover();
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