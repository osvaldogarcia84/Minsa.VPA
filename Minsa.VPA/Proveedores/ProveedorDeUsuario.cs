using System;
using System.Threading.Tasks;
using Minsa.VPA.Modelos;
using Minsa.VPA.Repositorio;
using Minsa.VPA.Servicios;
using Newtonsoft.Json;
using RestSharp;

namespace Minsa.VPA.Proveedores
{
    public class ProveedorDeUsuario
    {
        ResultadoDeOperacionGenerico<Vendedor> resultadoVendedor;
        ResultadoDeOperacionGenerico<string> resultadoKmInicial;
        ResultadoDeOperacionGenerico<ValidaKmInicial> resultadoValidaKmInicial;
        ResultadoDeOperacionGenerico<InicioSesion> resultadoInicioSesion;
        public async Task<ResultadoDeOperacionGenerico<Vendedor>> Autorizar(string usuario, string password)
        {
           
          //  ResultadoDeOperacionGenerico<Vendedor> _resultado;

            UsuarioAutorizar _usuario = new UsuarioAutorizar
            {
                Usuario = usuario,
                Password = password
            };

            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(_usuario, Formatting.None);
            var request = new RestRequest("web/Autorizar", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
            await Task.Run(() =>
            {
                var response =  Cliente.Execute(request);
                var n = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<Vendedor>>(response.Content);
                resultadoVendedor = n;
            });
            
            return resultadoVendedor;
        }
        public async Task<ResultadoDeOperacionGenerico<Vendedor>> AutorizarUsuarios(string usuario, string password)
        {         

            UsuarioAutorizar _usuario = new UsuarioAutorizar
            {
                Usuario = usuario,
                Password = password
            };

            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(_usuario, Formatting.None);
            var request = new RestRequest("web/AutorizarUsuarios", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
            await Task.Run(() =>
            {
                var response = Cliente.Execute(request);
                var n = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<Vendedor>>(response.Content);
                resultadoVendedor = n;
            });

            return resultadoVendedor;
        }
        public async Task<ResultadoDeOperacionGenerico<string>> KmInicial(DataKmInicial dataKmInicial)
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataKmInicial, Formatting.None);
                var request = new RestRequest("web/KmInicial", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    var n = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<string>>(response.Content);
                    resultadoKmInicial = n;
                });
                return resultadoKmInicial;
            }
            catch(Exception ex)
            {
                return resultadoKmInicial = new ResultadoDeOperacionGenerico<string>(TipoDeResultado.Error, ex.Message, null);
            }
          
        }      
        public async Task<ResultadoDeOperacionGenerico<ValidaKmInicial>> validaKmInicial(DataValidaKmInicial dataValidaKmInicial)
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataValidaKmInicial, Formatting.None);
                var request = new RestRequest("web/ValidaKmInicial", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    var n = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<ValidaKmInicial>>(response.Content);
                    resultadoValidaKmInicial = n;
                });
                return resultadoValidaKmInicial;
            }
            catch (Exception ex)
            {
                return resultadoValidaKmInicial = new ResultadoDeOperacionGenerico<ValidaKmInicial>(TipoDeResultado.Error, ex.Message, null);
            }
        }
        public async Task<ResultadoDeOperacionGenerico<InicioSesion>> ValidaInicioSesion()
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                //       string JsonDatos = JsonConvert.SerializeObject(dataValidaKmInicial, Formatting.None);
                var request = new RestRequest("web/ValidaInicioSesion", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    var resultado = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<InicioSesion>>(response.Content);
                    resultadoInicioSesion = resultado;
                });
                return resultadoInicioSesion;
            }
            catch (Exception ex)
            {
                return resultadoInicioSesion = new ResultadoDeOperacionGenerico<InicioSesion>(TipoDeResultado.Error, "Ocurrio un error, no se puede conectar al servidor", null);
            }
        }
        //public ResultadoDeOperacionGenerico<ResultadoRegistroCheckIn> CheckIn(CheckIn checkin)
        //{

        //    ResultadoDeOperacionGenerico<ResultadoRegistroCheckIn> _resultado;


        //    var Cliente = new RestClient(ProveedorGlobal.Conexion);
        //    string JsonDatos = JsonConvert.SerializeObject(checkin, Formatting.None);
        //    var request = new RestRequest("web/CheckIn", Method.POST);
        //    request.RequestFormat = DataFormat.Json;
        //    request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
        //    var response = Cliente.Execute(request);
        //    _resultado = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<ResultadoRegistroCheckIn>>(response.Content);            
        //    return _resultado;
        //}

        //public ResultadoDeOperacionGenerico<ResultadoRegistroCheckOut> CheckOut(CheckOut checkin)
        //{
        //    ResultadoDeOperacionGenerico<ResultadoRegistroCheckOut> resultado;
        //    try
        //    {

        //        var Cliente = new RestClient(ProveedorGlobal.Conexion);
        //        string JsonDatos = JsonConvert.SerializeObject(checkin, Formatting.None);
        //        var request = new RestRequest("web/CheckOut", Method.POST);
        //        request.RequestFormat = DataFormat.Json;
        //        request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
        //        var response = Cliente.Execute(request);
        //        resultado = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<ResultadoRegistroCheckOut>>(response.Content);
        //        return resultado;
        //    }
        //    catch (Exception)
        //    {
        //        return resultado = new ResultadoDeOperacionGenerico<ResultadoRegistroCheckOut>(TipoDeResultado.Fallo, "Ocurrio un error", null);
        //    }

        //}

    }
}