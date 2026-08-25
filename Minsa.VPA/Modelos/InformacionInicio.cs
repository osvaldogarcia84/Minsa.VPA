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
    public class InformacionInicio
    {
        public int Id { get; set; }
        public string NombreVendedor { get; set; }
        public string Codigo { get; set; }
        public string Almacen { get; set; }
        public int ClientesxDia { get; set; }
        public int VisitasHechas { get; set; }
        public decimal InventarioAlDia { get; set; }
        public decimal TotalTransacciones { get; set; }

        public InformacionInicio() { }
        public InformacionInicio(int id, string nombreVendedor, string codigo, string almacen, int clientesxdia,
                                int visitashechas, decimal inventatioaldia,
                                decimal totaltransacciones)
        {
            Id = id;
            NombreVendedor = nombreVendedor;
            Codigo = codigo;
            Almacen = almacen;
            ClientesxDia = clientesxdia;
            VisitasHechas = visitashechas;
            InventarioAlDia = inventatioaldia;
            TotalTransacciones = totaltransacciones;
        }
    }
}