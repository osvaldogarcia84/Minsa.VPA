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
    public class CorteCaja
    {
        public string Sitio { get; set; }
        public string NombreVendedor { get; set; }

        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public string KmInicial { get; set; }
        public string KmFinal { get; set; }
        public DateTime FechaCheckIn { get; set; }
        public DateTime FechaCheckOut { get; set; }

        public CorteCaja() { }
        public CorteCaja(string sitio, string nombre, string checkin, string checkout, string kminicial, string kmfinal, DateTime fechacheckin, DateTime fechacheckout)
        {
            Sitio = sitio;
            NombreVendedor = nombre;
            CheckIn = checkin;
            CheckOut = checkout;
            KmInicial = kminicial;
            KmFinal = kmfinal;
            FechaCheckIn = FechaCheckIn;
            FechaCheckOut = fechacheckout;
        }

    }
}