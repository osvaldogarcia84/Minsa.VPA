using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minsa.VPA.Modelos
{
    public class DataReferenciaPagoCODI
    {
        [JsonProperty("referencia")]
        public string referencia { get; set;}
        [JsonProperty("importe")]
        public decimal importe { get; set; }
        [JsonProperty("fuenteSolicita")]
        public string fuenteSolicita { get; set; }
        public DataReferenciaPagoCODI() { }
        public DataReferenciaPagoCODI(string _referencia, decimal _importe, string _fuenteSolicita) 
        {
            referencia = _referencia;
            importe = _importe;
            fuenteSolicita = _fuenteSolicita;
        }
    }
}