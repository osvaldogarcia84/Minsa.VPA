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
    public class DataVendedor
    {
        public string Vendedor { get; set; }
        public DataVendedor() { }
        public DataVendedor(string vendedor)
        {
            Vendedor = vendedor;
        }
    }
}