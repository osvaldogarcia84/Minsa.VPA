using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using System;

namespace Minsa.VPA.Fragments
{
    public class SalirFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            ISharedPreferences pref = PreferenceManager.GetDefaultSharedPreferences(Activity);
            ISharedPreferencesEditor editer = pref.Edit();
            editer.Remove("PREFERENCE_ACCESS_KEY").Commit();
            editer.Remove("CLEAR_APP_CACHE").Commit();
            editer.Remove("CLEAR_APP_USER_DATA").Commit();
            var activity = new Android.Content.Intent(Activity, typeof(MainActivity));
            //activity.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(_usuario));
            var cookieManager = CookieManager.Instance;
            cookieManager.RemoveAllCookie();
            StartActivityForResult(activity, 0);
            base.OnCreate(savedInstanceState);
            //((Android.App.ActivityManager)Android.App.Application.Context.GetSystemService(Context.ActivityService)).ClearApplicationUserData();

            //try
            //{
            //    var cachePath = System.IO.Path.GetTempPath();

            //    // If exist, delete the cache directory and everything in it recursivly
            //    if (System.IO.Directory.Exists(cachePath))
            //        System.IO.Directory.Delete(cachePath, true);

            //    // If not exist, restore just the directory that was deleted
            //    if (!System.IO.Directory.Exists(cachePath))
            //        System.IO.Directory.CreateDirectory(cachePath);
            //}
            //catch (Exception)
            //{

            //}
        }
        public static SalirFragment NewInstance()
        {
            var frag2 = new SalirFragment { Arguments = new Bundle() };

            return frag2;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {


            return base.OnCreateView(inflater, container, savedInstanceState);
        }
    }
}