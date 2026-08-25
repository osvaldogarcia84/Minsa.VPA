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
    public class ClienteGeocerca
    {
        public string ClienteId { get; set; }

        public string Nombre { get; set; }

        public double Latitud { get; set; }

        public double Longitud { get; set; }

        public int Secuencia { get; set; }

        /// <summary>
        /// Radio utilizado por Android para detectar que
        /// el dispositivo está cerca del cliente.
        /// </summary>
        public float RadioGeocercaMetros { get; set; }

        /// <summary>
        /// Radio real que utilizaremos posteriormente
        /// para validar la ubicación antes de registrar.
        /// </summary>
        public float RadioValidacionMetros { get; set; }

        public ClienteGeocerca()
        {
            RadioGeocercaMetros = 120;
            RadioValidacionMetros = 60;
        }
    }
}