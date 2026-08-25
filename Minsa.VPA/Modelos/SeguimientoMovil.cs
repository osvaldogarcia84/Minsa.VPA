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
    public class SeguimientoMovil
    {
        public int Id { get; set; }
        public AccionDeSeguimiento Accion { get; set; }
        public string ClienteId { get; set; }
        public string VendedorId { get; set; }

        public SeguimientoMovil()
        {
        }

        public SeguimientoMovil(int id, AccionDeSeguimiento accion, string clienteId, string vendedorid)
        {
            Id = id;
            Accion = accion;
            ClienteId = clienteId;
            VendedorId = vendedorid;
        }
        public enum AccionDeSeguimiento
        {
            EntregaDeFactura = 1,
            VentaDeProductos = 2,
            Reclamacion = 3,
            Servicio = 4
        }
    }
}