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

namespace Minsa.VPA.Fragments
{
    public class BajaClienteFragment : Fragment
    {
        Cliente cliente;
        public ResultadoDeOperacionGenerico<Vendedor> vendedor;
        ProveedorDeClientes proveedorDeClientes = new ProveedorDeClientes();
        public const string LlaveDeBaja = "Baja";
        TextView NombreCliente;
        ResultadoDeOperacionGenerico<List<MotivoBaja>> motivoBajas;
        Spinner SPMotivos;
        public AdaptadorMotivosBaja adapterMotivosBaja;
        public string Motivos;
        Button EnviarBaja;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            cliente = ProveedorGlobal.Cliente;
            vendedor = ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Arguments.Obtener<string>(MainActivity.LlaveUsuario));
            // Create your fragment here
            try
            {               
                var info = Task.Run(async () => {
                    motivoBajas = await proveedorDeClientes.MotivoBaja();
                });
                info.Wait();              
            }
            catch (Exception ex)
            {

                Activity.MostrarMensaje(ex.Message);
            }
        }
        public static BajaClienteFragment NewInstance()
        {
            var frag1 = new BajaClienteFragment { Arguments = new Bundle() };
            return frag1;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view;
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);            

            if (motivoBajas.Tipo == TipoDeResultado.Exito)
            {
                view = ConfigurarVista(inflater, container);
            }
            else
            {
                view = inflater.Inflate(Resource.Layout.Error, container, false);
                view.FindViewById<TextView>(Resource.Id.ErrorTexto).Text =
                   "Ocurrio un error comunicate con el administrador.";
            }

            return view;
        }
        private View ConfigurarVista(LayoutInflater inflater, ViewGroup container)
        {
            var view = inflater.Inflate(Resource.Layout.BajaCliente, null);
            NombreCliente = view.FindViewById<TextView>(Resource.Id.NombreCliente);
            NombreCliente.Text = cliente.ClienteId + " - "  + cliente.Nombre;
            SPMotivos = view.FindViewById<Spinner>(Resource.Id.SPMotivosBaja);
            adapterMotivosBaja = new AdaptadorMotivosBaja(Activity, motivoBajas.Valor);
            SPMotivos.Adapter = adapterMotivosBaja;
            SPMotivos.ItemSelected += SPMotivos_ItemSelected;
            EnviarBaja = view.FindViewById<Button>(Resource.Id.EnviarBaja);
            EnviarBaja.Click += EnviarBaja_Click;
            return view;
        }

       
        private void SPMotivos_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Motivos = motivoBajas.Valor[e.Position].MOTIVO;
        }
        private async void EnviarBaja_Click(object sender, EventArgs e)
        {
            try
            {
                EnviarBaja.Enabled = false;
                var baja = await proveedorDeClientes.BajaCliente(new DataBajaCliente(Motivos, cliente.ClienteId));
                if(baja.Tipo == TipoDeResultado.Exito)
                {
                    Snackbar.Make(View, "Se dio de baja correctamente el cliente: " + cliente.ClienteId, Snackbar.LengthLong)
                    .Show();
                }
                else
                {
                    Snackbar.Make(View, "Servicio no disponible: ", Snackbar.LengthLong)
                   .Show();
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}