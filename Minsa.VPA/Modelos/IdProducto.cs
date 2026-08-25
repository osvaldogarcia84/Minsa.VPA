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
    public class IdProducto
    {
        public string Id { get; set; }
        public IdProducto() { }
        public IdProducto(string id)
        {
            Id = id;
        }

    }
}