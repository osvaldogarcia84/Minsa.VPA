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
using Minsa.VPA.Enums;
using Minsa.VPA.Modelos;
using Minsa.VPA.Servicios;
using Newtonsoft.Json;
using RestSharp;

namespace Minsa.VPA.Proveedores
{
    public class ProveedorDeEstrategia
    {
        ResultadoDeOperacionGenerico<CorteCaja> resultadoCorte;
        ResultadoDeOperacionGenerico<List<Respuesta>> _resultado;
        ResultadoDeOperacionGenerico<string> resultadoSeguimiento;
        ResultadoDeOperacionGenerico<ResultadoCierreVentas> resultadocierreventas;
        ResultadoDeOperacionGenerico<string> resultadoDeVentaRegistrar;
        ResultadoDeOperacionGenerico<string> ResultadoRegistrarGeoLocacion;
        ResultadoDeOperacionGenerico<ResultadoRegistroDegustacion> resultadoDegustacion;
        ResultadoDeOperacionGenerico<ResultadoSeguimiento> resultadoSeguimientoDeVenta;
        ResultadoDeOperacionGenerico<PromocionBultos> resultadoPromocionBultos;
        ResultadoDeOperacionGenerico<ArticulosSinDescuentos> resultadoArticulosSinDescuentos;
        ResultadoDeOperacionGenerico<List<PrecioSeguimientoVenta>> resultadoPrecio;
        ResultadoDeOperacionGenerico<List<Desharinizacion>> resultadoDeHarinizacion;
        ResultadoDeOperacionGenerico<List<FrecuenciaCompra>> resultadoFrencuencia;
        ResultadoDeOperacionGenerico<List<Apoyos>> resultadoApoyos;
        ResultadoDeOperacionGenerico<ResultadoRegistroAddCausasNoVenta> resultadoAddCausasNoVenta;
        ResultadoDeOperacionGenerico<ValidaOVFacturaDEG> resultadoValidaOVFacturaDEG;
        public async Task<ResultadoDeOperacionGenerico<List<Respuesta>>> ObtenerCausasDeResultadoDeVenta()
        {         
            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(Formatting.None);
            var request = new RestRequest("web/ObtenerCausasDeResultadoDeVenta", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
            await Task.Run(() =>
            {
                var response = Cliente.Execute(request);
                var n = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<Respuesta>>>(response.Content);
                _resultado = n;
            });
           
            return _resultado;
        }

        public ResultadoDeOperacionGenerico<List<Harinera>> ObtenerHarineras()
        {
            ResultadoDeOperacionGenerico<List<Harinera>> _resultado;

            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(Formatting.None);
            var request = new RestRequest("web/ObtenerHarineras", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
            var response = Cliente.Execute(request);
            var n = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<Harinera>>>(response.Content);
            _resultado = n;
            return _resultado;
        }
        public async Task<ResultadoDeOperacionGenerico<ResultadoSeguimiento>> RegistrarSeguimientoDeVenta(SeguimientoDeVenta seguimientoDeVentas)
        {
            

            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(seguimientoDeVentas, Formatting.None);
            var request = new RestRequest("web/RegistrarSeguimientoDeVenta", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);            
            await Task.Run(() =>
            {
                var response = Cliente.Execute(request);
                resultadoSeguimientoDeVenta = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<ResultadoSeguimiento>>(response.Content);
            });
            
            return resultadoSeguimientoDeVenta;
        }

        public async Task<ResultadoDeOperacionGenerico<string>> RegistrarResultadoDeVenta(ResultadoDeVenta resultadoDeVenta)
        {
            //ResultadoDeOperacionGenerico<string> resultado;

            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(resultadoDeVenta, Formatting.None);
            var request = new RestRequest("web/RegistrarResultadoDeVenta", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);         
          
            await Task.Run(() =>
            {
                var response = Cliente.Execute(request);
                resultadoDeVentaRegistrar = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<string>>(response.Content);
            });
            return resultadoDeVentaRegistrar;
        }
        public async Task<ResultadoDeOperacionGenerico<string>> RegistrarSeguimiento(SeguimientoMovil seguimiento)
        {           

            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(seguimiento, Formatting.None);
            var request = new RestRequest("web/RegistrarSeguimiento", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);            
            await Task.Run(() =>
            {
                var response = Cliente.Execute(request);
                resultadoSeguimiento = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<string>>(response.Content);
            });
            return resultadoSeguimiento;
        }

        //=============================================================================================================================================================

        public ResultadoDeOperacionGenerico<List<Articulo>> ObtenerArticulosMinsa(ClienteId cliente)
        {
            ResultadoDeOperacionGenerico<List<Articulo>> resultado;

            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(cliente, Formatting.None);
            var request = new RestRequest("web/ObtenerArticulosMinsa", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
            var response = Cliente.Execute(request);
            resultado = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<Articulo>>>(response.Content);

            return resultado;
        }
        //public ResultadoDeOperacionGenerico<string> RegistrarDegustacion(Degustacion degustacion)
        //{

        //}
        public ResultadoDeOperacionGenerico<List<Articulo>> ObtenerArticulosCompetencia(ClienteId cliente)
        {
            ResultadoDeOperacionGenerico<List<Articulo>> resultado;
            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(cliente, Formatting.None);
            var request = new RestRequest("web/ObtenerArticulosCompetencia", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
            var response = Cliente.Execute(request);
            resultado = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<Articulo>>>(response.Content);

            return resultado;
        }
        public async Task<ResultadoDeOperacionGenerico<ResultadoRegistroDegustacion>> RegistrarDegustacion(Degustacion degustacion)
        {
            
            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(degustacion, Formatting.None);
            var request = new RestRequest("web/RegistrarDegustacion", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
            await Task.Run(() =>
            {
                var response = Cliente.Execute(request);
                resultadoDegustacion = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<ResultadoRegistroDegustacion>>(response.Content);
            });                    

            return resultadoDegustacion;
        }

        public async Task<ResultadoDeOperacionGenerico<string>> RegistrarGeoLocacion(GeoLocacion degustacion)
        {
            //ResultadoDeOperacionGenerico<string> resultado;
            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(degustacion, Formatting.None);
            var request = new RestRequest("web/RegistrarGeoLocacion", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
            await Task.Run(() =>
            {
                var response = Cliente.Execute(request);
                ResultadoRegistrarGeoLocacion = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<string>>(response.Content);
            });           

            return ResultadoRegistrarGeoLocacion;
        }
        //=============================================== Seguimiento Cierre de Ventas =========================================================
        public async Task<ResultadoDeOperacionGenerico<ResultadoCierreVentas>> SeguimientoCierreVentas(SeguimientoCierreVentas seguimientoCierreVentas)
        {
           // ResultadoDeOperacionGenerico<ResultadoCierreVentas> resultado;
            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(seguimientoCierreVentas, Formatting.None);
            var request = new RestRequest("web/SeguimientoCierreVentas", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
            await Task.Run(() =>
            {
                var response = Cliente.Execute(request);
                resultadocierreventas = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<ResultadoCierreVentas>>(response.Content);
            });       

            return resultadocierreventas;
        }
        public ResultadoDeOperacionGenerico<List<CBEstrategiaCerrarVenta>> CBCerrarVenta()
        {
            ResultadoDeOperacionGenerico<List<CBEstrategiaCerrarVenta>> resultado;
            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            //string JsonDatos = JsonConvert.SerializeObject(, Formatting.None);
            var request = new RestRequest("web/CBCerrarVenta", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", ParameterType.RequestBody);
            var response = Cliente.Execute(request);
            resultado = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<CBEstrategiaCerrarVenta>>>(response.Content);

            return resultado;
        }
        public ResultadoDeOperacionGenerico<List<CBPlazoEstrategia>> CBPlazoEstrategia()
        {
            ResultadoDeOperacionGenerico<List<CBPlazoEstrategia>> resultado;
            //string JsonDatos = JsonConvert.SerializeObject(, Formatting.None);
            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            var request = new RestRequest("web/CBPlazoEstrategia", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", ParameterType.RequestBody);
            var response = Cliente.Execute(request);
            resultado = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<CBPlazoEstrategia>>>(response.Content);

            return resultado;
        }
        public ResultadoDeOperacionGenerico<List<ProductosEstrategia>> Productos()
        {
            ResultadoDeOperacionGenerico<List<ProductosEstrategia>> resultado;
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                //string JsonDatos = JsonConvert.SerializeObject(producto, Formatting.None);
                var request = new RestRequest("web/ObtenerProductos", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", ParameterType.RequestBody);
                var response = Cliente.Execute(request);
                resultado = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<ProductosEstrategia>>>(response.Content);
            }
            catch (Exception ex)
            {
                resultado = new ResultadoDeOperacionGenerico<List<ProductosEstrategia>>(TipoDeResultado.Error, ex.Message, null);
            }

            return resultado;
        }
        public ResultadoDeOperacionGenerico<List<Compania>> ObtenerCompañias()
        {
            ResultadoDeOperacionGenerico<List<Compania>> resultado;
            try
            {

                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                var request = new RestRequest("web/ObtenerCompañias", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", ParameterType.RequestBody);
                var response = Cliente.Execute(request);
                resultado = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<Compania>>>(response.Content);
            }
            catch (Exception ex)
            {
                resultado = new ResultadoDeOperacionGenerico<List<Compania>>(TipoDeResultado.Error, ex.Message, null);
            }

            return resultado;
        }
        public ResultadoDeOperacionGenerico<List<ClientesProspecto>> ClientesProspecto(ClienteId cliente)
        {
            ResultadoDeOperacionGenerico<List<ClientesProspecto>> resultado;
            try
            {
               
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(cliente, Formatting.None);
                var request = new RestRequest("web/ClientesProspecto", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                var response = Cliente.Execute(request);
                resultado = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<ClientesProspecto>>>(response.Content);
            }
            catch(Exception ex)
            {
                resultado = new ResultadoDeOperacionGenerico<List<ClientesProspecto>>(TipoDeResultado.Error, ex.Message, null);
            }            

            return resultado;
        }
        public ResultadoDeOperacionGenerico<ResultadoUpdateCierre> UpdateVentaVPA(CerrarVenta cerrar)
        {
            ResultadoDeOperacionGenerico<ResultadoUpdateCierre> resultado;
            try
            {

                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(cerrar, Formatting.None);
                var request = new RestRequest("web/UpdateVentaVPA", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                var response = Cliente.Execute(request);
                resultado = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<ResultadoUpdateCierre>>(response.Content);
            }
            catch (Exception ex)
            {
                resultado = new ResultadoDeOperacionGenerico<ResultadoUpdateCierre>(TipoDeResultado.Error, ex.Message, null);
            }

            return resultado;
        }
        //========================================================= Corte de caja ==============================================================

        public async Task<ResultadoDeOperacionGenerico<CorteCaja>> CorteCaja(Sitio sitio)
        {            
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(sitio, Formatting.None);
                var request = new RestRequest("web/CorteCaja", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoCorte = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<CorteCaja>>(response.Content);
                });                   
            }
            catch (Exception ex)
            {
                resultadoCorte = new ResultadoDeOperacionGenerico<CorteCaja>(TipoDeResultado.Error, ex.Message, null);
            }
            return resultadoCorte;
        }
        public async Task<ResultadoDeOperacionGenerico<PromocionBultos>> PromocionBultos(DataPromocionBultos dataPromocionBultos)
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataPromocionBultos, Formatting.None);
                var request = new RestRequest("web/PromocionBultos", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoPromocionBultos = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<PromocionBultos>>(response.Content);
                });
            }
            catch (Exception ex)
            {
                resultadoPromocionBultos = new ResultadoDeOperacionGenerico<PromocionBultos>(TipoDeResultado.Error, ex.Message, null);
            }
            return resultadoPromocionBultos;
        }
        public async Task<ResultadoDeOperacionGenerico<ArticulosSinDescuentos>> ArticulosSinDescuentos(DataArticulosSinDescuentos dataArticulosSinDescuentos)
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataArticulosSinDescuentos, Formatting.None);
                var request = new RestRequest("web/ArticulosSinDescuentos", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoArticulosSinDescuentos = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<ArticulosSinDescuentos>>(response.Content);
                });
            }
            catch (Exception ex)
            {
                resultadoArticulosSinDescuentos = new ResultadoDeOperacionGenerico<ArticulosSinDescuentos>(TipoDeResultado.Error, ex.Message, null);
            }
            return resultadoArticulosSinDescuentos;
        }
        public async Task<ResultadoDeOperacionGenerico<string>> RegistrarGeoLocacionSupervisores(GeoLocacionSupervisores geoLocacionSupervisores)
        {
            //ResultadoDeOperacionGenerico<string> resultado;
            var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
            string JsonDatos = JsonConvert.SerializeObject(geoLocacionSupervisores, Formatting.None);
            var request = new RestRequest("web/GeoLocacionSupervisores", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
            await Task.Run(() =>
            {
                var response = Cliente.Execute(request);
                ResultadoRegistrarGeoLocacion = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<string>>(response.Content);
            });

            return ResultadoRegistrarGeoLocacion;
        }
        public async Task<ResultadoDeOperacionGenerico<List<PrecioSeguimientoVenta>>> Precio()
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
              //  string JsonDatos = JsonConvert.SerializeObject(dataPromocionBultos, Formatting.None);
                var request = new RestRequest("web/Precio", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoPrecio = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<PrecioSeguimientoVenta>>>(response.Content);
                });
            }
            catch (Exception ex)
            {
                resultadoPrecio = new ResultadoDeOperacionGenerico<List<PrecioSeguimientoVenta>>(TipoDeResultado.Error, ex.Message, null);
            }
            return resultadoPrecio;
        }
        public async Task<ResultadoDeOperacionGenerico<List<Desharinizacion>>> Desharinizacion()
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                //  string JsonDatos = JsonConvert.SerializeObject(dataPromocionBultos, Formatting.None);
                var request = new RestRequest("web/Desharinizacion", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoDeHarinizacion = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<Desharinizacion>>>(response.Content);
                });
            }
            catch (Exception ex)
            {
                resultadoDeHarinizacion = new ResultadoDeOperacionGenerico<List<Desharinizacion>>(TipoDeResultado.Error, ex.Message, null);
            }
            return resultadoDeHarinizacion;
        }
        public async Task<ResultadoDeOperacionGenerico<List<FrecuenciaCompra>>> FrecuenciaCompra()
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                //  string JsonDatos = JsonConvert.SerializeObject(dataPromocionBultos, Formatting.None);
                var request = new RestRequest("web/FrecuenciaCompra", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoFrencuencia = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<FrecuenciaCompra>>>(response.Content);
                });
            }
            catch (Exception ex)
            {
                resultadoFrencuencia = new ResultadoDeOperacionGenerico<List<FrecuenciaCompra>>(TipoDeResultado.Error, ex.Message, null);
            }
            return resultadoFrencuencia;
        }
        public async Task<ResultadoDeOperacionGenerico<List<Apoyos>>> Apoyos()
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                //  string JsonDatos = JsonConvert.SerializeObject(dataPromocionBultos, Formatting.None);
                var request = new RestRequest("web/Apoyos", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoApoyos = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<List<Apoyos>>>(response.Content);
                });
            }
            catch (Exception ex)
            {
                resultadoApoyos = new ResultadoDeOperacionGenerico<List<Apoyos>>(TipoDeResultado.Error, ex.Message, null);
            }
            return resultadoApoyos;
        }
        public async Task<ResultadoDeOperacionGenerico<ResultadoRegistroAddCausasNoVenta>> InsertAddCausasNoVenta(AddCausasNoVenta addCausasNoVenta)
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(addCausasNoVenta, Formatting.None);
                var request = new RestRequest("web/InsertAddCausasNoVenta", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos,ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoAddCausasNoVenta = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<ResultadoRegistroAddCausasNoVenta>>(response.Content);
                });
            }
            catch (Exception ex)
            {
                resultadoAddCausasNoVenta = new ResultadoDeOperacionGenerico<ResultadoRegistroAddCausasNoVenta>(TipoDeResultado.Error, ex.Message, null);
            }
            return resultadoAddCausasNoVenta;
        }
        public async Task<ResultadoDeOperacionGenerico<ValidaOVFacturaDEG>> ValidaOVFacturaDEG(DataValidaOVFactDEG dataValidaOVFactDEG)
        {
            try
            {
                var Cliente = new RestClient(Configuracion.ServidorDesarrollo);
                string JsonDatos = JsonConvert.SerializeObject(dataValidaOVFactDEG, Formatting.None);
                var request = new RestRequest("web/ValidaOVFacturaDEG", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json; charset=utf-8", JsonDatos, ParameterType.RequestBody);
                await Task.Run(() =>
                {
                    var response = Cliente.Execute(request);
                    resultadoValidaOVFacturaDEG = JsonConvert.DeserializeObject<ResultadoDeOperacionGenerico<ValidaOVFacturaDEG>>(response.Content);
                });
            }
            catch (Exception ex)
            {
                resultadoValidaOVFacturaDEG = new ResultadoDeOperacionGenerico<ValidaOVFacturaDEG>(TipoDeResultado.Error, ex.Message, null);
            }
            return resultadoValidaOVFacturaDEG;
        }
    }
}