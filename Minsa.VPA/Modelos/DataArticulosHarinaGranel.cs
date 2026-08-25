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
    public class DataArticulosHarinaGranel
    {
        public string ITEMID { get; set; }
        public DataArticulosHarinaGranel() { }
        public DataArticulosHarinaGranel(string itemid)
        {
            ITEMID = itemid;
        }
    }
}