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
    public class ValidaKmInicial
    {
        public string Usuario { get; set; }
        public string Almacen { get; set; }
        public bool Estatus { get; set; }
        public DateTime FechaCreacion { get; set; }
        public ValidaKmInicial() { }
        public ValidaKmInicial(string usuario, string almacen, bool estatus, DateTime fechacreacion)
        {
            Usuario = usuario;
            Almacen = almacen;
            Estatus = estatus;
            FechaCreacion = fechacreacion;
        }
    }
}