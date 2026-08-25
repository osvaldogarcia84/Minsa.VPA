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
    public class DataPromocionBultos
    {
        public string IdCliente { get; set; }
        public decimal Bultos { get; set; }
        public string Almacen { get; set; }
        public string Usuario { get; set; }
        public DataPromocionBultos() { }
        public DataPromocionBultos(string idcliente, decimal bultos, string almacen, string usuario)
        {
            IdCliente = idcliente;
            Bultos = bultos;
            Almacen = almacen;
            Usuario = usuario;
        }
    }
}