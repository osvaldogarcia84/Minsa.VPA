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
    public static class ProveedorGlobal
    {
        public const string InformacionOrdenId = "OrdenId";
        public static Cliente Cliente { get; set; }
        public static ClientePronostico  ClientePronostico {get; set;}
        public static DashboardCXC ClienteCXC { get; set; }
        public static List<ClientePronostico> ListClientePronostico { get; set; }
        public static List<Cliente> ClientesFlujo { get; set; }
        public static List<OrdenesYalo> OrdenesYalo { get; set; }
        public static ClientesProspecto ClientesProspecto { get; set; }
        public static Vendedor Vendedor { get; set; }
        public static GeoLocacion GeoLocacion { get; set; }
        public static Lan Lan { get; set; }
        public static int? PosicionCliente { get; set; }
        public static int? PosicionClientePronostico { get; set; }
        public static int? PosicionClienteCXC { get; set; }
        public static int? PosicionRecibo { get; set; }
        public static List<ClienteMovil> ClienteMovil { get; set; }

        public static string Conexion;
        public static string OrdenId { get; set; }
        public static decimal DescuentoTotalView { get; set; }
        public static decimal DescuentoXOrden { get; set; }

        private static readonly Dictionary<string, string> OrdenPorCliente = new Dictionary<string, string>();

        public static string ObtenerOrdenDeCliente()
        {
            return OrdenPorCliente.FirstOrDefault(e => e.Key == Cliente.ClienteId).Value;
        }

        public static void AsignarOrdenDeCliente(string id)
        {
            OrdenPorCliente.Remove(Cliente.ClienteId);
            OrdenPorCliente.Add(Cliente.ClienteId, id);
        }
    }
}