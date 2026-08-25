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
    public class DataCreaTicket
    {
        public string Factura { get; set; }
        public decimal ImportePagado { get; set; }
        public string FormaDePago { get; set; }
        public DataCreaTicket() { }
        public DataCreaTicket(string factura, decimal importepagado, string formadepago)
        {
            Factura = factura;
            ImportePagado = importepagado;
            FormaDePago = formadepago;
        }
    }
}