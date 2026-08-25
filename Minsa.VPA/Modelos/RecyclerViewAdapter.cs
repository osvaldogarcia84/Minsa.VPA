using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Grantland.Widget;

namespace Minsa.VPA.Modelos
{
 

    class RecyclerrViewHolder : RecyclerView.ViewHolder
    {
        public TextView textview { get; set; }
        public TextView txtAlmacen { get; set; }
        public TextView txtCodigo { get; set; }
        public AutofitTextView txtCountCteRutas { get; set; }
        public AutofitTextView txtCountCteVisitas { get; set; }

        public AutofitTextView txtCountCteInventarioIn { get; set; }
        public AutofitTextView txtCteImportDepositar{ get; set; }

        public RecyclerrViewHolder(View itemView) : base(itemView)
        {
            textview = itemView.FindViewById<TextView>(Resource.Id.NombreVendedor);
            txtAlmacen = itemView.FindViewById<TextView>(Resource.Id.AlmacenVendedor);
            txtCodigo = itemView.FindViewById<TextView>(Resource.Id.IniCodigo);
            txtCountCteRutas = itemView.FindViewById<AutofitTextView>(Resource.Id.CtesRuta);
            txtCountCteVisitas = itemView.FindViewById<AutofitTextView>(Resource.Id.CteVisitas);
            txtCountCteInventarioIn = itemView.FindViewById<AutofitTextView>(Resource.Id.CteInventarioIn);
            txtCteImportDepositar = itemView.FindViewById<AutofitTextView>(Resource.Id.CteImportDepositar);

        }
    }

    public class RecyclerViewAdapter : RecyclerView.Adapter
    {
        private int color = 0;      
        private List<InformacionInicio> Informacion;

        public RecyclerViewAdapter(List<InformacionInicio> informacion)
        {
            this.Informacion = informacion;
        }

        public override int ItemCount
        {
            get
            {
                return Informacion.Count;
            }
        }
      
        public void SetColor(int color)
        {
            this.color = color;
            NotifyDataSetChanged();
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RecyclerrViewHolder viewHolder = holder as RecyclerrViewHolder;
            viewHolder.textview.Text = Informacion[position].NombreVendedor;
            viewHolder.txtAlmacen.Text = "Almacen: " + Informacion[position].Almacen;
            viewHolder.txtCodigo.Text = "No. vendedor: " + Informacion[position].Codigo;

            if (Informacion[position].ClientesxDia == 0)
            {
                viewHolder.txtCountCteRutas.Text = "No tienes ruta cargada";
            }
            else
            {
                viewHolder.txtCountCteRutas.Text = Convert.ToString(Informacion[position].ClientesxDia);
            }

            if (Informacion[position].VisitasHechas == 0)
            {
                viewHolder.txtCountCteVisitas.Text = "No tienes clientes visitados";
            }
            else
            {
                viewHolder.txtCountCteVisitas.Text = Convert.ToString(Informacion[position].VisitasHechas);
            }
            if (Informacion[position].InventarioAlDia == 0)
            {
                viewHolder.txtCountCteInventarioIn.Text = "No tienes inventario cargado";
            }
            else
            {
                viewHolder.txtCountCteInventarioIn.Text =  Informacion[position].InventarioAlDia.ToString("##,###.##");
            }

            if (Informacion[position].TotalTransacciones == 0)
            {
                viewHolder.txtCteImportDepositar.Text = "0";
            }
            else
            {
                viewHolder.txtCteImportDepositar.Text = Informacion[position].TotalTransacciones.ToString("##,###.##");
            }
         
        }
    

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View itemView = inflater.Inflate(Resource.Layout.card_items, parent, false);
            return new RecyclerrViewHolder(itemView);
        }
    }
}