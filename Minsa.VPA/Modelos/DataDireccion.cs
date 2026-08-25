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
    public class DataDireccion
    {
        public string MUNICIPIO { get; set; }
        public string ESTADO { get; set; }
        public DataDireccion() { }
        public DataDireccion(string municipio, string estado)
        {
            MUNICIPIO = municipio;
            ESTADO = estado;
        }
    }
}