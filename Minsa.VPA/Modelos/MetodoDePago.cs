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
    public class MetodoDePago
    {
        public string PAYMMODE { get; set; }
        public string NAME { get; set; }
        public MetodoDePago()
        {
        }
        public MetodoDePago(string paymmode, string name)
        {
            PAYMMODE = paymmode;
            NAME = name;
        }
    }
}