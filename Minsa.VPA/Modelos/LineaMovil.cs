using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Minsa.VPA.Enums;

namespace Minsa.VPA.Modelos
{
    public class LineaMovil
    {
        public string ArticuloId { get; set; }
        public string ArticuloNombre { get; set; }
        public string AlmacenId { get; set; }        
        public decimal PrecioNeto { get; set; }
        public decimal Importe { get; set; }
        public decimal Descuento { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Disponible { get; set; }
        public string FechaDeEntrega { get; set; }
        public string FechaDeEnvio { get; set; }        
        public string ReferenciaDeCliente { get; set; }
        public string ReferenciaDeCompra { get; set; }
        public string VendedorId { get; set; }      
        
        public decimal CoordenadaX { get; set; }
        public decimal CoordenadaY { get; set; }
        public Drawable Image { get; set; }
        public decimal Impuesto { get; set; }
        public string GrupoArticulo { get; set; }

        public LineaMovil()
        {
        }

        public LineaMovil(string articuloId, string articulonombre, decimal cantidad, decimal precioNeto, decimal importe, decimal descuento,            
            string almacenId, string fechaDeEntrega, 
            string fechaDeEnvio, string referenciaDeCliente, string referenciaDeCompra, 
            string vendedorid, decimal coordenadax, decimal coordenaday,
            decimal impuesto,string grupoarticulo,
            Context context, ArticuloVistaTipo tipo = ArticuloVistaTipo.Preview)
        {
            
            
           
            ArticuloId = articuloId;
            ArticuloNombre = articulonombre;
            Cantidad = cantidad;
            PrecioNeto = precioNeto;
            Importe = importe;
            Descuento = descuento;
            AlmacenId = almacenId;            
            FechaDeEntrega = fechaDeEntrega;
            FechaDeEnvio = fechaDeEnvio;          
            ReferenciaDeCliente = referenciaDeCliente;
            ReferenciaDeCompra = referenciaDeCompra;
            VendedorId = vendedorid;
            CoordenadaX = coordenadax;
            CoordenadaY = coordenaday;
            Impuesto = impuesto;
            GrupoArticulo = grupoarticulo;
            try
            {
                string path = string.Format("{0}/{1}.jpg", tipo, articuloId);
                using (var stream = context.Assets.Open(path))
                {
                    Image = Drawable.CreateFromStream(stream, null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }    

        public decimal TotalNeto()
        {                      
            return ((PrecioNeto * Cantidad) - (Descuento * Cantidad));
        }
        public decimal TotalNetoIVA()
        {
            decimal TotalNeto = 0;
            decimal OperacionIVA = 0;
            TotalNeto = ((PrecioNeto * Cantidad) - (Descuento * Cantidad));
            OperacionIVA = TotalNeto * Impuesto;
            return TotalNeto + OperacionIVA;
        }
        public decimal Impuestos()
        {
            if(Impuesto != 0)
            {
                decimal Total = 0;
                Total = ((PrecioNeto * Cantidad) - (Descuento * Cantidad));
                return Total * Impuesto;
            }
            else
            {
                return Impuesto = 0;
            }
            
        }
        public decimal PrecioPorTM()
        {
            return ((PrecioNeto * 50) - Descuento);
        }
        public decimal PrecioFardo()
        {
            return (PrecioNeto * 1000); // - Descuento;
        }
        public decimal PrecioFardo450gr()
        {
            return (PrecioNeto * Cantidad);
        }
      

        
    }
}