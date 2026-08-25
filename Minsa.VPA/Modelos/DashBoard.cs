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
    public class DashBoard
    {
        public decimal SacoDisponible { get; set; }
        public decimal ImporteDepositar { get; set; }
        public int Visitas { get; set; }
        public int ClientesRuta { get; set; }
        public DashBoard() { }
        public DashBoard(decimal sacodisponible, decimal importedespositar, int visitas, int clientesruta)
        {
            SacoDisponible = sacodisponible;
            ImporteDepositar = importedespositar;
            Visitas = visitas;
            ClientesRuta = clientesruta;
        }
    }
}