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
    public class CerrarVenta
    {
        public string Sitio { get; set; }
        public string IdCliente { get; set; }
        public CerrarVenta() { }
        public CerrarVenta(string sitio, string idcliente)
        {
            Sitio = sitio;
            IdCliente = idcliente;
        }
    }
}