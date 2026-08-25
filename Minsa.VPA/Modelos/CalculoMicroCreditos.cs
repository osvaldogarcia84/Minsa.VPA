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
    public class CalculoMicroCreditos
    {
        public decimal Monto { get; set; } 
        public string Resultado { get; set; }
        public CalculoMicroCreditos() { }
        public CalculoMicroCreditos(decimal monto, string resultado) 
        {
            Monto = monto;
            Resultado = resultado;
        }
    }
}