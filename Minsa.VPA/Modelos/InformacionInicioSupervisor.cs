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
    public class InformacionInicioSupervisor
    {
        public int Id { get; set; }
        public string NombreVendedor { get; set; }
        public string Usuario { get; set; }
        public string Almacen { get; set; }
        public int ClientesxDia { get; set; }
        public int VisitasHechas { get; set; }
        public decimal InventarioAlDia { get; set; }
        public decimal TotalTransacciones { get; set; }

        public InformacionInicioSupervisor() { }
        public InformacionInicioSupervisor(int id, string nombreVendedor, string usuario, string almacen, int clientesxdia,
                                int visitashechas, decimal inventatioaldia,
                                decimal totaltransacciones)
        {
            Id = id;
            NombreVendedor = nombreVendedor;
            Usuario = usuario;
            Almacen = almacen;
            ClientesxDia = clientesxdia;
            VisitasHechas = visitashechas;
            InventarioAlDia = inventatioaldia;
            TotalTransacciones = totaltransacciones;
        }
    }
}