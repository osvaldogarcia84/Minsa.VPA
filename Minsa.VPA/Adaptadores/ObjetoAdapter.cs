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
using Grantland.Widget;

namespace Minsa.VPA.Adaptadores
{
    public class ObjetoAdapter<T> : BaseAdapter<T>
    {
        private readonly List<T> _items;
        private readonly Activity _context;

        public ObjetoAdapter(Activity context, List<T> items)
        {
            _context = context;
            _items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override T this[int position]
        {
            get { return _items[position]; }
        }

        public override int Count
        {
            get { return _items.Count; }
        }

        public T ObtenerSeleccion(long posicion)
        {
            return _items.ElementAt((int)posicion);
        }

        public long ObtenerIndice(T elemento)
        {
            return _items.IndexOf(elemento);
        }

        public int BuscarIndice(Func<T, bool> predicado)
        {
            var elemento = _items.FirstOrDefault(predicado);
            if (elemento == null)
                return -1;
            return _items.IndexOf(elemento);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = _items[position];
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.DrawerListItem, null);
            string texto = item.ToString();
            view.FindViewById<TextView> (Resource.Id.DrawerListItemText).Text = texto;

            return view;
        }
    }
}