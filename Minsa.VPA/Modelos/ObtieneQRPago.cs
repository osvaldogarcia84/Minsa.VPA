using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minsa.VPA.Modelos
{
    public class ObtieneQRPago
    {
        [JsonProperty("folioPagoMinsa")]
        public int FolioPagoMinsa { get; set; }

        [JsonProperty("jsonResposeCODI")]
        public JToken JsonResposeCODI { get; set; }

        [JsonProperty("mensaje")]
        public string Mensaje { get; set; }

        [JsonIgnore]
        public bool JsonCodiEsObjeto
        {
            get
            {
                if (JsonResposeCODI == null)
                    return false;

                if (JsonResposeCODI.Type == JTokenType.Object)
                    return true;

                if (JsonResposeCODI.Type == JTokenType.String)
                {
                    string contenido = JsonResposeCODI.ToString();

                    if (string.IsNullOrWhiteSpace(contenido))
                        return false;

                    contenido = contenido.Trim();

                    return contenido.StartsWith("{") &&
                           contenido.EndsWith("}");
                }

                return false;
            }
        }

        [JsonIgnore]
        public bool JsonCodiEsError
        {
            get
            {
                if (JsonResposeCODI == null)
                    return true;

                if (JsonResposeCODI.Type != JTokenType.String)
                    return false;

                string contenido = JsonResposeCODI.ToString();

                if (string.IsNullOrWhiteSpace(contenido))
                    return true;

                contenido = contenido.Trim();

                // Si empieza como objeto JSON, no es un mensaje de error.
                return !contenido.StartsWith("{");
            }
        }

        public jsonResponseCODI ObtenerRespuestaCodi()
        {
            if (JsonResposeCODI == null)
                return null;

            try
            {
                /*
                 * Caso 1:
                 * "jsonResposeCODI": { ... }
                 */
                if (JsonResposeCODI.Type == JTokenType.Object)
                {
                    return JsonResposeCODI.ToObject<jsonResponseCODI>();
                }

                /*
                 * Caso 2:
                 * "jsonResposeCODI": "{\"returnCodes\":{...}}"
                 */
                if (JsonResposeCODI.Type == JTokenType.String)
                {
                    string jsonInterno =
                        JsonResposeCODI.Value<string>();

                    if (string.IsNullOrWhiteSpace(jsonInterno))
                        return null;

                    jsonInterno = jsonInterno.Trim();

                    if (!jsonInterno.StartsWith("{"))
                        return null;

                    return JsonConvert
                        .DeserializeObject<jsonResponseCODI>(
                            jsonInterno);
                }

                return null;
            }
            catch (JsonException ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    "Error al convertir jsonResposeCODI: " +
                    ex.ToString());

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    "Error inesperado al convertir CODI: " +
                    ex.ToString());

                return null;
            }
        }

        public string ObtenerErrorCodi()
        {
            if (JsonResposeCODI == null)
                return "La respuesta CODI está vacía.";

            if (JsonResposeCODI.Type != JTokenType.String)
                return null;

            string contenido =
                JsonResposeCODI.Value<string>();

            if (string.IsNullOrWhiteSpace(contenido))
                return "La respuesta CODI está vacía.";

            contenido = contenido.Trim();

            // Es un JSON serializado como string, no un error.
            if (contenido.StartsWith("{"))
                return null;

            return contenido;
        }
    }
}