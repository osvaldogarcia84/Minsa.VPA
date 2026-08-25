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
    public class DataKmInicial
    {
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Almacen { get; set; }
        public string NoVendedor { get; set; }
        public int KMInicial { get; set; }
        public DataKmInicial() { }
        public DataKmInicial(string usuario, string nombre, string almacen, string novendedor, int kminicial)
        {
            Usuario = usuario;
            Nombre = nombre;
            Almacen = almacen;
            NoVendedor = novendedor;
            KMInicial = kminicial;

        }
    }
}