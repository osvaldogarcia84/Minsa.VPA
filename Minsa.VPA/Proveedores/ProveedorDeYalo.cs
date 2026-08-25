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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minsa.VPA.Proveedores
{
    public class ProveedorDeYalo
    {
        ResultadoDeOperacionGenerico<List<OrdenesYalo>> resultadoOrdenes;
        ResultadoDeOperacionGenerico<string> resultadoActualizarOrden;
        ResultadoDeOperacionGenerico<string> resultadoLiberaLineas;
        public async Task<ResultadoDeOperacionGenerico<List<OrdenesYalo>>> OrdenesYalo(DataOrdenesYalo dataOrdenesYalo)
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataOrdenesYalo, Formatting.None);
                var request = new RestRequest("web/OrdenesYalo", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    var data = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<OrdenesYalo>>>(response.Content);
                    resultadoOrdenes = data;
                });

            }
            catch (Exception ex)
            {
                resultadoOrdenes = new ResultadoDeOperacionGenerico<List<OrdenesYalo>>(TipoDeResultado.Fallo, "No se pudo conectar con el servidor intenta nuevamente.", null);
            }

            return resultadoOrdenes;
        }
        public async Task<ResultadoDeOperacionGenerico<string>> ActualizaFormaPagoYalo(DataActFormaPagoYalo dataActFormaPagoYalo)
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataActFormaPagoYalo, Formatting.None);
                var request = new RestRequest("web/ActualizaFormaPagoYalo", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    var data = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<string>>(response.Content);
                    resultadoActualizarOrden = data;
                });

            }
            catch (Exception ex)
            {
                resultadoActualizarOrden = new ResultadoDeOperacionGenerico<string>(TipoDeResultado.Fallo, "No se pudo conectar con el servidor intenta nuevamente.", null);
            }

            return resultadoActualizarOrden;
        }
        public async Task<ResultadoDeOperacionGenerico<string>> LiberaLineasPedido(DataLiberaLineas dataLiberaLineas)
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataLiberaLineas, Formatting.None);
                var request = new RestRequest("web/LiberaLineasPedido", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoLiberaLineas = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<string>>(response.Content);                    
                });

            }
            catch (Exception ex)
            {
                resultadoLiberaLineas = new ResultadoDeOperacionGenerico<string>(TipoDeResultado.Fallo, "No se pudo conectar con el servidor intenta nuevamente.", null);
            }

            return resultadoLiberaLineas;
        }
    }
}