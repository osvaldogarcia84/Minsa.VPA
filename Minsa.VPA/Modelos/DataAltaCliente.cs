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
    public class DataAltaCliente
    {
        public string NomZona { get; set; }
        public string TPrecio { get; set; }
        public string ListaPrecio { get; set; }
        public string ListaDescuento { get; set; }
        public string Socios { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Calle { get; set; }
        public string NoExt { get; set; }
        public string NoInt { get; set; }
        public string Colonia { get; set; }
        public string Municipio { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
        public string CP { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Correo { get; set; }
        public string PersonaFisica { get; set; }
        public string SitioPrecio { get; set; }
        public string CUSTGROUP { get; set; }
        public string Materno { get; set; }
        public string Paterno { get; set; }
        public string Pais { get; set; }
        public string RFC { get; set; }
        public DataAltaCliente() { }
        public DataAltaCliente(string nomzona, string tprecio, string listaprecio, string listadescuento, string socios, string codigo, string nombre, string calle, string noext, string noint,
                        string colonia, string municipio, string ciudad, string estado, string cp, string telefono, string celular, string correo, string personafisica,
                        string sitioprecio, string custgroup, string materno, string paterno, string pais, string rfc)
        {
            NomZona = nomzona;
            TPrecio = tprecio;
            ListaPrecio = listaprecio;
            ListaDescuento = listadescuento;
            Socios = socios;
            Codigo = codigo;
            Nombre = nombre;
            Calle = calle;
            NoExt = noext;
            NoInt = noint;
            Colonia = colonia;
            Municipio = municipio;
            Ciudad = ciudad;
            Estado = estado;
            CP = cp;
            Telefono = telefono;
            Celular = celular;
            Correo = correo;
            PersonaFisica = personafisica;
            SitioPrecio = sitioprecio;
            CUSTGROUP = custgroup;
            Materno = materno;
            Paterno = paterno;
            Pais = pais;
            RFC = rfc;
        }
    }
}