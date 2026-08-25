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
    public class DisponiblePorArticulo
    {
    
        public string Sitio { get; set; }        
        public string IdArticulo { get; set; }
        public DisponiblePorArticulo() { }
        public DisponiblePorArticulo(string sitio, string idarticulo)
        {
            Sitio = sitio;
            IdArticulo = idarticulo;
        }
    }
}