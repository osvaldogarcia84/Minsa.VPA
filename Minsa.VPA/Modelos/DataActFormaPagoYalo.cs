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
    public class DataActFormaPagoYalo
    {
        public string FormaDePago { get; set; }
        public string OrdenId { get; set; }
        public DataActFormaPagoYalo() { }
        public DataActFormaPagoYalo(string formadepago, string ordenid)
        {
            FormaDePago = formadepago;
            OrdenId = ordenid;
        }
    }
}