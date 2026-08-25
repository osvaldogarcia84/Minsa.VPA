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
    public class AdaptadorCBCierreVentas : Adaptador<CBEstrategiaCerrarVenta>
    {
        private new readonly List<CBEstrategiaCerrarVenta> Items;
        private new readonly Activity Context;
        

        public AdaptadorCBCierreVentas(Activity context, List<CBEstrategiaCerrarVenta> items) : base (context, items)
        {
            Context = context;
            Items = items;
        }


        public override CBEstrategiaCerrarVenta this[int position]
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
            CBEstrategiaCerrarVenta item = Items[position];
            var view = convertView ?? Context.LayoutInflater.Inflate(Resource.Layout.SpinnerSeguimientoCierreVentas, null);
            view.FindViewById<TextView>(Resource.Id.SeguimientoCierreVentas).Text = item.Descripcion;
            return view;
        }


    }
}