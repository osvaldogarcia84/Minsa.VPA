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
using Grantland.Widget;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Minsa.VPA.Fragments
{
    public class SeguimientoCierreVentaFragment : Fragment
    {
        public const string LlaveCierreVentas = "LlaveCierreVentas";
        ProveedorDeEstrategia proveedorDeEstrategia = new ProveedorDeEstrategia();
        public ResultadoDeOperacionGenerico<Vendedor> vendedor;
        Cliente cliente;
        public AdaptadorCBCierreVentas adaptadorCBCierreVentas;
        public AdaptadorCBPlazoEstrategia adaptadorCBPlazoEstrategia;
        public AdaptadorCompania adaptadorCompania;
        public AdaptadorRecurso adaptadorRecurso;

        private ResultadoDeOperacionGenerico<List<CBEstrategiaCerrarVenta>> CBEstrategiaCerrarVenta;
        private ResultadoDeOperacionGenerico<List<CBPlazoEstrategia>> CBPlazoEstrategia;
        public ResultadoDeOperacionGenerico<List<Compania>> Companias;
        public ResultadoDeOperacionGenerico<List<ProductosEstrategia>> Productos;
        public List<ProductosEstrategia> ProductosXMarca;

        private Spinner spCerrarVenta;
        private Spinner spPlazoEstrategia;
        private Spinner spCompania, spRecurso;
        private Button ButtonEnviarCierreVenta;
        private SeguimientoCierreVentas seguimientoCierreVentas;
        private string EstrategiaCerrarVenta, PlazoEstrategia, Marca, Recurso,Telefono, Correo;
        private decimal Volumen, Cantidad,Precio;
        ResultadoDeOperacionGenerico<ResultadoCierreVentas> resultadoCierreVentas;
        Timer timer;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            cliente = ProveedorGlobal.Cliente;
            vendedor = ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Arguments.Obtener<string>(MainActivity.LlaveUsuario));
            var Resultcompañias = proveedorDeEstrategia.ObtenerCompañias();
            if (Resultcompañias.Tipo == TipoDeResultado.Exito)
            {
                Companias = Resultcompañias;
            }
            else
            {
                Snackbar.Make(View, "Ocurrio un error comunicate con el departamento de Sistemas", Snackbar.LengthLong)
                .Show();
            }

            var ObtieneProductos = proveedorDeEstrategia.Productos();
            if(ObtieneProductos.Tipo == TipoDeResultado.Exito)
            {
                Productos = ObtieneProductos;
            }
            else
            {
                Snackbar.Make(View, "Ocurrio un error comunicate con el departamento de Sistemas", Snackbar.LengthLong)
                .Show();
            }
            var resultCBCerrarVenta = proveedorDeEstrategia.CBCerrarVenta();
            if (resultCBCerrarVenta.Tipo == TipoDeResultado.Exito)
            {
                CBEstrategiaCerrarVenta = resultCBCerrarVenta;
            }
            else
            {
                Snackbar.Make(View, "Ocurrio un error comunicate con el departamento de Sistemas", Snackbar.LengthLong)
                .Show();
            }

            var resultCBPlazoEstrategia = proveedorDeEstrategia.CBPlazoEstrategia();
            if (resultCBPlazoEstrategia.Tipo == TipoDeResultado.Exito)
            {
                CBPlazoEstrategia = resultCBPlazoEstrategia;
            }
            else
            {
                Snackbar.Make(View, "Ocurrio un error comunicate con el departamento de Sistemas", Snackbar.LengthLong)
               .Show();
            }
        }
        public static SeguimientoCierreVentaFragment NewInstance()
        {
            var frag1 = new SeguimientoCierreVentaFragment { Arguments = new Bundle() };
            return frag1;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                View view;
                var ignored = base.OnCreateView(inflater, container, savedInstanceState);
                if (CBEstrategiaCerrarVenta != null && CBPlazoEstrategia != null)
                {
                    view = ConfigurarVista(inflater, container);
                    //ConfigurarModelo(view);
                }
                else
                {
                    view = inflater.Inflate(Resource.Layout.Error, container, false);
                    view.FindViewById<TextView>(Resource.Id.ErrorTexto).Text =
                        "No se encontraron registros para el cliente.";
                }
                return view;
            }
            catch (Exception ex)
            {
                Toast.MakeText(this.Activity, "No estas conectado a internet y/o ocurrio un problema " + ex , ToastLength.Short).Show();               
                return null;
            }

        }

        private View ConfigurarVista(LayoutInflater inflater, ViewGroup container)
        {
            View view = inflater.Inflate(Resource.Layout.SeguimientoCierreVentas, null);
            spCerrarVenta = view.FindViewById<Spinner>(Resource.Id.EstrategiaCerrarVenta);
            spPlazoEstrategia = view.FindViewById<Spinner>(Resource.Id.PlazoEstrategia);
            spCompania = view.FindViewById<Spinner>(Resource.Id.CompaniaEstrategia);

            spRecurso = view.FindViewById<Spinner>(Resource.Id.RecursoEstrategia);
           // spRecurso.Enabled = false;

            view.FindViewById<AutofitTextView>(Resource.Id.NombreVendedorCierreVenta).Text = cliente.ClienteId + "-" + cliente.Nombre;

            adaptadorCBPlazoEstrategia = new AdaptadorCBPlazoEstrategia(Activity, CBPlazoEstrategia.Valor);
            adaptadorCBCierreVentas = new AdaptadorCBCierreVentas(Activity, CBEstrategiaCerrarVenta.Valor);
            adaptadorCompania = new AdaptadorCompania(Activity, Companias.Valor);

            spCerrarVenta.Adapter = adaptadorCBCierreVentas;
            spPlazoEstrategia.Adapter = adaptadorCBPlazoEstrategia;
            spCompania.Adapter = adaptadorCompania;

            spCerrarVenta.ItemSelected += SpCerrarVenta_ItemSelected;
            spPlazoEstrategia.ItemSelected += SpPlazoEstrategia_ItemSelected;
            spCompania.ItemSelected += SpCompania_ItemSelected;
            ButtonEnviarCierreVenta = view.FindViewById<Button>(Resource.Id.EnviarSeguimientoCerrarVenta);
            ButtonEnviarCierreVenta.Click += ButtonEnviarCierreVenta_Click;


            view.FindViewById<EditText>(Resource.Id.VolumenACapturar).TextChanged += (sender, args) =>
            {
                Volumen = ((EditText)sender).Text.ObtenerCantidadDecimal();
            };
            view.FindViewById<EditText>(Resource.Id.PrecioCierre).TextChanged += (sender, args) =>
            {
                Precio = ((EditText)sender).Text.ObtenerCantidadDecimal();
            };
            view.FindViewById<EditText>(Resource.Id.TelefonoCierre).TextChanged += (sender, args) =>
            {
                Telefono = ((EditText)sender).Text;
            };
            view.FindViewById<EditText>(Resource.Id.CorreoCierre).TextChanged += (sender, args) =>
            {
                Correo = ((EditText)sender).Text;
            };
            view.FindViewById<EditText>(Resource.Id.CantidadCierre).TextChanged += (sender, args) =>
            {
                Cantidad = ((EditText)sender).Text.ObtenerCantidadDecimal();
            };
         
            return view;
        }

        private void SpCompania_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                //Snackbar.Make(View, "Cargando información. Espere porfavor.", Snackbar.LengthLong)
                //        .Show();
                var idProductos = Companias.Valor[e.Position].Id;
                Marca = idProductos;
                 ProductosXMarca = BuscarProducto(idProductos);
                adaptadorRecurso = new AdaptadorRecurso(Activity, ProductosXMarca);                
                spRecurso.Adapter = adaptadorRecurso;
                spRecurso.ItemSelected += SpRecurso_ItemSelected;
            }
            catch(Exception ex)
            {
                Toast.MakeText(this.Activity, "Ocurrio un error" + ex, ToastLength.Short).Show();              
            }            
        }

        private void SpRecurso_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            
            try
            {
                Recurso = null;
                Recurso = ProductosXMarca[e.Position].ID;
            }
            catch (Exception ex)
            {
                Activity.MostrarMensaje("Ocurrio un error" + ex, ToastLength.Long);
            }
        }

        private void SpPlazoEstrategia_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                PlazoEstrategia = Convert.ToString(CBPlazoEstrategia.Valor[e.Position].Valor);
            }
            catch (Exception ex)
            {
                Activity.MostrarMensaje("Ocurrio un error" + ex, ToastLength.Long);
            }
        }

        private void SpCerrarVenta_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {

                EstrategiaCerrarVenta = Convert.ToString(CBEstrategiaCerrarVenta.Valor[e.Position].Valor);
            }
            catch (Exception ex)
            {
                Activity.MostrarMensaje("Ocurrio un error" + ex, ToastLength.Long);
            }
        }
        public List<ProductosEstrategia> BuscarProducto(string Id)
        {
            var Obtieneproductos = Productos.Valor.Where(e => e.COMPANY == Id).ToList();
            return Obtieneproductos;
        }
        private void ButtonEnviarCierreVenta_Click(object sender, EventArgs e)
        {
            try
            {
                if (cliente.ClienteId != null)
                {

                    if (Volumen == 0 || Volumen == null)
                    {
                        Snackbar.Make(View, "Ingresa el volumen.", Snackbar.LengthLong)
                        .Show();
                        return;
                    }
                    if(Cantidad == 0 || Cantidad == null)
                    {
                        Snackbar.Make(View, "Ingresa la cantidad", Snackbar.LengthLong)
                        .Show();
                        return;
                    }

                    seguimientoCierreVentas = new SeguimientoCierreVentas(cliente.ClienteId,Volumen,EstrategiaCerrarVenta,Cantidad,
                                              PlazoEstrategia,Marca,Recurso,Precio,Telefono,Correo,vendedor.Valor.Id);
                    DeshabilitarVista();
                    var info = Task.Run(async () => {
                         resultadoCierreVentas = await proveedorDeEstrategia.SeguimientoCierreVentas(seguimientoCierreVentas);
                    });
                    info.Wait();
                    if (resultadoCierreVentas.Tipo == TipoDeResultado.Exito)
                    {
                        
                        Snackbar.Make(View, "Se guardo correctamente tu información", Snackbar.LengthLong)
                        .Show();
                    }
                    else{
                        Snackbar.Make(View, resultadoCierreVentas.Mensaje, Snackbar.LengthLong)
                        .Show();
                    }                    
                }
                else
                {
                    Snackbar.Make(View, "No haz seleccionado ninguna cliente.", Snackbar.LengthLong)
                        .Show();
                }
            }
            catch (Exception ex)
            {
                Snackbar.Make(View, "Error: " + ex, Snackbar.LengthLong)
                       .Show();
            }
        }
        private void DeshabilitarVista()
        {
            ButtonEnviarCierreVenta.Enabled = false;
            spCerrarVenta.Enabled = false;
            spPlazoEstrategia.Enabled = false;
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