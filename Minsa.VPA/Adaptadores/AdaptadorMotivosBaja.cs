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
    public class AdaptadorMotivosBaja : BaseAdapter<MotivoBaja>
    {

        private readonly List<MotivoBaja> items;
        private readonly Activity _context;
        public AdaptadorMotivosBaja(Activity context, List<MotivoBaja> _items) : base()
        {
            _context = context;
            items = _items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public override MotivoBaja this[int position]
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

            MotivoBaja item = items[position];
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.SpinnerMotivosBaja, null);
            view.FindViewById<TextView>(Resource.Id.SPMotivos).Text = item.MOTIVO;
            return view;
        }        
    }    
}