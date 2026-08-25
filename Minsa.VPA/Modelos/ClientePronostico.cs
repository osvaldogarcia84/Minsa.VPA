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
    public class ClientePronostico
    {
        public string Cliente { get; set; }
        public string NombreCte { get; set; }
        public string Fecha { get; set; }
        public string IdRecurso { get; set; }
        public string NombreRecurso { get; set; }
        public string Volumen { get; set; }
        public Int64 RefRecid { get; set; }
        public ClientePronostico() { }
        public ClientePronostico(string cliente, string nombrecte, string fecha, string idrecurso, string nombrerecurso, string volumen, Int64 refrecid)
        {
            Cliente = cliente;
            NombreCte = nombrecte;
            Fecha = fecha;
            IdRecurso = idrecurso;
            NombreRecurso = nombrerecurso;
            Volumen = volumen;
            RefRecid = refrecid;
        }

    }
}