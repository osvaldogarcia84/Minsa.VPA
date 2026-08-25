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
    public class TipoPersona
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public TipoPersona() { }
        public TipoPersona(string text, string value)
        {
            Text = text;
            Value = value;
        }
    }
}