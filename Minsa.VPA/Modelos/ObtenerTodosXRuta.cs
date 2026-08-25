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
    public class ObtenerTodosXRuta
    {
        public string Id { get; set; }

        public string Usuario { get; set; }

        public string Nombre { get; set; }

        public string Filtro { get; set; }

        public ObtenerTodosXRuta() { }
        public ObtenerTodosXRuta(string id, string usuario, string nombre, string filtro)
        {
            Usuario = usuario;
            Id = id;
            Nombre = nombre;
            Filtro = filtro;
        }
    }
}