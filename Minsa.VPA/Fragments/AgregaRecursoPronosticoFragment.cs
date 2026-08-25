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

using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Minsa.VPA.Fragments
{
    [Obsolete]
    public class AgregaRecursoPronosticoFragment : Fragment
    {
        View view;
        public const string LLaveAgregaRecurso = "AgreaRecursoPronostico";
        public const string LLaveAgregaFiltros = "LLaveAgregaFiltros";
        public ResultadoDeOperacionGenerico<Vendedor> vendedor;
        private AdaptadorSpinnerArticulosPro adapter;
        public List<ClientePronostico> clientesPronostico;
        AutofitTextView CtePronostico;
        public ResultadoDeOperacionGenerico<List<InformacionDeVenta>> informacionDeVenta;
        ProveedorDeClientes proveedorDeCliente = new ProveedorDeClientes();
        private readonly List<ArticuloMovil> articulosDeVista = new List<ArticuloMovil>();
        private Spinner spRecursosPro;
        string IdRecurso;        
        EditText VolumenPro;
        TextView _dateDisplay;
        Button _dateSelectButton;
        Button GuardaPronosticoCte;
        Button BtnRegresar;
        Android.App.FragmentManager fragment = null;
        Android.Support.V4.App.Fragment fragmentV4 = null;
        string FechaPronostico;
        string filtroCliente;
        string filtroMes;
        ProveedorDePronostico proveedorDePronostico = new ProveedorDePronostico();
        ResultadoDeOperacionGenerico<List<ClientePronostico>> resultadoClientePronostico;
        string ObtieneFiltros;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            vendedor = ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Arguments.Obtener<string>(MainActivity.LlaveUsuario));
          
            // Create your fragment here
            try
            {
                Activity.MostrarMensaje("Cargando información del pronostico");
                clientesPronostico = ProveedorDeManipulacionDeDatos.ObtenerTodos<ClientePronostico>(Arguments.Obtener<string[]>(LLaveAgregaRecurso))
                .ConvertirLista();

                ObtieneFiltros = ProveedorDeManipulacionDeDatos.Obtener<string>(Arguments.Obtener<string>(LLaveAgregaFiltros)).ToString();
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
        public static AgregaRecursoPronosticoFragment NewInstance()
        {
            var frag1 = new AgregaRecursoPronosticoFragment { Arguments = new Bundle() };
            return frag1;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {


            if (articulosDeVista != null)
            {
                view = ConfigurarVista(inflater, container);
            }
            else
            {
                view = inflater.Inflate(Resource.Layout.Error, container, false);
                view.FindViewById<TextView>(Resource.Id.ErrorTexto).Text =
                   "No tienes cargada tu lista de articulos comunicate con el departamento de INE.";
            }
            
            return view;
            //// Use this to return your custom view for this Fragment
            //// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
        }
        private View ConfigurarVista(LayoutInflater inflater, ViewGroup container)
        {
            View view = inflater.Inflate(Resource.Layout.AgregaRecursoPronostico, container, false);
            //var NoCte = clientesPronostico.GroupBy(t => t.Cliente).FirstOrDefault();
            var NoCte = clientesPronostico.First();
            filtroCliente = NoCte.Cliente.ToString();
           
            CtePronostico = view.FindViewById<AutofitTextView>(Resource.Id.CtePronostico);
            CtePronostico.Text = NoCte.Cliente + " - " + NoCte.NombreCte;

            spRecursosPro = view.FindViewById<Spinner>(Resource.Id.spRecursosPro);
            adapter = new AdaptadorSpinnerArticulosPro(Activity, articulosDeVista);
            spRecursosPro.Adapter = adapter;
            spRecursosPro.ItemSelected += SpRecursosPro_ItemSelected;           
            
            VolumenPro = view.FindViewById<EditText>(Resource.Id.VolumenPro);

            _dateDisplay = view.FindViewById<TextView>(Resource.Id.date_display);
            _dateSelectButton = view.FindViewById<Button>(Resource.Id.date_select_button);
            _dateSelectButton.Click += _dateSelectButton_Click;
            GuardaPronosticoCte = view.FindViewById<Button>(Resource.Id.GuardaPronosticoCte);
            BtnRegresar = view.FindViewById<Button>(Resource.Id.BtnRegresar);
            BtnRegresar.Click += BtnRegresar_Click;
            GuardaPronosticoCte.Click += GuardaPronosticoCte_Click;

            return view;
        }

        private async void BtnRegresar_Click(object sender, EventArgs e)
        {
            try
            {
                resultadoClientePronostico = await proveedorDePronostico.ConsultaClientePronostico(new DataClientePronostico(filtroCliente, Configuracion.Compania, vendedor.Valor.Almacen, ObtieneFiltros));
                var arguments = new Bundle();
                if(resultadoClientePronostico.Tipo == TipoDeResultado.Exito)
                {
                    ProveedorGlobal.ClientePronostico = null;
                    Arguments.PutStringArray(PronosticoFragment.LLavePronostico, ProveedorDeManipulacionDeDatos.GenerarTodos(resultadoClientePronostico.Valor));
                }
                else
                {
                    Arguments.PutStringArray(PronosticoFragment.LLavePronostico, ProveedorDeManipulacionDeDatos.GenerarTodos(clientesPronostico));
                }                
                arguments = Arguments;
                fragmentV4 = PronosticoFragment.NewInstance();
                fragmentV4.Arguments = Arguments;
                FragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragmentV4)
                .Commit();
            }
            catch(Exception ex)
            {
                Activity.MostrarMensaje(ex.Message);
            }            
        }

        private async void GuardaPronosticoCte_Click(object sender, EventArgs e)
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
                    var cliente = clientesPronostico.First();
                    decimal Volumen = Convert.ToDecimal(VolumenPro.Text);
                    var insertar = await proveedorDePronostico.InsertarPronostico(new DataInsertPronostico(vendedor.Valor.Usuario, FechaPronostico,vendedor.Valor.Almacen, filtroCliente, vendedor.Valor.Id,IdRecurso,Volumen));
                    if(insertar.Tipo == TipoDeResultado.Exito)
                    {
                        GuardaPronosticoCte.Enabled = true;
                        Snackbar.Make(View, "Se agrego correctamente tu información", Snackbar.LengthLong)
                      .Show();
                    }
                }

            }catch(Exception ex)
            {
                Snackbar.Make(View, "Ocurrio un error", Snackbar.LengthLong)
                    .Show();
            }
        }

        private void _dateSelectButton_Click(object sender, EventArgs e)
        {
            DatePickerFragmentV4 frag = DatePickerFragmentV4.NewInstance(delegate (DateTime time)
            {
                _dateDisplay.Text = time.ToString("yyyy-MM-dd");
                FechaPronostico = time.ToString("yyyy-MM-dd");
            });
            frag.Show(FragmentManager, DatePickerFragmentV4.TAG);
        }              

        private void SpRecursosPro_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                IdRecurso = articulosDeVista[e.Position].ArticuloId;
            }
            catch(Exception ex)
            {
                Activity.MostrarMensaje("Ocurrio un error: " + ex, ToastLength.Long);
            }
        }
    }
}