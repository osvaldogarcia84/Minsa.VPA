using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Minsa.VPA.Adaptadores;
using Minsa.VPA.Modelos;
using Minsa.VPA.Proveedores;
using Minsa.VPA.Servicios;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace Minsa.VPA.Fragments
{
    public class SincronizarInformacionFragment : ListFragment
    {
        ProveedorLocalDatabase db = new ProveedorLocalDatabase();
        List<GeoLocacion> ListaGeo = new List<GeoLocacion>();
        private ResultadoDeOperacionGenerico<Vendedor> vendedor;
        private ProveedorDeEstrategia proveedorDeEstrategia = new ProveedorDeEstrategia();
        FloatingActionButton btnSincronizar;
        ResultadoDeOperacionGenerico<string> resultadoInsert;
        Timer timer;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            vendedor = ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Arguments.Obtener<string>(MainActivity.LlaveUsuario));
        }
        public static SincronizarInformacionFragment NewInstance()
        {
            var frag1 = new SincronizarInformacionFragment { Arguments = new Bundle() };
            return frag1;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view;
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            ListaGeo = db.SelectGeolocation();
            if(ListaGeo.Count > 0)
            {
                view = ConfigurarVista(inflater, container);
            }
            else
            {
                view = inflater.Inflate(Resource.Layout.Error, container, false);
                view.FindViewById<TextView>(Resource.Id.ErrorTexto).Text =
                   "No tienes información almacenada para sincronizar.";
            }
            return view;
        }
        private View ConfigurarVista(LayoutInflater inflater, ViewGroup container)
        {
            var view = inflater.Inflate(Resource.Layout.SincronizarInformacion, null);
            
            AsignarInformacion();
            btnSincronizar = view.FindViewById<FloatingActionButton>(Resource.Id.btnSincronizar);
            btnSincronizar.Click += BtnSincronizar_Click;
            return view;
        }

        private void BtnSincronizar_Click(object sender, System.EventArgs e)
        {
            sincronizarInformacion();
        }

        private void AsignarInformacion()
        {
            Activity.RunOnUiThread(() =>
            {

                ListAdapter = new AdaptadorSincronizarInformacion(Activity, ListaGeo);

                ((BaseAdapter)ListAdapter).NotifyDataSetChanged();
            });
        }

        public void sincronizarInformacion()
        {
            bool resultado = false;
            if(Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                Snackbar.Make(View, "No tienes conexion a internet", Snackbar.LengthLong)
                       .Show();
            }
            else
            {
               
                foreach (var geo in ListaGeo)
                {
                    var info = Task.Run(async () => {
                        resultadoInsert = await proveedorDeEstrategia.RegistrarGeoLocacion(new GeoLocacion(0, geo.ClienteId, geo.Longitud, geo.Latitud, geo.VendedorId));
                    });
                    info.Wait();
                   // var resultadoInsert = proveedorDeEstrategia.RegistrarGeoLocacion(new GeoLocacion(0, geo.ClienteId, geo.Longitud, geo.Latitud, geo.VendedorId));
                    if (resultadoInsert.Tipo == TipoDeResultado.Exito)
                    {
                        resultado = true;
                    }
                    else
                    {
                        resultado = false;
                    }

                }
                if (resultado)
                {
                    Snackbar.Make(View, "Sincronizacion correcta", Snackbar.LengthLong)
                           .Show();
                    db.deleteGeoLocacion();
                }
                else
                {
                    Snackbar.Make(View, "Ocurrio un error al sincronizar comunicate con un administrador.", Snackbar.LengthLong)
                       .Show();
                }
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
        //public static bool ExisteComunicacion()
        //{

        //    Android.Net.ConnectivityManager connectivityManager = (Android.Net.ConnectivityManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.ConnectivityService);
        //    Android.Net.NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;
        //    bool isOnline = (activeConnection != null) && activeConnection.IsConnected;
        //    if (isOnline == false)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
    }
}