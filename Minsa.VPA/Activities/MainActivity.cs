using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Views.InputMethods;
using Minsa.VPA.Proveedores;
using Minsa.VPA.Servicios;
using System;
using Minsa.VPA.Modelos;
using Minsa.VPA.Activities;
using Android.Content;
using System.Threading.Tasks;
using Android.Runtime;
using Android;
using System.IO;
using Android.Util;
using Android.Views;
using Minsa.VPA.Repositorio;
using Android.Support.Design.Widget;
using Android.Content.PM;
using Android.Locations;
using Android.Support.V4.App;
using Plugin.Connectivity;
using System.Timers;

namespace Minsa.VPA
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, View.IOnClickListener
    {
        public const string LlaveUsuario = "Usuario";
        ProveedorDeUsuario _proveedorDeUsuario = new ProveedorDeUsuario();
        Configuracion Configuracion = new Configuracion();
        LocationManager locationManager;
        Button btnAutentificar;
        bool isRequestingLocationUpdates;
        ProveedorLocalDatabase db;      
        public string nombreDeUsuario;
        public string contraseña;
        public string horaInicialDB;
        public string horaFinalDB;
        public string Version;
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        String databaseName = "DBVPA.db";
        View rootLayout;
        const long ONE_MINUTE = 60 * 1000;
        const long FIVE_MINUTES = 5 * ONE_MINUTE;
        static readonly string KEY_REQUESTING_LOCATION_UPDATES = "requesting_location_updates";

        static readonly int RC_LAST_LOCATION_PERMISSION_CHECK = 1000;
        static readonly int RC_LOCATION_UPDATES_PERMISSION_CHECK = 1100;
        ResultadoDeOperacionGenerico<string> resultadoKMInicial;
        public string NombreDeUsuario
        {
            get { return FindViewById<EditText>(Resource.Id.AccesoUsuario).Text; }
        }

        public string Contraseña
        {
            get { return FindViewById<EditText>(Resource.Id.AccesoPassword).Text; }
        }
        Timer timer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
           
            RequestLocationPermission(RC_LAST_LOCATION_PERMISSION_CHECK);
            CheckAppPermissions();
          //  TryToGetPermissions();
            // SetDozeOptimization();
            var dbFile = Path.Combine(folder, databaseName);
            if (!File.Exists(dbFile))
            {
                db = new ProveedorLocalDatabase();
                db.createDataBase();
                Log.Info("DB_PATH", folder);
            }
      
            SetContentView(Resource.Layout.activity_main);
            btnAutentificar = FindViewById<Button>(Resource.Id.AccesoEnviar);
            btnAutentificar.SetOnClickListener(this);
            //ProveedorGlobal.Conexion = Configuracion.Conexion();
            
        }
        [Obsolete]
        public async void OnClick(View v)
        {
            try
            {
                nombreDeUsuario = NombreDeUsuario;
                contraseña = Contraseña;

                if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
                {
                    Toast.MakeText(this, "No estas conectado a internet, revisar tu conexion WIFI o Red Movil.", ToastLength.Short).Show();                 
                    return;
                }
                //string HoraInicial = String.Empty;
                //string HoraFinal = String.Empty;

                var validaInicioSesion =  await _proveedorDeUsuario.ValidaInicioSesion();
                if(validaInicioSesion.Tipo == TipoDeResultado.Exito)
                {
                    horaInicialDB = validaInicioSesion.Valor.HoraInicial;
                    horaFinalDB = validaInicioSesion.Valor.HoraFinal;
                    Version = validaInicioSesion.Valor.Version;
                }
                else
                {
                    Toast.MakeText(this, "No es posible establecer la conexion con el servidor intenta mas tarde o revisar tu conexion a internet.", ToastLength.Short).Show();
                    return;
                }

                if(Version != Configuracion.Version)
                {
                    Toast.MakeText(this, "Tu aplicación no esta actualizada, solicita la ultima version de la APP VPA", ToastLength.Short).Show();
                    return;
                }

                DateTime horaActual = DateTime.Now;
                horaActual.ToString("HH:mm:ss tt");
                DateTime HoraInicial = DateTime.Parse(horaInicialDB);
                DateTime HoraFinal = DateTime.Parse(horaFinalDB);
               
                ProgressDialog progressDialog = new ProgressDialog(v.Context, Resource.Style.MyAlertDialogStyle);
                progressDialog.SetMessage(v.Context.GetString(Resource.String.main_dialog_progress_title));
                progressDialog.SetCancelable(false);
                progressDialog.Show();
                DismissKeyboard();

                if (nombreDeUsuario.EsValido() || contraseña.EsValido())
                {
                 // ==============================================================================================================================================//
                    var usuarioMinsa = await _proveedorDeUsuario.Autorizar(nombreDeUsuario, contraseña);                 
                    
                    if (usuarioMinsa.Tipo == Modelos.TipoDeResultado.Exito)
                    {   
                        if (horaActual >= HoraInicial && horaActual <= HoraFinal)
                        {
                            var validaKmInicial = await _proveedorDeUsuario.validaKmInicial(new DataValidaKmInicial(nombreDeUsuario, usuarioMinsa.Valor.Almacen));
                            if(validaKmInicial.Tipo == TipoDeResultado.Exito)
                            {
                                Navegar(usuarioMinsa);
                            }
                            else
                            {
                                if (validaKmInicial.Valor == null)
                                {
                                    var vistaDePreguntaKM = LayoutInflater.Inflate(Resource.Layout.PreguntaKM, null);

                                    vistaDePreguntaKM.FindViewById<TextView>(Resource.Id.PreguntaSimpleTexto1).Text =
                                               "Ingresa el Kilometraje de tu unidad para poder acceder a la app.";


                                    new Android.Support.V7.App.AlertDialog.Builder(this)
                                           .SetTitle(this.GetString(Resource.String.dialogRegistroKM))
                                           .SetView(vistaDePreguntaKM)
                                           //.SetMessage(this.GetString(Resource.String.main_dialog_simple_message))
                                           .SetPositiveButton(this.GetString(Resource.String.dialog_ok), (sender, args) =>
                                           {
                                               string KM = vistaDePreguntaKM.FindViewById<EditText>(Resource.Id.PreguntaSimpleRespuestaKM).Text;

                                               var obtenerKm = Task.Run(async () => {
                                                   resultadoKMInicial = await _proveedorDeUsuario.KmInicial(new DataKmInicial(nombreDeUsuario, usuarioMinsa.Valor.Nombre, usuarioMinsa.Valor.Almacen, usuarioMinsa.Valor.Id, Convert.ToInt32(KM)));
                                               });
                                               obtenerKm.Wait();

                                               if (resultadoKMInicial.Tipo == TipoDeResultado.Exito)
                                               {
                                                   //if (resultadoKMInicial.Valor == "1")
                                                   //{
                                                   //    Toast.MakeText(this, "La información ha sido guardada correctamente. ", ToastLength.Short).Show();
                                                   //    return;
                                                   //    progressDialog.Dismiss();
                                                   //    Navegar(usuarioMinsa);
                                                   //}
                                                   //else
                                                   //{

                                                   //}
                                                   progressDialog.Dismiss();
                                                   Navegar(usuarioMinsa);
                                               }
                                               else
                                               {
                                                   progressDialog.Dismiss();
                                                   Toast.MakeText(this, "Ocurrio un error al guardar la información.", ToastLength.Short).Show();
                                                   return;
                                               }

                                           })
                                           .SetNegativeButton(this.GetString(Resource.String.dialog_cancel), (sender, args) => { })
                                           // .SetNeutralButton(View.Context.GetString(Resource.String.dialog_neutral), (sender, args) => { })
                                           .Show();
                                }
                                else
                                {
                                    progressDialog.Dismiss();
                                    Navegar(usuarioMinsa);
                                }
                            }                          
                        }
                        else
                        {
                            progressDialog.Dismiss();
                            Navegar(usuarioMinsa);
                        }                        
                    }
                    else
                    {
                        progressDialog.Dismiss();
                        Toast.MakeText(this, usuarioMinsa.Mensaje, ToastLength.Long).Show();

                    }
                    // ============================================================== Supervisores ================================================================================//

                    var usuarioSupervisores = await _proveedorDeUsuario.AutorizarUsuarios(nombreDeUsuario, contraseña);

                    if(usuarioSupervisores.Tipo == TipoDeResultado.Exito)
                    {
                        progressDialog.Dismiss();
                        Navegar(usuarioSupervisores);
                    }
                    else
                    {
                        progressDialog.Dismiss();
                        Toast.MakeText(this, usuarioMinsa.Mensaje, ToastLength.Long).Show();

                    }
                }
                else
                {
                    RunOnUiThread(() =>
                    {
                        progressDialog.Dismiss();
                        Toast.MakeText(this, "Debe llenar todos los campos.", ToastLength.Long).Show();
                        return;
                    });
                }
            }catch(Exception ex)
            {
                Toast.MakeText(this, "Ocurrio un error: " + ex, ToastLength.Long).Show();
            }
        }

        private void DismissKeyboard()
        {
            var view = CurrentFocus;
            if (view != null)
            {
                var imm = (InputMethodManager)GetSystemService(InputMethodService);
                imm.HideSoftInputFromWindow(view.WindowToken, 0);
            }
        }
        private void Navegar(ResultadoDeOperacionGenerico<Vendedor> usuarioMinsa)
        {
            Type actividad = null;
            actividad = typeof(ContenidoActivity);
            var pantalla = new Intent(this, actividad);
            pantalla.PutExtra(LlaveUsuario, ProveedorDeSerializado.Generar(usuarioMinsa));
            StartActivity(pantalla);
        }

        void RequestLocationPermission(int requestCode)
        {
            isRequestingLocationUpdates = false;
            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation))
            {
                Snackbar.Make(rootLayout, "La aplicacion necesita permisos de localizacion", Snackbar.LengthIndefinite)
                        .SetAction("OK",
                                   delegate
                                   {
                                       ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.AccessFineLocation }, requestCode);
                                   })
                        .Show();
            }
            else
            {
                ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.AccessFineLocation }, requestCode);
            }
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
            Toast.MakeText(this, "Tu aplicación se cerrara en automatico por reglas de la empresa.", ToastLength.Long).Show();
            Java.Lang.JavaSystem.Exit(0);
        }

        #region RuntimePermissions

        //private async Task TryToGetPermissions()
        //{
        //    if ((int)Build.VERSION.SdkInt >= 23)
        //    {
        //        //  await GetPermissionsAsync();
        //        //   SetDozeOptimization();
        //        await CheckAppPermissions();
        //        return;
        //    }
        //}
        async Task CheckAppPermissions()
        {
            //if ((int)Build.VERSION.SdkInt >= 23)
            //{
            //    return;
            //}
            //else
            //{

            //}
            if (PackageManager.CheckPermission(Manifest.Permission.ReadExternalStorage, PackageName) != Permission.Granted
                    && PackageManager.CheckPermission(Manifest.Permission.WriteExternalStorage, PackageName) != Permission.Granted)
            {
                var permissions = new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
                RequestPermissions(permissions, 1);
            }
        }
        //const int RequestLocationId = 0;

        //readonly string[] PermissionsGroupLocation =
        //    {
        //                    //TODO add more permissions
        //                    Manifest.Permission.AccessCoarseLocation,
        //                    Manifest.Permission.AccessFineLocation,
        //     };
        //async Task GetPermissionsAsync()
        //{
        //    const string permission = Manifest.Permission.AccessFineLocation;

        //    if (CheckSelfPermission(permission) == (int)Android.Content.PM.Permission.Granted)
        //    {
        //        //TODO change the message to show the permissions name
        //        Toast.MakeText(this, "Permisos del GPS otorgados", ToastLength.Short).Show();
        //        return;
        //    }

        //    if (ShouldShowRequestPermissionRationale(permission))
        //    {
        //        //set alert for executing the task
        //        Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);
        //        alert.SetTitle("Permisos denegados, tendras problemas");
        //        alert.SetMessage("The application need special permissions to continue");
        //        alert.SetPositiveButton("Request Permissions", (senderAlert, args) =>
        //        {
        //            RequestPermissions(PermissionsGroupLocation, RequestLocationId);
        //        });

        //        alert.SetNegativeButton("Cancel", (senderAlert, args) =>
        //        {
        //            Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
        //        });

        //        Dialog dialog = alert.Create();
        //        dialog.Show();


        //        return;
        //    }

        //    RequestPermissions(PermissionsGroupLocation, RequestLocationId);

        //}
        //public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        //{
        //    //switch (requestCode)
        //    //{
        //    //    case RequestLocationId:
        //    //        {
        //    //            if (grantResults[0] == (int)Android.Content.PM.Permission.Granted)
        //    //            {
        //    //                Toast.MakeText(this, "Se ha activado correctamente el GPS", ToastLength.Short).Show();

        //    //            }
        //    //            else
        //    //            {
        //    //                //Permission Denied :(
        //    //                Toast.MakeText(this, "Permisos denegados, tendras problemas para guarda la tu ubicacion", ToastLength.Short).Show();

        //    //            }
        //    //        }
        //    //        break;
        //    //}
        //    //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        //    if (requestCode == RC_LAST_LOCATION_PERMISSION_CHECK || requestCode == RC_LOCATION_UPDATES_PERMISSION_CHECK)
        //    {
        //        if (grantResults.Length == 1 && grantResults[0] == Permission.Granted)
        //        {
        //            if (requestCode == RC_LAST_LOCATION_PERMISSION_CHECK)
        //            {
        //                GetLastLocationFromDevice();
        //            }
        //            else
        //            {
        //                isRequestingLocationUpdates = true;
        //               // StartRequestingLocationUpdates();
        //            }
        //        }
        //        else
        //        {
        //            Snackbar.Make(rootLayout, "Permisos denegados, tendras problemas para guarda la tu ubicacion", Snackbar.LengthIndefinite)
        //                    .SetAction("ok" ,delegate { FinishAndRemoveTask(); })
        //                    .Show();
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        Log.Debug("LocationSample", "Don't know how to handle requestCode " + requestCode);
        //    }

        //    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}

        //void GetLastLocationFromDevice()
        //{
        // //   getLastLocationButton.SetText(Resource.String.getting_last_location);

        //    var criteria = new Criteria { PowerRequirement = Power.Medium };

        //    var bestProvider = locationManager.GetBestProvider(criteria, true);
        //    var location = locationManager.GetLastKnownLocation(bestProvider);
        //    Toast.MakeText(this, "Latitud: " + location.Latitude + ", Longitud: " + location.Longitude , ToastLength.Short).Show();

        //}
        //void StartRequestingLocationUpdates()
        //{
        //  //  requestLocationUpdatesButton.SetText(Resource.String.request_location_in_progress_button_text);
        //    locationManager.RequestLocationUpdates(LocationManager.GpsProvider, ONE_MINUTE, 1, this);
        //}
        #endregion

        //public void SetDozeOptimization()
        //{
        //    Toast.MakeText(this, "Optimizando bateria", ToastLength.Short).Show();
        //    bool setDozeComplete;
        //    setDozeComplete = false;
        //    Intent intent = new Intent();
        //    String packageName = Manifest.Permission.RequestIgnoreBatteryOptimizations;
        //    PowerManager pm = (PowerManager)GetSystemService(Context.PowerService);
        //    //if (pm.IsIgnoringBatteryOptimizations(packageName))
        //    //    intent.SetAction("ACTION_IGNORE_BATTERY_OPTIMIZATION_SETTINGS");
        //    //else
        //    //{
        //    //setDozeComplete = pm.
        //    //setDozeComplete = pm.IsIgnoringBatteryOptimizations("ACTION_REQUEST_IGNORE_BATTERY_OPTIMIZATIONS");
        //    //intent.SetAction("ACTION_REQUEST_IGNORE_BATTERY_OPTIMIZATIONS");
        //    //Intent.ActionPowerUsageSummary;

        //    //intent.SetAction(Intent.ActionPowerUsageSummary);
        //    //intent.SetAction(Android.Provider.Settings.ActionIgnoreBatteryOptimizationSettings);
        //    //intent.SetAction(Android.Provider.Settings.ActionIgnoreBatteryOptimizationSettings);
        //    //intent.SetData(Android.Net.Uri.Parse("package:" + packageName));
        //    ////}       
        //    ///

        //    if (pm.IsIgnoringBatteryOptimizations(packageName))
        //        intent.SetAction("ACTION_IGNORE_BATTERY_OPTIMIZATION_SETTINGS");
        //    else
        //    {
        //        intent.SetAction("ACTION_REQUEST_IGNORE_BATTERY_OPTIMIZATIONS");
        //        intent.SetData(Android.Net.Uri.Parse("package:" + packageName));
        //    }
        //  // Context.StartActivity(intent);

        //}

    }
}

