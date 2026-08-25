using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Minsa.VPA.Modelos;
using System.Collections.Generic;
using System.Linq;

namespace Minsa.VPA.Adaptadores
{
    public class AdaptadorSincronizarInformacion : BaseAdapter<GeoLocacion>
    {
        private readonly List<GeoLocacion> Items;
        private readonly Activity _context;

        public AdaptadorSincronizarInformacion(Activity context, List<GeoLocacion> items)
        {
            _context = context;
            Items = items;
        }


        public override long GetItemId(int position)
        {
            return position;
        }

        public override GeoLocacion this[int position]
        {
            get
            {
                return Items[position];
            }
        }

        public override int Count
        {
            get { return Items.Count; }
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.card_sincronizarInformacion, null);

            if (!Items.Any())
                return view;

            GeoLocacion item = Items[position];
            if (item.ClienteId == null)
                return view;

            view.FindViewById<TextView>(Resource.Id.ClienteIdSincronizar).Text = "No. Cliente: " + item.ClienteId;
            view.FindViewById<TextView>(Resource.Id.SincronizarLatitud).Text =  "Latitud: "  + item.Latitud.ToString();
            view.FindViewById<TextView>(Resource.Id.SincronizarLongitud).Text = "Longitud: " + item.Longitud.ToString();

            return view;
        }
    }

  
}