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
    public class Respuesta
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public TipoDeCausa Tipo { get; set; }

        public Respuesta(int id, string texto, TipoDeCausa tipo)
        {
            Tipo = tipo;
            Id = id;
            Texto = texto;
        }

        public override string ToString()
        {
            return Texto;
        }
    }

    public enum TipoDeCausa
    {
        Venta = 1,
        NoVenta = 0
        //Disminucinon = 2
    }
}