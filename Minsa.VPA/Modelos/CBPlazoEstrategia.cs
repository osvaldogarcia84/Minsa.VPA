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
    public class CBPlazoEstrategia
    {
        public string Valor { get; set; }
        public string Descripcion { get; set; }
        public CBPlazoEstrategia() { }
        public CBPlazoEstrategia(string valor, string descripcion)
        {
            Valor = valor;
            Descripcion = descripcion;
        }
    }
}