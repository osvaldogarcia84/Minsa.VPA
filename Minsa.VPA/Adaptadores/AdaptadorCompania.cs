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
   public class AdaptadorCompania : BaseAdapter<Compania>
    {

        private readonly List<Compania> Items;
        private readonly Activity Context;

        public AdaptadorCompania(Activity context, List<Compania> items) : base()
        {
            Context = context;
            Items = items;
        }

        public override Compania this[int position]
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
            Compania item = Items[position];
            View view = convertView ?? Context.LayoutInflater.Inflate(Resource.Layout.SpinnerCompania, null);
            view.FindViewById<TextView>(Resource.Id.Compania).Text = item.Nombre;

            return view;
        }


    }

   
}