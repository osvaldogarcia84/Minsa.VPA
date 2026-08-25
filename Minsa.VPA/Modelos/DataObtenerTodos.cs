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
    public class DataObtenerTodos
    {
        public string Vendedor { get; set; }
        public string Filtro { get; set; }
        public DataObtenerTodos() { }
        public DataObtenerTodos(string vendedor, string filtro)
        {
            Vendedor = vendedor;
            Filtro = filtro;
        }
    }
}