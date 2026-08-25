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
    public class Municipio
    {
        public string COUNTYID { get; set; }
        public string DESCRIPCION { get; set; }
        public Municipio() { }
        public Municipio(string countyid, string descripcion)
        {
            COUNTYID = countyid;
            DESCRIPCION = descripcion;
        }
    }
}