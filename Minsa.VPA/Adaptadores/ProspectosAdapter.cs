using Android.App;
using Minsa.VPA.Modelos;
using Android.Views;
using Android.Widget;
using Minsa.VPA.Fragments;
using Minsa.VPA.Proveedores;
using System.Linq;
using Android.Locations;
using Android.Content;
using System.Collections.Generic;
using Grantland.Widget;

namespace Minsa.VPA.Adaptadores
{
    public class ProspectosAdapter : Adaptador<ClientesProspecto>
    {

        private readonly List<ClientesProspecto> Items;
        private readonly Activity Context;
        public CierreVentaFragment CierreVentaFragment;
        public ProspectosAdapter(Activity context, List<ClientesProspecto> items, CierreVentaFragment cierreVentaFragment) :base(context,items)
        {
            Context = context;
            Items = items;
            CierreVentaFragment = cierreVentaFragment;
        }

        public override long GetItemId(int position)
        {

            return position;
        }
        public void setSelectedIndex(int ind)
        {
            ProveedorGlobal.PosicionCliente = ind;
            NotifyDataSetChanged();
        }

        public override ClientesProspecto this[int position]
        {
            get
            {
                return Items[position];
            }
        }
        public override int Count
        {
            get { return Items.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ServiceViewHolderProspecto holder = null;
            View view = convertView ?? Context.LayoutInflater.Inflate(Resource.Layout.card_Prospectos,null);
            if (!Items.Any())
                return view;
            ClientesProspecto item = Items[position];
            if (item.IdCliente == null)
                return view;

            var cerrada = item.Cerrada == false ? "Abierta" : "Cerrada";

            holder = new ServiceViewHolderProspecto();
          
            holder.PropectoId = view.FindViewById<TextView>(Resource.Id.IdSocioProspecto);
            holder.ProspectoNombre = view.FindViewById<TextView>(Resource.Id.NombreProspectoCV);
            holder.EstatusVenta = view.FindViewById<TextView>(Resource.Id.EstatusVenta);
            //Asignacion
            holder.PropectoId.Text = "Codigo: " + item.IdCliente;
            holder.ProspectoNombre.Text = "Nombre: " + item.Nombre;           
            holder.EstatusVenta.Text = "Estatus de la venta: " + cerrada;
            return view;
        }
    }
    public class ServiceViewHolderProspecto : Java.Lang.Object
    {
        public TextView PropectoId { get; set; }
        public TextView ProspectoNombre { get; set; }
        public TextView EstatusVenta { get; set; }
    }
}