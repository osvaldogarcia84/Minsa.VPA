using Android.Locations;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Grantland.Widget;
using Minsa.VPA.Adaptadores;
using Minsa.VPA.Extensiones;
using Minsa.VPA.Modelos;
using Minsa.VPA.Proveedores;
using Minsa.VPA.Repositorio;
using Minsa.VPA.Servicios;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Timers;


namespace Minsa.VPA.Fragments
{
    [Obsolete]
    public class EditarPronostico : Fragment
    {
        public ResultadoDeOperacionGenerico<Vendedor> vendedor;
        public const string LlaveDeEditarPronostico = "EditarPronostico";
        public const string LLaveEditarFiltros = "LLaveEditarFiltros";
        //public List<ClientePronostico> clientesPronostico;
        AutofitTextView CtePronosticoEditar;
        EditText VolumenProEditar;
        Button EditarPronosticoCte;
        Button BtnRegresarEditar;
        ProveedorDePronostico proveedorDePronostico = new ProveedorDePronostico();
        ResultadoDeOperacionGenerico<List<ClientePronostico>> resultadoClientePronostico;
        string ObtieneFiltros;
        Android.Support.V4.App.Fragment fragmentV4 = null;
        public ResultadoDeOperacionGenerico<List<InformacionDeVenta>> informacionDeVenta;
        private readonly List<ArticuloMovil> articulosDeVista = new List<ArticuloMovil>();
        ProveedorDeClientes proveedorDeCliente = new ProveedorDeClientes();
        private Spinner SpRecursosEditarPro;
        private AdaptadorSpinnerArticulosPro adapter;
        string IdRecurso;
        TextView _dateDisplayEditarPro;
        string FechaPronostico;
        Button date_select_buttonPro;
        ClientePronostico clientePronostico;
        int Position;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            vendedor = ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Arguments.Obtener<string>(MainActivity.LlaveUsuario));
            ObtieneFiltros = ProveedorDeManipulacionDeDatos.Obtener<string>(Arguments.Obtener<string>(LLaveEditarFiltros)).ToString();
            //proveedorDeLocacion = new ProveedorDeLocacion(Activity, 0); 
            //proveedorDeLocacion.LocacionEncontrada += ActualizarLocacion;
            try
            {
                Activity.MostrarMensaje("Cargando información de los clientes");
                //clientesPronostico = ProveedorDeManipulacionDeDatos.ObtenerTodos<ClientePronostico>(Arguments.Obtener<string>(LlaveDeEditarPronostico));
                clientePronostico = ProveedorGlobal.ClientePronostico;

                var info = Task.Run(async () => {
                    informacionDeVenta = await proveedorDeCliente.ObtenerInformacionDeVenta(new DataVendedor(vendedor.Valor.Id));
                });
                info.Wait();
                if (informacionDeVenta.Tipo == TipoDeResultado.Exito)
                {

                    foreach (var articuloMovil in informacionDeVenta.Valor)
                    {
                        articulosDeVista.Add(new ArticuloMovil(articuloMovil.ArticuloId, articuloMovil.NombreArticulo, articuloMovil.Monto, articuloMovil.Sitio,
                                                                articuloMovil.Grupo, Activity, articuloMovil.Impuesto));
                    }
                }

            }
            catch (Exception ex)
            {

                Activity.MostrarMensaje(ex.Message);
            }
        }
        public static EditarPronostico NewInstance()
        {
            var frag1 = new EditarPronostico { Arguments = new Bundle() };
            return frag1;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view;
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            if (clientePronostico != null)
            {
                view = ConfigurarVista(inflater, container);
            }
            else
            {
                view = inflater.Inflate(Resource.Layout.Error, container, false);
                view.FindViewById<TextView>(Resource.Id.ErrorTexto).Text =
                   "No tienes ruta cargada comunicate con tu jefe inmediato o con el departamento de INE.";
            }
            return view;
        }
        private View ConfigurarVista(LayoutInflater inflater, ViewGroup container)
        {
           // var Cte = clientesPronostico.First();
           // filtroCliente = NoCte.Cliente.ToString();
            var view = inflater.Inflate(Resource.Layout.EditarPronostico, null);
            CtePronosticoEditar = view.FindViewById<AutofitTextView>(Resource.Id.CtePronosticoEditar);
            CtePronosticoEditar.Text = clientePronostico.Cliente + " - " + clientePronostico.NombreCte;
            VolumenProEditar = view.FindViewById<EditText>(Resource.Id.VolumenProEditar);
            VolumenProEditar.Text = clientePronostico.Volumen;

            SpRecursosEditarPro = view.FindViewById<Spinner>(Resource.Id.spRecursosProEditar);
            adapter = new AdaptadorSpinnerArticulosPro(Activity, articulosDeVista);
            SpRecursosEditarPro.Adapter = adapter;
            var obtieneRecurso = articulosDeVista.Where(e => e.ArticuloId == clientePronostico.IdRecurso);
       //     IdRecurso = obtieneRecurso.Select(t => t.ArticuloId);

            for (int x = 0; x < articulosDeVista.Count; x++)
            {
                if(articulosDeVista[x].ArticuloId == clientePronostico.IdRecurso)
                {
                    Position = articulosDeVista.IndexOf(articulosDeVista[x]);
                    IdRecurso = articulosDeVista[x].ArticuloId;
                }

            }

            SpRecursosEditarPro.SetSelection(Position);
            SpRecursosEditarPro.Enabled = false;
            SpRecursosEditarPro.ItemSelected += SpRecursosEditarPro_ItemSelected;

            _dateDisplayEditarPro  = view.FindViewById<TextView>(Resource.Id.date_displayPro);
            _dateDisplayEditarPro.Text = clientePronostico.Fecha;
            FechaPronostico = clientePronostico.Fecha;
            date_select_buttonPro = view.FindViewById<Button>(Resource.Id.date_select_buttonPro);
            date_select_buttonPro.Enabled = false;
            date_select_buttonPro.Click += Date_select_buttonPro_Click;

            EditarPronosticoCte = view.FindViewById<Button>(Resource.Id.EditarPronosticoCte);
            EditarPronosticoCte.Click += EditarPronosticoCte_Click;
            BtnRegresarEditar = view.FindViewById<Button>(Resource.Id.BtnRegresarEditar);
            BtnRegresarEditar.Click += BtnRegresarEditar_Click;


            return view;
        }

        private void Date_select_buttonPro_Click(object sender, EventArgs e)
        {
            DatePickerFragmentV4 frag = DatePickerFragmentV4.NewInstance(delegate (DateTime time)
            {
                _dateDisplayEditarPro.Text = time.ToString("yyyy-MM-dd");
                FechaPronostico = time.ToString("yyyy-MM-dd");
            });
            frag.Show(FragmentManager, DatePickerFragmentV4.TAG);
        }

        private void SpRecursosEditarPro_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                IdRecurso = articulosDeVista[e.Position].ArticuloId;
            }
            catch (Exception ex)
            {
                Activity.MostrarMensaje("Ocurrio un error: " + ex, ToastLength.Long);
            }
        }

        private async void BtnRegresarEditar_Click(object sender, EventArgs e)
        {
            try
            {
              
                resultadoClientePronostico = await proveedorDePronostico.ConsultaClientePronostico(new DataClientePronostico(clientePronostico.Cliente, Configuracion.Compania, vendedor.Valor.Almacen, ObtieneFiltros));
                var arguments = new Bundle();
                if (resultadoClientePronostico.Tipo == TipoDeResultado.Exito)
                {
                    Arguments.PutStringArray(PronosticoFragment.LLavePronostico, ProveedorDeManipulacionDeDatos.GenerarTodos(resultadoClientePronostico.Valor));
                }
                else
                {
                    Arguments.PutStringArray(PronosticoFragment.LLavePronostico, ProveedorDeManipulacionDeDatos.GenerarTodos(ProveedorGlobal.ListClientePronostico));
                }
                arguments = Arguments;
                fragmentV4 = PronosticoFragment.NewInstance();
                fragmentV4.Arguments = Arguments;
                FragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragmentV4)
                .Commit();
            }
            catch (Exception ex)
            {
                Activity.MostrarMensaje(ex.Message);
            }
        }

        private async void EditarPronosticoCte_Click(object sender, EventArgs e)
        {
            try
            {
                if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
                {
                    Snackbar.Make(View, "No estas conectado a internet, intentalo mas tarde", Snackbar.LengthLong)
                       .Show();
                }
                else
                {                 
                    decimal Volumen = Convert.ToDecimal(VolumenProEditar.Text);                   
                    var editar = await proveedorDePronostico.ActualizaPronostico(new DataInsertPronostico(vendedor.Valor.Usuario, FechaPronostico, vendedor.Valor.Almacen, clientePronostico.Cliente, 
                                                    vendedor.Valor.Id, IdRecurso, Volumen));
                    if (editar.Tipo == TipoDeResultado.Exito)
                    {
                        EditarPronosticoCte.Enabled = false;
                        Snackbar.Make(View, "Se actualizo correctamente tu información", Snackbar.LengthLong)
                      .Show();
                    }
                }

            }
            catch (Exception ex)
            {
                Snackbar.Make(View, "Ocurrio un error", Snackbar.LengthLong)
                    .Show();
            }
        }
    }
}