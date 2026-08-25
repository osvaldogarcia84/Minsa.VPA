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
using Minsa.VPA.Enums;
using SQLite;

namespace Minsa.VPA.Modelos
{
    public class ClienteMovil
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string ClienteId { get; set; }
        public bool Bloqueado { get; set; }
        //public int DiasDeTraslado { get; set; }
        public TipoDeMercado Mercado { get; set; }
        public string Nombre { get; set; }
        public int Secuencia { get; set; }
        public string SitioDeEnvio { get; set; }
        public string Tipo { get; set; }
        public string TipoDeVisita { get; set; }
        public bool Visitado { get; set; }
        public DateTime FechaDeCreacion { get; set; }
        //public int DigitoVerificador { get; set; }
        public ClienteMovil()
        {
        }

        public ClienteMovil(int id, string clienteId, string nombre, string tipo, string tipoDeVisita,
                            bool bloqueado, int secuencia, bool visitado)
        {
            Id = id;
            ClienteId = clienteId;
            Nombre = nombre;
            Tipo = tipo;
            TipoDeVisita = tipoDeVisita;
            //SitioDeEnvio = sitioDeEnvio;
           // DiasDeTraslado = diasDeTraslado;
            Bloqueado = bloqueado;
            Secuencia = secuencia;
            Visitado = visitado;
           // DigitoVerificador = digitoVerificador;
            FechaDeCreacion = DateTime.Now;
        }
    }
}