using Minsa.VPA.Modelos;
using Minsa.VPA.Servicios;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Minsa.VPA.Proveedores
{
    public class ProveedorDeVenta
    {
        ResultadoDeOperacionGenerico<string> resultadoCrearOrden;
        ResultadoDeOperacionGenerico<string> resultadoCrearLinea;
        ResultadoDeOperacionGenerico<string> resultadoFactura;
        private const string RutaObtenerQr =
           "https://taak.minsa.com.mx/codi/pagos/obtieneQRPago";
        public async Task<ResultadoDeOperacionGenerico<string>> CrearOrdenVPA(Orden orden)
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(orden, Formatting.None);
                var request = new RestRequest("web/CrearOrdenVPA", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    var data = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<string>>(response.Content);
                    resultadoCrearOrden = data;
                });

            }
            catch (Exception ex)
            {
                resultadoCrearOrden = new ResultadoDeOperacionGenerico<string>(TipoDeResultado.Fallo, "Ocurrio un error: " + ex.Message, null);
            }
            return resultadoCrearOrden;

        }

        public async Task<ResultadoDeOperacionGenerico<string>> CrearLineaVPA(Linea linea)
        {

            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(linea, Formatting.None);
            var request = new RestRequest("web/CrearLineaVPA", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
            await Task.Run(() =>
            {
                var response = Cliente.Execute(request);
                var data = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<string>>(response.Content);
                resultadoCrearLinea = data;
            });

            return resultadoCrearLinea;
        }
        public async Task<ResultadoDeOperacionGenerico<string>> ObtenerFactura(Factura factura)
        {
            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(factura, Formatting.None);
            var request = new RestRequest("web/FacturaVPA", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
            await Task.Run(() =>
            {
                var response = Cliente.Execute(request);
                var data = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<string>>(response.Content);
                resultadoFactura = data;
            });
            return resultadoFactura;
        }



        public async Task<ResultadoDeOperacionGenerico<ObtieneQRPago>>
       SolicitarQrCodi(DataReferenciaPagoCODI datos)
        {
            if (datos == null)
            {
                return new ResultadoDeOperacionGenerico<ObtieneQRPago>(
                    TipoDeResultado.Error,
                    "No se recibieron los datos para generar el QR.",
                    null);
            }

            try
            {
                string usuario = Configuracion.UsuarioCODI;
                string password = Configuracion.PasswordCODI;

                if (string.IsNullOrWhiteSpace(usuario))
                {
                    return new ResultadoDeOperacionGenerico<ObtieneQRPago>(
                        TipoDeResultado.Error,
                        "El usuario CODI no está configurado.",
                        null);
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    return new ResultadoDeOperacionGenerico<ObtieneQRPago>(
                        TipoDeResultado.Error,
                        "La contraseña CODI no está configurada.",
                        null);
                }

                string credenciales = usuario + ":" + password;

                string credencialesBase64 =
                    Convert.ToBase64String(
                        Encoding.UTF8.GetBytes(credenciales));

                using (var handler = CrearHttpClientHandler())
                using (var cliente = new HttpClient(handler))
                {
                    cliente.Timeout = TimeSpan.FromSeconds(120);

                    cliente.DefaultRequestHeaders.Clear();

                    cliente.DefaultRequestHeaders.TryAddWithoutValidation(
                        "Authorization",
                        "Basic " + credencialesBase64);

                    cliente.DefaultRequestHeaders.Accept.Clear();

                    cliente.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    string jsonSolicitud =
                        JsonConvert.SerializeObject(
                            datos,
                            Formatting.None);

                    System.Diagnostics.Debug.WriteLine("=========================================");
                    System.Diagnostics.Debug.WriteLine("URL:");
                    System.Diagnostics.Debug.WriteLine(RutaObtenerQr);

                    System.Diagnostics.Debug.WriteLine("USUARIO:");
                    System.Diagnostics.Debug.WriteLine(usuario);

                    System.Diagnostics.Debug.WriteLine("PASSWORD:");
                    System.Diagnostics.Debug.WriteLine(password);

                    System.Diagnostics.Debug.WriteLine("CREDENCIALES:");
                    System.Diagnostics.Debug.WriteLine(credenciales);

                    System.Diagnostics.Debug.WriteLine("BASE64:");
                    System.Diagnostics.Debug.WriteLine(credencialesBase64);

                    System.Diagnostics.Debug.WriteLine("HEADER AUTHORIZATION:");
                    System.Diagnostics.Debug.WriteLine("Basic " + credencialesBase64);

                    System.Diagnostics.Debug.WriteLine("JSON ENVIADO:");
                    System.Diagnostics.Debug.WriteLine(jsonSolicitud);
                    System.Diagnostics.Debug.WriteLine("=========================================");

                    using (var contenido = new StringContent(
                        jsonSolicitud,
                        Encoding.UTF8,
                        "application/json"))
                    {
                        HttpResponseMessage response =
                            await cliente.PostAsync(
                                RutaObtenerQr,
                                contenido);

                        string respuestaTexto =
                            await response.Content.ReadAsStringAsync();

                        System.Diagnostics.Debug.WriteLine("========== RESPUESTA ==========");
                        System.Diagnostics.Debug.WriteLine("HTTP: " +
                            (int)response.StatusCode +
                            " - " +
                            response.ReasonPhrase);

                        System.Diagnostics.Debug.WriteLine("Content-Type:");
                        System.Diagnostics.Debug.WriteLine(
                            response.Content.Headers.ContentType);

                        System.Diagnostics.Debug.WriteLine("Headers:");

                        foreach (var h in response.Headers)
                        {
                            System.Diagnostics.Debug.WriteLine(
                                h.Key + " = " +
                                string.Join(",", h.Value));
                        }

                        System.Diagnostics.Debug.WriteLine("Body:");
                        System.Diagnostics.Debug.WriteLine(respuestaTexto);

                        System.Diagnostics.Debug.WriteLine("===============================");

                        if (EsRedireccion(response.StatusCode))
                        {
                            string location =
                                response.Headers.Location != null
                                    ? response.Headers.Location.ToString()
                                    : "No especificada";

                            return new ResultadoDeOperacionGenerico<ObtieneQRPago>(
                                TipoDeResultado.Error,
                                "El servicio redireccionó la petición. Destino: " +
                                location,
                                null);
                        }

                        if (response.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            string autenticacion =
                                ObtenerWwwAuthenticate(response);

                            return new ResultadoDeOperacionGenerico<ObtieneQRPago>(
                                TipoDeResultado.Error,
                                "Credenciales inválidas. WWW-Authenticate: " +
                                autenticacion,
                                null);
                        }

                        if (string.IsNullOrWhiteSpace(respuestaTexto))
                        {
                            return new ResultadoDeOperacionGenerico<ObtieneQRPago>(
                                TipoDeResultado.Error,
                                "El servicio regresó una respuesta vacía.",
                                null);
                        }

                        if (!response.IsSuccessStatusCode)
                        {
                            return new ResultadoDeOperacionGenerico<ObtieneQRPago>(
                                TipoDeResultado.Error,
                                ConstruirMensajeHttp(
                                    response,
                                    respuestaTexto),
                                null);
                        }

                        ObtieneQRPago resultado =
                            DeserializarRespuesta(respuestaTexto);

                        if (resultado == null)
                        {
                            return new ResultadoDeOperacionGenerico<ObtieneQRPago>(
                                TipoDeResultado.Error,
                                "No fue posible convertir la respuesta.",
                                null);
                        }

                        if (resultado.JsonCodiEsError)
                        {
                            return new ResultadoDeOperacionGenerico<ObtieneQRPago>(
                                TipoDeResultado.Error,
                                "CODI regresó un error: " +
                                resultado.ObtenerErrorCodi(),
                                resultado);
                        }

                        if (!resultado.JsonCodiEsObjeto)
                        {
                            return new ResultadoDeOperacionGenerico<ObtieneQRPago>(
                                TipoDeResultado.Error,
                                "La respuesta no contiene jsonResposeCODI.",
                                resultado);
                        }

                        jsonResponseCODI respuesta =
                            resultado.ObtenerRespuestaCodi();

                        if (respuesta == null)
                        {
                            return new ResultadoDeOperacionGenerico<ObtieneQRPago>(
                                TipoDeResultado.Error,
                                "No fue posible convertir jsonResposeCODI.",
                                resultado);
                        }

                        return new ResultadoDeOperacionGenerico<ObtieneQRPago>(
                            TipoDeResultado.Exito,
                            string.IsNullOrWhiteSpace(resultado.Mensaje)
                                ? "Consulta CODI correcta."
                                : resultado.Mensaje,
                            resultado);
                    }
                }
            }
            catch (JsonReaderException ex)
            {
                return new ResultadoDeOperacionGenerico<ObtieneQRPago>(
                    TipoDeResultado.Error,
                    "JSON inválido: " + ex.Message,
                    null);
            }
            catch (JsonSerializationException ex)
            {
                return new ResultadoDeOperacionGenerico<ObtieneQRPago>(
                    TipoDeResultado.Error,
                    "Error de serialización: " + ex.Message,
                    null);
            }
            catch (TaskCanceledException)
            {
                return new ResultadoDeOperacionGenerico<ObtieneQRPago>(
                    TipoDeResultado.Fallo,
                    "Tiempo de espera agotado.",
                    null);
            }
            catch (HttpRequestException ex)
            {
                return new ResultadoDeOperacionGenerico<ObtieneQRPago>(
                    TipoDeResultado.Fallo,
                    "Error HTTP: " + ObtenerMensajeExcepcion(ex),
                    null);
            }
            catch (Exception ex)
            {
                return new ResultadoDeOperacionGenerico<ObtieneQRPago>(
                    TipoDeResultado.Fallo,
                    "Error: " + ObtenerMensajeExcepcion(ex),
                    null);
            }
        }

        private static HttpClientHandler CrearHttpClientHandler()
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                UseDefaultCredentials = false,
                UseProxy = false
            };

            /*
             * Solo temporal mientras se corrige el certificado de IIS.
             * En producción debe eliminarse y utilizar un certificado válido.
             */
            handler.ServerCertificateCustomValidationCallback =
                (request, certificate, chain, errors) => true;

            return handler;
        }

        private static ObtieneQRPago DeserializarRespuesta(
            string respuestaTexto)
        {
            JObject jsonRaiz = JObject.Parse(respuestaTexto);

            JToken tokenRespuestaCodi =
                jsonRaiz["jsonResposeCODI"];

            /*
             * Se conserva el JToken porque el API puede enviar:
             *
             * "jsonResposeCODI": { ... }
             *
             * o:
             *
             * "jsonResposeCODI":
             * "Response status code does not indicate success: 400."
             */
            return jsonRaiz.ToObject<ObtieneQRPago>();
        }

        private static bool EsRedireccion(
            HttpStatusCode statusCode)
        {
            int codigo = (int)statusCode;

            return codigo == 301 ||
                   codigo == 302 ||
                   codigo == 303 ||
                   codigo == 307 ||
                   codigo == 308;
        }

        private static string ObtenerWwwAuthenticate(
            HttpResponseMessage response)
        {
            if (response.Headers.WwwAuthenticate == null)
                return "No especificado";

            string valor = string.Join(
                " | ",
                response.Headers.WwwAuthenticate
                    .Select(x => x.ToString()));

            return string.IsNullOrWhiteSpace(valor)
                ? "No especificado"
                : valor;
        }

        private static string ConstruirMensajeHttp(
            HttpResponseMessage response,
            string contenidoRespuesta)
        {
            return "Error HTTP " +
                   (int)response.StatusCode +
                   " - " +
                   response.ReasonPhrase +
                   ". Respuesta: " +
                   contenidoRespuesta;
        }

        private static string ObtenerMensajeExcepcion(
            Exception ex)
        {
            if (ex == null)
                return "Error no identificado.";

            var mensajes = new StringBuilder();

            Exception excepcionActual = ex;

            while (excepcionActual != null)
            {
                if (mensajes.Length > 0)
                    mensajes.Append(" | InnerException: ");

                mensajes.Append(excepcionActual.Message);

                excepcionActual =
                    excepcionActual.InnerException;
            }

            return mensajes.ToString();
        }
    }
}


