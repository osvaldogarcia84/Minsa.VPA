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

namespace Minsa.VPA.Modelos
{
    public class ResultadoDeOperacionGenerico<T>
    {
        
        public TipoDeResultado Tipo { get; set; }        
        public string Mensaje { get; set; }        
        public T Valor { get; set; }

        public ResultadoDeOperacionGenerico(TipoDeResultado tipo, string mensaje, T valor)
        {
            Tipo = tipo;
            Mensaje = mensaje;
            Valor = valor;
        }

    
    }
    public enum TipoDeResultado
    {
       
        Error,       
        Exito,       
        Fallo
    }
}