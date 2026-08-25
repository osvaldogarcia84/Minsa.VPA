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
    public class AdaptadorTipoPersona : BaseAdapter<TipoPersona>
    {

        private readonly List<TipoPersona> items;
        private readonly Activity _context;
        public AdaptadorTipoPersona(Activity context, List<TipoPersona> _items) : base()
        {
            _context = context;
            items = _items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public override TipoPersona this[int position]
        {
            get
            {
                return items[position];
            }
        }
        public override int Count
        {
            get { return items.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            TipoPersona item = items[position];
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.SpinnerTipoPersona, null);
            view.FindViewById<TextView>(Resource.Id.SPTipoPersona).Text = item.Text;
            return view;
        }      

    }
    
}