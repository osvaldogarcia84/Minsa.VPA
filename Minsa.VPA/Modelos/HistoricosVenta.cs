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
    public class HistoricosVenta
    {
        public int Id { get; set; }
        public string Sitio { get; set; }
        public decimal VentaActual { get; set; }
        public decimal Presupuesto { get; set; }
        public decimal VentaMesAnterior { get; set; }
        public decimal VentaUltimosTresMeses { get; set; }
        public string NombreVendedor { get; set; }
        public HistoricosVenta() { }
        public HistoricosVenta(int id, string sitio, decimal ventaactual, decimal presupuesto, decimal ventamesanterior, decimal ventaultimostresmeses ,string nombrevendedor)
        {
            Id = id;
            Sitio = sitio;
            VentaActual = ventaactual;
            Presupuesto = presupuesto;
            VentaMesAnterior = ventamesanterior;
            VentaUltimosTresMeses = ventaultimostresmeses;
            NombreVendedor = nombrevendedor;
        }
    }
}