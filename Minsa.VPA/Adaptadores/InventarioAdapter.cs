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
using Grantland.Widget;
using Minsa.VPA.Modelos;

namespace Minsa.VPA.Adaptadores
{
    class InventarioAdapter : BaseAdapter<InformacionDeInventario>
    {
        public List<InformacionDeInventario> Items;

        private readonly Activity _context;
        public InventarioAdapter(Activity context, List<InformacionDeInventario> items)
        {
            _context = context;
            Items = items;


        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override InformacionDeInventario this[int position]
        {
            get { return Items[position]; }
        }

        public override int Count
        {
            get
            {
                return Items.Count;
            }
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            InformacionDeInventario item = Items[position];
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.InventarioClientes, null);

            view.FindViewById<TextView>(Resource.Id.NombreProducto).Text = item.NombreArticulo;
            view.FindViewById<AutofitTextView>(Resource.Id.DisponiblePorSaco).Text = item.DisponiblePorSaco != null ? DoFormat(Convert.ToDouble(item.DisponiblePorSaco)) : "0.00";
            var disponible = view.FindViewById<AutofitTextView>(Resource.Id.Disponible);
            if (item.UnidadDeMedida == "PZ")
            {
                disponible.Text = "Pieza: " + item.DisponiblePorTM;
            }
            else if (item.UnidadDeMedida == "TN")
            {
                disponible.Text = "Toneladas: " + item.DisponiblePorTM;
            }
            else if (item.UnidadDeMedida == "KG")
            {
                disponible.Text = "Toneladas: " + item.DisponiblePorTM;
            }

            var Articulos = new ArticulosVista(item.CodigoArticulo, _context);
            if (Articulos != null)
                view.FindViewById<ImageView>(Resource.Id.ImagenArticulo).SetImageDrawable(Articulos.Image);
          
            //view.FindViewById<TextView>(Resource.Id.PrecioPorSaco).Text = item.PrecioPorSaco;
            //view.FindViewById<TextView>(Resource.Id.PrecioPorTM).Text = item.PrecioPorTM;
            return view;
        }

        public class ArticulosVista
        {
            public Drawable Image { get; set; }
            public string ArticuloId { get; set; }
            public ArticulosVista(string articuloId, Context context)
            {
                ArticuloId = articuloId;
                try
                {
                    string path = string.Format("{0}/{1}.jpg", ArticuloVistaTipo.Preview, articuloId);
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
        }

        public enum ArticuloVistaTipo
        {
            Preview,
            Full
        }
        public static string DoFormat(double myNumber)
        {
            var s = string.Format("{0:0.00}", myNumber);

            if (s.EndsWith("00"))
            {
                return ((int)myNumber).ToString();
            }
            else
            {
                return s;
            }
        }
    }
}