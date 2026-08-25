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
using Grantland.Widget;
using Minsa.VPA.Fragments;
using Minsa.VPA.Modelos;

namespace Minsa.VPA.Adaptadores
{
    public class AdaptadorSpinnerArticulos : BaseAdapter<ArticuloMovil>
    {
        private readonly List<ArticuloMovil> _items;
        private readonly Activity _context;
        public CarritoFragment SetearPrecio;
        public AdaptadorSpinnerArticulos(Activity context, List<ArticuloMovil> items, CarritoFragment setearPrecio) : base()
        {
            _context = context;
            _items = items;
            SetearPrecio = setearPrecio;
        }
        public override long GetItemId(int position)
        {
            return position;
        }

        public override ArticuloMovil this[int position]
        {
            get
            {
                return _items[position];
            }
        }

        public override int Count
        {
            get { return _items.Count; }
        }



        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            ArticuloMovil item = _items[position];
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.SpinnerCustom, null);
            //View view = convertView ?? Context.LayoutInflater.Inflate(Resource.Layout.SpinnerCustom, null);
            if (item.Image != null)
                view.FindViewById<ImageView>(Resource.Id.ImgArticulo).SetImageDrawable(item.Image);
            view.FindViewById<TextView>(Resource.Id.IdProductos).Text = item.ArticuloId;
            view.FindViewById<AutofitTextView>(Resource.Id.NombreProductos).Text = item.NombreArticulo;
           // view.FindViewById<TextView>(Resource.Id.Precio).Text =  Convert.ToString(String.Format("{0:C}", item.Precio.Monto));
            return view;
        }
    }
}