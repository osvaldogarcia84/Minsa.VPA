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
    public class AdaptadorTiposSocios : BaseAdapter<TiposSocios>
    {

        private readonly List<TiposSocios> items;
        private readonly Activity _context;
        public AdaptadorTiposSocios(Activity context, List<TiposSocios> _items) : base()
        {
            _context = context;
            items = _items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public override TiposSocios this[int position]
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

            TiposSocios item = items[position];
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.SpinnerTiposSocios, null);
            view.FindViewById<TextView>(Resource.Id.SPTiposSocios).Text = item.IM_NOMBRE_SOCIO;
            return view;
        }

        //Fill in cound here, currently 0

    }
}