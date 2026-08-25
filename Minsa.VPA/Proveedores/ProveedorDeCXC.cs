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
    public class ProveedorDeCXC
    {
        ResultadoDeOperacionGenerico<List<DashboardCXC>> resultadoDashboardCXC;
        ResultadoDeOperacionGenerico<int> resultadoCreaTicketCXC;
        ResultadoDeOperacionGenerico<Ticket> resultadoTicketOVPago;
        ResultadoDeOperacionGenerico<List<Transacciones>> resultadoRecibosCredito;
        public async Task<ResultadoDeOperacionGenerico<List<DashboardCXC>>> DashboardCXC(DataDashboardCXC dataDashboardCXC)
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataDashboardCXC, Formatting.None);
                var request = new RestRequest("web/DashboardCXC", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoDashboardCXC = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<DashboardCXC>>>(response.Content);
                });

            }
            catch (Exception ex)
            {
                resultadoDashboardCXC = new ResultadoDeOperacionGenerico<List<DashboardCXC>>(TipoDeResultado.Error, ex.Message, null);
            }

            return resultadoDashboardCXC;
        }
        public async Task<ResultadoDeOperacionGenerico<int>> CreaTicketVPAAP(DataCreaTicket dataCreaTicket)
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataCreaTicket, Formatting.None);
                var request = new RestRequest("web/CreaTicketVPAAP", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoCreaTicketCXC = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<int>>(response.Content);
                });

            }
            catch (Exception ex)
            {
                resultadoCreaTicketCXC = new ResultadoDeOperacionGenerico<int>(TipoDeResultado.Error, ex.Message, 0);
            }

            return resultadoCreaTicketCXC;
        }
        public async Task<ResultadoDeOperacionGenerico<Ticket>> TicketOVPagado(DataTicketOVPago dataTicketOVPago)
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataTicketOVPago, Formatting.None);
                var request = new RestRequest("web/TicketOVPago", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoTicketOVPago = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<Ticket>>(response.Content);

                });
                return resultadoTicketOVPago;
            }
            catch (Exception ex)
            {
                return new ResultadoDeOperacionGenerico<Ticket>(TipoDeResultado.Error, ex.Message, null);
            }
        }
        public async Task<ResultadoDeOperacionGenerico<List<Transacciones>>> RecibosCredito(Sitio sitio)
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(sitio, Formatting.None);
                var request = new RestRequest("web/RecibosCredito", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoRecibosCredito = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<Transacciones>>>(response.Content);

                });
                return resultadoRecibosCredito;
            }
            catch (Exception ex)
            {
                return new ResultadoDeOperacionGenerico<List<Transacciones>>(TipoDeResultado.Error, ex.Message, null);
            }
        }
    }
}