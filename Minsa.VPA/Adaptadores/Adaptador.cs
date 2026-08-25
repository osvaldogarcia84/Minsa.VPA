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
    public abstract class Adaptador<T> : BaseAdapter<T>
    {
        public List<T> Items;
        protected readonly Activity Context;

        protected Adaptador(Activity context, List<T> items)
        {
            Context = context;
            Items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override T this[int position]
        {
            get { return Items[position]; }
        }

        public T ElementoSeleccionado(long posicion)
        {
            return Items.ElementAtOrDefault((int)posicion);
        }

        public long ObtenerIndice(T elemento)
        {
            return Items.IndexOf(elemento);
        }

        public int BuscarIndice(Func<T, bool> predicado)
        {
            var elemento = Items.FirstOrDefault(predicado);
            if (elemento == null)
                return -1;
            return Items.IndexOf(elemento);
        }

        public override int Count
        {
            get { return Items.Count; }
        }
    }
}