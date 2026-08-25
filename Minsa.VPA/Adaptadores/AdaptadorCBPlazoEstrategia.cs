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
    public class AdaptadorCBPlazoEstrategia : BaseAdapter<CBPlazoEstrategia>
    {

        private readonly List<CBPlazoEstrategia> Items;
        private readonly Activity Context;

        public AdaptadorCBPlazoEstrategia(Activity context, List<CBPlazoEstrategia> items) : base()
        {
            Context = context;
            Items = items;
        }


        public override CBPlazoEstrategia this[int position]
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
            CBPlazoEstrategia item = Items[position];
            View view = convertView ?? Context.LayoutInflater.Inflate(Resource.Layout.SpinnerPlazoEstrategia, null);
            view.FindViewById<TextView>(Resource.Id.PlazoEstrategia).Text = item.Descripcion;

            return view;
        }
    }
}