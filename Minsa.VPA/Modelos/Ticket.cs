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
    public class Ticket
    {
        public string Folio { get; set; }
        public string Almacen { get; set; }
        public string FechaFactura { get; set; }
        public string Hora { get; set; }
        public string Cliente { get; set; }
        public string Nombre { get; set; }
        public string CuentaFacturacion { get; set; }
        public string Factura { get; set; }
        public string DigitoVerificador { get; set; }

        public string Ruta { get; set; }
        public string Bodega { get; set; }
        public decimal Bultos { get; set; }
        public decimal Precio { get; set; }
        public decimal Importe { get; set; }
        public decimal ImporteTotal { get; set; }
        public decimal MontoxFactura { get; set; }
        public string CantidadLetras { get; set; }
        public string FormaPago { get; set; }
        public Ticket() { }
        public Ticket(string folio, string almacen, string fechafactura, string hora, string cliente, string nombre, string cuentafacturacion,
                      string factura, string digitoverificador, string ruta, string bodega, decimal bultos, decimal precio, decimal importe,
                      decimal importetotal, decimal montofactura, string cantidadletra, string formapago)
        {
            Folio = folio;
            Almacen = almacen;
            FechaFactura = fechafactura;
            Hora = hora;
            Cliente = cliente;
            Nombre = nombre;
            CuentaFacturacion = cuentafacturacion;
            Factura = factura;
            DigitoVerificador = digitoverificador;
            Ruta = ruta;
            Bodega = bodega;
            Bultos = bultos;
            Precio = precio;
            Importe = importe;
            ImporteTotal = importetotal;
            MontoxFactura = montofactura;
            CantidadLetras = cantidadletra;
            FormaPago = formapago;
        }
    }
}