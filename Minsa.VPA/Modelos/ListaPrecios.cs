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
    public class ListaPrecios
    {
        public string PRICEGROUP { get; set; }
        public int CONTADOR { get; set; }
        public ListaPrecios() { }
        public ListaPrecios(string pricegroup, int contador)
        {
            PRICEGROUP = pricegroup;
            CONTADOR = contador;
        }
    }
}