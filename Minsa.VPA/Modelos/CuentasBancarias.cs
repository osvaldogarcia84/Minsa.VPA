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
    public class CuentasBancarias
    {
        public string NombreCuenta { get; set; }
        public string Banco { get; set; }
        public string Referencia { get; set; }
        public CuentasBancarias() { }
        public CuentasBancarias(string nombrecuenta, string banco, string referencia)
        {
            NombreCuenta = nombrecuenta;
            Banco = banco;
            Referencia = referencia;
        }
    }
}