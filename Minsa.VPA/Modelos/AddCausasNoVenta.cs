using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minsa.VPA.Modelos
{
    public class AddCausasNoVenta
    {
		public string ClienteId { get; set; }
		public string VendedorId { get; set; }
		public string Usuario { get; set; }
		public int PreguntaId { get; set; }
		public string Pregunta { get; set; }
		public string Causa { get; set; }
		public string Motivo { get; set; }
		public decimal PrecioVentaxSaco { get; set; }
		public bool PromocionApoyoCompetencia { get; set; }
		public string PromocionApoyoCB { get; set; }
		public string Insumo { get; set; }
		public decimal CantidadXTM { get; set; }
		public string TipoApoyoCB { get; set; }
		public string VigenciaPromocionApoyo { get; set; }
		public decimal SacosDiario { get; set; }
		public string FrecuenciaCompra { get; set; }
		public string Desharinizacion { get; set; }
		public decimal PrecioCompra { get; set; }
		public decimal SacosKilosMasaConsume { get; set; }
		public string QuienAtiende { get; set; }
		public string NombreComercializador { get; set; }
		public string NuevoNoCteCompra { get; set; }
		public string NombreCteCompra { get; set; }
		public string FechaProximaCompra { get; set; }
		public string TipoCierre { get; set; }
		public AddCausasNoVenta() { }
		public AddCausasNoVenta(string clienteid, string vendedorid, string usuario,int preguntaid, string pregunta, string causa, string motivo,decimal precioventaxsaco, bool promocionapoyocompetencia,
								string promocionapoyocb, string insumo,decimal cantidadxTM, string tipoapoyocb, string vigenciapromocionapoyo, decimal sacosdiarios,
								string frecuenciacompra, string desharinizacion, decimal preciocompra, decimal sacoskilosmasaconsume,
								string quienatiende, string nombrecomercializador, string nuevonoctecompra, string nombrectecompra, string fechaproximacompra,
								string tipocierre)
		{
			ClienteId = clienteid;
			VendedorId = vendedorid;
			Usuario = usuario;
			PreguntaId = preguntaid;
			Pregunta = pregunta;
			Causa = causa;
			Motivo = motivo;
			PrecioVentaxSaco = precioventaxsaco;
			PromocionApoyoCompetencia = promocionapoyocompetencia;
			PromocionApoyoCB = promocionapoyocb;
			Insumo = insumo;
			CantidadXTM = cantidadxTM;
			TipoApoyoCB = tipoapoyocb;
			VigenciaPromocionApoyo = vigenciapromocionapoyo;
			SacosDiario = sacosdiarios;
			FrecuenciaCompra = frecuenciacompra;
			Desharinizacion = desharinizacion;
			PrecioCompra = preciocompra;
			SacosKilosMasaConsume = sacoskilosmasaconsume;
			QuienAtiende = quienatiende;
			NombreComercializador = nombrecomercializador;
			NuevoNoCteCompra = nuevonoctecompra;
			NombreCteCompra = nombrectecompra;
			FechaProximaCompra = fechaproximacompra;
			TipoCierre = tipocierre;
		}
	}
}