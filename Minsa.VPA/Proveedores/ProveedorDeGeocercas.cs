using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Minsa.VPA.Modelos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Minsa.VPA.Proveedores
{
    public class ProveedorDeGeocercas
    {
        private const float RADIO_GEOCERCA_METROS = 120;
        private const float RADIO_VALIDACION_METROS = 60;

        /// <summary>
        /// Convierte un Cliente en ClienteGeocerca.
        ///
        /// IMPORTANTE:
        /// Se está considerando provisionalmente:
        ///
        /// IM_COORDENADAS_X = Longitud
        /// IM_COORDENADAS_Y = Latitud
        ///
        /// Esto debe verificarse con datos reales.
        /// </summary>
        public ClienteGeocerca ConvertirCliente(Cliente cliente)
        {
            if (cliente == null)
                return null;

            double longitud;
            double latitud;

            if (!ObtenerCoordenada(
                cliente.IM_COORDENADAS_X,
                out longitud))
            {
                return null;
            }

            if (!ObtenerCoordenada(
                cliente.IM_COORDENADAS_Y,
                out latitud))
            {
                return null;
            }

            // Validación geográfica.
            if (latitud < -90 || latitud > 90)
                return null;

            if (longitud < -180 || longitud > 180)
                return null;

            // Coordenada 0,0 no es válida para nuestro escenario.
            if (latitud == 0 && longitud == 0)
                return null;

            return new ClienteGeocerca
            {
                ClienteId = cliente.ClienteId,
                Nombre = cliente.Nombre,

                Latitud = latitud,
                Longitud = longitud,

                Secuencia = cliente.Secuencia,

                RadioGeocercaMetros =
                    RADIO_GEOCERCA_METROS,

                RadioValidacionMetros =
                    RADIO_VALIDACION_METROS
            };
        }

        /// <summary>
        /// Obtiene solamente los clientes que tienen
        /// coordenadas válidas para registrar como geocercas.
        /// </summary>
        public List<ClienteGeocerca> ObtenerClientesValidos(
            IEnumerable<Cliente> clientes)
        {
            var resultado =
                new List<ClienteGeocerca>();

            if (clientes == null)
                return resultado;

            foreach (Cliente cliente in clientes)
            {
                ClienteGeocerca geocerca =
                    ConvertirCliente(cliente);

                if (geocerca != null)
                {
                    resultado.Add(geocerca);
                }
            }

            return resultado
                .OrderBy(x => x.Secuencia)
                .ToList();
        }

        /// <summary>
        /// Obtiene clientes que NO pueden utilizar geofencing
        /// porque no cuentan con coordenadas válidas.
        ///
        /// Estos clientes utilizarán posteriormente
        /// el fallback manual.
        /// </summary>
        public List<Cliente> ObtenerClientesSinCoordenadas(
            IEnumerable<Cliente> clientes)
        {
            var resultado =
                new List<Cliente>();

            if (clientes == null)
                return resultado;

            foreach (Cliente cliente in clientes)
            {
                if (ConvertirCliente(cliente) == null)
                {
                    resultado.Add(cliente);
                }
            }

            return resultado;
        }

        private bool ObtenerCoordenada(
            string valor,
            out double coordenada)
        {
            coordenada = 0;

            if (string.IsNullOrWhiteSpace(valor))
                return false;

            valor = valor.Trim();

            /*
             * Primero intentamos InvariantCulture:
             * -99.123456
             *
             * Después CurrentCulture por compatibilidad
             * con configuraciones regionales del dispositivo.
             */

            if (double.TryParse(
                valor,
                NumberStyles.Float |
                NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,
                out coordenada))
            {
                return true;
            }

            return double.TryParse(
                valor,
                NumberStyles.Float |
                NumberStyles.AllowLeadingSign,
                CultureInfo.CurrentCulture,
                out coordenada);
        }
    }
}