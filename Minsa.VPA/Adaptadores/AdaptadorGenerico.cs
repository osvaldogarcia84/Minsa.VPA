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

namespace Minsa.VPA.Adaptadores
{
    public class AdaptadorGenerico : Adaptador<ItemGenerico>
    {
        public AdaptadorGenerico(Activity context, List<ItemGenerico> items) : base(context, items)
        {
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = Items[position];
            var view = convertView as LinearLayout ?? Context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = item.Nombre;
            return view;
        }
    }

    public class ItemGenerico
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public ItemGenerico()
        {
        }

        public ItemGenerico(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}