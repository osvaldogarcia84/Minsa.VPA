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
    public class TiposSocios
    {
        public string IM_TIPO_SOCIO { get; set; }
        public string IM_NOMBRE_SOCIO { get; set; }
        public TiposSocios() { }
        public TiposSocios(string im_tipo_socio, string im_nombre_socio)
        {
            IM_TIPO_SOCIO = im_tipo_socio;
            IM_NOMBRE_SOCIO = IM_NOMBRE_SOCIO;
        }
    }
}