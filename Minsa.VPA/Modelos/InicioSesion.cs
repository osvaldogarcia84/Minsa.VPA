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
    public class InicioSesion
    {
        public string HoraInicial { get; set; }
        public string HoraFinal { get; set; }
        public string Version { get; set; }
        public InicioSesion() { }
        public InicioSesion(string horainicial, string horafinal, string version)
        {
            HoraInicial = horainicial;
            HoraFinal = horafinal;
            Version = version;
        }
    }
}