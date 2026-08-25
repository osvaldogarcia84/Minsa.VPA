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
using ServiceStack.Text;

namespace Minsa.VPA.Proveedores
{
    public static class ProveedorDeManipulacionDeDatos
    {
        public static T Obtener<T>(string data)
        {
            return JsonSerializer.DeserializeFromString<T>(data);
        }

        public static IEnumerable<T> ObtenerTodos<T>(IEnumerable<string> data)
        {
            if (data == null)
                return null;
            return data.Select(JsonSerializer.DeserializeFromString<T>);
        }

        public static string Generar<T>(T data)
        {
            return JsonSerializer.SerializeToString(data);
        }

        public static string[] GenerarTodos<T>(IEnumerable<T> data)
        {
            return data.Select(JsonSerializer.SerializeToString).ToArray();
        }

        public static List<T> ConvenrtirLista<T>(this IEnumerable<T> elementos) where T : class
        {
            return elementos as List<T> ?? elementos.ToList();
        }

    }
}