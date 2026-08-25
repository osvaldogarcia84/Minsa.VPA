using Android;
using Android.Content;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Minsa.VPA.Activities;
using Minsa.VPA.Modelos;
using Minsa.VPA.Proveedores;
using Minsa.VPA.Servicios;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Minsa.VPA.Fragments
{
    public class RegistrarVisitaFragment : Fragment, ILocationListener
    {
        public const string LLaveRegistrarVisita = "LLaveRegistrarVisita";
        Cliente cliente;
        public ResultadoDeOperacionGenerico<Vendedor> vendedor;
        ResultadoDeOperacionGenerico<List<Cliente>> resultadoCliente;
        public ProveedorDeLocacion proveedorDeLocacion;
        public ProveedorDeEstrategia proveedorDeEstrategia = new ProveedorDeEstrategia();
        Button btnEnviarCoordenadas;
        public Lan Lan;
        ProveedorLocalDatabase db;
       // LocationManager locationManager;
        const long ONE_MINUTE = 5 * 1000;
        const long FIVE_MINUTES = 5 * ONE_MINUTE;
        static readonly string KEY_REQUESTING_LOCATION_UPDATES = "requesting_location_updates";

        static readonly int RC_LAST_LOCATION_PERMISSION_CHECK = 1000;
        static readonly int RC_LOCATION_UPDATES_PERMISSION_CHECK = 1100;
        View rootLayout;
        //Button getLastLocationButton;
        bool isRequestingLocationUpdates;
        TextView latitude;
       // internal TextView latitude2;
        LocationManager locationManager;
        TextView longitude;
        public double Longitud;
        public double Latitud;
        ResultadoDeOperacionGenerico<string> resultadoInsert;
        ProveedorDeClientes proveedorDeClientes = new ProveedorDeClientes();
        Timer timer;
        Android.Support.V4.App.Fragment fragment = null;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            cliente = ProveedorGlobal.Cliente;
            vendedor = ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Arguments.Obtener<string>(MainActivity.LlaveUsuario));

            locationManager = (LocationManager)Context.GetSystemService(Context.LocationService);
            if (savedInstanceState != null)
            {
                isRequestingLocationUpdates = savedInstanceState.KeySet().Contains(KEY_REQUESTING_LOCATION_UPDATES) &&
                                              savedInstanceState.GetBoolean(KEY_REQUESTING_LOCATION_UPDATES);
            }
            else
            {
                isRequestingLocationUpdates = false;
            }

            ProveedorGlobal.Lan = new Lan(1,1);
            //  proveedorDeLocacion.Remover();

            //Lan = new Lan(1, 1);
            //Lan.Latitud = 1;
            //Lan.Longitud = 1;
            //proveedorDeLocacion = new ProveedorDeLocacion(Activity);
            //proveedorDeLocacion.LocacionEncontrada += LocacionEncontrada;

        }

        public static RegistrarVisitaFragment NewInstance()
        {
            var frag1 = new RegistrarVisitaFragment { Arguments = new Bundle() };
            return frag1;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = ConfigurarVista(inflater, container);
            ConfigurarModelo(view);
            return view;
        }
        private View ConfigurarVista(LayoutInflater inflater, ViewGroup container)
        {
            View view = inflater.Inflate(Resource.Layout.RegistrarVisita, container, false);
            btnEnviarCoordenadas = view.FindViewById<Button>(Resource.Id.EnviarRegistroCoordenadas);
            btnEnviarCoordenadas.Click += BtnEnviarCoordenadas_Click;
            return view;
        }
        private void ConfigurarModelo(View view)
        {
            view.FindViewById<TextView>(Resource.Id.ClienteRegistrar).Text = cliente.ClienteId + " - " + cliente.Nombre;

            latitude = view.FindViewById<TextView>(Resource.Id.VisitaLatitud);
            longitude = view.FindViewById<TextView>(Resource.Id.VisitaLongitud);


            //if (locationManager.AllProviders.Contains(LocationManager.NetworkProvider)
            //  && locationManager.IsProviderEnabled(LocationManager.NetworkProvider))
            //{
            //    if (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.AccessFineLocation) == Permission.Granted)
            //    {
            //        var criteria = new Criteria { PowerRequirement = Power.Medium };

            //        var bestProvider = locationManager.GetBestProvider(criteria, true);
            //        var location = locationManager.GetLastKnownLocation(bestProvider);

            //        if (location != null)
            //        {
            //            view.FindViewById<TextView>(Resource.Id.VisitaLatitud).Text = Convert.ToString(location.Latitude);
            //            view.FindViewById<TextView>(Resource.Id.VisitaLongitud).Text = Convert.ToString(location.Longitude);

            //        }
            //        else
            //        {
            //            view.FindViewById<TextView>(Resource.Id.VisitaLatitud).Text = "Error";
            //            view.FindViewById<TextView>(Resource.Id.VisitaLongitud).Text = "Error";
            //        }
            //    }
            //    else
            //    {
            //        RequestLocationPermission(RC_LAST_LOCATION_PERMISSION_CHECK);
            //    }
            //}
            //else
            //{
            //    Snackbar.Make(rootLayout, "Permisos denegados, tendras problemas para guarda la tu ubicacion", Snackbar.LengthIndefinite)
            //                .SetAction("ok", delegate { Activity.FinishAndRemoveTask(); })
            //                .Show();
            //}

            if (isRequestingLocationUpdates)
            {
                isRequestingLocationUpdates = false;
                StopRequestingLocationUpdates();
            }
            else
            {
                if (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.AccessFineLocation) == Permission.Granted)
                {
                    StartRequestingLocationUpdates();
                    isRequestingLocationUpdates = true;
                }
                else
                {
                    RequestLocationPermission(RC_LAST_LOCATION_PERMISSION_CHECK);
                }
            }


            latitude.TextChanged += (sender, args) =>
            {
                var texto = ((TextView)sender).Text;
                //Lan.Latitud = Convert.ToDouble(texto);
                Latitud = Convert.ToDouble(texto);
            };

            longitude.TextChanged += (sender, args) =>
            {
                var texto = ((TextView)sender).Text;
                //Lan.Longitud = Convert.ToDouble(texto);
                Longitud = Convert.ToDouble(texto);
            };


        }

        void StopRequestingLocationUpdates()
        {
            latitude.Text = string.Empty;
            longitude.Text = string.Empty;
            //provider2.Text = string.Empty;

          //  requestLocationUpdatesButton.SetText(Resource.String.request_location_button_text);
            locationManager.RemoveUpdates(this);
        }    
        private async void BtnEnviarCoordenadas_Click(object sender, EventArgs e)
        {
           
            if (Latitud > 1 || Longitud > 1)
            {
                if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
                {
                    db = new ProveedorLocalDatabase();
                    if(vendedor.Valor.Almacen == "Supervisor")
                    {
                        var resultado = db.insertGeolocacionSupervisores(new GeoLocacionSupervisores(0, cliente.ClienteId, vendedor.Valor.Usuario, Convert.ToDecimal(Longitud), Convert.ToDecimal(Latitud)));
                        if (resultado)
                        {
                            Snackbar.Make(View, "Tus coordenadas se registraron localmente.", Snackbar.LengthLong).Show();
                            btnEnviarCoordenadas.Enabled = false;
                        }
                        else
                        {
                            Snackbar.Make(View, "Ocurrio un problema al guardar tu información local.", Snackbar.LengthLong)
                            .Show();
                        }
                    }
                    else
                    {
                        var resultado = db.insertGeolocacion(new GeoLocacion(0, cliente.ClienteId, Convert.ToDecimal(Longitud), Convert.ToDecimal(Latitud), vendedor.Valor.Id));
                        if (resultado)
                        {
                            Snackbar.Make(View, "Tus coordenadas se registraron localmente.", Snackbar.LengthLong).Show();
                            btnEnviarCoordenadas.Enabled = false;
                        }
                        else
                        {
                            Snackbar.Make(View, "Ocurrio un problema al guardar tu información local.", Snackbar.LengthLong)
                            .Show();
                        }
                    }
                    
                }
                else
                {
                    if (vendedor.Valor.Almacen == "Supervisor")
                    {
                        var geo = Task.Run(async () => {
                            resultadoInsert = await proveedorDeEstrategia.RegistrarGeoLocacionSupervisores(new GeoLocacionSupervisores(0, cliente.ClienteId, vendedor.Valor.Usuario ,Convert.ToDecimal(Longitud), Convert.ToDecimal(Latitud)));
                        });
                        geo.Wait();
                    }
                    else
                    {
                        var geo = Task.Run(async () => {
                            resultadoInsert = await proveedorDeEstrategia.RegistrarGeoLocacion(new GeoLocacion(0, cliente.ClienteId, Convert.ToDecimal(Longitud), Convert.ToDecimal(Latitud), vendedor.Valor.Id));
                        });
                        geo.Wait();
                    }
                                            
                   // proveedorDeLocacion.Remover();
                    if (resultadoInsert.Tipo == TipoDeResultado.Exito)
                    {
                        Snackbar.Make(View, "Tus coordenadas se registraron correctamente.", Snackbar.LengthLong)
                            .Show();
                        btnEnviarCoordenadas.Enabled = false;           
                        
                        if(vendedor.Valor.Almacen == "Supervisor")
                        {
                            var activity = new Android.Content.Intent(Activity, typeof(SeguimientoVentaActivity));
                            activity.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(vendedor));
                            activity.PutExtra(SeguimientoVentaActivity.SeguimientoVenta, ProveedorDeSerializado.Generar(vendedor));
                            StartActivity(activity);
                        }
                        else
                        {
                            var vistaDePregunta = LayoutInflater.Inflate(Resource.Layout.PreguntaFlujo, null);
                           
                            vistaDePregunta.FindViewById<TextView>(Resource.Id.PreguntaFlujo).Text =
                                       "¿Desea registrar venta?";

                            new Android.Support.V7.App.AlertDialog.Builder(Context)
                            .SetTitle(this.GetString(Resource.String.dialogFlujo))
                            .SetView(vistaDePregunta)
                            .SetPositiveButton(this.GetString(Resource.String.dialog_Si), (sender, args) =>
                            {
                               
                                if (ProveedorGlobal.Cliente.Bloqueado)
                                {
                                    //Toast.MakeText(this.activity, "El cliente está bloqueado.", ToastLength.Short).Show();
                                    Snackbar.Make(View, "El cliente está bloqueado no puedes generar la venta", Snackbar.LengthLong)
                                        .Show();
                                    return;
                                }
                                var arguments = new Bundle();
                                Arguments.PutString(CarritoFragment.LlaveDeCarrito, ProveedorDeManipulacionDeDatos.Generar(vendedor));
                                arguments = Arguments;
                                fragment = CarritoFragment.NewInstance();
                                fragment.Arguments = Arguments;
                                FragmentManager.BeginTransaction()
                                .Replace(Resource.Id.content_frame, fragment)
                                .Commit();

                            }).SetNegativeButton(this.GetString(Resource.String.dialog_No), (sender, args) => {

                                // =============================================================================================================//
                                var vistaDePreguntaCausas = LayoutInflater.Inflate(Resource.Layout.PreguntaFlujoCausasVenta, null);
                                vistaDePreguntaCausas.FindViewById<TextView>(Resource.Id.PreguntaFlujoCausas).Text =
                                       "¿Desea registrar la causa de no venta?";

                                 new Android.Support.V7.App.AlertDialog.Builder(Context)
                                .SetTitle(this.GetString(Resource.String.dialogFlujo))
                                .SetView(vistaDePreguntaCausas)
                                .SetPositiveButton(this.GetString(Resource.String.dialog_Si), (sender, args) =>
                                {
                                    var activity = new Android.Content.Intent(Activity, typeof(SeguimientoVentaActivity));
                                    activity.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(vendedor));
                                    activity.PutExtra(SeguimientoVentaActivity.SeguimientoVenta, ProveedorDeSerializado.Generar(vendedor));
                                    StartActivity(activity);

                                }).SetNegativeButton(this.GetString(Resource.String.dialog_No), (sender, args) => {

                                    var activity = new Android.Content.Intent(Activity, typeof(SeguimientoVentaActivity));
                                    activity.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(vendedor));
                                    activity.PutExtra(SeguimientoVentaActivity.SeguimientoVenta, ProveedorDeSerializado.Generar(vendedor));
                                    StartActivity(activity);
                                    //var arguments = new Bundle();
                                    //var clientes = Task.Run(async () => {
                                    //    resultadoCliente = await proveedorDeClientes.ObtenerTodosPorRuta(new DataVendedor(vendedor.Valor.Id));
                                    //});
                                    //clientes.Wait();                                    
                                    //if (resultadoCliente.Tipo == TipoDeResultado.Exito) 
                                    //{
                                    //    Arguments.PutString(RutaFragment.LlaveDeRuta, ProveedorDeManipulacionDeDatos.Generar(vendedor));
                                    //    Arguments.PutStringArray(RutaFragment.LlaveDeRuta, ProveedorDeManipulacionDeDatos.GenerarTodos(resultadoCliente.Valor.OrderBy(e => e.Secuencia)));
                                    //    arguments = Arguments;
                                    //    fragment = RutaFragment.NewInstance();
                                    //    fragment.Arguments = Arguments;
                                    //    FragmentManager.BeginTransaction()
                                    //    .Replace(Resource.Id.content_frame, fragment)
                                    //    .Commit();
                                    //}
                                    //else
                                    //{
                                    //    Snackbar.Make(View, "Ocurrio un error al regresar al menu rutas.", Snackbar.LengthLong)
                                    //    .Show();
                                    //    return;
                                    //}
                                }).Show();
                                
                            }).Show();
                        }                                 
                    }
                    else
                    {
                        Snackbar.Make(View, "Ocurrio un problema al guardar tu información.", Snackbar.LengthLong)
                        .Show();
                    }

                }
            }
            else
            {                
                Toast.MakeText(Activity, "Ocurrio un error en la busqueda de tus coordenadas. Limpia tu cache", ToastLength.Short).Show();
                //proveedorDeLocacion = new ProveedorDeLocacion(Activity);
                //proveedorDeLocacion.LocacionEncontrada += LocacionEncontrada;
            }
        }
        
      //  public override void OnResume()
      //  {
      //      base.OnResume();
      ////      proveedorDeLocacion.Obtener();
      //  }

      //  public override void OnPause()
      //  {
      //      base.OnPause();
      //  //    proveedorDeLocacion.Remover();
      //  }

        void RequestLocationPermission(int requestCode)
        {
            isRequestingLocationUpdates = false;
            if (ActivityCompat.ShouldShowRequestPermissionRationale(Activity, Manifest.Permission.AccessFineLocation))
            {
                Snackbar.Make(rootLayout, "La aplicacion necesita permisos de localizacion", Snackbar.LengthIndefinite)
                        .SetAction("OK",
                                   delegate
                                   {
                                       ActivityCompat.RequestPermissions(Activity, new[] { Manifest.Permission.AccessFineLocation }, requestCode);
                                   })
                        .Show();
            }
            else
            {
                ActivityCompat.RequestPermissions(Activity, new[] { Manifest.Permission.AccessFineLocation }, requestCode);
            }
        }

        public void OnLocationChanged(Location location)
        {
            latitude.Text = Convert.ToString(location.Latitude);
            longitude.Text = Convert.ToString(location.Longitude);
        }

        public void OnProviderDisabled(string provider)
        {
            isRequestingLocationUpdates = false;
            //requestLocationUpdatesButton.SetText(Resource.String.request_location_button_text);
            latitude.Text = string.Empty;
            longitude.Text = string.Empty;
            //provider2.Text = string.Empty;
        }

        public void OnProviderEnabled(string provider)
        {
            Log.Debug("LocationExample", "The provider " + provider + " is enabled.");
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            if (status == Availability.OutOfService)
            {
                StopRequestingLocationUpdates();
                isRequestingLocationUpdates = false;
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == RC_LAST_LOCATION_PERMISSION_CHECK || requestCode == RC_LOCATION_UPDATES_PERMISSION_CHECK)
            {
                if (grantResults.Length == 1 && grantResults[0] == Permission.Granted)
                {
                    if (requestCode == RC_LAST_LOCATION_PERMISSION_CHECK)
                    {
                        GetLastLocationFromDevice();
                    }
                    else
                    {
                        isRequestingLocationUpdates = true;
                        StartRequestingLocationUpdates();
                    }
                }
                else
                {
                    Snackbar.Make(rootLayout, "La aplicacion necesita permisos de localizacion", Snackbar.LengthIndefinite)
                            .SetAction("OK", delegate { Activity.FinishAndRemoveTask(); })
                            .Show();
                    return;
                }
            }
            else
            {
                Log.Debug("LocationSample", "Don't know how to handle requestCode " + requestCode);
            }

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void StartRequestingLocationUpdates()
        {
            //requestLocationUpdatesButton.SetText(Resource.String.request_location_in_progress_button_text);
            locationManager.RequestLocationUpdates(LocationManager.GpsProvider, ONE_MINUTE, 1, this);
        }

        void GetLastLocationFromDevice()
        {
           // getLastLocationButton.SetText(Resource.String.getting_last_location);

            var criteria = new Criteria { PowerRequirement = Power.Medium };

            var bestProvider = locationManager.GetBestProvider(criteria, true);
            var location = locationManager.GetLastKnownLocation(bestProvider);

            if (location != null)
            {
                latitude.Text = Resources.GetString(Resource.String.latitude_string, location.Latitude);
                longitude.Text = Resources.GetString(Resource.String.longitude_string, location.Longitude);
            
            }
            else
            {
                latitude.SetText(Resource.String.location_unavailable);
                longitude.SetText(Resource.String.location_unavailable);
            
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