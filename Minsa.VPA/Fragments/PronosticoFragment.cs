using Android.Locations;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Minsa.VPA.Adaptadores;
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
    public class PronosticoFragment : ListFragment
    {
        View view;
        public List<ClientePronostico> clientesPronostico;
        public string ObtieneFiltros;
        public const string LLavePronostico = "Pronostico";
        public const string LLavePronosticoFiltros = "PronosticoFiltro";
        public ResultadoDeOperacionGenerico<Vendedor> vendedor;
        Button BtnAgregarRecurso;
        Android.Support.V4.App.Fragment fragment = null;
        ProveedorDePronostico proveedorDePronostico = new ProveedorDePronostico();
        ResultadoDeOperacionGenerico<string> resultadoEliminar;
        ResultadoDeOperacionGenerico<List<ClientePronostico>> resultadoCtePronostico;
        ResultadoDeOperacionGenerico<List<ClientePronostico>> resultadoCtePronosticoEliminar;
        
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            vendedor = ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Arguments.Obtener<string>(MainActivity.LlaveUsuario));
            // Create your fragment here
            try
            {
                Activity.MostrarMensaje("Cargando información de los clientes pronostico");
                clientesPronostico = ProveedorDeManipulacionDeDatos.ObtenerTodos<ClientePronostico>(Arguments.Obtener<string[]>(LLavePronostico))
                .ConvertirLista();

                ObtieneFiltros = ProveedorDeManipulacionDeDatos.Obtener<string>(Arguments.Obtener<string>(LLavePronosticoFiltros)).ToString();
                
            }
            catch (Exception ex)
            {

                Activity.MostrarMensaje(ex.Message);
            }
        }
        public static PronosticoFragment NewInstance()
        {
            var frag1 = new PronosticoFragment { Arguments = new Bundle() };
            return frag1;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if(clientesPronostico.Count > 0)
            {
                ProveedorGlobal.ListClientePronostico = null;
                ProveedorGlobal.ListClientePronostico = clientesPronostico;
                view = ConfigurarVista(inflater, container);
            }
            else
            {
                view = inflater.Inflate(Resource.Layout.Error, container, false);
                view.FindViewById<TextView>(Resource.Id.ErrorTexto).Text =
                   "El cliente no se encuentra cargado en tu pronostico, comunicate con el area correspondiente para su carga.";
            }
            
            return view;
        }
        private View ConfigurarVista(LayoutInflater inflater, ViewGroup container)
        {
            View view;
            view = inflater.Inflate(Resource.Layout.Pronostico, container, false);
            AsignarClientesPronostico();
            BtnAgregarRecurso = view.FindViewById<Button>(Resource.Id.AgregarRecurso);
            BtnAgregarRecurso.Click += BtnAgregarRecurso_Click;

            return view;
        }

        private void BtnAgregarRecurso_Click(object sender, EventArgs e)
        {
            try
            {

                var arguments = new Bundle();
                Arguments.PutStringArray(AgregaRecursoPronosticoFragment.LLaveAgregaRecurso, ProveedorDeManipulacionDeDatos.GenerarTodos(clientesPronostico));
                Arguments.PutString(AgregaRecursoPronosticoFragment.LLaveAgregaFiltros, ProveedorDeManipulacionDeDatos.Generar(ObtieneFiltros));
                arguments = Arguments;
                fragment = AgregaRecursoPronosticoFragment.NewInstance();
                fragment.Arguments = Arguments;
                FragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .Commit();
            }
            catch(Exception ex)
            {
                Activity.MostrarMensaje("Ocurrio un error: " + ex, ToastLength.Long);
            }
        }

        private void AsignarClientesPronostico()
        {
            Activity.RunOnUiThread(() =>
            {
                ListAdapter = new ClientesPronosticoAdapter(Activity, clientesPronostico.ToList(),this);
                ((BaseAdapter)ListAdapter).NotifyDataSetChanged();
            });
        }
        public void callfragmentPronostico(int method)
        {
            switch (method)
            {
                case 1:
                    Editar();
                    break;
                //case 2:
                //    var vistaDePregunta = LayoutInflater.Inflate(Resource.Layout.PreguntaFlujo, null);

                //    vistaDePregunta.FindViewById<TextView>(Resource.Id.PreguntaFlujo).Text =
                //               "¿Desea eliminar el registro?";

                //    new Android.Support.V7.App.AlertDialog.Builder(Context)
                //    .SetTitle(this.GetString(Resource.String.dialogFlujo))
                //    .SetView(vistaDePregunta)
                //    .SetPositiveButton(this.GetString(Resource.String.dialog_Si), (sender, args) =>
                //    {
                //        if (ProveedorGlobal.ClientePronostico != null)
                //        {
                //            var eliminar = Task.Run(async () =>
                //            {
                //                resultadoEliminar = await proveedorDePronostico.EliminaPronostico(new DataEliminaPronostico(ProveedorGlobal.ClientePronostico.Fecha,
                //                                                                                        vendedor.Valor.Almacen, ProveedorGlobal.ClientePronostico.Cliente,
                //                                                                                        vendedor.Valor.Id, ProveedorGlobal.ClientePronostico.IdRecurso));
                //                resultadoCtePronosticoEliminar = await proveedorDePronostico.ConsultaClientePronostico(new DataClientePronostico(ProveedorGlobal.ClientePronostico.Cliente, Configuracion.Compania, vendedor.Valor.Almacen, ObtieneFiltros));
                //            });
                //            eliminar.Wait();
                //            if (resultadoEliminar.Tipo == TipoDeResultado.Exito)
                //            {
                //                clientesPronostico = null;
                //                clientesPronostico = resultadoCtePronosticoEliminar.Valor;
                //                ListAdapter = new ClientesPronosticoAdapter(Activity, clientesPronostico, this);
                //                ((BaseAdapter)ListAdapter).NotifyDataSetChanged();
                //                Snackbar.Make(View, "Se elimino correctamente tu información", Snackbar.LengthLong);
                //            }
                //        }
                //        else
                //        {
                //            Snackbar.Make(View, "No haz seleecionado el cliente a eliminar.", Snackbar.LengthLong)
                //            .Show();
                //        }

                //    });
                    //break;
                    //case 3:
                    //    RegistraVisita();
                    //    // Carrito();
                    //    break;                
            }
            FragmentManager.BeginTransaction()
               .Replace(Resource.Id.content_frame, fragment)
               .Commit();
        }

        public Android.Support.V4.App.Fragment Editar()
        {
            try
            {
                if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
                {
                    Snackbar.Make(View, "No estas conectado a internet, intentalo mas tarde", Snackbar.LengthLong)
                       .Show();
                    return null;
                }
                else
                {
                    var arguments = new Bundle();
                    Arguments.PutString(EditarPronostico.LlaveDeEditarPronostico, ProveedorDeManipulacionDeDatos.Generar(vendedor));
                    Arguments.PutString(EditarPronostico.LLaveEditarFiltros, ProveedorDeManipulacionDeDatos.Generar(ObtieneFiltros));
                    arguments = Arguments;
                    fragment = EditarPronostico.NewInstance();
                    fragment.Arguments = Arguments;
                    return new EditarPronostico();
                }

            }
            catch(Exception ex)
            {
                Snackbar.Make(View, "Ocurrio un error", Snackbar.LengthLong)
                   .Show();
                return null;
            }
           
            
        }
        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            ClientePronostico cliente = ((ClientesPronosticoAdapter)ListAdapter).Items.ElementAt(position);
            ((ClientesPronosticoAdapter)ListAdapter).setSelectedIndex(position);
            ((BaseAdapter)ListAdapter).NotifyDataSetChanged();
            ProveedorGlobal.ClientePronostico = cliente;
            Snackbar.Make(View, "Cliente elegido: " + cliente.NombreCte, Snackbar.LengthLong)
            .Show();
        }
    }
}