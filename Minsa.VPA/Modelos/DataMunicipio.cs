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
    public class DataMunicipio
    {
        public string CP { get; set; }
        public DataMunicipio() { }
        public DataMunicipio(string cp)
        {
            CP = cp;
        }
    }
}