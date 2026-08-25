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
    public class Orden
    {
        
        public string ClienteId { get; set; }            
        public string TipoDeOrden { get; set; }
        public string CompaniaId { get; set; }
        public string Vendedor { get; set; }
        
        public string NombreVendedor { get; set; }       

        public decimal CoordenadaX { get; set; }
        
        public decimal CoordenadaY { get; set; }
        public string FormaPago { get; set; }
        public string ReferenciaDeCompra { get; set; }

        public string ReferenciaDeCliente { get; set; }


        public Orden() { }
        public Orden(string clienteid,
                     string tipodeorden,
                     string compania,                                     
                     string vendedor,
                     string nombrevendedor,
                     decimal coordenadax,
                     decimal coordenaday,
                     string formapago,
                     string referenciadecompra,
                     string referenciadecliente

            )
        {
            ClienteId = clienteid;            
            TipoDeOrden = tipodeorden;
            CompaniaId = compania;
            Vendedor = vendedor;
            NombreVendedor = nombrevendedor;
            CoordenadaX = coordenadax;
            CoordenadaY = coordenaday;
            FormaPago = formapago;
            ReferenciaDeCompra = referenciadecompra;
            ReferenciaDeCliente = referenciadecliente;
        }
    }
}