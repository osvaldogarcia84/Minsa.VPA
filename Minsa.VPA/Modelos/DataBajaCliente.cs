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
    public class DataBajaCliente
    {
        public string Motivo { get; set; }
        public string IdCliente { get; set; }
        public DataBajaCliente() { }
        public DataBajaCliente(string motivo, string idcliente)
        {
            Motivo = motivo;
            IdCliente = idcliente;
        }
    }
}