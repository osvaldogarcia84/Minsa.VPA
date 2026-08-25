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
    public class ClienteId
    {
        public string Id { get; set; }
        public ClienteId() { }
        public ClienteId(string id)
        {
            Id = id;
        }
    }
}