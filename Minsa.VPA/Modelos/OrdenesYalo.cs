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
    public class OrdenesYalo
    {
        public string ORDERID { get; set; }
        public string CTACLIENTE { get; set; }
        public string NOMBRECLIENTE { get; set; }
        public string ESTATUSBANCO { get; set; }
        public string ESTADOORDEN { get; set; }
        public string FORMADEPAGO { get; set; }
        public string FECHACREACION { get; set; }
        public List<LineasYalo> LINEAS { get; set; } = new List<LineasYalo>();
    }
    public class LineasYalo
    {
        public string ORDERIDLINEA { get; set; }
        public string IDPRODUCTO { get; set; }
        public string NOMBREPRODUCTO { get; set; }
        public decimal CANTIDAD { get; set; }
        public decimal PRECIOUNITARIO { get; set; }
        public decimal DESCUENTO { get; set; }
        public decimal IMPORTENETO { get; set; }
        public bool BLOQUEADO { get; set; }
    }
}