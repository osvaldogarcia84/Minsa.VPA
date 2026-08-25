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
    public class PrecioSeguimientoVenta
    {
        public int Valor { get; set; }
        public string Descripcion { get; set; }
        public PrecioSeguimientoVenta() { }
        public PrecioSeguimientoVenta(int valor, string descripcion)
        {
            Valor = valor;
            Descripcion = descripcion;
        }
    }
}