using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Minsa.VPA.Fragments;
using Minsa.VPA.Modelos;
using Minsa.VPA.Proveedores;
using Minsa.VPA.Servicios;

namespace Minsa.VPA.Activities
{
    [Activity(Label = "Cerrar venta", LaunchMode = LaunchMode.SingleTop, ScreenOrientation = ScreenOrientation.Portrait)]
    public class CerrarVentaCompletaActivity : AppCompatActivity
    {
        public const string LlaveVentaCompleta = "LlaveVentaCompleta";
        private ResultadoDeOperacionGenerico<Vendedor> vendedor;
        public ClientesProspecto ClientesProspecto;
        ProveedorDeEstrategia proveedorDeEstrategia = new ProveedorDeEstrategia();
        Android.Support.V4.App.Fragment fragment = null;
        public CierreVentaFragment CierreVentaFragment = new CierreVentaFragment();
        Timer timer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            vendedor = Servicios.ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Intent.Obtener<string>(MainActivity.LlaveUsuario));
            ClientesProspecto = ProveedorDeSerializado.Obtener<ClientesProspecto>(Intent.Obtener<string>(LlaveVentaCompleta));
            SetContentView(Resource.Layout.CerrarVentaCompleta);

            var cerrada = ClientesProspecto.Cerrada == false ? "Abierta" : "Cerrada";

            FindViewById<TextView>(Resource.Id.NombreProspecto).Text = "Nombre: " +ClientesProspecto.Nombre;
            FindViewById<TextView>(Resource.Id.CodigoProspecto).Text = "Codigo: " +ClientesProspecto.IdCliente;
            FindViewById<TextView>(Resource.Id.ProspectoVolumen).Text = "Volumen: " +Convert.ToString(ClientesProspecto.Volumen);
            FindViewById<TextView>(Resource.Id.ProspectoEstrategia).Text = "Estrategia: " + ClientesProspecto.EstrategiaCerrarVenta;
            FindViewById<TextView>(Resource.Id.ProspectoPlazo).Text = "Plazo: " + ClientesProspecto.PlazoEstrategia;
            FindViewById<TextView>(Resource.Id.ProspectoCantidad).Text = "Cantidad: " + Convert.ToString(ClientesProspecto.Cantidad);
            FindViewById<TextView>(Resource.Id.ProspectoMarca).Text = "Marca: " + ClientesProspecto.Marca;
            FindViewById<TextView>(Resource.Id.ProspectoRecurso).Text = "Recurso: "+ ClientesProspecto.Recurso;
            FindViewById<TextView>(Resource.Id.ProspectoPrecio).Text = "Precio: " + Convert.ToString(String.Format("{0:C}",ClientesProspecto.Precio));
            FindViewById<TextView>(Resource.Id.ProspectoTelefono).Text = "Telefono: " +ClientesProspecto.Telefono;
            FindViewById<TextView>(Resource.Id.ProspectoCorreo).Text = "Correo: " + ClientesProspecto.Correo;
            FindViewById<TextView>(Resource.Id.ProspectoVenta).Text = "Estatus de la venta: " + cerrada;
            FindViewById<TextView>(Resource.Id.ProspectoFecha).Text = "Fecha: " + ClientesProspecto.FechaCreacion;
            if (ClientesProspecto.Cerrada == false)
            {

                FindViewById<TextView>(Resource.Id.BtnCerraVenta).Click += CerrarVentaCompletaActivity_Click;
            }
            else
            {
                // FindViewById<TextView>(Resource.Id.BtnCerraVenta).Enabled = true;
                FindViewById<Button>(Resource.Id.BtnCerraVenta).Enabled = false;
                Toast.MakeText(this, "La venta esta cerrada", ToastLength.Long).Show();
            }
            
            
            
        }

        private void CerrarVentaCompletaActivity_Click(object sender, EventArgs e)
        {
            try
            {
                
                var resultado = proveedorDeEstrategia.UpdateVentaVPA(new CerrarVenta(ClientesProspecto.Sitio,ClientesProspecto.IdCliente));
                if(resultado.Tipo == TipoDeResultado.Exito)
                {
                    Toast.MakeText(this, "Se cerro correctamente la venta", ToastLength.Long).Show();             
                    var activity = new Android.Content.Intent(this, typeof(ContenidoActivity));
                    activity.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(vendedor));
                    activity.PutExtra(LlaveVentaCompleta, ProveedorDeSerializado.Generar(ClientesProspecto));
                    SetResult(Result.Ok, activity);
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, "Ocurrio un problema.", ToastLength.Long).Show();
                }
            }catch(Exception ex)
            {
                Toast.MakeText(this, "Error: " + ex, ToastLength.Long).Show();
            }
        }

        private void DesactivarVistas(View view = null)
        {
            var currentView = view;
            currentView.FindViewById<Button>(Resource.Id.BtnCerraVenta).Enabled = false;
            
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
            Java.Lang.JavaSystem.Exit(0);
        }
    }
}