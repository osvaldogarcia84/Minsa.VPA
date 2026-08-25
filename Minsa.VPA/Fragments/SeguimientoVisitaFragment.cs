using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Grantland.Widget;
using Minsa.VPA.Modelos;
using Minsa.VPA.Proveedores;
using Minsa.VPA.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using static Minsa.VPA.Modelos.SeguimientoMovil;

namespace Minsa.VPA.Fragments
{
    public class SeguimientoVisitaFragment : Fragment
    {       
        public ResultadoDeOperacionGenerico<Vendedor> vendedorVisita;
        public const string LLaveSeguimientoVisita = "SeguimientoVisita";
        Cliente cliente;
        public List<SeguimientoMovil> seguimientos = new List<SeguimientoMovil>();
        Button btnEnviarVisitaSeguimiento;
        ProveedorDeEstrategia proveedorDeEstrategia = new ProveedorDeEstrategia();
        ResultadoDeOperacionGenerico<string> resultado;
        CheckBox cbSeguimientoVisitaFactura;
        CheckBox cbSeguimientoVisitaReclamacion;
        CheckBox cbSeguimientoVisitaServicio;
        CheckBox cbSeguimientoVisitaVenta;
        AutofitTextView NombreVendedorVisita;
        Timer timer;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            cliente = ProveedorGlobal.Cliente;
            vendedorVisita = ProveedorDeSerializado.Obtener<ResultadoDeOperacionGenerico<Vendedor>>(Arguments.Obtener<string>(MainActivity.LlaveUsuario));
        }
        public static SeguimientoVisitaFragment NewInstance()
        {
            var frag1 = new SeguimientoVisitaFragment { Arguments = new Bundle() };
            return frag1;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.SeguimientoVisita, null);

            NombreVendedorVisita = view.FindViewById<AutofitTextView>(Resource.Id.NombreVendedorVisita);
            NombreVendedorVisita.Text = cliente.ClienteId + " - " + cliente.Nombre;
            cbSeguimientoVisitaFactura = view.FindViewById<CheckBox>(Resource.Id.SeguimientoVisitaFactura);
            cbSeguimientoVisitaFactura.CheckedChange += (sender, args) =>
            {
                if (((CheckBox)sender).Checked)
                    seguimientos.Add(new SeguimientoMovil(0, AccionDeSeguimiento.EntregaDeFactura, cliente.ClienteId, vendedorVisita.Valor.Id));
                else
                {
                    var factura = seguimientos.FirstOrDefault(e => e.Accion == AccionDeSeguimiento.EntregaDeFactura);
                    if (factura != null)
                        seguimientos.Remove(factura);
                }
            };

            cbSeguimientoVisitaReclamacion = view.FindViewById<CheckBox>(Resource.Id.SeguimientoVisitaReclamacion);
            cbSeguimientoVisitaReclamacion.CheckedChange += (sender, args) =>
            {
                if (((CheckBox)sender).Checked)
                    seguimientos.Add(new SeguimientoMovil(0, AccionDeSeguimiento.Reclamacion, cliente.ClienteId, vendedorVisita.Valor.Id));
                else
                {
                    var reclamacion = seguimientos.FirstOrDefault(e => e.Accion == AccionDeSeguimiento.Reclamacion);
                    if (reclamacion != null)
                        seguimientos.Remove(reclamacion);
                }
            };

            cbSeguimientoVisitaServicio = view.FindViewById<CheckBox>(Resource.Id.SeguimientoVisitaServicio);
            cbSeguimientoVisitaServicio.CheckedChange += (sender, args) =>
            {
                if (((CheckBox)sender).Checked)
                    seguimientos.Add(new SeguimientoMovil(0, AccionDeSeguimiento.Servicio, cliente.ClienteId, vendedorVisita.Valor.Id));
                else
                {
                    var servicio = seguimientos.FirstOrDefault(e => e.Accion == AccionDeSeguimiento.Servicio);
                    if (servicio != null)
                        seguimientos.Remove(servicio);
                }
            };

            cbSeguimientoVisitaVenta = view.FindViewById<CheckBox>(Resource.Id.SeguimientoVisitaVenta);
            cbSeguimientoVisitaVenta.CheckedChange += (sender, args) =>
            {
                if (((CheckBox)sender).Checked)
                    seguimientos.Add(new SeguimientoMovil(0, AccionDeSeguimiento.VentaDeProductos, cliente.ClienteId, vendedorVisita.Valor.Id));
                else
                {
                    var venta = seguimientos.FirstOrDefault(e => e.Accion == AccionDeSeguimiento.VentaDeProductos);
                    if (venta != null)
                        seguimientos.Remove(venta);
                }
            };
            btnEnviarVisitaSeguimiento = view.FindViewById<Button>(Resource.Id.EnviarSeguimientoVisita);
            btnEnviarVisitaSeguimiento.Click += BtnEnviarVisitaSeguimiento_Click;
            return view;
        }

        private void BtnEnviarVisitaSeguimiento_Click(object sender, EventArgs e)
        {
            try
            {
                if (seguimientos != null)
                {

                    foreach (var seguimiento in seguimientos)
                    {
                        if (seguimiento.Accion != null)
                        {
                            var info = Task.Run(async () => {
                                resultado =  await proveedorDeEstrategia.RegistrarSeguimiento(new SeguimientoMovil(seguimiento.Id, seguimiento.Accion, seguimiento.ClienteId, seguimiento.VendedorId));
                            });
                            info.Wait();
                        }
                        else
                        {
                            Snackbar.Make(View, "No se ha seleccionado ninguna opcion", Snackbar.LengthLong)
                             .Show();
                        }

                    }
                    if (resultado.Tipo == TipoDeResultado.Exito)
                    {
                        Snackbar.Make(View, resultado.Mensaje, Snackbar.LengthLong)
                             .Show();
                        cbSeguimientoVisitaFactura.Enabled = false;
                        cbSeguimientoVisitaReclamacion.Enabled = false;
                        cbSeguimientoVisitaServicio.Enabled = false;
                        cbSeguimientoVisitaVenta.Enabled = false;
                        btnEnviarVisitaSeguimiento.Enabled = false;
                    }
                    else
                    {
                        Snackbar.Make(View, resultado.Mensaje, Snackbar.LengthLong)
                             .Show();
                    }
                }
                else
                {
                    Snackbar.Make(View, "No se ha seleccionado ninguna opcion", Snackbar.LengthLong)
                              .Show();
                }
            }
            catch(Exception ex)
            {
                Snackbar.Make(View, "Error: " + ex, Snackbar.LengthLong)
                         .Show();
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
    }
}