using Android.Locations;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Minsa.VPA.Modelos;
using Minsa.VPA.Proveedores;
using Minsa.VPA.Repositorio;
using Minsa.VPA.Servicios;
using System;
using System.Timers;

namespace Minsa.VPA.Fragments
{
    public class EntradasSalidasFragment : Fragment
    {
        public static string LlaveEntradasSalidas = "LlaveEntradasSalidas";
        public ResultadoDeOperacionGenerico<Vendedor> vendedor;
        Cliente cliente;
        ProveedorDeUsuario proveedorDeUsuario = new ProveedorDeUsuario();
        private ProveedorDeLocacion proveedorDeLocacion;
        Button btnEnviarRegistroCheck;
        ProveedorDeClientes proveedorDeCliente = new ProveedorDeClientes();
        private int kmInicial, kmFinal;
        Timer timer;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            cliente = ProveedorGlobal.Cliente;
            vendedor = ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Arguments.Obtener<string>(MainActivity.LlaveUsuario));
            try
            {
                //proveedorDeLocacion = new ProveedorDeLocacion(Activity, 15); // 900000 15 minutos
                //proveedorDeLocacion.LocacionEncontrada += ActualizarLocacion;
                if (ProveedorGlobal.Lan == null)
                {
                    ProveedorGlobal.Lan = new Lan(0, 0);
                    //lineaMovil.CoordenadaX = Convert.ToDecimal(ProveedorGlobal.Lan.Longitud);
                    //lineaMovil.CoordenadaY = Convert.ToDecimal(ProveedorGlobal.Lan.Latitud);
                }
            }
            catch(Exception ex)
            {
                Activity.MostrarMensaje(ex.Message);
            }
        }

        public static EntradasSalidasFragment NewInstance()
        {
            var frag1 = new EntradasSalidasFragment { Arguments = new Bundle() };
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
            View view = inflater.Inflate(Resource.Layout.EntradasSalidas, container, false);           
            btnEnviarRegistroCheck = view.FindViewById<Button>(Resource.Id.EnviarRegistroCheck);
            view.FindViewById<EditText>(Resource.Id.KMInicial).TextChanged += (sender, args) =>
            {
                kmInicial = ((EditText)sender).Text.ObtenerCantidadEntera();
            };
            view.FindViewById<EditText>(Resource.Id.KMFinal).TextChanged += (sender, args) =>
            {
                kmFinal = ((EditText)sender).Text.ObtenerCantidadEntera();
            };
            btnEnviarRegistroCheck.Click += BtnEnviarRegistroCheck_Click;               
            return view;
        }

        private void BtnEnviarRegistroCheck_Click(object sender, EventArgs e)
        {
            //if (ProveedorGlobal.Lan != null)
            //{
            //    if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            //    {
            //        var resultadoInfomacionDeInventario = proveedorDeCliente.ObtenerInformacionDeInventario(new LlaveVendedor(vendedor.Valor.Id, vendedor.Valor.Usuario, vendedor.Valor.Nombre, InformacionGeneral.InformacionDeTipoDeOrden));
            //        var Sitio = resultadoInfomacionDeInventario.Valor;
            //        var ObtenerHora = DateTime.Now.ToString("HH");
            //        if (Convert.ToDecimal(ObtenerHora) >= 04 && Convert.ToDecimal(ObtenerHora) <= 10)
            //        {
            //            //================================================= Valida km Inicial ===============================================
            //            if(kmInicial == 0)
            //            {
            //                Snackbar.Make(View, "No haz ingresado el kilometraje inical.", Snackbar.LengthLong)
            //                .Show();
            //                return;
            //            }

            //            var CheckIn = proveedorDeUsuario.CheckIn(new CheckIn(Sitio, vendedor.Valor.Id, Convert.ToDecimal(ProveedorGlobal.Lan.Latitud),Convert.ToDecimal(ProveedorGlobal.Lan.Longitud), DateTime.Now,kmInicial));
            //            if(CheckIn.Tipo == TipoDeResultado.Exito)
            //            {
            //                Snackbar.Make(View, "CheckIn Exitoso", Snackbar.LengthLong)
            //                .Show();
            //            }
            //            else
            //            {
            //                Snackbar.Make(View, "Ocurrio un error al registrarse", Snackbar.LengthLong)
            //                .Show();
            //            }
            //        }
            //        else if (Convert.ToDecimal(ObtenerHora) >= 16 && Convert.ToDecimal(ObtenerHora) <= 22)
            //        {
            //            //================================================= Valida km Final ===============================================

            //            if (kmFinal == 0)
            //            {
            //                Snackbar.Make(View, "No haz ingresado el kilometraje final.", Snackbar.LengthLong)
            //                .Show();
            //                return;
            //            }
            //            var CheckOut = proveedorDeUsuario.CheckOut(new CheckOut(Sitio, vendedor.Valor.Id, Convert.ToDecimal(ProveedorGlobal.Lan.Latitud), Convert.ToDecimal(ProveedorGlobal.Lan.Longitud), kmFinal));
            //            if (CheckOut.Tipo == TipoDeResultado.Exito)
            //            {
            //                Snackbar.Make(View, "CheckOut Exitoso", Snackbar.LengthLong)
            //                .Show();
            //            }
            //            else
            //            {
            //                Snackbar.Make(View, "Ocurrio un error al registrarse", Snackbar.LengthLong)
            //                .Show();
            //            }
            //        }
            //        else
            //        {
            //            Snackbar.Make(View, "Estas fuera del horario de Entrada/Salida", Snackbar.LengthLong)
            //            .Show();
            //        }
            //    }
            //    else
            //    {
            //        Snackbar.Make(View, "Ocurrio un problema, no estas conectado a internet.", Snackbar.LengthLong)
            //            .Show();
            //    }
            //}
            //else
            //{
            //    Toast.MakeText(Activity, "Espera porfavor, estamos buscando tu ubicacion.", ToastLength.Short).Show();
            //    proveedorDeLocacion = new ProveedorDeLocacion(Activity);
            //    proveedorDeLocacion.LocacionEncontrada += ActualizarLocacion;
            //}       
        }

        private void ConfigurarModelo(View view)
        {           
            view.FindViewById<TextView>(Resource.Id.VisitaLatitudCheck).TextChanged += (sender, args) =>
            {
                var texto = ((TextView)sender).Text;
                ProveedorGlobal.Lan.Latitud = Convert.ToDouble(texto);
            };

            view.FindViewById<TextView>(Resource.Id.VisitaLongitudCheck).TextChanged += (sender, args) =>
            {
                var texto = ((TextView)sender).Text;
                ProveedorGlobal.Lan.Longitud = Convert.ToDouble(texto);
            };
        }
       
        //public override void OnResume()
        //{
        //    base.OnResume();
        //  //  proveedorDeLocacion.Obtener();
        //}

        //public override void OnPause()
        //{
        //    base.OnPause();
        //   // proveedorDeLocacion.Remover();
        //}
        private void ActualizarLocacion(object sender, Location location)
        {
            if (View != null)
            {
                View.FindViewById<TextView>(Resource.Id.VisitaLatitudCheck).Text = Convert.ToString(location.Latitude);
                View.FindViewById<TextView>(Resource.Id.VisitaLongitudCheck).Text = Convert.ToString(location.Longitude);
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