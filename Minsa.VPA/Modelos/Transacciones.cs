using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Minsa.VPA.Modelos
{
    public class Transacciones
    {
        public string FechaFactura { get; set; }
        public string Cliente { get; set; }
        public string Nombre { get; set; }
        public string CuentaFacturacion { get; set; }
        public string Factura { get; set; }
        public string DigitoVerificador { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Bultos { get; set; }
        public decimal Precio { get; set; }
        public decimal Importe { get; set; }
        public decimal ImporteTotal { get; set; }
        public decimal MontoxFactura { get; set; }
        public string HoraCreacion { get; set; }
        public string HoraMaxReimpresion { get; set; }
        public string FormaPago { get; set; }
        public Transacciones() { }
        public Transacciones(string fechafactura, string cliente, string nombre, string cuentafacturacion,
                             string factura, string digitoverificador, string itemid, string itemname,
                             decimal bultos, decimal precio, decimal importe, decimal importetotal,
                             decimal montoxfactura,string horacreacion, string horamaxreimpresion, string formapago)
        {
            FechaFactura = fechafactura;
            Cliente = cliente;
            Nombre = nombre;
            CuentaFacturacion = cuentafacturacion;
            Factura = factura;
            DigitoVerificador = digitoverificador;
            ItemId = itemid;
            ItemName = itemname;
            Bultos = bultos;
            Precio = precio;
            Importe = importe;
            ImporteTotal = importetotal;
            MontoxFactura = montoxfactura;
            HoraCreacion = horacreacion;
            HoraMaxReimpresion = horamaxreimpresion;
            FormaPago = formapago;
        }
    }
}