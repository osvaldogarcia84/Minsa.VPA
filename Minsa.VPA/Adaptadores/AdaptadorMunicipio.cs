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
    public class AdaptadorMunicipio : BaseAdapter<Municipio>
    {

        private readonly List<Municipio> municipio;
        private readonly Activity _context;
        public AdaptadorMunicipio(Activity context, List<Municipio> municipios) : base()
        {
            _context = context;
            municipio = municipios;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public override Municipio this[int position]
        {
            get
            {
                return municipio[position];
            }
        }
        public override int Count
        {
            get { return municipio.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            Municipio item = municipio[position];
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.SpinnerMunicipio, null);
            view.FindViewById<TextView>(Resource.Id.SPMunicpio).Text = item.DESCRIPCION;
            return view;
        }
    }
}