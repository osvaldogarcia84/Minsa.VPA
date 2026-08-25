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
    public class DataDashboardCXC
    {
        public string Sitio { get; set; }
        public string Cliente { get; set; }
        public DataDashboardCXC() { }
        public DataDashboardCXC(string sitio, string cliente)
        {
            Sitio = sitio;
            Cliente = cliente;
        }
    }
}