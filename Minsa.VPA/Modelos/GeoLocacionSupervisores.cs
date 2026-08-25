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
    public class GeoLocacionSupervisores
    {
        public int Id { get; set; }
        public string ClienteId { get; set; }
        public string UsuarioId { get; set; }
        public decimal Longitud { get; set; }
        public decimal Latitud { get; set; }
        public GeoLocacionSupervisores() { }
        public GeoLocacionSupervisores(int id, string clienteId, string usuarioid, decimal longitud, decimal latitud)
        {
            Id = id;
            ClienteId = clienteId;
            UsuarioId = usuarioid;
            Longitud = longitud;
            Latitud = latitud;
        }
    }
}