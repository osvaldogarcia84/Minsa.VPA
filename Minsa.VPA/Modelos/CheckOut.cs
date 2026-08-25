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
    public class CheckOut
    {
        public string Sitio { get; set; }
        public string Vendedor { get; set; }
        public decimal Longitud { get; set; }
        public decimal Latitud { get; set; }
        //   public int KmInicial { get; set; }
        public int KmFinal { get; set; }
        // public DateTime FechaCreacion { get; set; }

        public CheckOut()
        {

        }
        public CheckOut(string sitio, string vendedor, decimal longitud, decimal latitud, int kmfinal)
        {
            Sitio = sitio;
            Vendedor = vendedor;
            Longitud = longitud;
            Latitud = latitud;
            //  KmInicial = kminicial;
            KmFinal = kmfinal;
            // FechaCreacion = fechacreacion;
        }
    }
}