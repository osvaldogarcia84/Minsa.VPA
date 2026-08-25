using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Minsa.VPA.Modelos
{
    public class InformacionDeInventario
    {
        public string CodigoArticulo { get; set; }
        public string DisponiblePorSaco { get; set; }
        public string NombreArticulo { get; set; }
        public string DisponiblePorTM { get; set; }
        public string PrecioPorSaco { get; set; }
        public string PrecioPorTM { get; set; }
        public string UnidadDeMedida { get; set; }
        public InformacionDeInventario() { }
        public InformacionDeInventario(string codigoArticulo, string nombrearticulo, string disponible, string disponibleporTM, string PrecioSaco, string PrecioTM, string unidaddemedida)
        {
            CodigoArticulo = codigoArticulo;
            NombreArticulo = nombrearticulo;
            DisponiblePorSaco = disponible;
            DisponiblePorTM = disponibleporTM;
            PrecioPorSaco = PrecioSaco;
            PrecioPorTM = PrecioTM;
            UnidadDeMedida = unidaddemedida;
        }
    }
}