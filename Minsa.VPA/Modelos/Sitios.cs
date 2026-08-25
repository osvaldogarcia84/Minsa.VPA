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
    public class Sitios
    {
        public string SITIOS { get; set; }
        public string OBJECTID { get; set; }
        public Sitios() { }
        public Sitios(string sitio, string objectid)
        {
            SITIOS = sitio;
            OBJECTID = objectid;
        }
    }
}