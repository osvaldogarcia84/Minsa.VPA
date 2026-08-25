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
    public class PromocionBultos
    {
        public string Division { get; set; }
        public string ObjectId { get; set; }
        public int Descuento { get; set; }
        public PromocionBultos() { }
        public PromocionBultos(string division, string objectid, int descuento)
        {
            Division = division;
            ObjectId = objectid;
            Descuento = descuento;
        }
    }
}