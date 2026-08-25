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
    public class ResultadoDeVenta
    {
        public string ClienteId { get; set; }
        public int RespuestaId { get; set; }
        public string OrdenId { get; set; }
        public string VendedorId { get; set; }
        public ResultadoDeVenta()
        {
        }
        public ResultadoDeVenta(string clienteId,
                                int respuestaid,
                                string vendedorid)
        {

            ClienteId = clienteId;

            RespuestaId = respuestaid;
            VendedorId = vendedorid;
        }
    }
}