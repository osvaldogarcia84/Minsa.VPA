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
    public class InicioFragment : Fragment, SwipeRefreshLayout.IOnRefreshListener 
    {
        public const string LLaveInicio = "Inicio";
        public ResultadoDeOperacionGenerico<Vendedor> vendedor;
        public List<InformacionInicio> InformacionInicio = new List<InformacionInicio>();
        //public List<InformacionInicioSupervisor> InformacionInicioSup = new List<InformacionInicioSupervisor>();
        ResultadoDeOperacionGenerico<DashBoard> resultadoDashboard;
        //ResultadoDeOperacionGenerico<List<InfoDashboardSupervisores>> resultadoDashboardSup;
        public RecyclerView recycler;
        //public RecyclerView recyclerSup;
        private RecyclerViewAdapter adapter;
        //private RecyclerViewAdapterSupervisor adapterSup;
        private RecyclerView.LayoutManager LayoutManager;
        public const string LlaveDeInicio = "Inicio";
        ProveedorDeClientes proveedorDeClientes = new ProveedorDeClientes();
        private SwipeRefreshLayout swipeRefreshLayout;
        //private SwipeRefreshLayout swipeRefreshLayoutSup;
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
            Almacen = vendedor.Valor.Almacen;
            var t = Task.Run(async () => {
                resultadoDashboard = await proveedorDeClientes.DashBoard(new DataSitio(vendedor.Valor.Almacen));
            });
            t.Wait();
            

        }

        //public async Task<ResultadoDeOperacionGenerico<List<Cliente>>> ObtieneClientes()
        //{
        //    //Task<ResultadoDeOperacionGenerico<List<Cliente>>> clientes;
        //    var clientes = await proveedorDeClientes.ObtenerTodosPorRuta(new DataVendedor(vendedor.Valor.Id));
        //    return clientes;
        //}
        public static InicioFragment NewInstance()
        {
            var frag1 = new InicioFragment { Arguments = new Bundle() };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view;           

            var ignored = base.OnCreateView(inflater, container, savedInstanceState);    
            
            if (Xamarin.Essentials.Connectivity.NetworkAccess == Xamarin.Essentials.NetworkAccess.Internet)
            {            
                view = ConfigurarVista(inflater, container);              
            }
            else
            {
                view = inflater.Inflate(Resource.Layout.Error, container, false);
                view.FindViewById<TextView>(Resource.Id.ErrorTexto).Text =
                   "No se puede mostrar el dashboard. Sin conexion a internet";
            }

            return view;
        }

       
        private View ConfigurarVista(LayoutInflater inflater, ViewGroup container)
        {
            var view = inflater.Inflate(Resource.Layout.Inicio, null);
            InitData();

            recycler = view.FindViewById<RecyclerView>(Resource.Id.RecycledInicio);
            recycler.HasFixedSize = true;
            LayoutManager = new LinearLayoutManager(this.Activity);
            var layoutManager = new LinearLayoutManager(Activity);
            var onScrollListener = new XamarinRecyclerViewOnScrollListener(layoutManager);
            recycler.AddOnScrollListener(onScrollListener);
            recycler.SetLayoutManager(LayoutManager);
            adapter = new RecyclerViewAdapter(InformacionInicio);
            recycler.SetAdapter(adapter);

            swipeRefreshLayout = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout_recycler_view);
            swipeRefreshLayout.SetColorSchemeResources(Resource.Color.google_blue, Resource.Color.google_green, Resource.Color.google_red, Resource.Color.google_yellow);
            swipeRefreshLayout.SetOnRefreshListener(this);
            return view;
        }
        public void InitData()
        {    
            if (resultadoDashboard.Tipo == TipoDeResultado.Exito)
            {         
                InformacionInicio.Add(new InformacionInicio(1, vendedor.Valor.Nombre, vendedor.Valor.Id, Almacen, resultadoDashboard.Valor.ClientesRuta, resultadoDashboard.Valor.Visitas, resultadoDashboard.Valor.SacoDisponible,
                                                            resultadoDashboard.Valor.ImporteDepositar));
            }
        }       
        public void OnRefresh()
        {
            new Handler().PostDelayed(() =>
            {

                if (color > 4)
                {
                    color = 0;
                }
                adapter.SetColor(++color);
                swipeRefreshLayout.Refreshing = false;
                InformacionInicio.RemoveAt(0);
                InitData();
                adapter = new RecyclerViewAdapter(InformacionInicio);
                recycler.SetAdapter(adapter);
                adapter.NotifyDataSetChanged();
                             
            }, 1000);
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
        //public static bool ExisteComunicacion()
        //{

        //    Android.Net.ConnectivityManager connectivityManager = (Android.Net.ConnectivityManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.ConnectivityService);
        //    Android.Net.NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;
        //    bool isOnline = (activeConnection != null) && activeConnection.IsConnected;
        //    if (isOnline == false)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

    }
}