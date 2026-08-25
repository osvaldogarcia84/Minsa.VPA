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
    public class AdaptadorRutasOffline : BaseAdapter<ClienteMovil>
    {

        private readonly List<ClienteMovil> Items;
        private readonly Activity _context;


        public AdaptadorRutasOffline(Activity context, List<ClienteMovil> items)
        {
            _context = context;
            Items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override ClienteMovil this[int position]
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

            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.card_rutasOffline, null);

            if (!Items.Any())
                return view;

            ClienteMovil item = Items[position];
            if (item.ClienteId == null)
                return view;

            view.FindViewById<TextView>(Resource.Id.ClienteIdOffline).Text = item.ClienteId;
            view.FindViewById<TextView>(Resource.Id.ClienteNombreOffline).Text = item.Nombre;
            
            return view;
        }
    }
}