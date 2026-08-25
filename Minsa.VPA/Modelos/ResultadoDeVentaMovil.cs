using System;

using Android.Widget;

namespace Minsa.VPA.Modelos
{
    public class ResultadoDeVentaMovil
    {
        //[AutoIncrement]
        public int Id { get; set; }
        public string ClienteId { get; set; }
        public DateTime FechaDeCreacion { get; set; }
        public string OrdenId { get; set; }
        public Respuesta Respuesta { get; set; }

        public ResultadoDeVentaMovil()
        {
        }

        public ResultadoDeVentaMovil(int id, string clienteId, DateTime fechaDeCreacion, string ordenId,
            Respuesta respuesta)
        {
            Id = id;
            ClienteId = clienteId;
            FechaDeCreacion = fechaDeCreacion;
            OrdenId = ordenId;
            Respuesta = respuesta;
        }

    }
}