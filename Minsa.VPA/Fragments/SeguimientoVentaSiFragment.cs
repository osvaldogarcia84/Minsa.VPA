using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Grantland.Widget;
using Minsa.VPA.Adaptadores;
using Minsa.VPA.Enums;
using Minsa.VPA.Modelos;
using Minsa.VPA.Proveedores;
using Minsa.VPA.Repositorio;
using Minsa.VPA.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Minsa.VPA.Fragments
{
    public class SeguimientoVentaSiFragment : Fragment
    {
        public const string SeguimientoVentaSi = "SeguimientoVentaSi";
        ResultadoDeOperacionGenerico<List<Respuesta>> resultadoDeRespuestas;
        private ResultadoDeOperacionGenerico<Vendedor> vendedor;
        private string ordenId;
        public ResultadoDeVentaMovil resultadoDeVentaMovil;
        private TipoDeCausa _tipoDeCausa;
        //private List<Respuesta> _respuestas;
        private SeguimientoDeVenta resultadoDeVenta;
        //private List<SeguimientoDeVenta> ListaresultadoDeVenta = new List<SeguimientoDeVenta>();
        private List<Respuesta> _razones;
        private List<Harinera> _harineras;
        Button Enviar;
        Cliente cliente;
        //AutofitTextView NombreVendedorVenta;
        ProveedorDeEstrategia proveedorDeEstrategia = new ProveedorDeEstrategia();
        public const string InformacionDeTipoDeCausa = "TipoDeCausa";
        public const string InformacionDeHarineas = "Harineras";
        public const string InformacionDeRazones = "Razones";
        Timer timer;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            vendedor = Servicios.ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Arguments.Obtener<string>(MainActivity.LlaveUsuario));
            cliente = ProveedorGlobal.Cliente;

            _tipoDeCausa = ordenId.EsValido() ? TipoDeCausa.Venta : TipoDeCausa.NoVenta;
            _harineras = ProveedorDeManipulacionDeDatos.ObtenerTodos<Harinera>(
                    Arguments.Obtener<string[]>(InformacionDeHarineas)).ConvertirLista();
            _razones = ProveedorDeManipulacionDeDatos.ObtenerTodos<Respuesta>(
                    Arguments.Obtener<string[]>(InformacionDeRazones)).ConvertirLista();
            resultadoDeVenta = new SeguimientoDeVenta(ProveedorGlobal.Cliente.ClienteId, 0, 1, 0);
            resultadoDeVentaMovil = new ResultadoDeVentaMovil(0, ProveedorGlobal.Cliente.ClienteId, DateTime.Now, null, null);
            //ordenId = ProveedorDeManipulacionDeDatos.Obtener<string>(
            //       Arguments.Obtener<string>(ProveedorGlobal.InformacionOrdenId));
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view;
            if (_razones == null)
            {
                var resultado = Task.Run(async () => {
                    resultadoDeRespuestas = await proveedorDeEstrategia.ObtenerCausasDeResultadoDeVenta();
                });
                resultado.Wait();
                
                if (resultadoDeRespuestas.Tipo == TipoDeResultado.Exito)
                {
                    _razones = resultadoDeRespuestas.Valor;
                    Arguments.PutStringArray(InformacionDeRazones,
                        ProveedorDeManipulacionDeDatos.GenerarTodos(_razones));
                }
                else
                    Snackbar.Make(View, "Error: " + resultadoDeRespuestas, Snackbar.LengthLong)
                        .Show();
            }
            if (_harineras == null)
            {
                var resultadoDeHarineras = proveedorDeEstrategia.ObtenerHarineras();
                if (resultadoDeHarineras.Tipo == TipoDeResultado.Exito)
                {
                    _harineras = resultadoDeHarineras.Valor;
                    Arguments.PutStringArray(InformacionDeHarineas,
                        ProveedorDeManipulacionDeDatos.GenerarTodos(_harineras));
                }
                else
                    Snackbar.Make(View, "Error: " + resultadoDeHarineras.Valor, Snackbar.LengthLong)
                        .Show();
            }

            // ProveedorGlobal.OrdenId = "OC-52125405";
            //if (ProveedorGlobal.OrdenId != null)
            // {
            //     view = VistaVentaSi(inflater, container);        
            // }
            if (_razones != null && _harineras != null)
            {
                view = ConfigurarVista(inflater, container);
            }
            else
            {
                view = inflater.Inflate(Resource.Layout.Error, container, false);
                view.FindViewById<TextView>(Resource.Id.ErrorTexto).Text =
                    "No se encontraron respuestas para el cliente.";
            }

            return view;
        }
        public static SeguimientoVentaFragment NewInstance()
        {
            var frag1 = new SeguimientoVentaFragment { Arguments = new Bundle() };
            return frag1;
        }

        private View ConfigurarVista(LayoutInflater inflater, ViewGroup container)
        {
            var view = inflater.Inflate(Resource.Layout.SeguimientoVentaSiNo, container, false);
            view.FindViewById<TextView>(Resource.Id.SeguimientoDeVentaComparacionTexto).Visibility =
               _tipoDeCausa == TipoDeCausa.NoVenta ? ViewStates.Gone : ViewStates.Visible;
            view.FindViewById<RadioGroup>(Resource.Id.SeguimientoDeVentaComparacionGrupo).Visibility =
                _tipoDeCausa == TipoDeCausa.NoVenta ? ViewStates.Gone : ViewStates.Visible;
            view.FindViewById<CheckBox>(Resource.Id.SeguimientoDeVentaCompetenciaRespuesta).Checked =
                _tipoDeCausa == TipoDeCausa.NoVenta;
            view.FindViewById<Spinner>(Resource.Id.SeguimientoDeVentaCompetencia).Visibility =
                _tipoDeCausa == TipoDeCausa.NoVenta ? ViewStates.Visible : ViewStates.Gone;
            return view;
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