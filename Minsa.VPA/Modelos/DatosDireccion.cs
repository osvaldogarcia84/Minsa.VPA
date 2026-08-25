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
    public class DatosDireccion
    {
        public string COUNTRYREGIONID { get; set; }
        public string STATEID { get; set; }
        public string COUNTYID { get; set; }
        public DatosDireccion() { }
        public DatosDireccion(string countryregionid, string stateid, string countyid)
        {
            COUNTRYREGIONID = countryregionid;
            STATEID = stateid;
            COUNTYID = countyid;
        }
    }
}