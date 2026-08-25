using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
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
    public class DescargarRutaFragment : ListFragment
    {
        public const string LLaveDescargarRuta = "LLaveDescargarRuta";
        private ResultadoDeOperacionGenerico<Vendedor> vendedor;
        ProveedorDeClientes proveedorDeClientes = new ProveedorDeClientes();
        ProveedorLocalDatabase db;
        List<ClienteMovil> LstClienteMovil = new List<ClienteMovil>();
        ResultadoDeOperacionGenerico<List<Cliente>> resultadoCliente;
        Timer timer;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            vendedor = ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Arguments.Obtener<string>(MainActivity.LlaveUsuario));
            var descargar = Task.Run(async () => {
                resultadoCliente = await proveedorDeClientes.ObtenerTodosPorRuta(new DataVendedor(vendedor.Valor.Id));
            });
            descargar.Wait();
            if(resultadoCliente.Tipo == TipoDeResultado.Exito)
            {
                Activity.MostrarMensaje("Descargando tu ruta.");
                foreach(var ListaClienteMovil in resultadoCliente.Valor)
                {
                    db = new ProveedorLocalDatabase();
                    db.insertIntoTableRutas(new ClienteMovil(0,ListaClienteMovil.ClienteId,ListaClienteMovil.Nombre,
                                                             ListaClienteMovil.TipoSocio,ListaClienteMovil.TipoDeVisita,                                                             
                                                             ListaClienteMovil.Bloqueado, ListaClienteMovil.Secuencia,
                                                             ListaClienteMovil.Visitado
                                            ));
                }                
            }
        }
        public static DescargarRutaFragment NewInstance()
        {
            var frag1 = new DescargarRutaFragment { Arguments = new Bundle() };
            return frag1;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {         

            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.DescargarRuta, null);
            if(db.SelectClientes() != null)
            LstClienteMovil = db.SelectClientes();

            ProveedorGlobal.ClienteMovil = LstClienteMovil;
            AsignarClientesOffline();
            return view;
        }
        private void AsignarClientesOffline()
        {
            Activity.RunOnUiThread(() =>
            {
                
                ListAdapter = new AdaptadorRutasOffline(Activity, LstClienteMovil);

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