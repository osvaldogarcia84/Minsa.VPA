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
    public class DatosTicket
    {        
        public string Sitio { get; set; }       
        public string Factura { get; set; }
        public DatosTicket() { }
        public DatosTicket(string sitio, string factura)
        {
            Sitio = sitio;
            Factura = factura;
        }

    }
}