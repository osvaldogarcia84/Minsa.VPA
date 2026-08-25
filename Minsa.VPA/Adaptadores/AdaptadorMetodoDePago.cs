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
using Minsa.VPA.Modelos;

namespace Minsa.VPA.Adaptadores
{
    public class AdaptadorMetodoDePago : BaseAdapter<MetodoDePago>
    {
        private readonly List<MetodoDePago> _items;
        private readonly Activity _context;
        public AdaptadorMetodoDePago(Activity context, List<MetodoDePago> items) : base()
        {
            _context = context;
            _items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }

        public override MetodoDePago this[int position]
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

            MetodoDePago item = _items[position];
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.SpinnerMetodoPago, null);
            view.FindViewById<TextView>(Resource.Id.SpMetodoPago).Text = item.NAME;            
            return view;
        }
    }
}