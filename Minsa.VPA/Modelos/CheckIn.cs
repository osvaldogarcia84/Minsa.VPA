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
    public class CheckIn
    {
        public string Sitio { get; set; }
        public string Vendedor { get; set; }
        public decimal Longitud { get; set; }
        public decimal Latitud { get; set; }
        public int KmInicial { get; set; }
        public DateTime FechaCreacion { get; set; }

        public CheckIn()
        {

        }
        public CheckIn(string sitio, string vendedor, decimal longitud, decimal latitud, DateTime fechacreacion, int kminicial)
        {
            Sitio = sitio;
            Vendedor = vendedor;
            Longitud = longitud;
            Latitud = latitud;
            FechaCreacion = fechacreacion;
            KmInicial = kminicial;
        }
    }
}