using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Minsa.VPA.Enums;

namespace Minsa.VPA.Modelos
{
    public class Cliente
    {
        public string Id { get; set; }
        public string ClienteId { get; set; }
        public string Nombre { get; set; }
        public string TipoSocio { get; set; }
        public string TipoDeVisita { get; set; }
        public int Secuencia { get; set; }
        public bool Visitado { get; set; }
        public bool Bloqueado { get; set; }
        public string IM_COORDENADAS_X { get; set; }
        public string IM_COORDENADAS_Y { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public bool MicroCredito { get; set; }
        public bool ValidaCteVsCtaFactCred { get; set; }
        public Cliente() { }
        public Cliente(string id, string clienteid, string nombre, string tiposocio, string tipoDeVisita, int secuencia, bool visitado, bool bloqueado,
                       string im_coordenadas_x, string im_coordenadas_y, string direccion, string telefono, bool microcredito, bool validactevsctafactcred)
        {
            Id = id;
            ClienteId = clienteid;
            Nombre = nombre;
            TipoSocio = tiposocio;
            TipoDeVisita = tipoDeVisita;
            Secuencia = secuencia;
            Visitado = visitado;
            Bloqueado = bloqueado;
            IM_COORDENADAS_X = im_coordenadas_x;
            IM_COORDENADAS_Y = im_coordenadas_y;
            Direccion = direccion;
            Telefono = telefono;
            MicroCredito = microcredito;
            ValidaCteVsCtaFactCred = validactevsctafactcred;
        }
    
        //public class Cliente
        //{

        //    public int Id { get; set; }
        //    public string ClienteId { get; set; }
        //    public string Nombre { get; set; }
        //    public TipoDeSocio Tipo { get; set; }
        //    public TipoDeVisita TipoDeVisita { get; set; }
        //    public string SitioDeEnvio { get; set; }
        //    public int DiasDeTraslado { get; set; }
        //    public bool Bloqueado { get; set; }
        //    public int Secuencia { get; set; }
        //    public TipoDeMercado Mercado { get; set; }
        //    public bool Visitado { get; set; }
        //    public int DigitoVerificador { get; set; }
        //    public string IM_COORDENADAS_X { get; set; }
        //    public string IM_COORDENADAS_Y { get; set; }
        //    public string Direccion { get; set; }
        //    public string Telefono { get; set; }
        //    public Cliente() { }
        //    public Cliente(int id, string clienteid, string nombre, string tipo, string tipoDeVisita, string sitioDeEnvio,
        //        int diasDeTraslado, bool bloqueado, int secuencia, bool visitado, int digitoVerificador,
        //        string im_coordenadas_x, string im_coordenadas_y,
        //        string direccion, string telefono
        //        )
        //    {
        //        DigitoVerificador = digitoVerificador;
        //        Visitado = visitado;
        //        Id = id;
        //        ClienteId = clienteid;
        //        Nombre = nombre;
        //        Tipo = ObtenerTipo(tipo);
        //        TipoDeVisita = ObtenerVisita(tipoDeVisita);
        //        SitioDeEnvio = sitioDeEnvio;
        //        DiasDeTraslado = diasDeTraslado;
        //        Bloqueado = bloqueado;
        //        Secuencia = secuencia;
        //        Mercado = ObtenerMercado(Tipo);
        //        IM_COORDENADAS_X = im_coordenadas_x;
        //        IM_COORDENADAS_Y = im_coordenadas_y;
        //        Direccion = direccion;
        //        Telefono = telefono;
        //    }

        public override string ToString()
        {
            return String.Format("{0} - {1}", Id, Nombre);
        }

        private static TipoDeMercado ObtenerMercado(TipoDeSocio tipoDeSocio)
        {
            switch (tipoDeSocio)
            {
                case TipoDeSocio.Concesionario:
                    return TipoDeMercado.IMTs;
                case TipoDeSocio.Autoservicio:
                    return TipoDeMercado.AutoServicios;
                case TipoDeSocio.ClientesEspeciales:
                    return TipoDeMercado.Especiales;
                case TipoDeSocio.Exportacion:
                    return TipoDeMercado.Especiales;
                case TipoDeSocio.Friturero:
                    return TipoDeMercado.Especiales;
                case TipoDeSocio.VentaDeIpa:
                    return TipoDeMercado.Otros;
                case TipoDeSocio.VentaAbordo:
                    return TipoDeMercado.IMTs;
                case TipoDeSocio.VendedorFacturacion:
                    return TipoDeMercado.IMTs;
                case TipoDeSocio.InstitucionalComerc:
                    return TipoDeMercado.Gobierno;
                case TipoDeSocio.InstitucionalDicons:
                    return TipoDeMercado.Gobierno;
                case TipoDeSocio.InstitucionalDif:
                    return TipoDeMercado.Gobierno;
                case TipoDeSocio.Mayorista:
                    return TipoDeMercado.Mayoristas;
                case TipoDeSocio.Molino:
                    return TipoDeMercado.IMTs;
                case TipoDeSocio.MolinoTortilleria:
                    return TipoDeMercado.IMTs;
                case TipoDeSocio.PenalesComercializa:
                    return TipoDeMercado.Gobierno;
                case TipoDeSocio.Tortilleria:
                    return TipoDeMercado.IMTs;
                case TipoDeSocio.Tostadero:
                    return TipoDeMercado.Especiales;
                case TipoDeSocio.VentaMaizMinsa:
                    return TipoDeMercado.Mayoristas;
                case TipoDeSocio.Comercializador:
                    return TipoDeMercado.IMTs;
                case TipoDeSocio.DifComercializador:
                    return TipoDeMercado.Gobierno;
                case TipoDeSocio.DifDiconsa:
                    return TipoDeMercado.Gobierno;
                case TipoDeSocio.Institucional:
                    return TipoDeMercado.Gobierno;
                case TipoDeSocio.VtasMostrador:
                    return TipoDeMercado.IMTs;
                default:
                    return TipoDeMercado.Otros;
            }
        }

        private static TipoDeSocio ObtenerTipo(string tipoDeSocio)
        {
            switch (tipoDeSocio)
            {
                case "INTERCO":
                    return TipoDeSocio.Interco;
                case "INTERCOMPAÑIAS":
                    return TipoDeSocio.Intercompañias;
                case "SUBINTERCOMPAÑIA":
                    return TipoDeSocio.Subintercompañia;
                case "SUBINTERCOMPAÑIAS":
                    return TipoDeSocio.Subintercompañias;
                case "EMPLEADO":
                    return TipoDeSocio.Empleado;
                case "PROVEEDOR":
                    return TipoDeSocio.Proveedor;
                case "ACREEDOR":
                    return TipoDeSocio.Acreedor;
                case "MOL  Y TORTI":
                    return TipoDeSocio.MolYYorti;
                case "ASOCIACION CIVIL":
                    return TipoDeSocio.AsociacionCivil;
                case "AUTOSERVICIO":
                    return TipoDeSocio.Autoservicio;
                case "BANCO":
                    return TipoDeSocio.Banco;
                case "CLIENTES ESPECIALES":
                    return TipoDeSocio.ClientesEspeciales;
                case "COMISIONISTA":
                    return TipoDeSocio.Comisionista;
                case "EC DIRECTOR":
                    return TipoDeSocio.EcDirector;
                case "EC GERENTE":
                    return TipoDeSocio.EcGerente;
                case "EC PROMOTOR":
                    return TipoDeSocio.EcPromotor;
                case "EC SUPERVISOR":
                    return TipoDeSocio.EcSupervisor;
                case "CLIENTES":
                    return TipoDeSocio.Clientes;
                case "EC VENDEDOR":
                    return TipoDeSocio.EcVendedor;
                case "ESPECIALES FRITURAS":
                    return TipoDeSocio.EspecialesFrituras;
                case "ESPECIALES OTROS":
                    return TipoDeSocio.EspecialesOtros;
                case "ESPECIALES TORTILLA":
                    return TipoDeSocio.EspecialesTortilla;
                case "ESPECIALES TOSTADA":
                    return TipoDeSocio.EspecialesTostada;
                case "EXPORTACION":
                    return TipoDeSocio.Exportacion;
                case "FRITURERO":
                    return TipoDeSocio.Friturero;
                case "VENDEDOR FACTURACION":
                    return TipoDeSocio.VendedorFacturacion;
                case "CONCESIONARIO":
                    return TipoDeSocio.Concesionario;
                case "INSTITUCION PRIVADA":
                    return TipoDeSocio.InstitucionPrivada;
                case "INSTITUCIONAL COMERC":
                    return TipoDeSocio.InstitucionalComerc;
                case "INSTITUCIONAL DICONS":
                    return TipoDeSocio.InstitucionalDicons;
                case "INSTITUCIONAL DIF":
                    return TipoDeSocio.InstitucionalDif;
                case "INTERCOMPAÑIA":
                    return TipoDeSocio.Intercompañia;
                case "MAYORISTA":
                    return TipoDeSocio.Mayorista;
                case "MOLINO":
                    return TipoDeSocio.Molino;
                case "MOLINO TORTILLERIA":
                    return TipoDeSocio.MolinoTortilleria;
                case "OTRAS INSTITUCIONES":
                    return TipoDeSocio.OtrasInstituciones;
                case "OTROS DEUDORES":
                    return TipoDeSocio.OtrosDeudores;
                case "PENALES COMERCIALIZA":
                    return TipoDeSocio.PenalesComercializa;
                case "PENALES DIRECTOS":
                    return TipoDeSocio.PenalesDirectos;
                case "PROSPECTO":
                    return TipoDeSocio.Prospecto;
                case "SITIOS":
                    return TipoDeSocio.Sitios;
                case "SUBPRODUCTO":
                    return TipoDeSocio.Subproducto;
                case "TORTILLA":
                    return TipoDeSocio.Tortilla;
                case "TORTILLERIA":
                    return TipoDeSocio.Tortilleria;
                case "TOSTADERO":
                    return TipoDeSocio.Tostadero;
                case "VENTA MAIZ MINSA":
                    return TipoDeSocio.VentaMaizMinsa;
                case "ASEGURADORA":
                    return TipoDeSocio.Aseguradora;
                case "COMERCIALIZADOR":
                    return TipoDeSocio.Comercializador;
                case "CPMASA":
                    return TipoDeSocio.Cpmasa;
                case "CREDITOS BANCARIOS":
                    return TipoDeSocio.CreditosBancarios;
                case "CUENTA BANCARIA":
                    return TipoDeSocio.CuentaBancaria;
                case "DEUDORES":
                    return TipoDeSocio.Deudores;
                case "DICONSA PROG ESPECIA":
                    return TipoDeSocio.DiconsaProgEspecia;
                case "DICONSA RURAL":
                    return TipoDeSocio.DiconsaRural;
                case "DIF COMERCIALIZADOR":
                    return TipoDeSocio.DifComercializador;
                case "DIF DICONSA":
                    return TipoDeSocio.DifDiconsa;
                case "DIF DIRECTO":
                    return TipoDeSocio.DifDirecto;
                case "GOBIERNO":
                    return TipoDeSocio.Gobierno;
                case "IMTS ESPECIALES":
                    return TipoDeSocio.ImtsEspeciales;
                case "INSTITUCIONAL":
                    return TipoDeSocio.Institucional;
                case "IMTS FRANQUICIA":
                    return TipoDeSocio.ImtsFranquicia;
                case "SIN DEFINIR":
                    return TipoDeSocio.SinDefinir;
                case "VENTA DE IPA":
                    return TipoDeSocio.VentaDeIpa;
                case "VENTA ABORDO":
                    return TipoDeSocio.VentaAbordo;
                case "CTE EVENTOS":
                    return TipoDeSocio.CteEventos;
                case "SUBPRODUCTOS":
                    return TipoDeSocio.Subproductos;
                case "VTAS MOSTRADOR":
                    return TipoDeSocio.VtasMostrador;
                default:
                    return TipoDeSocio.OtrasInstituciones;
            }
        }

        //private TipoDeVisita ObtenerVisita(string tipoDeVisita)
        //{
        //    TipoDeVisita tipo;
        //    switch (tipoDeVisita)
        //    {
        //        case "P":
        //            tipo = TipoDeVisita.Prospectacion;
        //            break;
        //        case "S":
        //            tipo = TipoDeVisita.Sembrado;
        //            break;
        //        default:
        //            tipo = TipoDeVisita.Normal;
        //            break;
        //    }

        //    return tipo;
        //}


        //public string NombreDeTipoDeCliente()
        //{
        //    string nombre;
        //    switch (Mercado)
        //    {
        //        case TipoDeMercado.IMTs:
        //            nombre = "Tortillería";
        //            break;
        //        case TipoDeMercado.AutoServicios:
        //            nombre = "Auto servicio";
        //            break;
        //        default:
        //            nombre = Mercado.ToString();
        //            break;
        //    }

        //    return nombre;
        //}

        //public string NombreDeVisita()
        //{
        //    string nombre;
        //    switch (TipoDeVisita)
        //    {
        //        case TipoDeVisita.Normal:
        //            nombre = "Normal";
        //            break;
        //        case TipoDeVisita.Prospectacion:
        //            nombre = "Prospecto";
        //            break;
        //        case TipoDeVisita.Sembrado:
        //            nombre = "Sembrado/Semilla";
        //            break;
        //        case TipoDeVisita.Promocion:
        //            nombre = "Promoción";
        //            break;
        //        default:
        //            nombre = String.Empty;
        //            break;
        //    }

        //    return nombre;
        //}
    }
}