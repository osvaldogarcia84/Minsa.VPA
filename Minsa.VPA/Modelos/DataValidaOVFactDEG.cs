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
    public class DataValidaOVFactDEG
    {
        public string Valor { get; set; }
        public DataValidaOVFactDEG() {  }
        public DataValidaOVFactDEG(string valor)
        {
            Valor = valor;
        }
    }
}