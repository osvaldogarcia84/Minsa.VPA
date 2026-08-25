using Android.App;

using Android.Content.PM;
using Android.OS;

using Minsa.VPA.Modelos;
using Minsa.VPA.Servicios;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using Minsa.VPA.Fragments;
using Minsa.VPA.Proveedores;
using System.Linq;
using Android.Locations;
using System;
using Android.Content;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Timers;
using Minsa.VPA.Adaptadores;

namespace Minsa.VPA.Activities
{
    [Activity(Label = "@string/app_name", LaunchMode = LaunchMode.SingleTop, ScreenOrientation = ScreenOrientation.Portrait)]
    public class ContenidoActivity : BaseActivity
    {
        DrawerLayout drawerLayout;
        NavigationView navigationView;
        private ResultadoDeOperacionGenerico<Vendedor> _vendedor;
        ProveedorDeClientes proveedorDeClientes = new ProveedorDeClientes();
        ProveedorDePronostico proveedorDePronostico = new ProveedorDePronostico();
        Android.Support.V4.App.Fragment fragment = null;
        private ProveedorDeLocacion _proveedorDeLocacion;
        public const string LLave = "VPA";
        readonly View view;
        ResultadoDeOperacionGenerico<List<Cliente>> resultadoCliente;
        ResultadoDeOperacionGenerico<List<Cliente>> resultadoObtenerClientesSupervisores;
        ResultadoDeOperacionGenerico<List<Cliente>> resultadoObtenerTodos;
        ResultadoDeOperacionGenerico<List<Sitios>> resultadoSitios;
        ResultadoDeOperacionGenerico<List<ClientePronostico>> resultadoClientePronostico;
        List<Sitios> ListSitios;
        Spinner SPSitios;
        Spinner SPMes;
        string Sitio;
        List<ItemGenerico> itemsGenericos;
        public AdapterSitios adapterSitios;
        List<ItemGenerico> mes;
        string Mes;
        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.ContenidoActivity;
            }
        }
        Timer timer;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            _vendedor = ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Intent.Obtener<string>(MainActivity.LlaveUsuario));
            drawerLayout = this.FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            resultadoCliente = await proveedorDeClientes.ObtenerTodosPorRuta(new DataVendedor(_vendedor.Valor.Id));
            resultadoSitios = await proveedorDeClientes.Sitios(new DataSitios( _vendedor.Valor.Usuario));
            resultadoObtenerClientesSupervisores = new ResultadoDeOperacionGenerico<List<Cliente>>(TipoDeResultado.Exito, "",null);
            itemsGenericos = ObtieneMes();

            //var geofenceService = new GeofenceService(Activity);

            //bool resultadoGeocercas =
            //    await geofenceService
            //        .RegistrarGeocercasAsync(
            //            clientesGeocerca);

            //Android.Util.Log.Debug(
            //    "VPA_GEOFENCE",
            //    "Resultado registro Android: " +
            //    resultadoGeocercas);

            //  resultadoObtenerClientesSupervisores = new ResultadoDeOperacionGenerico<List<Cliente>>;
            //_proveedorDeLocacion = new ProveedorDeLocacion(this, 900000); // 900000 15 minutos
            //_proveedorDeLocacion.LocacionEncontrada += ActualizarLocacion;

            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);


            navigationView.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);

                switch (e.MenuItem.ItemId)
                {
                    case Resource.Id.nav_home_1:
                        ListItemClicked(0);
                        break;
                    case Resource.Id.nav_Rutas:
                        ListItemClicked(1);
                        break;
                    case Resource.Id.nav_Descargar_Rutas:
                        ListItemClicked(2);
                        break;
                    case Resource.Id.nav_Busqueda_Clientes:
                        ListItemClicked(3);
                        break;
                    //case Resource.Id.nav_Carrito:
                    //    ListItemClicked(4);
                    //    break;
                    case Resource.Id.nav_YALO:
                        ListItemClicked(4);
                        break;
                    case Resource.Id.nav_Inventario:
                        ListItemClicked(5);
                        break;
                    case Resource.Id.nav_Recibo:
                        ListItemClicked(6);
                        break;
                    case Resource.Id.nav_Pronostico:
                        ListItemClicked(7);
                        break;
                    case Resource.Id.nav_HistoricoVenta:
                        ListItemClicked(8);
                        break;
                    case Resource.Id.nav_AltasClientes:
                        ListItemClicked(9);
                        break;
                  
                    //case Resource.Id.nav_SeguimientoVisita:
                    //    ListItemClicked(9);
                    // break;
                    //case Resource.Id.nav_Geocerca:
                    //    ListItemClicked(7);
                    //    break;
                    //case Resource.Id.nav_Degustación:
                    //    ListItemClicked(11);
                    //    break;
                    case Resource.Id.nav_Ajustes:
                        ListItemClicked(10);
                        break;
                    case Resource.Id.nav_salir:
                        ListItemClicked(11);
                        break;                   
                }

                //Snackbar.Make(drawerLayout, "Seleccionaste: " + e.MenuItem.TitleFormatted, Snackbar.LengthLong)
                //.Show();

                drawerLayout.CloseDrawers();
            };

            //if first time you will want to go ahead and click first item.
            if (savedInstanceState == null)
            {
                ListItemClicked(0);
            }

        }
        //=======================================================================================================================

        //private void ActualizarLocacion(object sender, Location location)
        //{
        //    Toast.MakeText(this, String.Format("{0} - {1}", location.Longitude, location.Latitude), ToastLength.Short).Show();
     
        //}
        //=======================================================================================================================

        int oldPosition = -1;
        public void ListItemClicked(int position)
        {
            //if (position == oldPosition)
            //    return;
            oldPosition = position;
            //Android.Support.V4.App.Fragment fragment = null;
            var arguments = new Bundle();
            switch (position)
            {
                case 0:
                    Inicio();
                    break;
                case 1:
                    Ruta();
                    break;
                case 2:
                    DescargarRuta();
                    break;
                case 3:
                    BuscarClientes();
                    break;
                //case 4:
                //    Carrito();
                //    break;
                case 4:
                    YALO();
                    break;
                case 5:
                    Inventario();
                    break;
                case 6:
                    Recibo();
                    break;
                case 7:
                    Pronostico();
                    break;
                case 8:
                    HistoricoVenta();
                    break;
                case 9:
                    AltaClientes();
                    break;                   
                
                //case 9:
                //    SeguimientoVisita();
                //    break;
                //case 7:
                //    Geocerca();
                //    break;
                //case 11:
                //    Degustacion();
                //    break;
                case 10:
                    Ajustes();
                    break;
                case 11:
                    Salir();
                    break;             
            }
            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .Commit();

        }
        //=======================================================================================================================
        private bool TieneClienteSeleccionado()
        {
            if (ProveedorGlobal.Cliente != null)
                return true;
            Toast.MakeText(this, "Debes seleccionar un cliente de tu ruta", ToastLength.Short).Show();
            return false;
        }
        private Android.Support.V4.App.Fragment Inicio()
        {
            var arguments = new Bundle();
            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                Toast.MakeText(this, "No estas conectado a internet para navegar en dashboard.", ToastLength.Short).Show();
                return null;
            }
            if (_vendedor.Valor.Almacen == "Supervisor")
            {
             
                Intent.PutExtra(InicioSupervisorFragment.LlaveDeInicioSupervisor, ProveedorDeManipulacionDeDatos.Generar(_vendedor));
                arguments = Intent.Extras;
                fragment = InicioSupervisorFragment.NewInstance();
                fragment.Arguments = arguments;
                return new InicioSupervisorFragment();
            }
            else
            {
                Intent.PutExtra(InicioFragment.LLaveInicio, ProveedorDeManipulacionDeDatos.Generar(_vendedor));
                arguments = Intent.Extras;
                fragment = InicioFragment.NewInstance();
                fragment.Arguments = arguments;
                return new InicioFragment();
            }           
            
        }
        private Android.Support.V4.App.Fragment Ruta()
        {

            var arguments = new Bundle();
            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                    if (ProveedorGlobal.ClienteMovil != null)
                    {
                        Intent.PutExtra(RutaFragment.LlaveDeRuta, ProveedorDeManipulacionDeDatos.GenerarTodos(ProveedorGlobal.ClienteMovil.OrderBy(e => e.Secuencia)));
                        arguments = Intent.Extras;
                        fragment = RutaFragment.NewInstance();
                        fragment.Arguments = arguments;
                        return new RutaFragment();
                    }
                    else
                    {
                        Toast.MakeText(this, "Descarga tu ruta", ToastLength.Short).Show();
                        return null;
                    }                            
            }
            else
            {                
                if (_vendedor.Valor.Almacen == "Supervisor")
                {
                    
                    var vistaSitios = LayoutInflater.Inflate(Resource.Layout.SitiosSupervisores, null);
                    vistaSitios.FindViewById<TextView>(Resource.Id.Titulo2).Text = "Selecciona un sitio";
                    SPSitios = vistaSitios.FindViewById<Spinner>(Resource.Id.SPSitiosSupervisores);
                    if (resultadoSitios.Tipo == TipoDeResultado.Exito)
                    {
                        ListSitios = resultadoSitios.Valor;

                        adapterSitios = new AdapterSitios(this, ListSitios);
                        SPSitios.Adapter = adapterSitios;

                        SPSitios.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
                        {
                            Sitio = Convert.ToString(resultadoSitios.Valor[e.Position].SITIOS);
                        };
                    }
                    else
                    {
                        Toast.MakeText(this, "Debes seleccionar un cliente de tu ruta", ToastLength.Short).Show();
                    }
                    var builder = new AlertDialog.Builder(this);
                    builder.SetTitle("Información");
                    builder.SetView(vistaSitios);                   
                    builder.SetPositiveButton("Aceptar", (EventHandler<DialogClickEventArgs>)null);
                    builder.SetNegativeButton("Cancelar", (EventHandler<DialogClickEventArgs>)null);
                    var dialog = builder.Create();                  
                    dialog.Show();                    
                    var yesBtn = dialog.GetButton((int)DialogButtonType.Positive);
                    var noBtn = dialog.GetButton((int)DialogButtonType.Negative);                    
                    yesBtn.Click += (sender, args) =>
                    {
                        var supervisores = Task.Run(async () =>
                        {
                            resultadoObtenerClientesSupervisores = await proveedorDeClientes.ObtenerTodosPorRutaSupervisor(new DataSupervisores(_vendedor.Valor.Usuario, Sitio));
                        });
                        supervisores.Wait();

                        if (resultadoObtenerClientesSupervisores.Tipo == TipoDeResultado.Exito)
                        {
                            var clientesSupervisores = resultadoObtenerClientesSupervisores.Valor;
                            Intent.PutExtra(RutaFragment.LlaveDeRuta, ProveedorDeManipulacionDeDatos.GenerarTodos(clientesSupervisores.OrderBy(e => e.Secuencia)));
                            arguments = Intent.Extras;
                            fragment = RutaFragment.NewInstance();
                            fragment.Arguments = arguments;
                            SupportFragmentManager.BeginTransaction()
                            .Replace(Resource.Id.content_frame, fragment)
                            .Commit();
                            dialog.Hide();
                        }
                        else
                        {
                            Toast.MakeText(this, "No hay ruta cargada para el sitio seleccionado.", ToastLength.Short).Show();
                            // return null;
                        }
                    };
                    
                    return new RutaFragment();
                }
                else
                {
                    if (resultadoCliente.Tipo != TipoDeResultado.Exito)
                    {
                        Toast.MakeText(this, resultadoCliente.Mensaje, ToastLength.Short).Show();
                        return null;
                    }
                    else
                    {
                        Intent.PutExtra(RutaFragment.LlaveDeRuta, ProveedorDeManipulacionDeDatos.GenerarTodos(resultadoCliente.Valor.OrderBy(e => e.Secuencia)));
                        ProveedorGlobal.ClientesFlujo = resultadoCliente.Valor.OrderBy(e => e.Secuencia).ToList();
                        arguments = Intent.Extras;
                        fragment = RutaFragment.NewInstance();
                        fragment.Arguments = arguments;
                        return new RutaFragment();
                    }
                }               
            }
           
        }   
        private Android.Support.V4.App.Fragment DescargarRuta()
        {
            var arguments = new Bundle();
            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                //Snackbar.Make(view, "No estas conectado a internet para realizar la descarga de tu ruta", Snackbar.LengthLong)
                //    .Show();
                Toast.MakeText(this, "No estas conectado a internet para realizar la descarga de tu ruta", ToastLength.Short).Show();
                return null;
            }

            if(_vendedor.Valor.Almacen == "Supervisor")
            {
                Toast.MakeText(this, "No tienes acceso a este apartado", ToastLength.Short).Show();
                return null;
            }
            arguments = Intent.Extras;
            fragment = DescargarRutaFragment.NewInstance();
            fragment.Arguments = arguments;
            return new DescargarRutaFragment();
        }
        private Android.Support.V4.App.Fragment BuscarClientes()
        {
            var arguments = new Bundle();
            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                //Snackbar.Make(view, "No estas conectado a internet para realizar la descarga de tu ruta", Snackbar.LengthLong)
                //    .Show();
                Toast.MakeText(this, "No estas conectado a internet para realizar la busqueda", ToastLength.Short).Show();
                return null;
            }
            var vistaDePregunta = LayoutInflater.Inflate(Resource.Layout.Pregunta, null);

            vistaDePregunta.FindViewById<TextView>(Resource.Id.PreguntaSimpleTexto).Text =
                       "Búsqueda por nombre / número de socio";


            new Android.Support.V7.App.AlertDialog.Builder(this)
                   .SetTitle(this.GetString(Resource.String.dialogBuscarCliente))
                   .SetView(vistaDePregunta)
                   //.SetMessage(this.GetString(Resource.String.main_dialog_simple_message))
                   .SetPositiveButton(this.GetString(Resource.String.dialog_ok), (sender, args) =>
                   {
                       string filtro = vistaDePregunta.FindViewById<EditText>(Resource.Id.PreguntaSimpleRespuesta).Text;

                       if(_vendedor.Valor.Almacen == "Supervisor")
                       {
                           var obtener = Task.Run(async () => {
                               resultadoObtenerTodos = await proveedorDeClientes.ObtenerTodos(new DataObtenerTodos(_vendedor.Valor.Usuario, filtro));
                           });
                           obtener.Wait();

                           if (resultadoObtenerTodos.Tipo != TipoDeResultado.Exito)
                           {
                               Toast.MakeText(this, "El cliente buscado no esta cargado en tu ruta. Comunicate con tu supervisor o tu jefe inmediato.", ToastLength.Short).Show();
                               return;
                           }
                           else
                           {
                               ProveedorGlobal.PosicionCliente = -1;
                               Intent.PutExtra(RutaFragment.LlaveDeRuta,
                               ProveedorDeManipulacionDeDatos.GenerarTodos(resultadoObtenerTodos.Valor));
                               arguments = Intent.Extras;
                               fragment = RutaFragment.NewInstance();
                               fragment.Arguments = arguments;
                               SupportFragmentManager.BeginTransaction()
                                .Replace(Resource.Id.content_frame, fragment)
                                .Commit();
                           }
                       }
                       else
                       {
                           var obtener = Task.Run(async () => {
                               resultadoObtenerTodos = await proveedorDeClientes.ObtenerTodos(new DataObtenerTodos(_vendedor.Valor.Id, filtro));
                           });
                           obtener.Wait();

                           if (resultadoObtenerTodos.Tipo != TipoDeResultado.Exito)
                           {
                               Toast.MakeText(this, "El cliente buscado no esta cargado en tu ruta. Comunicate con tu supervisor o tu jefe inmediato.", ToastLength.Short).Show();
                               return;
                           }
                           else
                           {
                               ProveedorGlobal.PosicionCliente = -1;
                               Intent.PutExtra(RutaFragment.LlaveDeRuta,
                               ProveedorDeManipulacionDeDatos.GenerarTodos(resultadoObtenerTodos.Valor));
                               arguments = Intent.Extras;
                               fragment = RutaFragment.NewInstance();
                               fragment.Arguments = arguments;
                               SupportFragmentManager.BeginTransaction()
                                .Replace(Resource.Id.content_frame, fragment)
                                .Commit();
                           }
                       }
                       

                   })
                   .SetNegativeButton(this.GetString(Resource.String.dialog_cancel), (sender, args) => { })
                   // .SetNeutralButton(View.Context.GetString(Resource.String.dialog_neutral), (sender, args) => { })
                   .Show();

            return null;
        }
        private Android.Support.V4.App.Fragment Carrito()
        {
            var arguments = new Bundle();
            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                //Snackbar.Make(view, "No estas conectado a internet para realizar una venta", Snackbar.LengthLong)
                //    .Show();
                Toast.MakeText(this, "No estas conectado a internet para realizar una venta", ToastLength.Short).Show();
                return null;
            }
            if (!TieneClienteSeleccionado())
            {
                return null;
            }
            if (ClienteBloqueado())
                return null;

            Intent.PutExtra(CarritoFragment.LlaveDeCarrito, ProveedorDeManipulacionDeDatos.Generar(_vendedor));
            arguments = Intent.Extras;
            fragment = CarritoFragment.NewInstance();
            fragment.Arguments = arguments;
            return new CarritoFragment();
        }
      
        //private Android.Support.V4.App.Fragment SeguimientoVenta()
        //{
        //    var arguments = new Bundle();
        //    if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
        //    {
        //        //Snackbar.Make(view, "No estas conectado a internet para realizar el seguimiento de venta", Snackbar.LengthLong)
        //        //    .Show();
        //        Toast.MakeText(this, "No estas conectado a internet para realizar el seguimiento de venta", ToastLength.Short).Show();
        //        return null;
        //    }
        //    if (!TieneClienteSeleccionado())
        //    {
        //        return null;
        //    }            
        //    Intent.PutExtra(SeguimientoVentaFragment.SeguimientoVenta, ProveedorDeManipulacionDeDatos.Generar(_vendedor));         
        //    arguments = Intent.Extras;
        //    fragment = SeguimientoVentaFragment.NewInstance();
        //    fragment.Arguments = arguments;
        //    return new SeguimientoVentaFragment();
        //}
        public Android.Support.V4.App.Fragment Inventario()
        {
            var arguments = new Bundle();
            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                //Snackbar.Make(view, "No estas conectado a internet para revisar tu inventario", Snackbar.LengthLong)
                //    .Show();
                Toast.MakeText(this, "No estas conectado a internet para revisar tu inventario", ToastLength.Short).Show();
                return null;
            }

            if (_vendedor.Valor.Almacen == "Supervisor")
            {
                Toast.MakeText(this, "No tienes acceso a este apartado", ToastLength.Short).Show();
                return null;
            }
            Intent.PutExtra(InventarioClienteFragment.LLaveInventarioClienteFragment, ProveedorDeManipulacionDeDatos.Generar(_vendedor));
            //  Intent.PutExtra(SeguimientoVentaFragment.SeguimientoVenta, ProveedorDeManipulacionDeDatos.Generar(_vendedor));
            arguments = Intent.Extras;
            fragment = InventarioClienteFragment.NewInstance();
            fragment.Arguments = arguments;
            return new InventarioClienteFragment();
        }
        public Android.Support.V4.App.Fragment Recibo()
        {
            var arguments = new Bundle();
            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                //Snackbar.Make(view, "No estas conectado a internet para revisar tu inventario", Snackbar.LengthLong)
                //    .Show();
                Toast.MakeText(this, "No estas conectado a internet para revisar tu inventario", ToastLength.Short).Show();
                return null;
            }
            if (_vendedor.Valor.Almacen == "Supervisor")
            {
                Toast.MakeText(this, "No tienes acceso a este apartado", ToastLength.Short).Show();
                return null;
            }

            Intent.PutExtra(ReciboFragment.LlaveRecibo, ProveedorDeManipulacionDeDatos.Generar(_vendedor));
            arguments = Intent.Extras;
            fragment = ReciboFragment.NewInstance();
            fragment.Arguments = arguments;
            return new ReciboFragment();
        }
        public Android.Support.V4.App.Fragment YALO()
        {
            var arguments = new Bundle();
            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                //Snackbar.Make(view, "No estas conectado a internet para revisar tu inventario", Snackbar.LengthLong)
                //    .Show();
                Toast.MakeText(this, "No estas conectado a internet para revisar tu inventario", ToastLength.Short).Show();
                return null;
            }
            if (_vendedor.Valor.Almacen == "Supervisor")
            {
                Toast.MakeText(this, "No tienes acceso a este apartado", ToastLength.Short).Show();
                return null;
            }

            Intent.PutExtra(YALOFragment.LlaveYALO, ProveedorDeManipulacionDeDatos.Generar(_vendedor));
            arguments = Intent.Extras;
            fragment = YALOFragment.NewInstance();
            fragment.Arguments = arguments;
            return new YALOFragment();
        }

        //public Android.Support.V4.App.Fragment CXC()
        //{
        //    var arguments = new Bundle();
        //    if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
        //    {               
        //        Toast.MakeText(this, "No estas conectado a internet para revisar tu inventario", ToastLength.Short).Show();
        //        return null;
        //    }
        //    if (_vendedor.Valor.Almacen == "Supervisor")
        //    {
        //        Toast.MakeText(this, "No tienes acceso a este apartado", ToastLength.Short).Show();
        //        return null;
        //    }

        //    Intent.PutExtra(CXCFragment.LlaveRecibo, ProveedorDeManipulacionDeDatos.Generar(_vendedor));
        //    arguments = Intent.Extras;
        //    fragment = ReciboFragment.NewInstance();
        //    fragment.Arguments = arguments;
        //    return new ReciboFragment();
        //}
        public Android.Support.V4.App.Fragment Pronostico()
        {
            var arguments = new Bundle();
            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {                
                Toast.MakeText(this, "No estas conectado a internet para revisar tu inventario", ToastLength.Short).Show();
                return null;
            }
            if (_vendedor.Valor.Almacen == "Supervisor")
            {
                Toast.MakeText(this, "No tienes acceso a este apartado", ToastLength.Short).Show();
                return null;
            }
                      

            var vistaDePreguntaPro = LayoutInflater.Inflate(Resource.Layout.PreguntaPronostico, null);

            vistaDePreguntaPro.FindViewById<TextView>(Resource.Id.PreguntaPronostico).Text =
                       "Búsqueda por nombre / número de socio";

            vistaDePreguntaPro.FindViewById<TextView>(Resource.Id.TituloPronostico).Text = "Selecciona el mes";

            SPMes = vistaDePreguntaPro.FindViewById<Spinner>(Resource.Id.SPMes);
            
            SPMes.Adapter = new ObjetoAdapter<ItemGenerico>(this, itemsGenericos);

            SPMes.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
            {
                Mes = Convert.ToString(itemsGenericos[e.Position].Id);
            };

            new Android.Support.V7.App.AlertDialog.Builder(this)
                  .SetTitle(this.GetString(Resource.String.dialogBuscarCliente))
                  .SetView(vistaDePreguntaPro)
                  //.SetMessage(this.GetString(Resource.String.main_dialog_simple_message))
                  .SetPositiveButton(this.GetString(Resource.String.dialog_ok), (sender, args) =>
                  {
                      string filtro = vistaDePreguntaPro.FindViewById<EditText>(Resource.Id.PreguntaRespuestaPronostico).Text;

                      var obtener = Task.Run(async () => {
                          //resultadoObtenerTodos = await proveedorDeClientes.ObtenerTodos(new DataObtenerTodos(_vendedor.Valor.Usuario, filtro));
                          resultadoClientePronostico = await proveedorDePronostico.ConsultaClientePronostico(new DataClientePronostico(filtro,Configuracion.Compania,_vendedor.Valor.Almacen, Mes));
                      });
                      obtener.Wait();

                      if(resultadoClientePronostico.Tipo == TipoDeResultado.Exito)
                      {
                          //Intent.PutExtra(PronosticoFragment.LLavePronosticoFragment, ProveedorDeManipulacionDeDatos.GenerarTodos(resultadoClientePronostico.Valor));
                          //Intent.PutExtra(PronosticoFragment.LLavePronostico, ProveedorDeManipulacionDeDatos.GenerarTodos(resultadoClientePronostico.Valor));
                          //arguments = Intent.Extras;
                          //fragment = RutaFragment.NewInstance();
                          //fragment.Arguments = arguments;
                          //return new PronosticoFragment();
                          ProveedorGlobal.PosicionClientePronostico = -1;
                          Intent.PutExtra(PronosticoFragment.LLavePronostico,
                               ProveedorDeManipulacionDeDatos.GenerarTodos(resultadoClientePronostico.Valor));
                          Intent.PutExtra(PronosticoFragment.LLavePronosticoFiltros,
                               ProveedorDeManipulacionDeDatos.Generar(Mes));
                          arguments = Intent.Extras;
                          fragment = PronosticoFragment.NewInstance();
                          fragment.Arguments = arguments;
                          SupportFragmentManager.BeginTransaction()
                           .Replace(Resource.Id.content_frame, fragment)
                           .Commit();
                      }
                      else
                      {
                          Toast.MakeText(this, "No se encontrol el cliente en tu pronostico.", ToastLength.Short).Show();
                      }
                      //var clientesSupervisores = resultadoObtenerClientesSupervisores.Valor;
                      //Intent.PutExtra(RutaFragment.LlaveDeRuta, ProveedorDeManipulacionDeDatos.GenerarTodos(clientesSupervisores.OrderBy(e => e.Secuencia)));
                      //arguments = Intent.Extras;
                      //fragment = RutaFragment.NewInstance();
                      //fragment.Arguments = arguments;
                      //SupportFragmentManager.BeginTransaction()
                      //.Replace(Resource.Id.content_frame, fragment)
                      //.Commit();
                      //dialog.Hide();

                  })
                  .SetNegativeButton(this.GetString(Resource.String.dialog_cancel), (sender, args) => { })
                  // .SetNeutralButton(View.Context.GetString(Resource.String.dialog_neutral), (sender, args) => { })
                  .Show();

            return null;
            //Intent.PutExtra(ReciboFragment.LlaveRecibo, ProveedorDeManipulacionDeDatos.Generar(_vendedor));
            //arguments = Intent.Extras;
            //fragment = ReciboFragment.NewInstance();
            //fragment.Arguments = arguments;
            //return new ReciboFragment();
        }
        public Android.Support.V4.App.Fragment CierreVentas()
        {
            var arguments = new Bundle();
            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                //Snackbar.Make(view, "No estas conectado a internet para revisar tu inventario", Snackbar.LengthLong)
                //    .Show();
                Toast.MakeText(this, "No estas conectado a internet para revisar tu inventario", ToastLength.Short).Show();
                return null;
            }
            Intent.PutExtra(CierreVentaFragment.LlaveListadoCierreVenta, ProveedorDeManipulacionDeDatos.Generar(_vendedor));
            arguments = Intent.Extras;
            fragment = CierreVentaFragment.NewInstance();
            fragment.Arguments = arguments;
            return new CierreVentaFragment();
        }
        private Android.Support.V4.App.Fragment HistoricoVenta()
        {
            var arguments = new Bundle();
            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                //Snackbar.Make(view, "No estas conectado a internet para revisar tu inventario", Snackbar.LengthLong)
                //    .Show();
                Toast.MakeText(this, "No estas conectado a internet para revisar tu inventario", ToastLength.Short).Show();
                return null;
            }
            if (_vendedor.Valor.Almacen != "Supervisor")
            {
                Toast.MakeText(this, "No tienes acceso a este apartado, es solo para supervisores", ToastLength.Short).Show();
                return null;
            }
            Intent.PutExtra(HistoricoVentaFragment.LLaveHistoricoVentaFragment , ProveedorDeManipulacionDeDatos.Generar(_vendedor));
            arguments = Intent.Extras;

            fragment = HistoricoVentaFragment.NewInstance();
            fragment.Arguments = arguments;

            return new HistoricoVentaFragment();
        }
        public Android.Support.V4.App.Fragment AltaClientes()
        {
            var arguments = new Bundle();
            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                //Snackbar.Make(view, "No estas conectado a internet para revisar tu inventario", Snackbar.LengthLong)
                //    .Show();
                Toast.MakeText(this, "No estas conectado a internet para revisar tu inventario", ToastLength.Short).Show();
                return null;
            }
            if (_vendedor.Valor.Almacen == "Supervisor")
            {
                Toast.MakeText(this, "No tienes acceso a este apartado", ToastLength.Short).Show();
                return null;
            }
            Intent.PutExtra(AltaClientesFragment.LlaveAltaClientes, ProveedorDeManipulacionDeDatos.Generar(_vendedor));
            arguments = Intent.Extras;

            fragment = AltaClientesFragment.NewInstance();
            fragment.Arguments = arguments;
            
            return new AltaClientesFragment();
        }
        public Android.Support.V4.App.Fragment EntradaSalida()
        {
            var arguments = new Bundle();
            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                Toast.MakeText(this, "No estas conectado a internet para revisar tu inventario", ToastLength.Short).Show();
                return null;
            }
            Intent.PutExtra(EntradasSalidasFragment.LlaveEntradasSalidas, ProveedorDeManipulacionDeDatos.Generar(_vendedor));
            arguments = Intent.Extras;
            fragment = EntradasSalidasFragment.NewInstance();
            fragment.Arguments = arguments;
            return new EntradasSalidasFragment();
        }

        public Android.Support.V4.App.Fragment RegistrarVisita()
        {
            if (!TieneClienteSeleccionado())
            {
                return null;
            }

            var arguments = new Bundle();
            Intent.PutExtra(RegistrarVisitaFragment.LLaveRegistrarVisita, ProveedorDeManipulacionDeDatos.Generar(_vendedor));
            arguments = Intent.Extras;
            fragment = RegistrarVisitaFragment.NewInstance();
            fragment.Arguments = arguments;
            return new RegistrarVisitaFragment();
        }
        public Android.Support.V4.App.Fragment SeguimientoVisita()
        {
            var arguments = new Bundle();
            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                Toast.MakeText(this, "No estas conectado a internet para realizar un seguimiento de visita.", ToastLength.Short).Show();
                return null;
            }
            if (!TieneClienteSeleccionado())
            {
                return null;
            }
            Intent.PutExtra(SeguimientoVisitaFragment.LLaveSeguimientoVisita, ProveedorDeManipulacionDeDatos.Generar(_vendedor));
            arguments = Intent.Extras;
            fragment = SeguimientoVisitaFragment.NewInstance();
            fragment.Arguments = arguments;
            return new SeguimientoVisitaFragment();
        }
        //public Android.Support.V4.App.Fragment Geocerca()
        //{
        //    try
        //    {
        //        var arguments = new Bundle();
        //        if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
        //        {
        //            Toast.MakeText(this, "No estas conectado a internet para ver tu geocerca", ToastLength.Short).Show();
        //            return null;
        //        }
        //        Intent.PutExtra(GeocercaFragment.LLaveGeocerca, ProveedorDeManipulacionDeDatos.Generar(_vendedor));
        //        arguments = Intent.Extras;
        //        fragment = GeocercaFragment.NewInstance();
        //        fragment.Arguments = arguments;
        //        return new GeocercaFragment();
        //    }
        //    catch(Exception)
        //    {
        //        Toast.MakeText(this, "No estas conectado a internet para ver tu geocerca", ToastLength.Short).Show();
        //        return null;
        //    }
          
        //}
        public Android.Support.V4.App.Fragment Degustacion()
        {
            var arguments = new Bundle();
            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {               
                Toast.MakeText(this, "No estas conectado a internet para realizar una degustación", ToastLength.Short).Show();
                return null;
            }
            if (!TieneClienteSeleccionado())
            {
                return null;
            }
            Intent.PutExtra(DegustacionesFragment.LLaveDegustaciones, ProveedorDeManipulacionDeDatos.Generar(_vendedor));

            arguments = Intent.Extras;
            fragment = DegustacionesFragment.NewInstance();
            fragment.Arguments = arguments;
            return new DegustacionesFragment();
        }
        public Android.Support.V4.App.Fragment Ajustes()
        {

            //new Android.Support.V7.App.AlertDialog.Builder(view.Context)
            //            .SetTitle(view.Context.GetString(Resource.String.main_dialog_simple_title))
            //            .SetMessage(view.Context.GetString(Resource.String.main_dialog_simple_message))
            //            .SetPositiveButton(view.Context.GetString(Resource.String.dialog_ok), (sender, args) => { })
            //            .SetNegativeButton(view.Context.GetString(Resource.String.dialog_cancel), (sender, args) => { })
            //            .SetNeutralButton(view.Context.GetString(Resource.String.dialog_neutral), (sender, args) => { })
            //            .Show();
            var arguments = new Bundle();
            arguments = Intent.Extras;
            fragment = AjustesFragment.NewInstance();
            fragment.Arguments = arguments;
            return new AjustesFragment();
        }
        public Android.Support.V4.App.Fragment Salir()
        {
         
            //new Android.Support.V7.App.AlertDialog.Builder(view.Context)
            //            .SetTitle(view.Context.GetString(Resource.String.main_dialog_simple_title))
            //            .SetMessage(view.Context.GetString(Resource.String.main_dialog_simple_message))
            //            .SetPositiveButton(view.Context.GetString(Resource.String.dialog_ok), (sender, args) => { })
            //            .SetNegativeButton(view.Context.GetString(Resource.String.dialog_cancel), (sender, args) => { })
            //            .SetNeutralButton(view.Context.GetString(Resource.String.dialog_neutral), (sender, args) => { })
            //            .Show();
            var arguments = new Bundle();
            arguments = Intent.Extras;
            fragment = SalirFragment.NewInstance();
            fragment.Arguments = arguments;
            return new SalirFragment();
        }
        private bool ClienteBloqueado()
        {
            if (ProveedorGlobal.Cliente.Bloqueado)
            {
                Toast.MakeText(this, "El cliente está bloqueado.", ToastLength.Short).Show();
                //return null;
                //RepositorioDeUtilidades.MostrarMensaje("El cliente está bloqueado.");
                return true;
            }

            return false;
        }

        //public void DialogSitios()
        //{
        //    var vistaSitios = LayoutInflater.Inflate(Resource.Layout.SitiosSupervisores, null);
        //    vistaSitios.FindViewById<TextView>(Resource.Id.Titulo2).Text = "Selecciona un sitio";
        //    SPSitios = vistaSitios.FindViewById<Spinner>(Resource.Id.SPSitiosSupervisores);
        //    if (resultadoSitios.Tipo == TipoDeResultado.Exito)
        //    {
        //        ListSitios = resultadoSitios.Valor;

        //        adapterSitios = new AdapterSitios(this, ListSitios);
        //        SPSitios.Adapter = adapterSitios;

        //        SPSitios.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
        //        {
        //            Sitio = Convert.ToString(resultadoSitios.Valor[e.Position].SITIOS);
        //        };
        //    }
        //    else
        //    {
        //        Toast.MakeText(this, "Debes seleccionar un cliente de tu ruta", ToastLength.Short).Show();
        //    }

        //    new Android.Support.V7.App.AlertDialog.Builder(this)
        //               .SetTitle(this.GetString(Resource.String.main_dialog_Ruta))
        //                //.SetMessage(this.GetString(Resource.String.main_dialog_simple_message))
        //               .SetView(vistaSitios)
        //               .SetPositiveButton(this.GetString(Resource.String.dialog_ok), OkAction)
        //              .SetNegativeButton(this.GetString(Resource.String.dialog_cancel), (sender, args) => { })
        //              // .SetNeutralButton(View.Context.GetString(Resource.String.dialog_neutral), (sender, args) => { })
        //              .Show();
        //}
        
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
                case Resource.Id.nav_Sincronizar:
                    var arguments = new Bundle();
                    arguments = Intent.Extras;
                    fragment = SincronizarInformacionFragment.NewInstance();
                    fragment.Arguments = arguments;
                    SupportFragmentManager.BeginTransaction()
                    .Replace(Resource.Id.content_frame, fragment)
                    .Commit();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.main, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    switch (item.ItemId)
        //    {
        //        case Resource.Id.action_menu_main_1:
        //            Intent intent = new Intent(this, typeof(Class.Activitys.AboutActivity));
        //            StartActivity(intent);
        //            break;
        //    }

        //    return base.OnOptionsItemSelected(item);
        //}
        public List<ItemGenerico> ObtieneMes()
        {
            mes = new List<ItemGenerico>();
            mes.Add(new ItemGenerico
            {
                Id = 1,
                Nombre = "Enero"
            });
            mes.Add(new ItemGenerico
            {
                Id = 2,
                Nombre = "Febrero"
            });
            mes.Add(new ItemGenerico
            {
                Id = 3,
                Nombre = "Marzo"
            });
            mes.Add(new ItemGenerico
            {
                Id = 4,
                Nombre = "Abril"
            });
            mes.Add(new ItemGenerico
            {
                Id = 5,
                Nombre = "Mayo"
            });
            mes.Add(new ItemGenerico
            {
                Id = 6,
                Nombre = "Junio"
            });
            mes.Add(new ItemGenerico
            {
                Id = 7,
                Nombre = "Julio"
            });
            mes.Add(new ItemGenerico
            {
                Id = 8,
                Nombre = "Agosto"
            });
            mes.Add(new ItemGenerico
            {
                Id = 9,
                Nombre = "Septiembre"
            });
            mes.Add(new ItemGenerico
            {
                Id = 10,
                Nombre = "Octubre"
            });
            mes.Add(new ItemGenerico
            {
                Id = 10,
                Nombre = "Noviembre"
            });
            mes.Add(new ItemGenerico
            {
                Id = 12,
                Nombre = "Diciembre"
            });

            return mes;

        }
        public override void OnBackPressed()
        {        

            //base.OnBackPressed();
            var arguments = new Bundle();
            arguments = Intent.Extras;
            Android.Support.V4.App.Fragment fragment = null;
            fragment = InicioFragment.NewInstance();
            fragment.Arguments = arguments;
            SupportFragmentManager.BeginTransaction()
               .Replace(Resource.Id.content_frame, fragment)
               .Commit();
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
        protected override void OnDestroy()
        {
            base.OnDestroy();
            base.Dispose();          
            GC.Collect(GC.MaxGeneration);
            
        }

    }
}