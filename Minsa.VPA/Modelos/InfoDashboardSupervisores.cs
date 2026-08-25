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
    public class InfoDashboardSupervisores
    {
        public string Almacen { get; set; }
        public decimal SacoDisponible { get; set; }
        public decimal ImporteDepositar { get; set; }
        public int Visitas { get; set; }
        public int ClientesRuta { get; set; }
        public InfoDashboardSupervisores() { }
        public InfoDashboardSupervisores(string almacen, decimal sacodisponible, decimal importedepositar, int visitas, int clientesruta)
        {
            Almacen = almacen;
            SacoDisponible = sacodisponible;
            ImporteDepositar = importedepositar;
            Visitas = visitas;
            ClientesRuta = clientesruta;
        }
    }
}