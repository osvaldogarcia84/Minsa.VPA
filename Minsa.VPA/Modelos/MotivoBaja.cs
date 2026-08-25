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
    public class MotivoBaja
    {
        public string MOTIVO { get; set; }
        public string TIPOMOVTO { get; set; }
        public MotivoBaja() { }
        public MotivoBaja(string motivo, string tipomovto)
        {
            MOTIVO = motivo;
            TIPOMOVTO = tipomovto;
        }
    }
}