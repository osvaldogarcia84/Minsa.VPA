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
    public class DataValidaKmInicial
    {
        public string Usuario { get; set; }
        public string Almacen { get; set; }
        public DataValidaKmInicial() { }
        public DataValidaKmInicial(string usuario, string almacen)
        {
            Usuario = usuario;
            Almacen = almacen;
        }
    }
}