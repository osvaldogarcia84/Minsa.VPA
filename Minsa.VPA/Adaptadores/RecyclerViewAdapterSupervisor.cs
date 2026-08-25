using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Grantland.Widget;
using Minsa.VPA.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minsa.VPA.Adaptadores
{
    class RecyclerrViewHolder : RecyclerView.ViewHolder
    {
    //   
        public AutofitTextView AlmacenSup { get; set; }
      
        public AutofitTextView CteRutasSup { get; set; }
        public AutofitTextView IniCteVisitas { get; set; }

        public AutofitTextView IniCteInventario { get; set; }
        public AutofitTextView CteImportDepositarSup { get; set; }

        public RecyclerrViewHolder(View itemView) : base(itemView)
        {
            //    textview = itemView.FindViewById<AutofitTextView>(Resource.Id.NombreSupervisor);
            AlmacenSup = itemView.FindViewById<AutofitTextView>(Resource.Id.AlmacenSup);
            //    txtUsuario = itemView.FindViewById<AutofitTextView>(Resource.Id.IniUsuario);
            CteRutasSup = itemView.FindViewById<AutofitTextView>(Resource.Id.CtesRutaSup);
            IniCteVisitas = itemView.FindViewById<AutofitTextView>(Resource.Id.IniCteVisitas);
            IniCteInventario = itemView.FindViewById<AutofitTextView>(Resource.Id.IniCteInventario);
            CteImportDepositarSup = itemView.FindViewById<AutofitTextView>(Resource.Id.CteImportDepositarSup);

        }
    }

    public class RecyclerViewAdapterSupervisor : RecyclerView.Adapter
    {
        private int color = 0;
        private List<InformacionInicioSupervisor> Informacion;

        public RecyclerViewAdapterSupervisor(List<InformacionInicioSupervisor> informacion)
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
          //  viewHolder.textview.Text = Informacion[position].NombreVendedor;
            viewHolder.AlmacenSup.Text = "Almacen: " + Informacion[position].Almacen;
          //  viewHolder.txtUsuario.Text = "No. vendedor: " + Informacion[position].Usuario;

            if (Informacion[position].ClientesxDia == 0)
            {
                viewHolder.CteRutasSup.Text = "No tienes ruta cargada";
            }
            else
            {
                viewHolder.CteRutasSup.Text = "Clientes en ruta: " + Convert.ToString(Informacion[position].ClientesxDia);
            }

            if (Informacion[position].VisitasHechas == 0)
            {
                viewHolder.IniCteVisitas.Text = "No tienes clientes visitados";
            }
            else
            {
                viewHolder.IniCteVisitas.Text = "Clientes visitados: " + Convert.ToString(Informacion[position].VisitasHechas);
            }
            if (Informacion[position].InventarioAlDia == 0)
            {
                viewHolder.IniCteInventario.Text = "No tienes inventario cargado";
            }
            else
            {
                viewHolder.IniCteInventario.Text =  "Inventario cargado: " + Informacion[position].InventarioAlDia.ToString("##,###.##");
            }

            if (Informacion[position].TotalTransacciones == 0)
            {
                viewHolder.CteImportDepositarSup.Text = "Importe a depositar: $0";
            }
            else
            {
                viewHolder.CteImportDepositarSup.Text = "Importe a depositar: $" + Informacion[position].TotalTransacciones.ToString("##,###.##");
            }

        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View itemView = inflater.Inflate(Resource.Layout.card_Supervisores, parent, false);
            return new RecyclerrViewHolder(itemView);
        }
    }
}