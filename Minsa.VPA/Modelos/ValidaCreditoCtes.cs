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
    public class ValidaCreditoCtes
    {
        public string Cliente { get; set; }
        public decimal Saldo { get; set; }
        public decimal LimiteCred { get; set; }
      
        public ValidaCreditoCtes() { }
        public ValidaCreditoCtes(string cliente, decimal saldo, decimal limitecred)
        {
            Cliente = cliente;
            Saldo = saldo;
            LimiteCred = limitecred;
          
        }
    }
}