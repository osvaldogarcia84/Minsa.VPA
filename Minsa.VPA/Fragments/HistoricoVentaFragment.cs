using Android.Locations;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Minsa.VPA.Adaptadores;
using Minsa.VPA.Adapters;
using Minsa.VPA.Modelos;
using Minsa.VPA.Proveedores;
using Minsa.VPA.Repositorio;
using Minsa.VPA.Servicios;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
namespace Minsa.VPA.Fragments
{
    [Obsolete]
    public class HistoricoVentaFragment : ListFragment
    {
        public const string LLaveHistoricoVentaFragment = "InventarioClienteFragment";
        public ResultadoDeOperacionGenerico<Vendedor> vendedor;
        ProveedorDeClientes proveedorDeCliente = new ProveedorDeClientes();
        //Timer timer;
        ResultadoDeOperacionGenerico<List<HistoricosVenta>> resultadoHistoricosVenta;
        List<HistoricosVenta> ListaHistoricoVenta;
        View view;
        Android.App.ProgressDialog _progress;
        CancellationTokenSource _cts;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            vendedor = ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Arguments.Obtener<string>(MainActivity.LlaveUsuario));


            //var historico = Task.Run(async () =>
            //{
            //    resultadoHistoricosVenta = await proveedorDeCliente.HistoricoVenta(new DataHistoricosVenta(vendedor.Valor.Usuario));
            //});
            //historico.Wait();
        }
        public static HistoricoVentaFragment NewInstance()
        {
            var frag1 = new HistoricoVentaFragment { Arguments = new Bundle() };
            return frag1;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Solo infla y regresa la vista: NO bloquees aquí
            view = inflater.Inflate(Resource.Layout.HistoricoVenta, container, false);
            return view;
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _cts = new CancellationTokenSource();

            _progress = new Android.App.ProgressDialog(Activity, Resource.Style.MyAlertDialogStyle);
            _progress.SetMessage("Cargando histórico de ventas...");
            _progress.SetCancelable(false);
            _progress.Show();
            /// Estimado companiero Coria, no mueva este bloque de código, ya que es el que permite que la consulta se ejecute en segundo plano y no bloquee la UI.
            try
            {
                // Ejecuta la consulta en segundo plano SIN bloquear la UI
                resultadoHistoricosVenta = await proveedorDeCliente.HistoricoVenta(
                    new DataHistoricosVenta(vendedor.Valor.Usuario));

                var hayHistorico = resultadoHistoricosVenta?.Valor != null && resultadoHistoricosVenta.Valor.Any();

                if (!IsAdded) return; // el fragment pudo desmontarse

                if (hayHistorico)
                {
                    ListaHistoricoVenta = resultadoHistoricosVenta.Valor;
                    AsignarHistorico(); // ya actualiza en UI
                }
                else
                {
                    // Muestra tu layout de error en la vista actual
                    var txt = view.FindViewById<TextView>(Resource.Id.ErrorTexto);
                    if (txt != null)
                        txt.Text = "No existe información, comunícate con un administrador.";
                    // o bien infla Resource.Layout.Error y reemplaza el contenido del contenedor
                }
            }
            catch (Exception ex)
            {
                if (!IsAdded) return;
                var txt = view.FindViewById<TextView>(Resource.Id.ErrorTexto);
                if (txt != null)
                    txt.Text = "Ocurrió un error al cargar: " + ex.Message;
            }
            finally
            {
                if (_progress?.IsShowing == true && Activity != null && !Activity.IsFinishing)
                    _progress.Dismiss();
                _progress = null;
            }
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            _cts?.Cancel();
            if (_progress?.IsShowing == true)
                _progress.Dismiss();
            _progress = null;
        }

        private void AsignarHistorico()
        {
            Activity.RunOnUiThread(() =>
            {
                ListAdapter = new HistoricoVentaAdapter(Activity, ListaHistoricoVenta.ToList());
                ((BaseAdapter)ListAdapter).NotifyDataSetChanged();
            });
        }
    }
}