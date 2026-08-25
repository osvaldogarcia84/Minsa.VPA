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
    public class DataOrdenesYalo
    {
        public string Sitio { get; set; }
        public DataOrdenesYalo() { }
        public DataOrdenesYalo(string sitio)
        {
            Sitio = sitio;
        }
    }
}