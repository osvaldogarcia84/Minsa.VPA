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
    public class AdaptadorRecurso : BaseAdapter<ProductosEstrategia>
    {

        private readonly List<ProductosEstrategia> Items;
        private readonly Activity Context;

        public AdaptadorRecurso(Activity context, List<ProductosEstrategia> items): base()
        {
            Context = context;
            Items = items;
        }


        public override ProductosEstrategia this[int position]
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
            ProductosEstrategia item = Items[position];
            View view = convertView ?? Context.LayoutInflater.Inflate(Resource.Layout.SpinnerRecurso, null);
            view.FindViewById<TextView>(Resource.Id.RecursoEstrategia).Text = item.NAME;
            return view;
        }
    }
}