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
    public class AdapterSitios : BaseAdapter<Sitios>
    {

        private readonly List<Sitios> Items;
        private readonly Activity Context;

        public AdapterSitios(Activity context, List<Sitios> items) : base()
        {
            Context = context;
            Items = items;
        }


        public override Sitios this[int position]
        {
            get
            {
                return Items[position];
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public override int Count
        {
            get { return Items.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Sitios item = Items[position];
            View view = convertView ?? Context.LayoutInflater.Inflate(Resource.Layout.SpinnerSitios, null);
            view.FindViewById<TextView>(Resource.Id.SpSitios).Text = item.SITIOS;
            return view;
        }
    }
}