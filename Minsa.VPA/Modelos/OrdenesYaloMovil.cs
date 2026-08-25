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
    public class OrdenesYaloMovil
    {
        public string ORDERID { get; set; }
        public string CTACLIENTE { get; set; }
        public string NOMBRECLIENTE { get; set; }
        public string ESTATUSBANCO { get; set; }
        public string ESTADOORDEN { get; set; }
        public string FORMADEPAGO { get; set; }
        public string FECHACREACION { get; set; }
        public bool TIENEDISPONIBLE { get; set; }
        public bool BLOQUEADA { get; set; }
        public decimal TOTALAPAGAR { get; set; }
        public List<LineasYaloMovil> LINEAS { get; set; } = new List<LineasYaloMovil>();
    }
    public class LineasYaloMovil
    {
        public string ORDERIDLINEA { get; set; }
        public string IDPRODUCTO { get; set; }
        public string NOMBREPRODUCTO { get; set; }
        public decimal CANTIDAD { get; set; }
        public decimal PRECIOUNITARIO { get; set; }
        public decimal DESCUENTO { get; set; }
        public decimal IMPORTENETO { get; set; }
        public decimal DISPONIBLE { get; set; }
        public bool BLOQUEADO { get; set; }
        //public LineasYaloMovil() { }
        //public LineasYaloMovil(string orderidlinea, string idproducto, string nombreproducto, decimal cantidad, decimal preciounitario, decimal descuento, decimal importeneto, decimal disponible)
        //{
        //    ORDERIDLINEA = orderidlinea;
        //    IDPRODUCTO = idproducto;
        //    NOMBREPRODUCTO = nombreproducto;
        //    CANTIDAD = cantidad;
        //    PRECIOUNITARIO = preciounitario;
        //    DESCUENTO = descuento;
        //    IMPORTENETO = importeneto;
        //    DISPONIBLE = disponible;
        //}
    }
}