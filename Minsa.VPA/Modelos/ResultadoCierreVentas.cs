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
    public class ResultadoCierreVentas
    {
        public string Respuesta { get; set; }
        public ResultadoCierreVentas() { }
        public ResultadoCierreVentas(string respuesta)
        {
            Respuesta = respuesta;
        }
    }
}