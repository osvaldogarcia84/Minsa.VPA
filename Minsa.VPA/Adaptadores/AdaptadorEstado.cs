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
    public class AdaptadorEstado : BaseAdapter<Estados>
    {

        private readonly List<Estados> estados;
        private readonly Activity _context;
        public AdaptadorEstado(Activity context, List<Estados> estado) : base()
        {
            _context = context;
            estados = estado;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public override Estados this[int position]
        {
            get
            {
                return estados[position];
            }
        }
        public override int Count
        {
            get { return estados.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            Estados item = estados[position];
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.SpinnerEstado, null);
            view.FindViewById<TextView>(Resource.Id.SPEstado).Text = item.NOMBREESTADO;
            return view;
        }

        //Fill in cound here, currently 0

    }

    }
   