using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Minsa.VPA.Activities;
using Minsa.VPA.Fragments;
using Minsa.VPA.Modelos;
using Minsa.VPA.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minsa.VPA.Adaptadores
{
    public class TransaccionesAdapterCXC : BaseAdapter<DashboardCXC>
    {
               
        private const int GET_THREE = 2;
        private const int GET_SECOND = 1;
        private const int GET_FIRST = 0;
        public List<DashboardCXC> Items;
        private readonly Activity _context;
        public CXCFragment _cxcFragment;        
        public DashboardCXC item;
        public string Sitio;
        public List<Transacciones> _transacciones;
        public TransaccionesAdapterCXC(Activity context, List<DashboardCXC> items, CXCFragment cxcfragment, string sitio, List<Transacciones> transacciones)
        {

            _context = context;
            Items = items;
            _cxcFragment = cxcfragment;
            Sitio = sitio;
            _transacciones = transacciones;
        }


        public override long GetItemId(int position)
        {
            return position;
        }

        public void setSelectedIndex(int ind)
        {
            ProveedorGlobal.PosicionRecibo = ind;
            NotifyDataSetChanged();
        }
        public override DashboardCXC this[int position]
        {
            get { return Items[position]; }
        }
        public override int Count
        {
            get
            {
                return Items.Count;
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            TransaccionesAdapterCXCViewHolder holder = new TransaccionesAdapterCXCViewHolder();
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.CXCLineas, null);
            if (!Items.Any())
                return view;

            item = Items[position];
            if (item.Factura == null)
                return view;

            holder.ClienteNombre = view.FindViewById<TextView>(Resource.Id.ClienteNombreCXC);
            holder.FacturaCXC = view.FindViewById<TextView>(Resource.Id.FacturasCXC);
            holder.DgVerificadorCXC = view.FindViewById<TextView>(Resource.Id.DgVerificadorCXC);
            holder.ReferenciaCXC = view.FindViewById<TextView>(Resource.Id.ReferenciaCXC);
            holder.DiasTranscurridos = view.FindViewById<TextView>(Resource.Id.DiasTranscurridos);
            holder.MontoOriginal = view.FindViewById<TextView>(Resource.Id.MontoOriginal);
            holder.ImporteVencido = view.FindViewById<TextView>(Resource.Id.ImporteVencido);
            holder.FechaDocumento = view.FindViewById<TextView>(Resource.Id.FechaDocumento);
            holder.FormaPagoCXC = view.FindViewById<TextView>(Resource.Id.FormaPagoCXC);
            holder.PagarTicket = view.FindViewById<ImageView>(Resource.Id.PagarTicket);
            //holder.ImprimirVendedorCXC = view.FindViewById<ImageView>(Resource.Id.ImprimirVendedorCXC);

            var importeVencido = item.ImporteVencido == 0 ? "0.00" : item.ImporteVencido.ToString("##,###.##");

            holder.ClienteNombre.Text = item.Cliente;
            holder.FacturaCXC.Text = item.Factura;
            holder.DgVerificadorCXC.Text = item.DigitoVerificador;
            holder.ReferenciaCXC.Text = item.ClienteFT;
            holder.DiasTranscurridos.Text = Convert.ToString(item.DiasAVencer);
            holder.MontoOriginal.Text = "$" + item.MontoOriginal.ToString("##,###.##");
            holder.ImporteVencido.Text = "$" + importeVencido;
            holder.FechaDocumento.Text = item.FechaDocumento;            
            if (item.FormaDePago == "01")
            {
                holder.FormaPagoCXC.Text = "EFECTIVO";
            }
            else if (item.FormaDePago == "02")
            {
                holder.FormaPagoCXC.Text = "CHEQUE NOMINATIVO";
            }
            else if (item.FormaDePago == "03")
            {
                holder.FormaPagoCXC.Text = "TRANSFERENCIA ELECTRONICA DE FONDOS";
            }
            else if (item.FormaDePago == "04")
            {
                holder.FormaPagoCXC.Text = "TARJETA DE CREDITO";
            }
            else if (item.FormaDePago == "28")
            {
                holder.FormaPagoCXC.Text = "TARJETA DE DEBITO";
            }
            else if (item.FormaDePago == "99")
            {
                holder.FormaPagoCXC.Text = "SIN DEFINIR";
            }
            else
            {
                holder.FormaPagoCXC.Text = "N/A";
            }

            holder.PagarTicket.SetOnClickListener(new ButtonClickListener(this._context, _cxcFragment, item, Sitio, _transacciones));
            //holder.ImprimirVendedorCXC.SetOnClickListener(new ButtonClickListener(this._context, _cxcFragment, item, Sitio, _transacciones));

            return view;
        }
        private class ButtonClickListener : Java.Lang.Object, View.IOnClickListener
        {
            private Activity activity;
            private CXCFragment ReciboFragment;
            private DashboardCXC _dashboard;
            private string Sitio;
            public List<Transacciones> _transaccion;

            public ButtonClickListener(Activity activity, CXCFragment reciboFragment, DashboardCXC dashboard, string Sitio, List<Transacciones> _transaccion)
            {
                this.activity = activity;
                ReciboFragment = reciboFragment;
                _dashboard = dashboard;
                Sitio = Sitio;
                _transaccion = _transaccion;
            }
            public void OnClick(View v)
            {
               
                //if (_dashboard.FormaDePago == "99")
                //{


                //    var activity01 = new Android.Content.Intent(activity, typeof(ReciboValoresActivity));
                //    activity01.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(ReciboFragment.vendedor));
                //    activity01.PutExtra(ReciboValoresActivity.LlaveReciboValores, ProveedorDeSerializado.Generar(_transaccion));
                //    activity01.PutExtra(ReciboValoresActivity.LlaveSitio, ProveedorDeSerializado.Generar(Sitio));
                //    //StartActivity(activity);
                //    activity.StartActivityForResult(activity01, GET_FIRST);
                //}
                //else
                //{
                //    Toast.MakeText(this.activity, "Los tickets que se puede imprimir son pago en efectivo, cheque nominativo, o sin definir.", ToastLength.Long).Show();
                //}
                switch (v.Id)
                {
                    case Resource.Id.PagarTicket:
                        ReciboFragment.callfragmentCXC(1);
                        //if (_dashboard.FormaDePago == "99")
                        //{

                        //    var activity01 = new Android.Content.Intent(activity, typeof(ReciboValoresActivity));
                        //    activity01.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(ReciboFragment.vendedor));
                        //    activity01.PutExtra(ReciboValoresActivity.LlaveReciboValores, ProveedorDeSerializado.Generar(_transaccion));
                        //    activity01.PutExtra(ReciboValoresActivity.LlaveSitio, ProveedorDeSerializado.Generar(Sitio));
                        //    //StartActivity(activity);
                        //    activity.StartActivityForResult(activity01, GET_FIRST);
                        //}
                        //else
                        //{
                        //    Toast.MakeText(this.activity, "Los tickets que se puede imprimir son pago en efectivo, cheque nominativo, o sin definir.", ToastLength.Long).Show();
                        //}
                        break;
                    //case Resource.Id.ImprimirVendedorCXC:
                    //    if (_dashboard.FormaDePago == "01" || _dashboard.FormaDePago == "02" || _dashboard.FormaDePago == "99")
                    //    {
                    //        var activity02 = new Android.Content.Intent(activity, typeof(ReciboValoresVendedorActivity));
                    //        activity02.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(ReciboFragment.vendedor));
                    //        activity02.PutExtra(ReciboValoresVendedorActivity.LlaveReciboValores, ProveedorDeSerializado.Generar(_transaccion));
                    //        activity02.PutExtra(ReciboValoresVendedorActivity.LlaveSitio, ProveedorDeSerializado.Generar(Sitio));
                    //        activity.StartActivityForResult(activity02, GET_SECOND);

                    //    }
                    //    else
                    //    {
                    //        Toast.MakeText(this.activity, "Los tickets que se puede imprimir son pago en efectivo o cheque nominativo, o sin definir.", ToastLength.Long).Show();
                    //    }
                    //    break;
                }

            }

            public static bool IsWithinTime(string stringNowTime, string stringStartTime, string stringEndTime)
            {

                var nowTime = DateTime.Parse(stringNowTime);
                var startTime = DateTime.Parse(stringStartTime);
                var endTime = DateTime.Parse(stringEndTime);

                if ((nowTime <= endTime) && (nowTime >= startTime))
                {
                    return true;
                }

                return false;
            }
        }

    }

    public class TransaccionesAdapterCXCViewHolder : Java.Lang.Object
    {
        public TextView ClienteNombre { get; set; }
        public TextView FacturaCXC { get; set; }
        public TextView DgVerificadorCXC { get; set; }
        public TextView ReferenciaCXC { get; set; }
        public TextView DiasTranscurridos { get; set; }
        public TextView MontoOriginal { get; set; }
        public TextView ImporteVencido { get; set; }
        public TextView FechaDocumento { get; set; }
        public TextView FormaPagoCXC { get; set; }
        public ImageView PagarTicket { get; set; }
      //  public ImageView ImprimirVendedorCXC { get; set; }
    }
}