using Android.Locations;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Minsa.VPA.Adaptadores;
using Minsa.VPA.Modelos;
using Minsa.VPA.Proveedores;
using Minsa.VPA.Repositorio;
using Minsa.VPA.Servicios;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace Minsa.VPA.Fragments
{   
    public class AltaClientesFragment : Fragment
    {
        public const string LlaveAltaClientes = "LlaveAltaClientes";
        private ResultadoDeOperacionGenerico<Vendedor> vendedor;
        ProveedorDeClientes proveedorDeClientes = new ProveedorDeClientes();
        Timer timer;
        EditText CodigoPostal;
        Spinner Estado;
        Spinner Municipio;
        public string IDEstado;
        public string IdMunicipio;
        public AdaptadorEstado adapterEstado;
        public AdaptadorMunicipio adapterMunicipio;
        public AdaptadorTipoPersona adapterTipoPersona;
        public AdaptadorTiposSocios adapterTipoSocios;
        ResultadoDeOperacionGenerico<List<Estados>> consultaEstado;
        ResultadoDeOperacionGenerico<List<Municipio>> consultaMunicipio;
        EditText Nombre;
        EditText Paterno;
        EditText Materno;
        EditText Calle;
        EditText NoExt;
        EditText NoInt;
        EditText Colonia;
        EditText CP;
        EditText Telefono;
        EditText Celular;
        EditText Correo;
        EditText RFC;
        //Spinner TipoSocio;
        //Spinner ResponsabilidadFiscal;
        ResultadoDeOperacionGenerico<List<TipoPersona>> resultadoTipoPersona;
        ResultadoDeOperacionGenerico<List<TiposSocios>> resultadoTipoSocios;
        public string TipoSocioSP;
        public string TipoPersona;
        ResultadoDeOperacionGenerico<string>  ResultadogeneraCodigo;
        ResultadoDeOperacionGenerico<string> ResultadoAltaCliente;
        public DataAltaCliente dataAltaCliente;
        CheckBox CheckRFCGenerico;
        Button EnviarInformacion;
        public string Codigo;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            vendedor = Servicios.ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Arguments.Obtener<string>(MainActivity.LlaveUsuario));
            // Create your fragment here
        }
        public static AltaClientesFragment NewInstance()
        {
            var frag1 = new AltaClientesFragment { Arguments = new Bundle() };
            return frag1;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                View view;
                var ignored = base.OnCreateView(inflater, container, savedInstanceState);
                var info = Task.Run(async () => {
                    resultadoTipoPersona = await proveedorDeClientes.TipoPersona();
                    resultadoTipoSocios = await proveedorDeClientes.TipoSocios();
                });
                info.Wait();
             
               
                //if (ClientesProspecto != null)
                //{
                view = ConfigurarVista(inflater, container);
                //    //ConfigurarModelo(view);
                //}
                //else
                //{
                //    view = inflater.Inflate(Resource.Layout.Error, container, false);
                //    view.FindViewById<TextView>(Resource.Id.ErrorTexto).Text =
                //        "No se encontraron registros para el cliente.";
                //}
                return view;
            }
            catch (Exception ex)
            {
                Toast.MakeText(this.Activity, "No estas conectado a internet y/o ocurrio un problema " + ex, ToastLength.Short).Show();
                return null;
            }
        }
        private View ConfigurarVista(LayoutInflater inflater, ViewGroup container)
        {
            var view = inflater.Inflate(Resource.Layout.AltaClientes, null);           

            CodigoPostal = view.FindViewById<EditText>(Resource.Id.CP);
            Estado = view.FindViewById<Spinner>(Resource.Id.Estado);
            Estado.ItemSelected += Estado_ItemSelected;
            Municipio = view.FindViewById<Spinner>(Resource.Id.Municpio);
            Municipio.ItemSelected += Municipio_ItemSelected;
            CodigoPostal.TextChanged += CodigoPostal_TextChanged;

            Nombre = view.FindViewById<EditText>(Resource.Id.Nombre);
            Paterno = view.FindViewById<EditText>(Resource.Id.Paterno);
            Materno = view.FindViewById<EditText>(Resource.Id.Materno);
            Calle = view.FindViewById<EditText>(Resource.Id.Calle);
            NoExt = view.FindViewById<EditText>(Resource.Id.NoExt);
            NoInt = view.FindViewById<EditText>(Resource.Id.NoInt); 
            Colonia = view.FindViewById<EditText>(Resource.Id.Colonia);
            CP = view.FindViewById<EditText>(Resource.Id.CP);
            Telefono = view.FindViewById<EditText>(Resource.Id.Telefono);
            Celular = view.FindViewById<EditText>(Resource.Id.Celular);
            Correo = view.FindViewById<EditText>(Resource.Id.Correo);
            RFC = view.FindViewById<EditText>(Resource.Id.RFC);
            RFC.Text = "XAXX010101000";
            RFC.Enabled = false;
            //  CheckRFCGenerico = view.FindViewById<CheckBox>(Resource.Id.CheckRFCGenerico);
            //TipoSocio = view.FindViewById<Spinner>(Resource.Id.SPTSocio);
            //ResponsabilidadFiscal = view.FindViewById<Spinner>(Resource.Id.SpFiscal);


            //adapterTipoPersona = new AdaptadorTipoPersona(Activity, resultadoTipoPersona.Valor);
            //ResponsabilidadFiscal.Adapter = adapterTipoPersona;
            //ResponsabilidadFiscal.ItemSelected += ResponsabilidadFiscal_ItemSelected;

            //adapterTipoSocios = new AdaptadorTiposSocios(Activity, resultadoTipoSocios.Valor);
            //TipoSocio.Adapter = adapterTipoSocios;
            //TipoSocio.ItemSelected += TipoSocio_ItemSelected;

            // CheckRFCGenerico.CheckedChange += CheckRFCGenerico_CheckedChange;
            EnviarInformacion = view.FindViewById<Button>(Resource.Id.EnviarAltaCliente);
            EnviarInformacion.Click += EnviarInformacion_Click;
            return view;
        }        

        //private void CheckRFCGenerico_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        //{
        //   if(e.IsChecked == true)
        //    {
        //        RFC.Text = "XAXX010101000";
        //    }
        //    else
        //    {
        //        RFC.Text = "";
        //    }

        //}

        //private void TipoSocio_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        //{
        //    TipoSocioSP = resultadoTipoSocios.Valor[e.Position].IM_TIPO_SOCIO;
        //}

        //private void ResponsabilidadFiscal_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        //{
        //    TipoPersona = resultadoTipoPersona.Valor[e.Position].Text;
        //}

        private void Municipio_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                IdMunicipio = consultaMunicipio.Valor[e.Position].COUNTYID;
            }
            catch (Exception ex)
            {
                Activity.MostrarMensaje("Ocurrio un error" + ex, ToastLength.Long);
            }
        }

        private void Estado_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                //IdMunicipio = consultaEstado.Valor[e.Position].COUNTYID;
                IDEstado = consultaEstado.Valor[e.Position].IDESTADO;
            }
            catch (Exception ex)
            {
                Activity.MostrarMensaje("Ocurrio un error" + ex, ToastLength.Long);
            }
        }

        private async void CodigoPostal_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (CodigoPostal.Text.Length == 5)
            {
                 consultaEstado = await proveedorDeClientes.BuscaEstado(new DataEstado(CodigoPostal.Text));
                if (consultaEstado.Tipo == TipoDeResultado.Exito)
                {
                    adapterEstado = new AdaptadorEstado(Activity, consultaEstado.Valor);
                    Estado.Adapter = adapterEstado;
                }

                 consultaMunicipio = await proveedorDeClientes.BuscaMunicpio(new DataMunicipio(CodigoPostal.Text));
                if(consultaMunicipio.Tipo == TipoDeResultado.Exito)
                {
                    adapterMunicipio = new AdaptadorMunicipio(Activity, consultaMunicipio.Valor);
                    Municipio.Adapter = adapterMunicipio;
                }
            }
        }
        private async void EnviarInformacion_Click(object sender, EventArgs e)
        {
            try
            {
                if (Nombre.Text == null || Nombre.Text == "")
                {
                    Snackbar.Make(View, "El campo nombre es requerido.", Snackbar.LengthLong)
                    .Show();
                    return;
                }
                if (CP.Text == null || CP.Text == "")
                {
                    Snackbar.Make(View, "El campo codigo postal es requerido.", Snackbar.LengthLong)
                    .Show();
                    return;
                }
                if (IdMunicipio == null)
                {
                    Snackbar.Make(View, "El campo municipio es requerido.", Snackbar.LengthLong)
                    .Show();
                    return;
                }
                if (IDEstado == null)
                {
                    Snackbar.Make(View, "El campo estado es requerido.", Snackbar.LengthLong)
                    .Show();
                    return;
                }
                if (RFC.Text == null || RFC.Text == "")
                {
                    Snackbar.Make(View, "El campo RFC es requerido.", Snackbar.LengthLong)
                    .Show();
                    return;
                }
                ResultadogeneraCodigo = await proveedorDeClientes.GeneraCodigo();
                if (ResultadogeneraCodigo.Tipo == TipoDeResultado.Exito)
                {
                    Codigo = ResultadogeneraCodigo.Valor;
                    EnviarInformacion.Enabled = false;
                }
                var listaPrecios = await proveedorDeClientes.ListaPrecios(new DataListaPrecios("MEX", IDEstado, IdMunicipio));

                var DatosDireccion = await proveedorDeClientes.DatosAltaDireccion(new DataDireccion(IdMunicipio, IDEstado));

                var Pais = DatosDireccion.Valor.COUNTRYREGIONID;
                var Estado = DatosDireccion.Valor.STATEID;
                var Municipio = DatosDireccion.Valor.COUNTYID;

                string TipoPrecio = String.Empty;
                string NomZona = String.Empty;
                string ListaDePrecio = String.Empty;
                string ListaDeDescuento = String.Empty;
                string CUSTGROUP = "LIBRE";
                string SitioPrecio = "1CTO";
                if (listaPrecios.Tipo == TipoDeResultado.Exito)
                {
                    string QuitaPZona = listaPrecios.Valor.PRICEGROUP;
                    ListaDePrecio = listaPrecios.Valor.PRICEGROUP;
                    string Descuento = listaPrecios.Valor.PRICEGROUP;
                    NomZona = QuitaPZona.Substring(0, 7);
                    var VDescuento = Descuento.Substring(7);

                    if (VDescuento == "P")
                    {
                        ListaDeDescuento = listaPrecios.Valor.PRICEGROUP + "COM";
                        TipoPrecio = "Planta";
                    }
                    else
                    {
                        ListaDeDescuento = NomZona + "BV01";
                        TipoPrecio = "Bodega";
                    }
                }

                TipoPersona = "FISICA";
                TipoSocioSP = "TORTILLERIA";
                ResultadoAltaCliente = await proveedorDeClientes.AltaCliente(new DataAltaCliente(NomZona, TipoPrecio, ListaDePrecio, ListaDeDescuento, TipoSocioSP,
                                                                                                Codigo, Nombre.Text, Calle.Text,
                                                                                                NoExt.Text, NoInt.Text, Colonia.Text, Municipio, "Mexico", Estado, CP.Text,
                                                                                                Telefono.Text, Celular.Text, Correo.Text,
                                                                                                TipoPersona, SitioPrecio, CUSTGROUP,
                                                                                                Materno.Text, Paterno.Text, Pais, RFC.Text));
                if (ResultadoAltaCliente.Tipo == TipoDeResultado.Exito)
                {
                    var vistaDePregunta = LayoutInflater.Inflate(Resource.Layout.MensajeExitoso, null);

                    vistaDePregunta.FindViewById<TextView>(Resource.Id.MsjExitoso).Text =
                               "Cliente generado exitosamente, el número de socio es: " + Codigo;

                    new Android.Support.V7.App.AlertDialog.Builder(this.Activity)
                  .SetTitle(this.GetString(Resource.String.dialogMSJExistoso))
                  .SetView(vistaDePregunta)                 
                  .SetNegativeButton(this.GetString(Resource.String.dialog_cancel), (sender, args) => { })
                  // .SetNeutralButton(View.Context.GetString(Resource.String.dialog_neutral), (sender, args) => { })
                  .Show();
                    //Snackbar.Make(View, "El codigo del cliente generado es: " + Codigo, Snackbar.LengthLong)
                    // .Show();
                }
            }
            catch (Exception ex)
            {
                Snackbar.Make(View, "Ocurrio un error contacta con el administrador", Snackbar.LengthLong)
                     .Show();
            }
        }
        protected bool ValidateZipCode(string zipCode)
        {
            string pattern = @"^\d{5}(\-\d{4})?$";
            var regex = new Regex(pattern);
            Log.Debug("V", regex.IsMatch(zipCode).ToString());
            return regex.IsMatch(zipCode);
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