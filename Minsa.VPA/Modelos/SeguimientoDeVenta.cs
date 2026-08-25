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
    public class SeguimientoDeVenta
    {
      
        //public int Id { get; set; }
      
        public string ClienteId { get; set; }
      
        public int CompetenciaId { get; set; }
      
        public int RecursoId { get; set; }            
      
        public int CuestionarioId { get; set; }      
        public SeguimientoDeVenta() { }

        public SeguimientoDeVenta(string clienteId, int competenciaId, int recursoId,  int cuestionarioId)
        {
            //Id = id;
            ClienteId = clienteId;            
            CompetenciaId = competenciaId;
            RecursoId = recursoId;
            CuestionarioId = cuestionarioId;
        }
    }
}