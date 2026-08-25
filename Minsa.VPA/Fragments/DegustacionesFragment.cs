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
using System.Globalization;
using System.Collections.Generic;
using Minsa.VPA.Repositorio;
using Android.Support.Design.Widget;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Linq;
using System.Threading;

namespace Minsa.VPA.Fragments
{
    public class DegustacionesFragment : Fragment
    {
        public const string LLaveDegustaciones = "LLaveDegustaciones";
        public ResultadoDeOperacionGenerico<Vendedor> vendedor;
        Cliente cliente;
        ProveedorDeEstrategia proveedorDeEstrategia = new ProveedorDeEstrategia();
        private Degustacion degustacion;
        List<Articulo> articulosMinsa;
        List<Articulo> articulosCompetencia;
        Button degustaciones;
        ResultadoDeOperacionGenerico<ResultadoRegistroDegustacion> resultadoRegistrarDesgutaciones;
        System.Timers.Timer  timer;
        private List<Respuesta> _respuestas;
        public ResultadoDeOperacionGenerico<List<Respuesta>> ResultadoDeListaDeVenta;
        private List<Respuesta> _razones;
        private TipoDeCausa _tipoDeCausa;
        List<ItemGenerico> itemsGenericos;
        List<ItemGenerico> causas;
        CancellationTokenSource token;
        ResultadoDeOperacionGenerico<ValidaOVFacturaDEG> resultado;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            cliente = ProveedorGlobal.Cliente;
            vendedor = ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Arguments.Obtener<string>(MainActivity.LlaveUsuario));
            degustacion = new Degustacion
            {
                ClienteId = cliente.ClienteId,
                ClienteEstado = OpcionesDeDetalleDeCuestionario.Activo               
            };

            var resultadoMinsa = proveedorDeEstrategia.ObtenerArticulosMinsa(new ClienteId(cliente.ClienteId));
            if (resultadoMinsa.Tipo == TipoDeResultado.Exito)
                articulosMinsa = resultadoMinsa.Valor;
            else
                Snackbar.Make(View, "Ocurrio un error comunicate con el departamento de Sistemas", Snackbar.LengthLong)
                     .Show();

            var resultadoCompetencia = proveedorDeEstrategia.ObtenerArticulosCompetencia(new ClienteId(cliente.ClienteId));
            if (resultadoCompetencia.Tipo == TipoDeResultado.Exito)
                articulosCompetencia = resultadoCompetencia.Valor;
            else
                Snackbar.Make(View, "Ocurrio un error comunicate con el departamento de Sistemas", Snackbar.LengthLong)
                     .Show();
            var razon = Task.Run(async () => {
                ResultadoDeListaDeVenta = await proveedorDeEstrategia.ObtenerCausasDeResultadoDeVenta();
            });
            razon.Wait();

            _tipoDeCausa = TipoDeCausa.Venta;
            _razones = ResultadoDeListaDeVenta.Valor;
            _respuestas = _razones.Where(e => e.Tipo == _tipoDeCausa).ToList();
            itemsGenericos = CargaCausas();
           
        }
        public static DegustacionesFragment NewInstance()
        {
            var frag1 = new DegustacionesFragment { Arguments = new Bundle() };
            return frag1;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                View view;
                var ignored = base.OnCreateView(inflater, container, savedInstanceState);

                if (articulosMinsa != null && articulosCompetencia != null)
                {
                    view = ConfigurarVista(inflater, container);
                    //ConfigurarModelo(view);
                }
                else
                {
                    view = inflater.Inflate(Resource.Layout.Error, container, false);
                    view.FindViewById<TextView>(Resource.Id.ErrorTexto).Text =
                        "No se encontraron artículos para el cliente.";
                }

                return view;
            }catch(Exception ex)
            {
                Toast.MakeText(this.Activity, "No estas conectado a internet y/o ocurrio un problema.", ToastLength.Short).Show();

                //view = inflater.Inflate(Resource.Layout.Error, container, false);
                //view.FindViewById<TextView>(Resource.Id.ErrorTexto).Text =
                //   "No se puede mostrar tu Geocerca. Sin conexion a internet / Sin coordenadas";
                return null;
            }
         
        }
        private View ConfigurarVista(LayoutInflater inflater, ViewGroup container)
        {
            View view = inflater.Inflate(Resource.Layout.Degustaciones, null);

          

            view.FindViewById<Spinner>(Resource.Id.DegustacionHarinaMinsa).Adapter =
                new ObjetoAdapter<Articulo>(Activity, articulosMinsa);
            view.FindViewById<Spinner>(Resource.Id.DegustacionHarinaCompetencia).Adapter =
                new ObjetoAdapter<Articulo>(Activity, articulosCompetencia);
            

            view.FindViewById<Spinner>(Resource.Id.DegustacionComparacionRendimientoTortilla).Adapter =
                new ObjetoAdapter<OpcionDeCuestionario>(Activity,
                    OpcionDeCuestionario.ObtenerOpciones(OpcionesDeCuestionario.Comparacion));
            view.FindViewById<Spinner>(Resource.Id.DegustacionTortillaTextura).Adapter =
                new ObjetoAdapter<OpcionDeCuestionario>(Activity,
                    OpcionDeCuestionario.ObtenerOpciones(OpcionesDeCuestionario.Comparacion));
            view.FindViewById<Spinner>(Resource.Id.DegustacionTortillaColor).Adapter =
                new ObjetoAdapter<OpcionDeCuestionario>(Activity,
                    OpcionDeCuestionario.ObtenerOpciones(OpcionesDeCuestionario.Comparacion));
            view.FindViewById<Spinner>(Resource.Id.DegustacionTortillaAroma).Adapter =
                new ObjetoAdapter<OpcionDeCuestionario>(Activity,
                    OpcionDeCuestionario.ObtenerOpciones(OpcionesDeCuestionario.Comparacion));
            view.FindViewById<Spinner>(Resource.Id.DegustacionTortillaDuracion).Adapter =
                new ObjetoAdapter<OpcionDeCuestionario>(Activity,
                    OpcionDeCuestionario.ObtenerOpciones(OpcionesDeCuestionario.Comparacion));
            view.FindViewById<Spinner>(Resource.Id.DegustacionTortillaCorrea).Adapter =
                new ObjetoAdapter<OpcionDeCuestionario>(Activity,
                    OpcionDeCuestionario.ObtenerOpciones(OpcionesDeCuestionario.Comparacion));
            view.FindViewById<Spinner>(Resource.Id.DegustacionTortillaSabor).Adapter =
                new ObjetoAdapter<OpcionDeCuestionario>(Activity,
                    OpcionDeCuestionario.ObtenerOpciones(OpcionesDeCuestionario.Comparacion));
            view.FindViewById<Spinner>(Resource.Id.DegustacionConvencido).Adapter =
                new ObjetoAdapter<OpcionDeCuestionario>(Activity,
                    OpcionDeCuestionario.ObtenerOpciones(OpcionesDeCuestionario.Compra));

          
            view.FindViewById<Spinner>(Resource.Id.PreguntaSINO).Adapter =
                new ObjetoAdapter<ItemGenerico>(Activity, itemsGenericos);

           

            view.FindViewById<Spinner>(Resource.Id.PreguntaSINO).ItemSelected += (sender, args) =>
            {
                var spinner = ((Spinner)sender);
                long posicion = spinner.SelectedItemId;
                degustacion.Pregunta =
                    ((ObjetoAdapter<ItemGenerico>)spinner.Adapter).ObtenerSeleccion(posicion).Nombre.ToString();

                if (degustacion.Pregunta == "Venta")
                    _tipoDeCausa = TipoDeCausa.Venta;
                else
                    _tipoDeCausa = TipoDeCausa.NoVenta;

                _respuestas = _razones.Where(e => e.Tipo == _tipoDeCausa).ToList();
                view.FindViewById<Spinner>(Resource.Id.CausasDeVenta).Adapter =
               new ObjetoAdapter<Respuesta>(Activity, _respuestas);
            };
            view.FindViewById<Spinner>(Resource.Id.CausasDeVenta).ItemSelected += (sender, args) =>
            {
                var spinner = ((Spinner)sender);
                long posicion = spinner.SelectedItemId;
                degustacion.IdCausa =
                    (int)((ObjetoAdapter<Respuesta>)spinner.Adapter).ObtenerSeleccion(posicion).Id;
                degustacion.Respuesta =
                    (string)((ObjetoAdapter<Respuesta>)spinner.Adapter).ObtenerSeleccion(posicion).Texto;
            };

            

            view.FindViewById<Spinner>(Resource.Id.DegustacionConvencido).ItemSelected += (sender, args) =>
            {
                var spinner = ((Spinner)sender);
                long posicion = spinner.SelectedItemId;
                degustacion.ClienteConvencido =
                    ((ObjetoAdapter<OpcionDeCuestionario>)spinner.Adapter).ObtenerSeleccion(posicion).Id;
            };
            view.FindViewById<Spinner>(Resource.Id.DegustacionHarinaCompetencia).ItemSelected += (sender, args) =>
            {
                var spinner = ((Spinner)sender);
                long posicion = spinner.SelectedItemId;
                degustacion.CompetenciaRecursoId =
                    (int)((ObjetoAdapter<Articulo>)spinner.Adapter).ObtenerSeleccion(posicion).Id.ObtenerCantidadDecimal();
            };
            view.FindViewById<Spinner>(Resource.Id.DegustacionHarinaMinsa).ItemSelected += (sender, args) =>
            {
                var spinner = ((Spinner)sender);
                long posicion = spinner.SelectedItemId;
                degustacion.MinsaRecursoId = ((ObjetoAdapter<Articulo>)spinner.Adapter).ObtenerSeleccion(posicion).Id;
            };

            view.FindViewById<EditText>(Resource.Id.DegustacionOVFactura).TextChanged += async (sender, args) =>
            {
                degustacion.OVFActura = ((EditText)sender).Text;
                var texto = degustacion.OVFActura.Trim();

               // degustacion.OVFActura = texto;

                if (texto.Length < 9)
                    return;

                token?.Cancel();
                token = new CancellationTokenSource();

                try
                {
                    await Task.Delay(300, token.Token);

                    resultado = await proveedorDeEstrategia.ValidaOVFacturaDEG(new DataValidaOVFactDEG(texto));

                    if (resultado.Valor.Existe == 0)
                    {
                        degustacion.OVFActura = resultado.Valor.Valor;
                        Snackbar.Make(View, "No existe la OV o Factura", Snackbar.LengthLong)
                                    .Show();
                    }
                    else
                    {
                        //editText.Error = null;                       
                        Snackbar.Make(View, "Tu OV o Factura es correcta", Snackbar.LengthLong)
                                    .Show();

                    }
                }
                catch (TaskCanceledException)
                {

                }
              
            };

            view.FindViewById<EditText>(Resource.Id.DegustacionTelefono).TextChanged += (sender, args) =>
            {
                degustacion.ClienteTelefono = ((EditText)sender).Text;
            };

            view.FindViewById<EditText>(Resource.Id.DegustacionPtencialHarina).TextChanged += (sender, args) =>
            {
                degustacion.PotencialHarina = ((EditText)sender).Text.ObtenerCantidadDecimal();
            };

            view.FindViewById<EditText>(Resource.Id.DegustacionPotencialMaiz).TextChanged += (sender, args) =>
            {
                degustacion.PotencialMaiz = ((EditText)sender).Text.ObtenerCantidadDecimal();
            };

            view.FindViewById<EditText>(Resource.Id.DegustacionPotencialMasa).TextChanged += (sender, args) =>
            {
                degustacion.PotencialMasa = ((EditText)sender).Text.ObtenerCantidadDecimal();
            };

            view.FindViewById<EditText>(Resource.Id.DegustacionSacosMinsa).TextChanged += (sender, args) =>
            {
                degustacion.HarinaSacosUsados = ((EditText)sender).Text.ObtenerCantidadDecimal();
                View.FindViewById<EditText>(Resource.Id.DegustacionHarinaKg).Text = (degustacion.HarinaSacosUsados * 20).ToString(CultureInfo.InvariantCulture);
            };

            view.FindViewById<EditText>(Resource.Id.DegustacionHarinaKg).TextChanged += (sender, args) =>
            {
                degustacion.HarinaKgUsados = ((EditText)sender).Text.ObtenerCantidadDecimal();
            };

            view.FindViewById<EditText>(Resource.Id.DegustacionRendimientoHarinaMasa).TextChanged += (sender, args) =>
            {
                degustacion.HarinaRendimientoMasa = ((EditText)sender).Text.ObtenerCantidadDecimal();
                CalcularDeshidratacionDeTestal();
            };

            view.FindViewById<EditText>(Resource.Id.DegustacionRendimientoHarinaTortilla).TextChanged +=
                (sender, args) =>
                {
                    degustacion.HarinaRendimientoTortilla = ((EditText)sender).Text.ObtenerCantidadDecimal();
                    CalcularDeshidratacionDeTestal();
                    CalcularCumplimiento();
                };

            view.FindViewById<EditText>(Resource.Id.DegustacionDeshidratacionTestal).TextChanged += (sender, args) =>
            {
                degustacion.HarinaDeshidratacionMasa = ((EditText)sender).Text.ObtenerCantidadDecimal();
            };

            view.FindViewById<EditText>(Resource.Id.DegustacionRendimientoHarinaTortillaCompetencia).TextChanged +=
                (sender, args) =>
                {
                    degustacion.HarinaRendimientoTortillaCompetencia = ((EditText)sender).Text.ObtenerCantidadDecimal();
                    CalcularCumplimiento();
                };
            view.FindViewById<Spinner>(Resource.Id.DegustacionComparacionRendimientoTortilla).ItemSelected +=
                (sender, args) =>
                {
                    var spinner = ((Spinner)sender);
                    long posicion = spinner.SelectedItemId;
                    degustacion.TortillaClienteRed =
                        ((ObjetoAdapter<OpcionDeCuestionario>)spinner.Adapter).ObtenerSeleccion(posicion).Id;
                };
            view.FindViewById<Spinner>(Resource.Id.DegustacionTortillaTextura).ItemSelected += (sender, args) =>
            {
                var spinner = ((Spinner)sender);
                long posicion = spinner.SelectedItemId;
                degustacion.TortillaTextura =
                    ((ObjetoAdapter<OpcionDeCuestionario>)spinner.Adapter).ObtenerSeleccion(posicion).Id;
            };
            view.FindViewById<Spinner>(Resource.Id.DegustacionTortillaColor).ItemSelected += (sender, args) =>
            {
                var spinner = ((Spinner)sender);
                long posicion = spinner.SelectedItemId;
                degustacion.TortillaColor =
                    ((ObjetoAdapter<OpcionDeCuestionario>)spinner.Adapter).ObtenerSeleccion(posicion).Id;
            };
            view.FindViewById<Spinner>(Resource.Id.DegustacionTortillaAroma).ItemSelected += (sender, args) =>
            {
                var spinner = ((Spinner)sender);
                long posicion = spinner.SelectedItemId;
                degustacion.TortillaAroma =
                    ((ObjetoAdapter<OpcionDeCuestionario>)spinner.Adapter).ObtenerSeleccion(posicion).Id;
            };
            view.FindViewById<Spinner>(Resource.Id.DegustacionTortillaDuracion).ItemSelected += (sender, args) =>
            {
                var spinner = ((Spinner)sender);
                long posicion = spinner.SelectedItemId;
                degustacion.TortillaDuracion =
                    ((ObjetoAdapter<OpcionDeCuestionario>)spinner.Adapter).ObtenerSeleccion(posicion).Id;
            };
            view.FindViewById<Spinner>(Resource.Id.DegustacionTortillaCorrea).ItemSelected += (sender, args) =>
            {
                var spinner = ((Spinner)sender);
                long posicion = spinner.SelectedItemId;
                degustacion.TortillaCorrea =
                    ((ObjetoAdapter<OpcionDeCuestionario>)spinner.Adapter).ObtenerSeleccion(posicion).Id;
            };

            view.FindViewById<Spinner>(Resource.Id.DegustacionTortillaSabor).ItemSelected += (sender, args) =>
            {
                var spinner = ((Spinner)sender);
                long posicion = spinner.SelectedItemId;
                degustacion.TortillaSabor =
                    ((ObjetoAdapter<OpcionDeCuestionario>)spinner.Adapter).ObtenerSeleccion(posicion).Id;
            };
            view.FindViewById<Spinner>(Resource.Id.DegustacionConvencido).ItemSelected += (sender, args) =>
            {
                var spinner = ((Spinner)sender);
                long posicion = spinner.SelectedItemId;
                degustacion.ClienteConvencido =
                    ((ObjetoAdapter<OpcionDeCuestionario>)spinner.Adapter).ObtenerSeleccion(posicion).Id;
            };

            degustaciones = view.FindViewById<Button>(Resource.Id.EnviarDegustaciones);
            degustaciones.Click += Degustaciones_Click;

            return view;
        }
        public List<ItemGenerico> CargaCausas()
        {
             causas = new List<ItemGenerico>();
            causas.Add(new ItemGenerico
            {
                Id = 1,
                Nombre = "Venta"
            });
            causas.Add(new ItemGenerico
            {
                Id = 2,
                Nombre = "No venta"
            });
            return causas;

        }
        private void Degustaciones_Click(object sender, EventArgs e)
        {
            try
            {
                bool validaTelefonoMovil = false;
                validaTelefonoMovil = new Regex(@"\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})").IsMatch(Convert.ToString(degustacion.ClienteTelefono));
                if (validaTelefonoMovil == false)
                {
                    Snackbar.Make(View, "Debes llenar todos campos (telefono)", Snackbar.LengthLong)
                        .Show();
                    return;
                }
                if (degustacion.PotencialHarina == 0)
                {
                    Snackbar.Make(View, "Debes llenar todos campos (Potencial harina TM x mes)", Snackbar.LengthLong)
                        .Show();
                    return;
                }
                if (degustacion.OVFActura.Length < 9)
                {
                    Snackbar.Make(View, "Debes llenar todos campos (No existe la OV o Factura)", Snackbar.LengthLong)
                        .Show();
                    return;
                }
                else
                {
                    if (resultado.Valor.Existe == 0)
                    {

                        Snackbar.Make(View, "No existe la OV o Factura", Snackbar.LengthLong)
                                    .Show();
                        return;
                    }
                    else
                    {
                        //editText.Error = null;                       
                        Snackbar.Make(View, "Tu OV o Factura es correcta", Snackbar.LengthLong)
                                    .Show();

                    }
                }

                //if(degustacion.OVFActura == "")
                //{
                //    Snackbar.Make(View, "Debes llenar todos campos (OV o Factura valida)", Snackbar.LengthLong)
                //        .Show();
                //    return;
                //}
                //if (degustacion.PotencialMaiz == 0)
                //{
                //    Snackbar.Make(View, "Debes llenar todos campos (Potencial Maiz TM x mes)", Snackbar.LengthLong)
                //        .Show();
                //    return;
                //}
                //if (degustacion.PotencialMasa == 0)
                //{
                //    Snackbar.Make(View, "Debes llenar todos campos (Potencial Masa TM x mes)", Snackbar.LengthLong)
                //        .Show();
                //    return;
                //}
                if (degustacion.HarinaSacosUsados == 0)
                {
                    Snackbar.Make(View, "El valor ingresado no es valido, debes ingresar un valor mayor a 0 (Sacos de hariana Minsa)", Snackbar.LengthLong)
                        .Show();
                    return;
                }
                if (degustacion.HarinaRendimientoMasa == 0)
                {
                    Snackbar.Make(View, "El valor ingresado no es valido, debes ingresar un valor mayor a 0 (Rendimiento harina - masa)", Snackbar.LengthLong)
                        .Show();
                    return;
                }
                if (degustacion.HarinaRendimientoTortilla == 0)
                {
                    Snackbar.Make(View, "El valor ingresado no es valido, debes ingresar un valor mayor a 0 (Rendimiento harina - tortilla)", Snackbar.LengthLong)
                        .Show();
                    return;
                }


                if (cliente.Id != null)
                {
                    degustacion.ClienteId = cliente.ClienteId;
                    degustacion.VendedorId = vendedor.Valor.Id;
                    var info = Task.Run(async () => {
                         resultadoRegistrarDesgutaciones = await proveedorDeEstrategia.RegistrarDegustacion(degustacion);
                    });
                    info.Wait();
                   
                    if (resultadoRegistrarDesgutaciones.Tipo == TipoDeResultado.Exito)
                    {
                        degustaciones.Enabled = false;
                        Snackbar.Make(View, resultadoRegistrarDesgutaciones.Mensaje, Snackbar.LengthLong)
                         .Show();
                     //   DeshabilitarVista();
                    }
                    else
                    {
                        Snackbar.Make(View, resultadoRegistrarDesgutaciones.Mensaje, Snackbar.LengthLong)
                         .Show();
                    }
                }
                else
                {
                    Snackbar.Make(View, "No haz seleccionado ninguna cliente.", Snackbar.LengthLong)
                         .Show();
                }

            }
            catch(Exception ex)
            {
                Snackbar.Make(View, "Error: " + ex, Snackbar.LengthLong)
                         .Show();
            }

        }

        private void CalcularCumplimiento()
        {
            var comparacion = View.FindViewById<Spinner>(Resource.Id.DegustacionComparacionRendimientoTortilla);
            string buscarPor;
            if (degustacion.HarinaRendimientoTortillaCompetencia == degustacion.HarinaRendimientoTortilla)
                buscarPor = "Igual";
            else
                buscarPor = degustacion.HarinaRendimientoTortilla > degustacion.HarinaRendimientoTortillaCompetencia
                    ? "Si"
                    : "No";
            var indice = ((ObjetoAdapter<OpcionDeCuestionario>)comparacion.Adapter).BuscarIndice(e => e.Nombre.Contains(buscarPor));
            comparacion.SetSelection(indice);
        }

        private void CalcularDeshidratacionDeTestal()
        {
            decimal deshidratacion = 0;
            if (degustacion.HarinaRendimientoMasa > 0)
            {
                deshidratacion = Math.Abs(((degustacion.HarinaRendimientoTortilla / degustacion.HarinaRendimientoMasa) - 1) * 100);
            }
            View.FindViewById<EditText>(Resource.Id.DegustacionDeshidratacionTestal).Text = deshidratacion.ToString("##.###");
        }
        public void DeshabilitarVista(View view = null)
        {
            var currentView = view;
            currentView.FindViewById<EditText>(Resource.Id.DegustacionTelefono).Enabled = false;
            currentView.FindViewById<EditText>(Resource.Id.DegustacionPtencialHarina).Enabled = false;
            currentView.FindViewById<EditText>(Resource.Id.DegustacionPotencialMaiz).Enabled = false;
            currentView.FindViewById<EditText>(Resource.Id.DegustacionPotencialMasa).Enabled = false;
            currentView.FindViewById<EditText>(Resource.Id.DegustacionSacosMinsa).Enabled = false;
            currentView.FindViewById<EditText>(Resource.Id.DegustacionHarinaKg).Enabled = false;
            currentView.FindViewById<EditText>(Resource.Id.DegustacionRendimientoHarinaMasa).Enabled = false;
            currentView.FindViewById<EditText>(Resource.Id.DegustacionRendimientoHarinaTortilla).Enabled = false;
            currentView.FindViewById<EditText>(Resource.Id.DegustacionRendimientoHarinaTortillaCompetencia).Enabled = false;
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
                System.Timers.Timer timer = new System.Timers.Timer();
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