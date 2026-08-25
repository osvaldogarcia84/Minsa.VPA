using System.Collections.Generic;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using Minsa.VPA.Modelos;
using Minsa.VPA.Proveedores;
using Android.Support.V4.Widget;
using Minsa.VPA.Servicios;
using System;
using System.Linq;
using Android.Widget;
using System.Threading.Tasks;
using System.Timers;
using Minsa.VPA.Adaptadores;
using Grantland.Widget;

namespace Minsa.VPA.Fragments
{
    public class InicioSupervisorFragment :  Fragment, SwipeRefreshLayout.IOnRefreshListener 
    {
        public const string LLaveInicio = "Inicio";
        public ResultadoDeOperacionGenerico<Vendedor> vendedor;
        
        public List<InformacionInicioSupervisor> InformacionInicioSup = new List<InformacionInicioSupervisor>();
        
        ResultadoDeOperacionGenerico<List<InfoDashboardSupervisores>> resultadoDashboardSup;
        
        public RecyclerView recyclerSup;
        
        private RecyclerViewAdapterSupervisor adapterSup;
        private RecyclerView.LayoutManager LayoutManager;
        public const string LlaveDeInicioSupervisor = "InicioSupervisor";
        ProveedorDeClientes proveedorDeClientes = new ProveedorDeClientes();        
        private SwipeRefreshLayout swipeRefreshLayoutSup;
        private int color = 0;
        public decimal SumaSacosActual = 0;
        public string Almacen = String.Empty;
        public int contador;
        public List<Transacciones> resultadoTransacciones;
        public AutofitTextView NombreSupervisor { get; set; }
        public AutofitTextView Usuario { get; set; }
        Timer timer;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            vendedor = ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Arguments.Obtener<string>(MainActivity.LlaveUsuario));
            var info = Task.Run(async () => {
                resultadoDashboardSup = await proveedorDeClientes.DashBoardSupervisores(new DataDashBoardSupervisores(vendedor.Valor.Usuario));
            });
            info.Wait();
        }
        public static InicioSupervisorFragment NewInstance()
        {
            var frag1 = new InicioSupervisorFragment { Arguments = new Bundle() };
            return frag1;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view;

            var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            if (Xamarin.Essentials.Connectivity.NetworkAccess == Xamarin.Essentials.NetworkAccess.Internet)
            {
                view = ConfigurarVistaSupervisor(inflater, container);               
            }
            else
            {
                view = inflater.Inflate(Resource.Layout.Error, container, false);
                view.FindViewById<TextView>(Resource.Id.ErrorTexto).Text =
                   "No se puede mostrar el dashboard. Sin conexion a internet";
            }

            return view;
        }
        private View ConfigurarVistaSupervisor(LayoutInflater inflater, ViewGroup container)
        {
            var view = inflater.Inflate(Resource.Layout.InicioSupervisor, null);
            InitDataSupervisor();

            //NombreSupervisor = view.FindViewById<AutofitTextView>(Resource.Id.NombreSupervisor);
            //Usuario = view.FindViewById<AutofitTextView>(Resource.Id.IniUsuario);

            //NombreSupervisor.Text = "Nombre del supervisor: " + vendedor.Valor.Nombre;
            //Usuario.Text = "Usuario :" + vendedor.Valor.Usuario;

            recyclerSup = view.FindViewById<RecyclerView>(Resource.Id.RecycledInicioSupervisor);
            recyclerSup.HasFixedSize = true;
            LayoutManager = new LinearLayoutManager(this.Activity);
            var layoutManager = new LinearLayoutManager(Activity);
            var onScrollListener = new XamarinRecyclerViewOnScrollListener(layoutManager);
            recyclerSup.AddOnScrollListener(onScrollListener);
            recyclerSup.SetLayoutManager(LayoutManager);
            adapterSup = new RecyclerViewAdapterSupervisor(InformacionInicioSup);
            recyclerSup.SetAdapter(adapterSup);

            swipeRefreshLayoutSup = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout_recycler_view2);
            swipeRefreshLayoutSup.SetColorSchemeResources(Resource.Color.google_blue, Resource.Color.google_green, Resource.Color.google_red, Resource.Color.google_yellow);
            swipeRefreshLayoutSup.SetOnRefreshListener(this);
            return view;
        }

        public void OnRefresh()
        {
            new Handler().PostDelayed(() =>
            {

                if (color > 4)
                {
                    color = 0;
                }
                adapterSup.SetColor(++color);
                swipeRefreshLayoutSup.Refreshing = false;
                InformacionInicioSup.Clear();
                //InformacionInicioSup.RemoveAt(0);
                InitDataSupervisor();
                adapterSup = new RecyclerViewAdapterSupervisor(InformacionInicioSup);
                recyclerSup.SetAdapter(adapterSup);
                adapterSup.NotifyDataSetChanged();               
            }, 1000);
        }
        public void InitDataSupervisor()
        {

            if (resultadoDashboardSup.Tipo == TipoDeResultado.Exito)
            {

                foreach (var data in resultadoDashboardSup.Valor)
                {
                    InformacionInicioSup.Add(new InformacionInicioSupervisor(1, vendedor.Valor.Nombre, vendedor.Valor.Usuario, data.Almacen, data.ClientesRuta, data.Visitas, data.SacoDisponible,
                                                            data.ImporteDepositar));
                }

            }
        }
        public class XamarinRecyclerViewOnScrollListener : RecyclerView.OnScrollListener
        {
            public delegate void LoadMoreEventHandler(object sender, EventArgs e);
            public event LoadMoreEventHandler LoadMoreEvent;

            private LinearLayoutManager LayoutManager;

            public XamarinRecyclerViewOnScrollListener(LinearLayoutManager layoutManager)
            {
                LayoutManager = layoutManager;
            }

            public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
            {
                base.OnScrolled(recyclerView, dx, dy);

                var visibleItemCount = recyclerView.ChildCount;
                var totalItemCount = recyclerView.GetAdapter().ItemCount;
                var pastVisiblesItems = LayoutManager.FindFirstVisibleItemPosition();

                if ((visibleItemCount + pastVisiblesItems) >= totalItemCount)
                {
                    LoadMoreEvent(this, null);
                }
            }
        }
    }
}