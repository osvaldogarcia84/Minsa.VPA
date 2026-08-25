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
    public class Estados
    {
        public string ZIPCODE { get; set; }
        public string IDESTADO { get; set; }
        public string NOMBREESTADO { get; set; }
        public Estados() { }
        public Estados(string zipcode, string idestado, string nombreestado)
        {
            ZIPCODE = zipcode;
            IDESTADO = idestado;
            NOMBREESTADO = nombreestado;
        }

    }
}