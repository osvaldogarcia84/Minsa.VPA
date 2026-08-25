using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Minsa.VPA.Modelos;
using Minsa.VPA.Servicios;
using Newtonsoft.Json;
using RestSharp;

namespace Minsa.VPA.Proveedores
{
    public class ProveedorDePronostico
    {
        ResultadoDeOperacionGenerico<List<ClientePronostico>> resultadoClientePronostico;
        ResultadoDeOperacionGenerico<string> resultadoInsertPronostico;
        public async Task<ResultadoDeOperacionGenerico<List<ClientePronostico>>> ConsultaClientePronostico(DataClientePronostico dataClientePronostico)
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataClientePronostico, Formatting.None);
                var request = new RestRequest("web/ConsultaClientePronostico", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    var data = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<ClientePronostico>>>(response.Content);
                    resultadoClientePronostico = data;
                });

            }
            catch (Exception ex)
            {
                resultadoClientePronostico = new ResultadoDeOperacionGenerico<List<ClientePronostico>>(TipoDeResultado.Fallo, "No se pudo conectar con el servidor intenta nuevamente.", null);
            }

            return resultadoClientePronostico;
        }
        public async Task<ResultadoDeOperacionGenerico<string>> InsertarPronostico(DataInsertPronostico dataClientePronostico)
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataClientePronostico, Formatting.None);
                var request = new RestRequest("web/InsertarPronostico", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    var data = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<string>>(response.Content);
                    resultadoInsertPronostico = data;
                });

            }
            catch (Exception ex)
            {
                resultadoInsertPronostico = new ResultadoDeOperacionGenerico<string>(TipoDeResultado.Fallo, "No se pudo conectar con el servidor intenta nuevamente.", null);
            }

            return resultadoInsertPronostico;
        }
        public async Task<ResultadoDeOperacionGenerico<string>> ActualizaPronostico(DataInsertPronostico dataEditPronostico)
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataEditPronostico, Formatting.None);
                var request = new RestRequest("web/ActualizaPronostico", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    var data = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<string>>(response.Content);
                    resultadoInsertPronostico = data;
                });

            }
            catch (Exception ex)
            {
                resultadoInsertPronostico = new ResultadoDeOperacionGenerico<string>(TipoDeResultado.Fallo, "No se pudo conectar con el servidor intenta nuevamente.", null);
            }

            return resultadoInsertPronostico;
        }

        public async Task<ResultadoDeOperacionGenerico<string>> EliminaPronostico(DataEliminaPronostico dataEliminaPronostico)
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataEliminaPronostico, Formatting.None);
                var request = new RestRequest("web/EliminaPronostico", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    var data = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<string>>(response.Content);
                    resultadoInsertPronostico = data;
                });

            }
            catch (Exception ex)
            {
                resultadoInsertPronostico = new ResultadoDeOperacionGenerico<string>(TipoDeResultado.Fallo, "No se pudo conectar con el servidor intenta nuevamente.", null);
            }

            return resultadoInsertPronostico;
        }
    }
}