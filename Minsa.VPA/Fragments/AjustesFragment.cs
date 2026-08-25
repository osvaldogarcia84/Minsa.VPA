using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Minsa.VPA.Servicios;
using System;
using System.Timers;

namespace Minsa.VPA.Fragments
{
    public class AjustesFragment : Fragment
    {
        Switch LimpiarCache;
        Button BtnLimpiarCache;
        Timer timer;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.Ajustes, null);
            LimpiarCache = view.FindViewById<Switch>(Resource.Id.LimpiarCache);
            LimpiarCache.CheckedChange += LimpiarCache_CheckedChange;
            BtnLimpiarCache = view.FindViewById<Button>(Resource.Id.btnLimpiarCache);
            BtnLimpiarCache.Click += BtnLimpiarCache_Click;

            return view;

        }
        public static AjustesFragment NewInstance()
        {
            var frag1 = new AjustesFragment { Arguments = new Bundle() };
            return frag1;
        }
        private void BtnLimpiarCache_Click(object sender, EventArgs e)
        {
            Dialog();
        }
        public void Dialog()
        {
            new Android.Support.V7.App.AlertDialog.Builder(View.Context)
                       .SetTitle(View.Context.GetString(Resource.String.main_dialog_simple_title))
                       .SetMessage(View.Context.GetString(Resource.String.main_dialog_simple_message))
                       .SetPositiveButton(View.Context.GetString(Resource.String.dialog_ok), (sender, args) => {

                        ((Android.App.ActivityManager)Android.App.Application.Context.GetSystemService(Context.ActivityService)).ClearApplicationUserData();
                          try
                          {
                              var cachePath = System.IO.Path.GetTempPath();

                              // If exist, delete the cache directory and everything in it recursivly
                              if (System.IO.Directory.Exists(cachePath))
                                  System.IO.Directory.Delete(cachePath, true);

                              // If not exist, restore just the directory that was deleted
                              if (!System.IO.Directory.Exists(cachePath))
                                  System.IO.Directory.CreateDirectory(cachePath);

                              var activity = new Android.Content.Intent(Activity, typeof(MainActivity));                                                       
                              StartActivityForResult(activity, 0);
                          }
                          catch (Exception ex)
                          {
                              Snackbar.Make(View, "Ocurrio un problema: " + ex, Snackbar.LengthLong)
                                 .Show();
                          }
                      })
                      .SetNegativeButton(View.Context.GetString(Resource.String.dialog_cancel), (sender, args) => { })
                     // .SetNeutralButton(View.Context.GetString(Resource.String.dialog_neutral), (sender, args) => { })
                      .Show();
        }

        private void LimpiarCache_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            Snackbar.Make(View, "Se limpiara el cache de la aplicacion. Te redirigira al login.", Snackbar.LengthLong)
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