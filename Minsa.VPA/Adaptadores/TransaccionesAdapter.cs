using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Grantland.Widget;
using Minsa.VPA.Activities;
using Minsa.VPA.Fragments;
using Minsa.VPA.Modelos;
using Minsa.VPA.Proveedores;
using Minsa.VPA.Servicios;

namespace Minsa.VPA.Adaptadores
{
    public class TransaccionesAdapter : BaseAdapter<Transacciones>
    {
        private const int GET_THREE = 2;
        private const int GET_SECOND = 1;
        private const int GET_FIRST = 0;
        public List<Transacciones> Items;
        private readonly Activity _context;
        public ReciboFragment _reciboFragment;
        public Transacciones item;
        public string Sitio;
        //private ResultadoDeOperacionGenerico<int> digitoVerificador;
        //ProveedorDeClientes proveedorDeCliente = new ProveedorDeClientes();
        public TransaccionesAdapter(Activity context, List<Transacciones> items, ReciboFragment reciboFragment, string sitio)
        {
            _context = context;
            Items = items;
            _reciboFragment = reciboFragment;
            Sitio = sitio;
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
        public override Transacciones this[int position]
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
            ServiceViewHolderImprimir holder = new ServiceViewHolderImprimir();
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.ReciboLineas, null);
            if (!Items.Any())
                return view;

            item = Items[position];
            if (item.ItemId == null)
                return view;
           // var cuentaFacturacion = item.CuentaFacturacion.Substring(0, 8);
           // digitoVerificador = proveedorDeCliente.DigitoVerificador(new AccountNum(cuentaFacturacion));

            holder.ClienteTransacciones = view.FindViewById<TextView>(Resource.Id.ClienteTransacciones); //.Text = item.Nombre;
            holder.FacturaTransacciones = view.FindViewById<TextView>(Resource.Id.FacturaTransacciones); //.Text = item.Factura;
            holder.DgVerificador = view.FindViewById<TextView>(Resource.Id.DgVerificador);//.Text = item.DigitoVerificador;
            holder.CtaDep = view.FindViewById<TextView>(Resource.Id.CtaDep); //.Text = item.CuentaFacturacion;
            holder.Bultos = view.FindViewById<TextView>(Resource.Id.Bultos); //.Text = string.Format("{0:n0}", Convert.ToString(item.Bultos));
            holder.Precio = view.FindViewById<TextView>(Resource.Id.Precio); //.Text = "$" + item.Precio.ToString("##,###.##");
            holder.ImporteT = view.FindViewById<TextView>(Resource.Id.ImporteT); //.Text = "$" + item.Importe.ToString("##,###.##");

            holder.ImprimirCliente = view.FindViewById<ImageView>(Resource.Id.ImprimirCliente);
            holder.ImprimirVendedor = view.FindViewById<ImageView>(Resource.Id.ImprimirVendedor);
            holder.HoraCreacion = view.FindViewById<TextView>(Resource.Id.HoraCreacion); //.Text = "$" + item.Importe.ToString("##,###.##");
            holder.FormaPago = view.FindViewById<TextView>(Resource.Id.PagoT); 
            holder.TipoDeVenta = view.FindViewById<TextView>(Resource.Id.TipoDeVenta);

            holder.CODI = view.FindViewById<ImageView>(Resource.Id.CODI);
       

            holder.ClienteTransacciones.Text = item.Nombre;
            holder.FacturaTransacciones.Text = item.Factura;
            holder.DgVerificador.Text = item.DigitoVerificador;
            holder.CtaDep.Text = item.CuentaFacturacion;
            holder.Bultos.Text = string.Format("{0:n0}", Convert.ToString(item.Bultos));
            holder.Precio.Text = item.Precio.ToString("##,###.##");
            holder.ImporteT.Text = "$" + item.Importe.ToString("##,###.##");
            holder.HoraCreacion.Text = item.HoraCreacion;      
            if(item.FormaPago == "01")
            {
                holder.FormaPago.Text = "EFECTIVO";
                
            }else if(item.FormaPago == "02")
            {
                holder.FormaPago.Text = "CHEQUE NOMINATIVO";
            }else if(item.FormaPago == "03")
            {
                holder.FormaPago.Text = "TRANSFERENCIA ELECTRONICA DE FONDOS";
            }
            else if (item.FormaPago == "04")
            {
                holder.FormaPago.Text = "TARJETA DE CREDITO";
            }
            else if (item.FormaPago == "28")
            {
                holder.FormaPago.Text = "TARJETA DE DEBITO";
            }
            else if (item.FormaPago == "99")
            {
                holder.FormaPago.Text = "SIN DEFINIR";
            }
            else
            {
                holder.FormaPago.Text = "N/A";
            }
            
            if(item.FormaPago == "99")
            {
                holder.TipoDeVenta.Text = "Credito";
            }
            else
            {
                holder.TipoDeVenta.Text = "Normal";
            }

            holder.ImprimirCliente.SetOnClickListener(new ButtonClickListener(this._context, _reciboFragment , item, Sitio));
            holder.ImprimirVendedor.SetOnClickListener(new ButtonClickListener(this._context, _reciboFragment, item, Sitio));
            holder.CODI.SetOnClickListener(new ButtonClickListener(this._context, _reciboFragment, item, Sitio));
            return view;
        }

        public class ServiceViewHolderImprimir : Java.Lang.Object
        {
            public TextView ClienteTransacciones { get; set; }
            public TextView FacturaTransacciones { get; set; }
            public TextView DgVerificador { get; set; }
            public TextView CtaDep { get; set; }
            public TextView Bultos { get; set; }
            public TextView Precio { get; set; }
            public TextView ImporteT { get; set; }
            public ImageView ImprimirCliente { get; set; }
            public ImageView ImprimirVendedor { get; set; }            
            public ImageView CODI { get; set; }
            public TextView HoraCreacion { get; set; }
            public TextView FormaPago { get; set; }
            public TextView TipoDeVenta { get; set; }
        }

        private class ButtonClickListener : Java.Lang.Object, View.IOnClickListener
        {
            private Activity activity;
            private ReciboFragment ReciboFragment;
            private Transacciones _transacciones;
            private string Sitio;
            public ButtonClickListener(Activity activity, ReciboFragment reciboFragment, Transacciones transacciones, string Sitio)
            {
                this.activity = activity;
                ReciboFragment = reciboFragment;
                _transacciones = transacciones;
                Sitio = Sitio;
            }
            public void OnClick(View v)
            {
                switch (v.Id)
                {
                    case Resource.Id.ImprimirCliente:

                        if (_transacciones.FormaPago == "01" || _transacciones.FormaPago == "02" || _transacciones.FormaPago == "99") 
                        {
                            //if (IsWithinTime(DateTime.Now.ToString("HH:mm:ss"), _transacciones.HoraCreacion, _transacciones.HoraMaxReimpresion))
                            //{
                                var activity01 = new Android.Content.Intent(activity, typeof(ReciboValoresActivity));
                                activity01.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(ReciboFragment.vendedor));
                                activity01.PutExtra(ReciboValoresActivity.LlaveReciboValores, ProveedorDeSerializado.Generar(_transacciones));
                                activity01.PutExtra(ReciboValoresActivity.LlaveSitio, ProveedorDeSerializado.Generar(Sitio));
                                //StartActivity(activity);
                                activity.StartActivityForResult(activity01, GET_FIRST);

                            //}
                            //else
                            //{
                            //    Toast.MakeText(this.activity, "Se agoto el tiempo limite para reimprimir. Llama al Centro de soluciones para solicitar una reimpresión", ToastLength.Long).Show();
                            //}
                        }
                        else
                        {
                                Toast.MakeText(this.activity, "Los tickets que se puede imprimir son pago en efectivo, cheque nominativo, o sin definir.", ToastLength.Long).Show();
                        }                        
                        break;
                    case Resource.Id.ImprimirVendedor:
                        if (_transacciones.FormaPago == "01" || _transacciones.FormaPago == "02" || _transacciones.FormaPago == "99") 
                        {
                            //if (IsWithinTime(DateTime.Now.ToString("HH:mm:ss"), _transacciones.HoraCreacion, _transacciones.HoraMaxReimpresion))
                            //{
                                var activity02 = new Android.Content.Intent(activity, typeof(ReciboValoresVendedorActivity));
                                activity02.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(ReciboFragment.vendedor));
                                activity02.PutExtra(ReciboValoresVendedorActivity.LlaveReciboValores, ProveedorDeSerializado.Generar(_transacciones));
                                activity02.PutExtra(ReciboValoresVendedorActivity.LlaveSitio, ProveedorDeSerializado.Generar(Sitio));
                                //StartActivity(activity);
                                activity.StartActivityForResult(activity02, GET_SECOND);
                            //}
                            //else
                            //{
                            //    Toast.MakeText(this.activity, "Se agoto el tiempo limite para reimprimir. Llama al Centro de soliciones para solicitar una reimpresión", ToastLength.Long).Show();
                            //}
                        }
                        else
                        {
                                Toast.MakeText(this.activity, "Los tickets que se puede imprimir son pago en efectivo o cheque nominativo, o sin definir.", ToastLength.Long).Show();
                        }                                                
                        break;
                    case Resource.Id.CODI:

                //        Toast.MakeText(this.activity, "Este metodo no esta disponible, se encuentra en construccion.", ToastLength.Long).Show();
                        var activityCODI = new Android.Content.Intent(activity, typeof(CODIActivity));
                        activityCODI.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(ReciboFragment.vendedor));
                        activityCODI.PutExtra(CODIActivity.LlaveCODI, ProveedorDeSerializado.Generar(_transacciones));
                        activity.StartActivityForResult(activityCODI, GET_THREE);
                        break;
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
}