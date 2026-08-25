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
    public class AdapterDesharinizacion : BaseAdapter<Desharinizacion>
    {

        private readonly List<Desharinizacion> _items;
        private readonly Activity _context;
        public AdapterDesharinizacion(Activity context, List<Desharinizacion> items) : base()
        {
            _context = context;
            _items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }

        public override Desharinizacion this[int position]
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

            Desharinizacion item = _items[position];
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.SpinnerDesharinizacion, null);
            view.FindViewById<TextView>(Resource.Id.SpDesharinizacion).Text = item.Descripcion;
            return view;
        }

    }    
}