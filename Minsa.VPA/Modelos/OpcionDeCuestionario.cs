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
    public class OpcionDeCuestionario
    {
        public OpcionesDeDetalleDeCuestionario Id { get; set; }
        public string Nombre { get; set; }

        public OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }

        // TODO Arregalar en DB
        public static List<OpcionDeCuestionario> ObtenerOpciones(OpcionesDeCuestionario opcion)
        {
            switch (opcion)
            {
                case OpcionesDeCuestionario.Compra:
                    return new List<OpcionDeCuestionario>
                    {
                        new OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario.SiCompra, "Sí, comprará"),
                        new OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario.SiNoCompra, "Sí, no comprará"),
                        new OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario.NoCompra, "No, no comprará")
                    };
                case OpcionesDeCuestionario.Convencido:
                    return new List<OpcionDeCuestionario>
                    {
                        new OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario.Si, "Sí"),
                        new OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario.No, "No")
                    };
                case OpcionesDeCuestionario.Estado:
                    return new List<OpcionDeCuestionario>
                    {
                        new OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario.NoCambiaStatus, "No Cambia Status"),
                        new OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario.Perdido, "Perdido"),
                        new OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario.Activo, "Activo"),
                        new OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario.Prospecto, "Prospecto")
                    };
                case OpcionesDeCuestionario.Comparacion:
                    return new List<OpcionDeCuestionario>
                    {
                        new OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario.Mejor, "Si cumple expectativas"),
                        new OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario.Igual, "Igual"),
                        new OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario.Peor, "No cumple expectativas")
                    };
                case OpcionesDeCuestionario.Cuestionario:
                    return new List<OpcionDeCuestionario>
                    {
                        new OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario.Harinizacion, "Harinización"),
                        new OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario.Degustacion, "Degustación")
                    };
                case OpcionesDeCuestionario.Nixtamalero:
                    return new List<OpcionDeCuestionario>
                    {
                        new OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario.Comprador, "Comprador de masa"),
                        new OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario.Molinero, "Molinero")
                    };
                case OpcionesDeCuestionario.Motivo:
                    return new List<OpcionDeCuestionario>
                    {
                        new OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario.CumpleExpectativas,
                            "Cumple expectativas"),
                        new OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario.NoCumpleExpectativas,
                            "No cumple expectativas")
                    };
                case OpcionesDeCuestionario.Venta:
                    return new List<OpcionDeCuestionario>
                    {
                        new OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario.VentaSiCompra, "Si va a comprar"),
                        new OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario.VentaNoCompra, "No va a comprar"),
                        new OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario.VentaNoDisminucion,
                            "No va a comprar (Disminución)")
                    };
                case OpcionesDeCuestionario.HarinaFavor:
                    return new List<OpcionDeCuestionario>
                    {
                        new OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario.AFavor, "A favor"),
                        new OpcionDeCuestionario(OpcionesDeDetalleDeCuestionario.EnContra, "En contra")
                    };
                default:
                    return null;
            }
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
    public enum OpcionesDeCuestionario
    {
        Compra = 1,
        Convencido = 2,
        Estado = 3,
        Comparacion = 4,
        Cuestionario = 5,
        Nixtamalero = 6,
        Motivo = 7,
        Venta = 8,
        HarinaFavor = 10
    }

    public enum OpcionesDeDetalleDeCuestionario
    {
        Mejor = 1,
        Igual = 2,
        Peor = 3,
        SiCompra = 4,
        SiNoCompra = 5,
        NoCompra = 6,
        Estado1 = 7,
        Estado2 = 8,
        CumpleExpectativas = 9,
        NoCumpleExpectativas = 11,
        Harinizacion = 12,
        Degustacion = 13,
        Comprador = 14,
        Molinero = 15,
        Si = 16,
        No = 17,
        VentaSiCompra = 18,
        VentaNoCompra = 20,
        VentaNoDisminucion = 21,
        AFavor = 22,
        EnContra = 23,
        NoCambiaStatus = 26,
        Perdido = 27,
        Activo = 28,
        Prospecto = 29
    }
}