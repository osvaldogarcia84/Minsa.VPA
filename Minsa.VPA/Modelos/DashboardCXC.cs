using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minsa.VPA.Modelos
{
    public class DashboardCXC
    {
        public string Sitio { get; set; }
        public string Cliente { get; set; }
        public string NombreCliente { get; set; }
        public decimal LimiteCred { get; set; }
        public string TipoDeDocumento { get; set; }
        public string FormaDePago { get; set; }
        public string Factura { get; set; }
        public string FechaDocumento { get; set; }
        public string FechaVencimiento { get; set; }
        public int DiasAVencer { get; set; }
        public string Moneda { get; set; }
        public decimal MontoOriginal { get; set; }
        public decimal MontoAbierto { get; set; }
        public decimal ImporteCorriente { get; set; }
        public decimal ImporteVencido { get; set; }
        public string DigitoVerificador { get; set; }
        public string ClienteFT { get; set; }

        public DashboardCXC() { }
        public DashboardCXC(string sitio, string cliente, string nombrecliente, decimal limitecred, string tipodedocumento, string formadepago, string factura, string fechadocumento,
                            string fechavencimiento, int diasAvencer, string moneda, decimal montooriginal, decimal montoabierto, decimal importecorriente, decimal importevencido, string clienteft)
        {
            Sitio = sitio;
            Cliente = cliente;
            NombreCliente = nombrecliente;
            LimiteCred = limitecred;
            TipoDeDocumento = tipodedocumento;
            FormaDePago = formadepago;
            Factura = factura;
            FechaDocumento = fechadocumento;
            FechaVencimiento = fechavencimiento;
            DiasAVencer = diasAvencer;
            Moneda = moneda;
            MontoOriginal = montooriginal;
            MontoAbierto = montoabierto;
            ImporteCorriente = importecorriente;
            ImporteVencido = importevencido;
            ClienteFT = clienteft;
        }

    }
}