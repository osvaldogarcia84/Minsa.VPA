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
    public class LlaveVendedor
    {
        public string Id { get; set; }
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Llave { get; set; }
        public LlaveVendedor() { }
        public LlaveVendedor
            (string id, string usuario, string nombre, string llave)
        {
            Usuario = usuario;
            Id = id;
            Nombre = nombre;
            Llave = llave;
        }
    }
}