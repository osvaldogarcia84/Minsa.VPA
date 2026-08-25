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
    public class ObtenerInfoVenta
    {
       
        public string VendedorId { get; set; }       
        public string Usuario { get; set; }      
        public string Nombre { get; set; }       
        public string Llave { get; set; }       
        public string ClienteId { get; set; }

        public ObtenerInfoVenta() { }
        public ObtenerInfoVenta
            (string vendedorid, string usuario, string nombre, string llave, string clienteid)
        {
            Usuario = usuario;
            VendedorId = vendedorid;
            Nombre = nombre;
            Llave = llave;
            ClienteId = clienteid;
        }
    }
}