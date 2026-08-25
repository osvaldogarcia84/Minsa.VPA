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
    public class DataSupervisores
    {
        public string Vendedor { get; set; }
        public string Almacen { get; set; }
        public DataSupervisores() { }
        public DataSupervisores(string vendedor, string almacen)
        {
            Vendedor = vendedor;
            Almacen = almacen;
        }
    }
}