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
    public class DataListaPrecios
    {
        public string Pais { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public DataListaPrecios() { }
        public DataListaPrecios(string pais, string estado, string municipio)
        {
            Pais = pais;
            Estado = estado;
            Municipio = municipio;
        }
    }
}