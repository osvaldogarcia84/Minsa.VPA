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
    public class InformacionDeVenta
    {
        public string ArticuloId { get; set; }
        public string NombreArticulo { get; set; }
        public decimal Monto { get; set; }
        public string Sitio { get; set; }
        public string Grupo { get; set; }
        public decimal Impuesto { get; set; }
        public InformacionDeVenta() { }
        public InformacionDeVenta(string articuloid, string nombrearticulo, decimal monto, string sitio, string grupo, decimal impuesto)
        {
            ArticuloId = articuloid;
            NombreArticulo = nombrearticulo;
            Monto = monto;
            Sitio = sitio;
            Grupo = grupo;
            Impuesto = impuesto;
        }

        //public List<Articulo> Articulos { get; set; }
        //public List<Precio> Precios { get; set; }
        ////public IList<Descuento> Descuentos { get; set; }
        //public bool GeneraCargoComercial { get; set; }
        //public string SitioDePrecioId { get; set; }

        //public InformacionDeVenta() { }

        //public InformacionDeVenta
        //        (
        //         List<Precio> precios,
        //         List<Articulo> articulos,
        //    bool generaCargoComercial, string sitioDePrecioId)
        //{
        //    SitioDePrecioId = sitioDePrecioId;
        //    GeneraCargoComercial = generaCargoComercial;
        //    Articulos = articulos;
        //    Precios = precios;
        //    //    Descuentos = descuentos;

        //}
    }
}