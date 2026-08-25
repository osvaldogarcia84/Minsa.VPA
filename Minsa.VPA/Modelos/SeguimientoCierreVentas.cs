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
    public class SeguimientoCierreVentas
    {
        public string IdCliente { get; set; }
        public decimal Volumen { get; set; }
        public string EstrategiaCerrarVenta { get; set; }
        public decimal Cantidad { get; set; }
        public string PlazoEstrategia { get; set; }
        public string Marca { get; set; }
        public string Recurso { get; set; }
        public decimal Precio { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Sitio { get; set; }
        public SeguimientoCierreVentas() { }
        public SeguimientoCierreVentas(string idcliente, decimal volumen, string estrategiacerrarventa, decimal cantidad,
                                       string plazoestrategia,
                                       string marca,
                                       string recurso,
                                       decimal precio,
                                       string telefono,
                                       string correo,
                                       string sitio)
        {
            IdCliente = idcliente;
            Volumen = volumen;
            EstrategiaCerrarVenta = estrategiacerrarventa;
            Cantidad = cantidad;
            PlazoEstrategia = plazoestrategia;
            Marca = marca;
            Recurso = recurso;
            Precio = precio;
            Telefono = telefono;
            Correo = correo;
            Sitio = sitio;
        }
    }
}