using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Minsa.VPA.Enums;

namespace Minsa.VPA.Modelos
{
    public class ArticuloMovil
    {
        public string ArticuloId { get; set; }
        public string NombreArticulo { get; set; }
        public decimal Monto { get; set; }
        public string Sitio { get; set; }
        public string Grupo { get; set; }
        public Drawable Image { get; set; }
        public decimal Impuesto { get; set; }
        public ArticuloMovil() { }
        public ArticuloMovil(string articuloid, string nombrearticulo, decimal monto, string sitio, string grupo, Context context, decimal impuesto, ArticuloVistaTipo tipo = ArticuloVistaTipo.Preview)
        {
            ArticuloId = articuloid;
            NombreArticulo = nombrearticulo;
            Monto = monto;
            Sitio = sitio;
            Grupo = grupo;
            Impuesto = impuesto;
            try
            {
                string path = string.Format("{0}/{1}.jpg", tipo, articuloid);
                using (var stream = context.Assets.Open(path))
                {
                    Image = Drawable.CreateFromStream(stream, null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
      //  public Articulo Articulo { get; set; }
      ////  public decimal Neto { get; set; }
      //  public Precio Precio { get; set; }
      //  // public decimal Descuento { get; set; }
      //  public Drawable Image { get; set; }

            //  public ArticuloMovil(Articulo articulo, Precio precio, Context context, ArticuloVistaTipo tipo = ArticuloVistaTipo.Preview)
            //  {
            //      Articulo = articulo;
            //      Precio = precio;
            //      //  Descuento = descuento;
            //     // Neto = precio; //- descuento;
            //      try
            //      {
            //          string path = string.Format("{0}/{1}.jpg", tipo, articulo.Id);
            //          using (var stream = context.Assets.Open(path))
            //          {
            //              Image = Drawable.CreateFromStream(stream, null);
            //          }
            //      }
            //      catch (Exception ex)
            //      {
            //          Console.WriteLine(ex);
            //      }
            //  }
        }
}