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
    public class Linea
    {
        public string OrdenId { get;  set; }
        public string UsuarioId { get; set; }
        public string AlmacenId { get;  set; }
        public string ArticuloId { get; set; }
        public string FechaDeEnvio { get;  set; }
        public string FechaDeEntrega { get;  set; }
        public decimal Cantidad { get;  set; }
        public string Nota1 { get;  set; }
        public string Nota2 { get;  set; }
        public decimal Disponible { get;  set; }
      //  public long RecId { get;  set; }
        public string ReferenciaDeCompra { get;  set; }
        public string ReferenciaDeCliente { get;  set; }
        public string MetodoDePago { get; set; }
        public decimal Descuento { get; set; }
        public string FormaDePago { get; set; }
        public Linea() { }
        public Linea(string ordenId, string usuarioId, string almacenId, string articuloId, string fechaDeEnvio,
            string fechaDeEntrega, decimal cantidad, string nota1, string nota2, decimal disponible, 
            string referenciaDeCompra, string referenciaDeCliente, string metododepago, decimal descuento, string formaDePago)
        {
           
          //  RecId = recId;
            OrdenId = ordenId;
            UsuarioId = usuarioId;
            AlmacenId = almacenId;
            ArticuloId = articuloId;
            FechaDeEnvio = fechaDeEnvio;
            FechaDeEntrega = fechaDeEntrega;
            Cantidad = cantidad;
            Nota1 = nota1;
            Nota2 = nota2;
            Disponible = disponible;
            ReferenciaDeCliente = referenciaDeCliente;
            ReferenciaDeCompra = referenciaDeCompra;
            MetodoDePago = metododepago;
            Descuento = descuento;
            FormaDePago = formaDePago;
        }
    }
}