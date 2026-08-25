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
    public class Lan
    {
        public double Longitud { get; set; }
        public double Latitud { get; set; }
        public Lan(double longitud, double latitud)
        {
            Longitud = longitud;
            Latitud = latitud;
        }

    }
}