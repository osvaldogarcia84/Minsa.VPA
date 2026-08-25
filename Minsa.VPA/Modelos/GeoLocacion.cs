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
    public class GeoLocacion
    {
        public int Id { get; set; }
        public string ClienteId { get; set; }
        public decimal Longitud { get; set; }
        public decimal Latitud { get; set; }
        public string VendedorId { get; set; }
        public GeoLocacion() { }
        public GeoLocacion(int id, string clienteid, decimal longitud, decimal latitud, string vendedorid)
        {
            Id = id;
            ClienteId = clienteid;
            Longitud = longitud;
            Latitud = latitud;
            VendedorId = vendedorid;
        }
    }
}