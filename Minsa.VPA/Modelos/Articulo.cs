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
using Minsa.VPA.Enums;

namespace Minsa.VPA.Modelos
{
    public class Articulo
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public Harinera Harinera { get; set; }
        public string Grupo { get; set; }
        public Articulo() { }
        //public Articulo(string id, string nombre, Harinera harinera, string grupo)
        //{
        //    Grupo = grupo;
        //    Harinera = harinera;
        //    Id = id;
        //    Nombre = nombre;
        //}

        public Articulo(string id, string nombre, string harinera, string grupo)
        {
            Grupo = grupo;
            Harinera = Harineras.ObtenerHarinaera(harinera);
            Id = id;
            Nombre = nombre;
        }

        public override string ToString()
        {
            return String.Format("{0}-{1}", Id, Nombre);
        }
    }
}