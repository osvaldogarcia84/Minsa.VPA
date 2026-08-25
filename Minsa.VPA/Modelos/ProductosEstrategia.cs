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
    public class ProductosEstrategia
    {
        public string ID { get; set; }
        public string NAME { get; set; }
        public string COMPANY { get; set; }
        public string COMPANYNAME { get; set; }
        public ProductosEstrategia() { }
        public ProductosEstrategia(string id, string name, string company, string companyname)
        {
            ID = id;
            NAME = name;
            COMPANY = company;
            COMPANYNAME = companyname;
        }
    }
}