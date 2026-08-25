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
    public class DataEliminaPronostico
    {
        public string Fecha { get; set; }
        public string Almacen { get; set; }
        public string Cliente { get; set; }
        public string Vendedor { get; set; }
        public string Recurso { get; set; }
        public DataEliminaPronostico() { }
        public DataEliminaPronostico(string fecha, string almacen, string cliente, string vendedor, string recurso)
        {
            Fecha = fecha;
            Almacen = almacen;
            Cliente = cliente;
            Vendedor = vendedor;
            Recurso = recurso;
        }
    }
}