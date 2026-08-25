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

namespace Minsa.VPA.Servicios
{
    public static class Extensiones
    {
        public static bool EsValido(this string valor)
        {
            return !String.IsNullOrEmpty(valor);
        }

        public static bool EsInvalido(this string valor)
        {
            return !valor.EsValido();
        }

        public static void MostrarMensaje(this Context contexto, string texto, ToastLength length = ToastLength.Short)
        {
            Toast.MakeText(contexto, texto, length).Show();
        }

        public static double ObtenerCantidadDouble(this string texto)
        {
            double cantidad;
            Double.TryParse(texto, out cantidad);

            return cantidad;
        }
        public static decimal ObtenerCantidad(this string texto)
        {
            decimal cantidad;
            Decimal.TryParse(texto, out cantidad);

            return cantidad;
        }
        //public static int ObtenerCantidadEntero(this string texto)
        //{
        //    int ObCantidad;
        //    Int16.TryParse(texto, out short ObCantidad);

        //    return ObCantidad;
        //}

        /// <summary>
        /// Gets the value from the context for a given key, the value must be a struct or string
        /// </summary>
        /// <typeparam name="T">Type struct/string/struct[]/string[]</typeparam>
        /// <param name="intent">Context</param>
        /// <param name="key">Object key</param>
        /// <returns></returns>
        public static T Obtener<T>(this Intent intent, string key)
        {
            object value;
            switch (typeof(T).Name)
            {
                case "Int32":
                    value = intent.GetIntExtra(key, 0);
                    break;
                case "String":
                    value = intent.GetStringExtra(key);
                    break;
                case "Boolean":
                    value = intent.GetBooleanExtra(key, false);
                    break;
                case "Float":
                case "Double":
                case "Decimal":
                    value = intent.GetDoubleExtra(key, 0D);
                    break;
                case "Char":
                    value = intent.GetCharExtra(key, ' ');
                    break;
                case "Int64":
                    value = intent.GetLongExtra(key, 0);
                    break;
                case "Int32[]":
                    value = intent.GetIntArrayExtra(key);
                    break;
                case "String[]":
                    value = intent.GetStringArrayExtra(key);
                    break;
                case "Boolean[]":
                    value = intent.GetBooleanArrayExtra(key);
                    break;
                case "Decimal[]":
                    value = intent.GetDoubleArrayExtra(key);
                    break;
                case "Char[]":
                    value = intent.GetCharArrayExtra(key);
                    break;
                case "Int64[]":
                    value = intent.GetLongArrayExtra(key);
                    break;
                default:
                    value = default(T);
                    break;

            }

            return (T)value;
        }

        /// <summary>
        /// Gets the value from the context for a given key, the value must be a struct or string
        /// </summary>
        /// <typeparam name="T">Type struct/string/struct[]/string[]</typeparam>
        /// <param name="bundle">Context</param>
        /// <param name="key">Object key</param>
        /// <returns></returns>
        public static T Obtener<T>(this Bundle bundle, string key)
        {
            object value;
            switch (typeof(T).Name)
            {
                case "Int32":
                    value = bundle.GetInt(key, 0);
                    break;
                case "String":
                    value = bundle.GetString(key);
                    break;
                case "Boolean":
                    value = bundle.GetBoolean(key, false);
                    break;
                case "Decimal":
                    value = bundle.GetDouble(key, 0D);
                    break;
                case "Char":
                    value = bundle.GetChar(key, ' ');
                    break;
                case "Int64":
                    value = bundle.GetLong(key, 0);
                    break;
                case "Int32[]":
                    value = bundle.GetIntArray(key);
                    break;
                case "String[]":
                    value = bundle.GetStringArray(key);
                    break;
                case "Boolean[]":
                    value = bundle.GetBooleanArray(key);
                    break;
                case "Decimal[]":
                    value = bundle.GetDoubleArray(key);
                    break;
                case "Char[]":
                    value = bundle.GetCharArray(key);
                    break;
                case "Int64[]":
                    value = bundle.GetLongArray(key);
                    break;
                default:
                    value = default(T);
                    break;

            }

            return (T)value;
        }
    }
}