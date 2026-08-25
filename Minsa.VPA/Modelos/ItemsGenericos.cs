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
    public class ItemsGenericos
    {
        public string Id { get; set; }
        public string Texto { get; set; }
        public ItemsGenericos() { }
        public ItemsGenericos(string id, string texto)
        {
            Id = id;
            Texto = texto;
        }
    }
}