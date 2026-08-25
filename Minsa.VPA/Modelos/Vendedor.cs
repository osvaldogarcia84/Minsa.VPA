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
    public class Vendedor
    {

        public string Id { get; set; }
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Almacen { get; set; }
        public Vendedor() { }
        public Vendedor(string id, string usuario, string nombre, string almacen)
        {
            Usuario = usuario;
            Id = id;
            Nombre = nombre;
            Almacen = almacen;
        }
    }
}