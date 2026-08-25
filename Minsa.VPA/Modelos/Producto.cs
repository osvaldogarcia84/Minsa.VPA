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
    public class Producto
    {
        public string Id { get; set; }

        public string Nombre { get; set; }
        public Compania Compañia { get; set; }

        public Producto()
        {
        }

        public Producto(string id, string nombre, Compania compañia)
        {
            Id = id;
            Nombre = nombre;
            Compañia = compañia;
        }

        public override string ToString()
        {
            return String.Format("{0}-{1}", Id, Nombre);
        }
    }
}