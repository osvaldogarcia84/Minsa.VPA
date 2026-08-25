using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Minsa.VPA.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minsa.VPA.Adaptadores
{
    public class AdapterPrecioSeguimiento : BaseAdapter<PrecioSeguimientoVenta>
    {

        private readonly List<PrecioSeguimientoVenta> _items;
        private readonly Activity _context;
        public AdapterPrecioSeguimiento(Activity context, List<PrecioSeguimientoVenta> items) : base()
        {
            _context = context;
            _items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }

        public override PrecioSeguimientoVenta this[int position]
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

            PrecioSeguimientoVenta item = _items[position];
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.SpinnerPrecioSeguimiento, null);
            view.FindViewById<TextView>(Resource.Id.SpPrecioSeguimiento).Text = item.Descripcion;
            return view;
        }
    }
  
}