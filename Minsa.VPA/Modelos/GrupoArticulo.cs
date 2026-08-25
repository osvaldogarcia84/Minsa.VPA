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
    public class GrupoArticulo
    {        
        public string ArticuloId { get; set; }
        public GrupoArticulo(string articuloid)
        {
            ArticuloId = articuloid;
        }
        public GrupoArticulo() { }
    }
}