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

namespace Minsa.VPA.Servicios
{
    public static class InformacionGeneral
    {
        public const string Compañia = "207";
        public const string ErrorDeBd = "No se pudo persistir la información.";
        public const string ErrorDeProceso = "Ocurrió un error en el proceso, reportar a sistemas.";
        public const string FormatoDeFecha = "yyyy-MM-dd";
        public static DateTime FechaInvalida;
        public const string InformacionDeTipoDeOrden = "VPA";
        public const string HoraCierre = "22:00:00";
        public const string HoraApertura = "05:00:00";
        public const double Sacos25KG = 0.025;
        public const double ConversionTonelada = 0.02;
        public const double Pieza = 1.00;
    }
}