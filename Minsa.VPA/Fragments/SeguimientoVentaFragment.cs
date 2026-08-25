using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
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
    public class SeguimientoVentaFragment : Fragment
    {
        public const string SeguimientoVenta = "SeguimientoVenta";
        public ResultadoDeOperacionGenerico<List<Respuesta>> ResultadoDeListaDeVenta;       
        private ResultadoDeOperacionGenerico<Vendedor> vendedor;
        private string ordenId;
        public ResultadoDeVentaMovil resultadoDeVentaMovil;
        private TipoDeCausa _tipoDeCausa;
        private List<Respuesta> _respuestas;
        private SeguimientoDeVenta seguimientoDeVenta;// resultadoDeVenta;
        private ResultadoDeVenta resultadoDeVenta;
        private List<SeguimientoDeVenta> ListaresultadoDeVenta = new List<SeguimientoDeVenta>();
        private List<Respuesta> _razones;
        private List<Harinera> _harineras;
        Button Enviar;
        Button EnviarSiNo;
        Cliente cliente;
        AutofitTextView NombreVendedorVenta;
        ProveedorDeEstrategia proveedorDeEstrategia = new ProveedorDeEstrategia();
        public const string InformacionDeTipoDeCausa = "TipoDeCausa";
        public const string InformacionDeHarineas = "Harineras";
        public const string InformacionDeRazones = "Razones";
        ResultadoDeOperacionGenerico<string> resultadoProcesaVenta;
        ResultadoDeOperacionGenerico<string> resultadoProcesaVenta2;
        ResultadoDeOperacionGenerico<ResultadoSeguimiento> result1;
        ResultadoDeOperacionGenerico<List<PrecioSeguimientoVenta>> resultadoPrecio;
        List<PrecioSeguimientoVenta> precioLis;
        ResultadoDeOperacionGenerico<List<Desharinizacion>> resultadoHarinizacion;
        ResultadoDeOperacionGenerico<List<FrecuenciaCompra>> resultadoFrecuenciaCompra;
        Timer timer;
        DrawerLayout drawerLayout;
        Spinner SPPrecio;
        Spinner SPFrecuencia;
        Spinner SPApoyos;
      //  DrawerLayout drawerLayout;
        //Android.Support.V4.App.Fragment fragment = null;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            vendedor = Servicios.ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Arguments.Obtener<string>(MainActivity.LlaveUsuario));
            cliente = ProveedorGlobal.Cliente;
            
            //ordenId = ProveedorGlobal.OrdenId;
            _tipoDeCausa = ordenId.EsValido() ? TipoDeCausa.Venta : TipoDeCausa.NoVenta;

            _harineras = ProveedorDeManipulacionDeDatos.ObtenerTodos<Harinera>(
                    Arguments.Obtener<string[]>(InformacionDeHarineas)).ConvertirLista();

            _razones = ProveedorDeManipulacionDeDatos.ObtenerTodos<Respuesta>(
                    Arguments.Obtener<string[]>(InformacionDeRazones)).ConvertirLista();

            seguimientoDeVenta = new SeguimientoDeVenta(ProveedorGlobal.Cliente.ClienteId,0,1,0);

            resultadoDeVentaMovil = new ResultadoDeVentaMovil(0, ProveedorGlobal.Cliente.ClienteId,DateTime.Now,null,null);

            resultadoDeVenta = new ResultadoDeVenta(ProveedorGlobal.Cliente.ClienteId,
                                                    0,
                                                    vendedor.Valor.Id
                                                    );           

        }
        public static SeguimientoVentaFragment NewInstance()
        {
            var frag1 = new SeguimientoVentaFragment { Arguments = new Bundle() };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                View view;
               
                if (_razones == null)
                {
                    var razon = Task.Run(async () => {
                        ResultadoDeListaDeVenta = await proveedorDeEstrategia.ObtenerCausasDeResultadoDeVenta();
                    });
                    razon.Wait();
                    
                    if (ResultadoDeListaDeVenta.Tipo == TipoDeResultado.Exito)
                    {
                        _razones = ResultadoDeListaDeVenta.Valor;
                        Arguments.PutStringArray(InformacionDeRazones,
                            ProveedorDeManipulacionDeDatos.GenerarTodos(_razones));
                    }
                    else
                        Snackbar.Make(View, "Error: " + ResultadoDeListaDeVenta.Mensaje, Snackbar.LengthLong)
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

                var spprecios = Task.Run(async () => {
                    resultadoPrecio = await proveedorDeEstrategia.Precio();
                });
                spprecios.Wait();

                var spHarinizacion = Task.Run(async () => {
                    resultadoHarinizacion = await proveedorDeEstrategia.Desharinizacion();
                });
                spHarinizacion.Wait();

                var spFrecuenciaCompra = Task.Run(async () => {
                    resultadoFrecuenciaCompra = await proveedorDeEstrategia.FrecuenciaCompra();
                });
                spFrecuenciaCompra.Wait();

                ordenId = String.Empty;             
                if (ProveedorGlobal.OrdenId != null)
                    ordenId = ProveedorGlobal.OrdenId;
                else
                    ordenId = null;

                if (ordenId != null)
                {
                    view = VistaVentaSi(inflater, container);
                }
                else if (_razones != null && _harineras != null && ProveedorGlobal.OrdenId == null)
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
            catch(Exception ex)
            {
                Snackbar.Make(View, "Error: " + ex, Snackbar.LengthLong)
                           .Show();
                return null;
            }
            
        }

        private View VistaVentaSi(LayoutInflater inflater, ViewGroup container)
        {
            var view = inflater.Inflate(Resource.Layout.SeguimientoVentaSiNo, container, false);

            _tipoDeCausa = ProveedorGlobal.OrdenId.EsValido() ? TipoDeCausa.Venta : TipoDeCausa.NoVenta;

            //========================================= Carga informacion =================================

            NombreVendedorVenta = view.FindViewById<AutofitTextView>(Resource.Id.NombreVendedorVenta);
            NombreVendedorVenta.Text = "Cliente: " + cliente.ClienteId + " - " + cliente.Nombre;

            view.FindViewById<TextView>(Resource.Id.ResultadoDeVentaRespuestaTexto).Text =
             _tipoDeCausa == TipoDeCausa.Venta
                 ? "¿Cuál es la razón por la cual Sí compra?"
                 : "¿Cuál es la razón por la cual No compra?";

            view.FindViewById<TextView>(Resource.Id.SeguimientoDeVentaComparacionTexto).Visibility =
               _tipoDeCausa == TipoDeCausa.NoVenta ? ViewStates.Gone : ViewStates.Visible;                 // La venta fue mayor TEXTO

            view.FindViewById<RadioGroup>(Resource.Id.SeguimientoDeVentaComparacionGrupo).Visibility =     // RadioButton
                _tipoDeCausa == TipoDeCausa.NoVenta ? ViewStates.Gone : ViewStates.Visible;

            _respuestas = _razones.Where(e => e.Tipo == _tipoDeCausa).ToList();

            //===========================================================================================  // Cliente compraba/competencia 

            //view.FindViewById<CheckBox>(Resource.Id.SeguimientoDeVentaCompetenciaRespuesta).Checked =
            //    _tipoDeCausa == TipoDeCausa.NoVenta;

            //view.FindViewById<Spinner>(Resource.Id.SeguimientoDeVentaCompetencia).Adapter =  // Harineras
            //new ObjetoAdapter<Harinera>(Activity, _harineras);

            //view.FindViewById<Spinner>(Resource.Id.SeguimientoDeVentaCompetencia).ItemSelected += (sender, args) =>  // Harineras
            //{
            //    var spinner = ((Spinner)sender);
            //    long posicion = spinner.SelectedItemId;
            //    resultadoDeVentaMovil.Respuesta = ((ObjetoAdapter<Respuesta>)spinner.Adapter).ObtenerSeleccion(posicion);              
            //};

            //view.FindViewById<CheckBox>(Resource.Id.SeguimientoDeVentaCompetenciaRespuesta).CheckedChange +=
            //(sender, args) =>
            //{
            //    view.FindViewById<Spinner>(Resource.Id.SeguimientoDeVentaCompetencia).Visibility = args.IsChecked
            //        ? ViewStates.Visible
            //        : ViewStates.Gone;
            //    seguimientoDeVenta.CompetenciaId = args.IsChecked ? seguimientoDeVenta.CompetenciaId : 4;
            //};

            //==================================================================================   // Razon por la cual si compra

            view.FindViewById<Spinner>(Resource.Id.ResultadoDeVentaRespuesta).Adapter =
          new ObjetoAdapter<Respuesta>(Activity, _respuestas);

            view.FindViewById<Spinner>(Resource.Id.ResultadoDeVentaRespuesta).ItemSelected += (sender, args) =>
            {
                var spinner = ((Spinner)sender);
                long posicion = spinner.SelectedItemId;
                
                var respuestaId = ((ObjetoAdapter<Respuesta>)spinner.Adapter).ObtenerSeleccion(posicion);
                resultadoDeVenta.RespuestaId = respuestaId.Id;
            };


            view.FindViewById<TextView>(Resource.Id.ResultadoDeVentaOrdenText).Visibility =
               _tipoDeCausa != TipoDeCausa.NoVenta ? ViewStates.Visible : ViewStates.Gone;
            view.FindViewById<TextView>(Resource.Id.ResultadoDeVentaOrden).Visibility =
                _tipoDeCausa != TipoDeCausa.NoVenta ? ViewStates.Visible : ViewStates.Gone;
            view.FindViewById<TextView>(Resource.Id.ResultadoDeVentaOrden).Text = ordenId;

            //===========================================================================================================


           // view.FindViewById<Spinner>(Resource.Id.SeguimientoDeVentaCompetencia).Visibility =
           //_tipoDeCausa == TipoDeCausa.NoVenta ? ViewStates.Visible : ViewStates.Gone;
            
            //===========================================================================================================

            EnviarSiNo = view.FindViewById<Button>(Resource.Id.EnviarSeguimientoVentaSiNo);
            EnviarSiNo.Click += EnviarSiNo_Click;

            return view;
        }
        //=========================================================== Click Venta si =======================================================
        private void EnviarSiNo_Click(object sender, EventArgs e)
        {
            try
            {
                if (resultadoDeVenta.RespuestaId == 0)
                {
                    Snackbar.Make(View, "Ocurrio un problema con el cuestionario. ", Snackbar.LengthLong)
                    .Show();
                    return;                    
                }
               
                var info = Task.Run(async () => {
                     resultadoProcesaVenta = await proveedorDeEstrategia.RegistrarResultadoDeVenta(resultadoDeVenta);
                });
                info.Wait();

                if (resultadoProcesaVenta.Tipo == TipoDeResultado.Exito)
                {
                    //EnviarSiNo.Enabled = false;
                    Snackbar.Make(View, resultadoProcesaVenta.Mensaje, Snackbar.LengthLong)
                    .Show();
                }
                else
                {
                    Snackbar.Make(View, resultadoProcesaVenta.Mensaje, Snackbar.LengthLong);
                }           


                ProveedorGlobal.OrdenId = null;
                EnviarSiNo.Enabled = false;

            }
            catch (Exception ex)
            {
                Snackbar.Make(View, "Ocurrio un error" + ex, Snackbar.LengthLong)
                      .Show();
            }            
        }

        // ============================================ VIEW CAUSAS DE NO VENTA ===========================================

        private View ConfigurarVista(LayoutInflater inflater, ViewGroup container)
        {
            var view = inflater.Inflate(Resource.Layout.SeguimientoVenta, container, false);

            // =============================================== Nuevos campos ================================================//

            SPPrecio = view.FindViewById<Spinner>(Resource.Id.SPPrecio);
            SPPrecio.Adapter = new ObjetoAdapter<PrecioSeguimientoVenta>(Activity, resultadoPrecio.Valor);
            SPPrecio.Visibility = ViewStates.Invisible;
            SPPrecio.Enabled = false;

            //SPFrecuencia = view.FindViewById<Spinner>(Resource.Id.SPFrencuencia);
            //SPFrecuencia.Adapter = new ObjetoAdapter<FrecuenciaCompra>(Activity, resultadoFrecuenciaCompra.Valor);
            //SPFrecuencia.Visibility = ViewStates.Invisible;
            //SPFrecuencia.Enabled = false;


            // ============================================================================================================//

            NombreVendedorVenta = view.FindViewById<AutofitTextView>(Resource.Id.NombreVendedorVenta);
            NombreVendedorVenta.Text = "Cliente: " + cliente.ClienteId + " - " + cliente.Nombre;

            view.FindViewById<TextView>(Resource.Id.TxtResultadoRazon).Text =
             _tipoDeCausa == TipoDeCausa.Venta
                 ? "¿Cuál es la razón por la cual Sí compra?"
                 : "¿Cuál es la razón por la cual No compra?";

            //========================================= Carga informacion =================================

            view.FindViewById<Spinner>(Resource.Id.SeguimientoDeVentaCompetencia).Visibility =
               _tipoDeCausa == TipoDeCausa.NoVenta ? ViewStates.Visible : ViewStates.Gone;

            view.FindViewById<Spinner>(Resource.Id.SeguimientoDeVentaCompetencia).Adapter =
            new ObjetoAdapter<Harinera>(Activity, _harineras);

            view.FindViewById<Spinner>(Resource.Id.SeguimientoDeVentaCompetencia).ItemSelected += (sender, args) =>
            {
                var spinner = ((Spinner)sender);
                long posicion = spinner.SelectedItemId;
                seguimientoDeVenta.CompetenciaId = (int)((ObjetoAdapter<Harinera>)spinner.Adapter).ObtenerSeleccion(posicion);
            };

          view.FindViewById<CheckBox>(Resource.Id.SeguimientoDeVentaCompetenciaRespuesta).CheckedChange +=
          (sender, args) =>
          {
              view.FindViewById<Spinner>(Resource.Id.SeguimientoDeVentaCompetencia).Visibility = args.IsChecked
                  ? ViewStates.Visible
                  : ViewStates.Gone;
              seguimientoDeVenta.CompetenciaId = args.IsChecked ? seguimientoDeVenta.CompetenciaId : 4;
          };

            _respuestas = _razones.Where(e => e.Tipo == _tipoDeCausa).ToList();

            view.FindViewById<Spinner>(Resource.Id.SPRazon).Adapter = new ObjetoAdapter<Respuesta>(Activity, _respuestas);

            view.FindViewById<CheckBox>(Resource.Id.SeguimientoDeVentaCompetenciaRespuesta).Checked =
                _tipoDeCausa == TipoDeCausa.NoVenta;

            // ====================================== Preguntas dinamicas =================================================

            view.FindViewById<Spinner>(Resource.Id.SPRazon).ItemSelected += (sender, args) =>
            {
                var spinner = ((Spinner)sender);
                long posicion = spinner.SelectedItemId;
                resultadoDeVentaMovil.Respuesta = ((ObjetoAdapter<Respuesta>)spinner.Adapter).ObtenerSeleccion(posicion);
                seguimientoDeVenta.CuestionarioId = resultadoDeVentaMovil.Respuesta.Id;
                resultadoDeVenta.RespuestaId = resultadoDeVentaMovil.Respuesta.Id;
                // OnRespuestaSeleccionada(_resultadoDeVenta.Respuesta);


            };
            Enviar = view.FindViewById<Button>(Resource.Id.EnviarSeguimientoVenta);
            Enviar.Click += SeguimientoVentaFragment_Click;
            return view;
        }
        public void setDrawerEnabled(bool enabled)
        {
            int lockMode = enabled ? DrawerLayout.LockModeUnlocked :
                                     DrawerLayout.LockModeLockedClosed;          
        }
        // ============================================ BUTTON CAUSAS DE NO VENTA ===========================================
        private void SeguimientoVentaFragment_Click(object sender, EventArgs e)
        {
           if(seguimientoDeVenta.CompetenciaId != 0)
            {
                var info = Task.Run(async () => {
                    result1 = await proveedorDeEstrategia.RegistrarSeguimientoDeVenta(seguimientoDeVenta);
                });
                info.Wait();                
                if(result1.Tipo == TipoDeResultado.Exito)
                {
                    Enviar.Enabled = false;
                    Snackbar.Make(View, result1.Valor.Respuesta, Snackbar.LengthLong)
                    .Show();
                }
                else
                {
                    Snackbar.Make(View, result1.Mensaje, Snackbar.LengthLong)
                   .Show();
                }
            }
            else
            {
                Snackbar.Make(View, "Ocurrio un problema con el cuestionario. ", Snackbar.LengthLong)
           .Show();
            }

           var exec =Task.Run(async () => {
               resultadoProcesaVenta2 = await proveedorDeEstrategia.RegistrarResultadoDeVenta(resultadoDeVenta);
           });
            exec.Wait();
            
            if (resultadoProcesaVenta2.Tipo == TipoDeResultado.Exito)
            {
                //EnviarSiNo.Enabled = false;
                Snackbar.Make(View, resultadoProcesaVenta2.Mensaje, Snackbar.LengthLong)
                .Show();
            }
            else
            {
                Snackbar.Make(View, resultadoProcesaVenta2.Mensaje, Snackbar.LengthLong);
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