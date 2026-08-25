using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Minsa.VPA.Modelos;
using Minsa.VPA.Proveedores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minsa.VPA.Fragments;

namespace Minsa.VPA.Adaptadores
{
    public class OrdenesYaloAdapter : BaseExpandableListAdapter
    {

        private readonly Activity _context;
        public List<OrdenesYaloMovil> ordenesYalo;
        public YALOFragment YaloFragment;
        public OrdenesYaloMovil ordenYaloMovilSelected;

        public OrdenesYaloAdapter(Activity context, List<OrdenesYaloMovil> ordenesyalo, YALOFragment yalofragment)
        {
            _context = context;
            ordenesYalo = ordenesyalo;
            YaloFragment = yalofragment;
        }     
        public override int GroupCount => ordenesYalo.Count;
        public override int GetChildrenCount(int groupPosition) => ordenesYalo[groupPosition].LINEAS?.Count ?? 0;
        public override Java.Lang.Object GetGroup(int groupPosition) => null;
        public override Java.Lang.Object GetChild(int groupPosition, int childPosition) => null;
        public override long GetGroupId(int groupPosition) => groupPosition;
        public override long GetChildId(int groupPosition, int childPosition) => childPosition;
        public override bool HasStableIds => false;
        public override bool IsChildSelectable(int groupPosition, int childPosition) => true;

        // --- ViewHolder para grupo ---
        public class GroupViewHolder :  Java.Lang.Object
        {
            public TextView ORDERID;
            public TextView NOMBRECLIENTE;
            public TextView ESTATUSBANCO;
            public TextView ESTADOORDEN;
            public TextView FECHACREACION;
            public TextView FORMADEPAGOYALO;
            public TextView TIENEDISPONIBLE;
            public TextView BLOQUEADA;
            public TextView TOTALAPAGAR;
            public ImageView imgExpand;
            public Button btnFacturacion;
        }
        // --- ViewHolder para hijo ---
        private class ChildViewHolder : Java.Lang.Object
        {
            public TextView IDPRODUCTO;           
            public TextView CANTIDAD;
          //  public TextView DISPONIBLE;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            GroupViewHolder holder;
            if (convertView == null)
            {
                convertView = _context.LayoutInflater.Inflate(Resource.Layout.item_orden_group, null);

                holder = new GroupViewHolder
                {
                    ORDERID = convertView.FindViewById<TextView>(Resource.Id.ORDERID),
                    NOMBRECLIENTE = convertView.FindViewById<TextView>(Resource.Id.NOMBRECLIENTE),
                    ESTATUSBANCO = convertView.FindViewById<TextView>(Resource.Id.ESTATUSBANCO),
                    ESTADOORDEN = convertView.FindViewById<TextView>(Resource.Id.ESTADOORDEN),
                    FORMADEPAGOYALO = convertView.FindViewById<TextView>(Resource.Id.FORMADEPAGOYALO),
                    FECHACREACION = convertView.FindViewById<TextView>(Resource.Id.FECHACREACION),
                    BLOQUEADA = convertView.FindViewById<TextView>(Resource.Id.BLOQUEADA),
                    TIENEDISPONIBLE = convertView.FindViewById<TextView>(Resource.Id.TIENEDISPONIBLE),
                    TOTALAPAGAR = convertView.FindViewById<TextView>(Resource.Id.TOTALAPAGAR),
                    imgExpand = convertView.FindViewById<ImageView>(Resource.Id.imgExpand),
                    btnFacturacion = convertView.FindViewById<Button>(Resource.Id.btnFacturacion)
                };

                holder.btnFacturacion.Click += (s, e) =>
                {
                    var btn = (Button)s;
                    int pos = (int)btn.Tag;
                    ordenYaloMovilSelected = null;
                    ordenYaloMovilSelected = ordenesYalo[pos];                
                    YaloFragment.callyalofragment(ordenYaloMovilSelected);
                    // 👇 aquí ya tienes el ORDERID
                    // Toast.MakeText(_context, $"Botón acción ORDERID: {orderId}", ToastLength.Short).Show();
                };
                holder.imgExpand.Click += (s, e) =>
                {
                    var img = (ImageView)s;
                    int pos = (int)img.Tag;

                    var listView = (ExpandableListView)parent;
                    if (listView.IsGroupExpanded(pos))
                        listView.CollapseGroup(pos);
                    else
                        listView.ExpandGroup(pos);

                    // 👇 aquí puedes obtener el ORDERID
                    var ordenSeleccionada = ordenesYalo[pos];
                    string orderId = ordenSeleccionada.ORDERID;
                    //Toast.MakeText(_context, $"Expand icon ORDERID: {orderId}", ToastLength.Short).Show();
                };
                convertView.Tag = holder;
                
            }
            else
            {
                holder = (GroupViewHolder)convertView.Tag;
            }

            var orden = ordenesYalo[groupPosition];
            holder.imgExpand.Tag = groupPosition;
            holder.btnFacturacion.Tag = groupPosition;

            holder.ORDERID.Text = $"Orden: {orden.ORDERID}";
            holder.NOMBRECLIENTE.Text = $"Cliente: {orden.CTACLIENTE} - {orden.NOMBRECLIENTE}";
            holder.ESTATUSBANCO.Text = $"Estatus de pago: {orden.ESTATUSBANCO}";
            holder.ESTADOORDEN.Text = $"Estado de la orden: {orden.ESTADOORDEN}";
            if (orden.TIENEDISPONIBLE)
            {
                holder.TIENEDISPONIBLE.Text = "Tienes disponible: Sí";
                holder.TIENEDISPONIBLE.SetTextColor(Android.Graphics.Color.Green);
            }
            else
            {
                holder.TIENEDISPONIBLE.Text = "Tienes disponible: No";
                holder.TIENEDISPONIBLE.SetTextColor(Android.Graphics.Color.Red);
            }

            if(orden.FORMADEPAGO == "01" || orden.FORMADEPAGO == "1")
            {
                holder.FORMADEPAGOYALO.Text = "Forma de pago: Efectivo";
            }
            else if(orden.FORMADEPAGO == "02" || orden.FORMADEPAGO == "2")
            {
                holder.FORMADEPAGOYALO.Text = "Forma de pago: Cheque nominativo";
            }
            else if(orden.FORMADEPAGO == "03" || orden.FORMADEPAGO == "3")
            {
                holder.FORMADEPAGOYALO.Text = "Forma de pago: Transferencia electronica de fondos";
            }
            else if (orden.FORMADEPAGO == "04" || orden.FORMADEPAGO == "4")
            {
                holder.FORMADEPAGOYALO.Text = "Forma de pago: Tarjeta de credito";
            }
            else if (orden.FORMADEPAGO == "28" || orden.FORMADEPAGO == "28")
            {
                holder.FORMADEPAGOYALO.Text = "Forma de pago: Tarjeta de debito";
            }
            else if (orden.FORMADEPAGO == "99")
            {
                holder.FORMADEPAGOYALO.Text = "Forma de pago: Por definir";
            }
            else
            {
                holder.FORMADEPAGOYALO.Text = "Forma de pago: NA";
            }

            holder.FECHACREACION.Text = $"Fecha de entrega: {orden.FECHACREACION:d}";
            if (orden.BLOQUEADA)
            {
                holder.BLOQUEADA.Text = "Orden: BLOQUEADA";
            }
            else
            {
                holder.BLOQUEADA.Text = "Orden: LIBERADA";
            }
            if(orden.ESTATUSBANCO == "Pendiente de Pago")
            {
                holder.TOTALAPAGAR.Text = "Total a pagar: $" + orden.TOTALAPAGAR.ToString("##,###.##"); ;
            }
            else
            {
                holder.TOTALAPAGAR.Text = "Total a pagar: Pagado"; // 
            }
            // Setea ícono dinámico
            holder.imgExpand.SetImageResource(isExpanded
                                                ? Android.Resource.Drawable.ArrowUpFloat
                                                : Android.Resource.Drawable.ArrowDownFloat);

            // Guardamos el groupPosition en el Tag del botón para usar en el Click
            holder.imgExpand.Tag = groupPosition;

            return convertView;
        }

        //private async void BtnFacturacion_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var btn = (Button)sender;
        //        int pos = (int)btn.Tag;

        //        var ordenSeleccionada = ordenesYalo[pos];
        //        string orderId = ordenSeleccionada.ORDERID;

        //        // 👇 aquí ya tienes el ORDERID
        //        Toast.MakeText(_context, $"Se va a factura la siguiente orden: {orderId}", ToastLength.Short).Show();
        //        //ObtenerFactura = await proveedorDeVenta.ObtenerFactura(new Factura(orderId));
               
        //    }
        //    catch(Exception ex)
        //    {

        //    }
            
        //}

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            ChildViewHolder holder;
            if (convertView == null)
            {
                convertView = _context.LayoutInflater.Inflate(Resource.Layout.item_linea_child, null);
                holder = new ChildViewHolder
                {
                    IDPRODUCTO = convertView.FindViewById<TextView>(Resource.Id.IDPRODUCTO),
                 //   NOMBREPRODUCTO = convertView.FindViewById<TextView>(Resource.Id.NOMBREPRODUCTO),
                    CANTIDAD = convertView.FindViewById<TextView>(Resource.Id.CANTIDAD),
                 //   DISPONIBLE = convertView.FindViewById<TextView>(Resource.Id.DISPONIBLE)
                };
                convertView.Tag = holder;
            }
            else
            {
                holder = (ChildViewHolder)convertView.Tag;
            }

            var linea = ordenesYalo[groupPosition].LINEAS[childPosition];
            holder.IDPRODUCTO.Text = $"{linea.IDPRODUCTO} - {linea.NOMBREPRODUCTO}";
            holder.CANTIDAD.Text = $"Cant: {linea.CANTIDAD} - Disp: {linea.DISPONIBLE.ToString("##,###.##")} - Precio: {linea.PRECIOUNITARIO:C} - Total: {linea.IMPORTENETO:C}";
            //holder.DISPONIBLE.Text = $"Disp: {linea.DISPONIBLE}";

            convertView.SetBackgroundColor(childPosition % 2 == 0
                                          ? Android.Graphics.Color.ParseColor("#FFFFFF")
                                          : Android.Graphics.Color.ParseColor("#F9F9F9"));

            return convertView;
        }
    }
}