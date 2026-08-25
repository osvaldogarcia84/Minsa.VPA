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
    public class ProveedorDeClientes
    {
        ResultadoDeOperacionGenerico<List<Cliente>> resultadoCliente;
        ResultadoDeOperacionGenerico<List<InformacionDeVenta>> resultadoInfoVenta;
        ResultadoDeOperacionGenerico<DashBoard> resultadoDashBoard;
        ResultadoDeOperacionGenerico<List<Cliente>> resultadoObtenerTodos;
        ResultadoDeOperacionGenerico<List<InformacionDeInventario>> resultadoDeInventario;
        ResultadoDeOperacionGenerico<GpoArticulos> resultadoGpoArticulo;
        ResultadoDeOperacionGenerico<InformacionDeInventario> resultadoInventario;
        ResultadoDeOperacionGenerico<List<Transacciones>> resultadoTransacciones;
        ResultadoDeOperacionGenerico<Ticket> resultadoTicket;
        ResultadoDeOperacionGenerico<List<MetodoDePago>> resultadoMetodo;
        ResultadoDeOperacionGenerico<List<CuentasBancarias>> resultadoCuentasBancarias;
        ResultadoDeOperacionGenerico<string> ResultadoReimpresion;
        ResultadoDeOperacionGenerico<ValidaArticulosHarinaGranel> resultadoValidaArticulosHarinaGranel;
        ResultadoDeOperacionGenerico<List<Estados>> resultadoEstados;
        ResultadoDeOperacionGenerico<List<Municipio>> resultadoMunicipio;
        ResultadoDeOperacionGenerico<string> ResultadoDeCodigoCte;
        ResultadoDeOperacionGenerico<List<TipoPersona>> resultadoTipoPersona;
        ResultadoDeOperacionGenerico<List<TiposSocios>> resultadoTipoSocios;
        ResultadoDeOperacionGenerico<ListaPrecios> resultadoListPrecios;
        ResultadoDeOperacionGenerico<DatosDireccion> resultadoDatosDireccion;
        ResultadoDeOperacionGenerico<List<MotivoBaja>> resultadoMotivoBaja;
        ResultadoDeOperacionGenerico<string> ResultadoBajaCliente;
        ResultadoDeOperacionGenerico<List<Sitios>> resultadoSitios;
        ResultadoDeOperacionGenerico<List<HistoricosVenta>> resultadoHistoricosVenta;
        ResultadoDeOperacionGenerico<List<InfoDashboardSupervisores>> resultadoInfoDashboardSup;
        ResultadoDeOperacionGenerico<ValidaCreditoCtes> resultadoValidaCred;
        ResultadoDeOperacionGenerico<Ticket>  resultadoTicketOV;
        
        public async Task<ResultadoDeOperacionGenerico<List<Cliente>>> ObtenerTodosPorRuta(DataVendedor dataVendedor)
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataVendedor, Formatting.None);
                var request = new RestRequest("web/ObtenerTodosPorRuta", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    var data = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<Cliente>>>(response.Content);
                    resultadoCliente = data;
                });
             
            }
            catch(Exception ex)
            {
                resultadoCliente = new ResultadoDeOperacionGenerico<List<Cliente>>(TipoDeResultado.Fallo, "No se pudo conectar con el servidor intenta nuevamente.", null);
            }

            return resultadoCliente;
        }
        public async Task<ResultadoDeOperacionGenerico<DashBoard>> DashBoard(DataSitio dataSitio)
        {

            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(dataSitio, Formatting.None);
            var request = new RestRequest("web/InformacionDeInicio", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
            await Task.Run(() =>
            {
                var response = Cliente.Execute(request);
                var data = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<DashBoard>>(response.Content);
                resultadoDashBoard = data;
            });

            return resultadoDashBoard;
        }
        public async Task<ResultadoDeOperacionGenerico<List<Cliente>>> ObtenerTodos(DataObtenerTodos dataObtenerTodos)
        {            

            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(dataObtenerTodos, Formatting.None);
            var request = new RestRequest("web/ObtenerTodos", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
            await Task.Run(() =>
            {
                var response = Cliente.Execute(request);
                var data = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<Cliente>>>(response.Content);
                resultadoObtenerTodos = data;
            });
            return resultadoObtenerTodos;
        }

        public async Task<ResultadoDeOperacionGenerico<List<InformacionDeVenta>>> ObtenerInformacionDeVenta(DataVendedor dataVendedor)
        {          
           
            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(dataVendedor, Formatting.None);
            var request = new RestRequest("web/ObtenerInformacionDeVenta", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
            await Task.Run(() =>
            {
                var response = Cliente.Execute(request);
                var data = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<InformacionDeVenta>>>(response.Content);
                resultadoInfoVenta = data;
            });               
            return resultadoInfoVenta;
        }
        public async Task<ResultadoDeOperacionGenerico<List<MetodoDePago>>> MetodoDePago()
        {           
            
            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(Formatting.None);
            var request = new RestRequest("web/MetodoDePago", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", ParameterType.RequestBody);
            await Task.Run(() =>
            {
                var response = Cliente.Execute(request);
                var data = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<MetodoDePago>>>(response.Content);
                resultadoMetodo = data;
            });
         
            return resultadoMetodo;
        }

        public async Task<ResultadoDeOperacionGenerico<GpoArticulos>> GpoArticulos(GrupoArticulo grupoArticulo)
        {          
           
            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(grupoArticulo, Formatting.None);
            var request = new RestRequest("web/ConsultaGpoArticulo", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
            await Task.Run(() =>
            {
                var response = Cliente.Execute(request);
                var data = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<GpoArticulos>>(response.Content);
                resultadoGpoArticulo = data;
            });
                
            return resultadoGpoArticulo;
        }
        public async Task<ResultadoDeOperacionGenerico<InformacionDeInventario>> ConsultaDisponiblePorArticulo(DisponiblePorArticulo disponible)
        {            

            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(disponible, Formatting.None);
            var request = new RestRequest("web/ConsultaDisponiblePorArticulo", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
            await Task.Run(() =>
            {
                var response = Cliente.Execute(request);
                var data = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<InformacionDeInventario>>(response.Content);
                resultadoInventario = data;
            });            
            return resultadoInventario;
        }

        public async Task<ResultadoDeOperacionGenerico<List<InformacionDeInventario>>> ObtenerConsultaDisponible(DataSitio sitio)
        {            
            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(sitio, Formatting.None);
            var request = new RestRequest("web/ConsultaDisponible", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
            await Task.Run(() =>
            {
                var response = Cliente.Execute(request);
                var data = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<InformacionDeInventario>>>(response.Content);
                resultadoDeInventario = data;
            });           

            return resultadoDeInventario;
        }

        //public List<InformacionDeInventario> ConsultaDisponibleInvt(Sitio sitio)
        //{
        //    List<InformacionDeInventario> resultado;
           
        //    var Cliente = new RestClient(ProveedorGlobal.Conexion);
        //    string JsonDatos = JsonConvert.SerializeObject(sitio, Formatting.None);
        //    var request = new RestRequest("web/ConsultaDisponible", Method.POST);
        //    request.RequestFormat = DataFormat.Json;
        //    request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
        //    var response = Cliente.Execute(request);
        //    var data = JsonConvert.DeserializeObject<List<InformacionDeInventario>>(response.Content);
        //    resultado = data;
        //    return resultado;
        //}

        //public ResultadoDeOperacionGenerico<InformacionDeVenta> ObtenerInformacionDeInventario(LlaveVendedor vendedor)
        //{
        //    ResultadoDeOperacionGenerico<InformacionDeVenta> resultado;

        //    var Cliente = new RestClient(ProveedorGlobal.Conexion);
        //    string JsonDatos = JsonConvert.SerializeObject(vendedor, Formatting.None);
        //    var request = new RestRequest("web/ObtenerInformacionDeInventario", Method.POST);
        //    request.RequestFormat = DataFormat.Json;
        //    request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
        //    var response = Cliente.Execute(request);
        //    var data = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<InformacionDeVenta>>(response.Content);
        //    resultado = data;
        //    return resultado;
        //}

        public async Task<ResultadoDeOperacionGenerico<List<Transacciones>>> ObtenerTransacciones(Sitio sitio)
        {       

            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(sitio, Formatting.None);
            var request = new RestRequest("web/ConsultaTransacciones", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
            await Task.Run(() =>
            {
                var response = Cliente.Execute(request);
                resultadoTransacciones =  JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<Transacciones>>>(response.Content);                
            });
            
            return resultadoTransacciones;
        }

        //public ResultadoDeOperacionGenerico<ContadorPunteos> VisitasPunteadas(VisitaPunteos visito)
        //{
        //    ResultadoDeOperacionGenerico<ContadorPunteos> resultado;

        //    var Cliente = new RestClient(ProveedorGlobal.Conexion);
        //    string JsonDatos = JsonConvert.SerializeObject(visito, Formatting.None);
        //    var request = new RestRequest("web/VisitasPunteadas", Method.POST);
        //    request.RequestFormat = DataFormat.Json;
        //    request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
        //    var response = Cliente.Execute(request);
        //    var data = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<ContadorPunteos>>(response.Content);
        //    resultado = data;
        //    return resultado;
        //}
        public async Task<ResultadoDeOperacionGenerico<Ticket>> Ticket(DatosTicket datosTicket)
        {            
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(datosTicket, Formatting.None);
                var request = new RestRequest("web/Ticket", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoTicket = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<Ticket>>(response.Content);
                });
               
            }
            catch (Exception ex)
            {
                resultadoTicket = new ResultadoDeOperacionGenerico<Ticket>(TipoDeResultado.Error, ex.Message, null);
            }

            return resultadoTicket;
        }
        public async Task<ResultadoDeOperacionGenerico<string>> ReimprimeTicketVPA(DataReimpresion dataReimpresion)
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataReimpresion, Formatting.None);
                var request = new RestRequest("web/ReimprimeTicketVPA", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    ResultadoReimpresion = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<string>>(response.Content);
                });

            }
            catch (Exception ex)
            {
                resultadoTicket = new ResultadoDeOperacionGenerico<Ticket>(TipoDeResultado.Error, ex.Message, null);
            }

            return ResultadoReimpresion;
        }
        public async Task<ResultadoDeOperacionGenerico<List<CuentasBancarias>>> CuentasBancarias()
        {

            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
               // string JsonDatos = JsonConvert.SerializeObject(datosTicket, Formatting.None);
                var request = new RestRequest("web/CuentasBancarias", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoCuentasBancarias = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<CuentasBancarias>>>(response.Content);
                });

            }
            catch (Exception ex)
            {
                resultadoCuentasBancarias = new ResultadoDeOperacionGenerico<List<CuentasBancarias>>(TipoDeResultado.Error, ex.Message, null);
            }

            return resultadoCuentasBancarias;
        }
        public async Task<ResultadoDeOperacionGenerico<ValidaArticulosHarinaGranel>> ValidaArticulosHarinaGranel(DataArticulosHarinaGranel dataArticulosHarinaGranel)
        {

            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataArticulosHarinaGranel, Formatting.None);
                var request = new RestRequest("web/ValidaArticulosHarinaGranel", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoValidaArticulosHarinaGranel = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<ValidaArticulosHarinaGranel>>(response.Content);
                });
            }
            catch (Exception ex)
            {
                resultadoValidaArticulosHarinaGranel = new ResultadoDeOperacionGenerico<ValidaArticulosHarinaGranel>(TipoDeResultado.Error, "Ocurrio un error al generar la consulta", null);
            }

            return resultadoValidaArticulosHarinaGranel;
        }
        public async Task<ResultadoDeOperacionGenerico<List<Estados>>> BuscaEstado(DataEstado dataEstado)
        {

            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataEstado, Formatting.None);
                var request = new RestRequest("web/BuscaEstado", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoEstados = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<Estados>>>(response.Content);
                });
            }
            catch (Exception ex)
            {
                resultadoEstados = new ResultadoDeOperacionGenerico<List<Estados>>(TipoDeResultado.Error, "Ocurrio un error al generar la consulta", null);
            }

            return resultadoEstados;
        }
        public async Task<ResultadoDeOperacionGenerico<List<Municipio>>> BuscaMunicpio(DataMunicipio dataMunicipio)
        {

            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataMunicipio, Formatting.None);
                var request = new RestRequest("web/BuscaMunicipio", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoMunicipio = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<Municipio>>>(response.Content);
                });
            }
            catch (Exception ex)
            {
                resultadoMunicipio = new ResultadoDeOperacionGenerico<List<Municipio>>(TipoDeResultado.Error, "Ocurrio un error al generar la consulta", null);
            }

            return resultadoMunicipio;
        }
        public async Task<ResultadoDeOperacionGenerico<string>> GeneraCodigo()
        {

            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                //string JsonDatos = JsonConvert.SerializeObject(dataMunicipio, Formatting.None);
                var request = new RestRequest("web/GeneraCodigo", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8",  ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    ResultadoDeCodigoCte = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<string>>(response.Content);
                });
            }
            catch (Exception ex)
            {
                ResultadoDeCodigoCte = new ResultadoDeOperacionGenerico<string>(TipoDeResultado.Error, "Ocurrio un error al generar la consulta", null);
            }

            return ResultadoDeCodigoCte;
        }
        public async Task<ResultadoDeOperacionGenerico<string>> AltaCliente(DataAltaCliente dataAltaCliente)
        {

            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataAltaCliente, Formatting.None);
                var request = new RestRequest("web/AltaCliente", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos ,ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    ResultadoDeCodigoCte = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<string>>(response.Content);
                });
            }
            catch (Exception ex)
            {
                ResultadoDeCodigoCte = new ResultadoDeOperacionGenerico<string>(TipoDeResultado.Error, "Ocurrio un error al generar la consulta", null);
            }

            return ResultadoDeCodigoCte;
        }
        public async Task<ResultadoDeOperacionGenerico<List<TipoPersona>>> TipoPersona()
        {

            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                //string JsonDatos = JsonConvert.SerializeObject(dataMunicipio, Formatting.None);
                var request = new RestRequest("web/TipoPersona", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoTipoPersona = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<TipoPersona>>>(response.Content);
                });
            }
            catch (Exception ex)
            {
                resultadoTipoPersona = new ResultadoDeOperacionGenerico<List<TipoPersona>>(TipoDeResultado.Error, "Ocurrio un error al generar la consulta", null);
            }

            return resultadoTipoPersona;
        }
        public async Task<ResultadoDeOperacionGenerico<List<TiposSocios>>> TipoSocios()
        {

            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                //string JsonDatos = JsonConvert.SerializeObject(dataMunicipio, Formatting.None);
                var request = new RestRequest("web/TiposSocios", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoTipoSocios = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<TiposSocios>>>(response.Content);
                });
            }
            catch (Exception ex)
            {
                resultadoTipoSocios = new ResultadoDeOperacionGenerico<List<TiposSocios>>(TipoDeResultado.Error, "Ocurrio un error al generar la consulta", null);
            }

            return resultadoTipoSocios;
        }
        public async Task<ResultadoDeOperacionGenerico<ListaPrecios>> ListaPrecios(DataListaPrecios dataListaPrecios)
        {

            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataListaPrecios, Formatting.None);
                var request = new RestRequest("web/ListaPrecios", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoListPrecios = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<ListaPrecios>>(response.Content);
                });
            }
            catch (Exception ex)
            {
                resultadoListPrecios = new ResultadoDeOperacionGenerico<ListaPrecios>(TipoDeResultado.Error, "Ocurrio un error al generar la consulta", null);
            }

            return resultadoListPrecios;
        }
        public async Task<ResultadoDeOperacionGenerico<DatosDireccion>> DatosAltaDireccion(DataDireccion dataDireccion)
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataDireccion, Formatting.None);
                var request = new RestRequest("web/DatosAltaDireccion", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoDatosDireccion = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<DatosDireccion>>(response.Content);
                });
            }
            catch (Exception ex)
            {
                resultadoDatosDireccion = new ResultadoDeOperacionGenerico<DatosDireccion>(TipoDeResultado.Error, "Ocurrio un error al generar la consulta", null);
            }

            return resultadoDatosDireccion;
        }
        public async Task<ResultadoDeOperacionGenerico<List<MotivoBaja>>> MotivoBaja()
        {

            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                //string JsonDatos = JsonConvert.SerializeObject(dataMunicipio, Formatting.None);
                var request = new RestRequest("web/MotivoBaja", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoMotivoBaja = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<MotivoBaja>>>(response.Content);
                });
            }
            catch (Exception ex)
            {
                resultadoMotivoBaja = new ResultadoDeOperacionGenerico<List<MotivoBaja>>(TipoDeResultado.Error, "Ocurrio un error al generar la consulta", null);
            }

            return resultadoMotivoBaja;
        }
        public async Task<ResultadoDeOperacionGenerico<string>> BajaCliente(DataBajaCliente dataBajaCliente)
        {

            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataBajaCliente, Formatting.None);
                var request = new RestRequest("web/BajaCliente", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    ResultadoBajaCliente = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<string>>(response.Content);
                });
            }
            catch (Exception ex)
            {
                ResultadoBajaCliente = new ResultadoDeOperacionGenerico<string>(TipoDeResultado.Error, "Ocurrio un error al generar la consulta", null);
            }

            return ResultadoBajaCliente;
        }

        public async Task<ResultadoDeOperacionGenerico<List<Sitios>>> Sitios(DataSitios dataSitios)
        {

            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataSitios, Formatting.None);
                var request = new RestRequest("web/Sitios", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoSitios = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<Sitios>>>(response.Content);
                });
            }
            catch (Exception)
            {
                resultadoSitios = new ResultadoDeOperacionGenerico<List<Sitios>>(TipoDeResultado.Error, "Ocurrio un error al generar la consulta", null);
            }

            return resultadoSitios;
        }
        public async Task<ResultadoDeOperacionGenerico<List<Cliente>>> ObtenerTodosPorRutaSupervisor(DataSupervisores dataSupervisores)
        {

            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(dataSupervisores, Formatting.None);
            var request = new RestRequest("web/ObtenerTodosPorRutaSupervisores", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
            await Task.Run(() =>
            {
                var response = Cliente.Execute(request);
                var data = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<Cliente>>>(response.Content);
                resultadoCliente = data;
            });

            return resultadoCliente;
        }
        public async Task<ResultadoDeOperacionGenerico<List<HistoricosVenta>>> HistoricoVenta(DataHistoricosVenta dataHistoricosVenta)
        {

            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(dataHistoricosVenta, Formatting.None);
            var request = new RestRequest("web/HistoricoVenta", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
            await Task.Run(() =>
            {
                var response = Cliente.Execute(request);
                var data = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<HistoricosVenta>>>(response.Content);
                resultadoHistoricosVenta = data;
            });

            return resultadoHistoricosVenta;
        }
        public async Task<ResultadoDeOperacionGenerico<List<InfoDashboardSupervisores>>> DashBoardSupervisores(DataDashBoardSupervisores dataDashBoardSupervisores)
        {

            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(dataDashBoardSupervisores, Formatting.None);
            var request = new RestRequest("web/DashBoardSupervisores", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
            await Task.Run(() =>
            {
                var response = Cliente.Execute(request);
                var data = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<InfoDashboardSupervisores>>>(response.Content);
                resultadoInfoDashboardSup = data;
            });

            return resultadoInfoDashboardSup;
        }
        public async Task<ResultadoDeOperacionGenerico<ValidaCreditoCtes>> ValidaCreditoCtes(DataCreditoCtes dataCreditoCtes)
        {            
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataCreditoCtes, Formatting.None);
                var request = new RestRequest("web/ValidaCreditoCtes", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    var data = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<ValidaCreditoCtes>>(response.Content);
                    resultadoValidaCred = data;
                });
                return resultadoValidaCred;
            }
            catch (Exception ex)
            {
                return new ResultadoDeOperacionGenerico<ValidaCreditoCtes>(TipoDeResultado.Error, ex.Message, null);
            }            
        }
        public async Task<ResultadoDeOperacionGenerico<Ticket>> TicketOV(DatosTicket datosTicket)
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(datosTicket, Formatting.None);
                var request = new RestRequest("web/TicketOV", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoTicketOV = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<Ticket>>(response.Content);
                   
                });
                return resultadoTicketOV;
            }
            catch (Exception ex)
            {
                return new ResultadoDeOperacionGenerico<Ticket>(TipoDeResultado.Error, ex.Message, null);
            }
        }
        public async Task<ResultadoDeOperacionGenerico<string>> ReimprimeTicketOVVPA(DataReimpresion dataReimpresion)
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataReimpresion, Formatting.None);
                var request = new RestRequest("web/ReimprimeTicketVPA", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    ResultadoReimpresion = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<string>>(response.Content);
                });

            }
            catch (Exception ex)
            {
                resultadoTicket = new ResultadoDeOperacionGenerico<Ticket>(TipoDeResultado.Error, ex.Message, null);
            }

            return ResultadoReimpresion;
        }
        
    }
}