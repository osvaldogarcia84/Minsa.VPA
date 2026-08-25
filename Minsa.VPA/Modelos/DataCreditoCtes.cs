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
    public class DataCreditoCtes
    {
        public string Compania { get; set; }
        public string Cliente { get; set; }
        public DataCreditoCtes() { }
        public DataCreditoCtes(string compania, string cliente)
        {
            Compania = compania;
            Cliente = cliente;
        }
    }
}