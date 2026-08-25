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
    public class Degustacion
    {
       
        public int Id { get; set; }
       
        public string ClienteId { get; set; }
        public string OVFActura { get; set; }
       
        public string ClienteTelefono { get; set; }
       
        public OpcionesDeDetalleDeCuestionario ClienteEstado { get; set; }
       
        public OpcionesDeDetalleDeCuestionario ClienteConvencido { get; set; }
       
        public decimal PotencialHarina { get; set; }
       
        public decimal PotencialMaiz { get; set; }
       
        public decimal PotencialMasa { get; set; }
       
        public decimal HarinaSacosUsados { get; set; }
       
        public decimal HarinaKgUsados { get; set; }
       
        public decimal HarinaRendimientoMasa { get; set; }
        
        public decimal HarinaRendimientoTortilla { get; set; }
       
        public decimal HarinaRendimientoTortillaCompetencia { get; set; }
       
        public decimal HarinaDeshidratacionMasa { get; set; }
       
        public OpcionesDeDetalleDeCuestionario TortillaClienteRed { get; set; }
       
        public OpcionesDeDetalleDeCuestionario TortillaTextura { get; set; }
       
        public OpcionesDeDetalleDeCuestionario TortillaColor { get; set; }
       
        public OpcionesDeDetalleDeCuestionario TortillaAroma { get; set; }
       
        public OpcionesDeDetalleDeCuestionario TortillaDuracion { get; set; }
       
        public OpcionesDeDetalleDeCuestionario TortillaCorrea { get; set; }
       
        public OpcionesDeDetalleDeCuestionario TortillaSabor { get; set; }
       
        public string MinsaRecursoId { get; set; }
       
        public int CompetenciaRecursoId { get; set; }
       
        public string VendedorId { get; set; }
        public int IdCausa { get; set; }      
        public string Pregunta { get; set; }      
        public string Respuesta { get; set; }

        public Degustacion()
        {

        }

        public Degustacion(int id,
                           string clienteId,
                           string ovfactura,
                           string clienteTelefono,
                           OpcionesDeDetalleDeCuestionario clienteEstado,
                           OpcionesDeDetalleDeCuestionario clienteConvencido,
                           decimal potencialHarina,
                           decimal potencialMaiz,
                           decimal potencialMasa,
                           decimal harinaSacosUsados,
                           decimal harinaKgUsados,
                           decimal harinaRendimientoMasa,
                           decimal harinaRendimientoTortilla,
                           decimal harinaRendimientoTortillaCompetencia,
                           decimal harinaDeshidratacionMasa,
                           OpcionesDeDetalleDeCuestionario tortillaClienteRed,
                           OpcionesDeDetalleDeCuestionario tortillaTextura,
                           OpcionesDeDetalleDeCuestionario tortillaColor,
                           OpcionesDeDetalleDeCuestionario tortillaAroma,
                           OpcionesDeDetalleDeCuestionario tortillaDuracion,
                           OpcionesDeDetalleDeCuestionario tortillaCorrea,
                           OpcionesDeDetalleDeCuestionario tortillaSabor,
                            string minsaRecursoId,
                            int competenciaRecursoId,
                            string vendedorid,
                            int idcausa,
                            string pregunta,
                            string respuesta
            )
        {
            Id = id;
            ClienteId = clienteId;
            OVFActura = ovfactura;
            ClienteTelefono = clienteTelefono;
            ClienteEstado = clienteEstado;
            ClienteConvencido = clienteConvencido;
            PotencialHarina = potencialHarina;
            PotencialMaiz = potencialMaiz;
            PotencialMasa = potencialMasa;
            HarinaSacosUsados = harinaSacosUsados;
            HarinaKgUsados = harinaKgUsados;
            HarinaRendimientoMasa = harinaRendimientoMasa;
            HarinaRendimientoTortilla = harinaRendimientoTortilla;
            HarinaRendimientoTortillaCompetencia = harinaRendimientoTortillaCompetencia;
            HarinaDeshidratacionMasa = harinaDeshidratacionMasa;
            TortillaClienteRed = tortillaClienteRed;
            TortillaTextura = tortillaTextura;
            TortillaColor = tortillaColor;
            TortillaAroma = tortillaAroma;
            TortillaDuracion = tortillaDuracion;
            TortillaCorrea = tortillaCorrea;
            TortillaSabor = tortillaSabor;
            MinsaRecursoId = minsaRecursoId;
            CompetenciaRecursoId = competenciaRecursoId;
            VendedorId = vendedorid;
            IdCausa = idcausa;
            Pregunta = pregunta;
            Respuesta = respuesta;
        }
    }
}