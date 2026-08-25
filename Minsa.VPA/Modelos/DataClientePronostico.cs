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
    public class DataClientePronostico
    {
        public string Cliente { get; set; }
        public string Company { get; set; }
        public string Almacen { get; set; }
        public string Mes { get; set; }
        public DataClientePronostico() { }
        public DataClientePronostico(string cliente, string company, string almacen, string mes)
        {
            Cliente = cliente;
            Company = company;
            Almacen = almacen;
            Mes = mes;
        }
    }
}