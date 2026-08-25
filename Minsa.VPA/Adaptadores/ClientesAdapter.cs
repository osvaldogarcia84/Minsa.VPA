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
using System;

namespace Minsa.VPA.Adaptadores
{
    public class ClientesAdapter : Adaptador<Cliente>
    {
      //  private ProveedorDeLocacion proveedorDeLocacion;
        private readonly List<Cliente> Items;
        private readonly Activity _context;       
        public RutaFragment RutaFragment;
        public string Visitado = String.Empty;
        public string microcredito;
        public ClientesAdapter(Activity context, List<Cliente> items, RutaFragment rutaFragment)
            : base(context,items)
        {
            _context = context;
            Items = items;
            if (ProveedorGlobal.Cliente == null)
                ProveedorGlobal.PosicionCliente = -1;
        
            RutaFragment = rutaFragment;
            //proveedorDeLocacion = new ProveedorDeLocacion(_context, 0);
            //proveedorDeLocacion.LocacionEncontrada += ActualizarLocacion;
            if (ProveedorGlobal.Lan == null)
            {
                ProveedorGlobal.Lan = new Lan(0, 0);
                //lineaMovil.CoordenadaX = Convert.ToDecimal(ProveedorGlobal.Lan.Longitud);
                //lineaMovil.CoordenadaY = Convert.ToDecimal(ProveedorGlobal.Lan.Latitud);
            }
            //else
            //{
            //    lineaMovil.CoordenadaX = 1;
            //    lineaMovil.CoordenadaY = 1;
            //}
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

        public override Cliente this[int position]
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
            ServiceViewHolderCliente holder = null;
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.card_clientes, null);

            if (!Items.Any())
                return view;

            Cliente item = Items[position];
            if (item.Id == null)
                return view;     

           //var visitado = item.Visitado ? "Sí" : "No";
            //if(item.Visitado == true)
            //{
            //    Visitado = "Sí";
            //}
            //else
            //{
            //    Visitado = "No";
            //}
           var bloqueado = item.Bloqueado ? "Sí" : "No";
            //var prueba = 
            //if (item.MicroCredito == true && item.ValidaCteVsCtaFactCred == true)
            //{
            //     microcredito = "Si";
            //}
            //else
            //{
            //    microcredito = "No";
            //}
           
            // Instancias 
            holder = new ServiceViewHolderCliente();
            holder.ClienteId = view.FindViewById<TextView>(Resource.Id.ClienteId); 
            holder.ClienteNombre = view.FindViewById<TextView>(Resource.Id.ClienteNombre);
            holder.ClienteSecuencia = view.FindViewById<TextView>(Resource.Id.ClienteSecuencia);
            holder.ClienteVisitado = view.FindViewById<TextView>(Resource.Id.ClienteVisitado);
            holder.DireccionCliente = view.FindViewById<AutofitTextView>(Resource.Id.DireccionCliente);
            holder.ClienteSeleccionado = view.FindViewById<ImageView>(Resource.Id.ClienteSeleccionado);
            holder.Telefono = view.FindViewById<TextView>(Resource.Id.ClienteTelefono);
            holder.MicroCredtio = view.FindViewById<TextView>(Resource.Id.MicroCredito);
            // holder.location = view.FindViewById<ImageView>(Resource.Id.location);
            holder.locationGoogleMaps = view.FindViewById<ImageView>(Resource.Id.locationGoogle);
            holder.AccesoDegustacion = view.FindViewById<ImageView>(Resource.Id.AccesoDegustacion);
            holder.AccesoCarrito = view.FindViewById<ImageView>(Resource.Id.AccesoCarrito);
            holder.TipoDeVisita = view.FindViewById<TextView>(Resource.Id.TipoDeVisita);            
            //holder.AccesoSeguimientoVenta = view.FindViewById<ImageView>(Resource.Id.AccesoSeguimientoVenta);            
            //  holder.AccesoSeguimientoVisita = view.FindViewById<ImageView>(Resource.Id.AccesoSeguimientoVisita);
            // holder.AccesoSeguimientoCierreVenta = view.FindViewById<ImageView>(Resource.Id.AccesoCierreVentas);
            holder.BajaCliente = view.FindViewById<ImageView>(Resource.Id.BajaCliente);
            holder.CXC = view.FindViewById<ImageView>(Resource.Id.CXC);
          
            
            //Asignacion
            holder.ClienteId.Text = "Socio: " + item.ClienteId;
            holder.ClienteNombre.Text = "Nombre: " + item.Nombre;            
            holder.ClienteSecuencia.Text = "Secuencia: " + item.Secuencia.ToString();            
            holder.ClienteVisitado.Text = "Visitado: " + Convert.ToString(item.Visitado ? "Sí" : "No");
            holder.Telefono.Text = "Telefono: " + item.Telefono;
            holder.DireccionCliente.Text = "Dirección: " + item.Direccion;
            holder.TipoDeVisita.Text = "Tipo de visita: " + item.TipoDeVisita;
            holder.MicroCredtio.Text ="Micro credito: " + Convert.ToString(item.MicroCredito ? "Si" : "No"); 
            holder.AccesoCarrito.SetOnClickListener(new ButtonClickListener(this._context,  RutaFragment));
            holder.AccesoDegustacion.SetOnClickListener(new ButtonClickListener(this._context, RutaFragment));
            
            holder.CXC.SetOnClickListener(new ButtonClickListener(this._context, RutaFragment));

            holder.locationGoogleMaps.SetOnClickListener(new ButtonClickListener(this._context, RutaFragment));
          //  holder.AccesoSeguimientoVenta.SetOnClickListener(new ButtonClickListener(this._context, RutaFragment));
            holder.BajaCliente.SetOnClickListener(new ButtonClickListener(this._context, RutaFragment));
          //  holder.AccesoSeguimientoCierreVenta.SetOnClickListener(new ButtonClickListener(this._context, RutaFragment));

            if (ProveedorGlobal.PosicionCliente != -1 && position == ProveedorGlobal.PosicionCliente)
            {
                holder.ClienteSeleccionado.SetImageResource(Resource.Drawable.bookmarkSuccess);
            }
            else
            {
                holder.ClienteSeleccionado.SetImageResource(Resource.Drawable.bookmark);
            }          

            return view;
        }
        //private void ActualizarLocacion(object sender, Location e)
        //{
        //    ProveedorGlobal.Lan.Longitud = e.Longitude;
        //    ProveedorGlobal.Lan.Latitud = e.Latitude;
        //}
        
        private class ButtonClickListener : Java.Lang.Object, View.IOnClickListener
        {
            private Activity activity;
            private RutaFragment RutaFragment;
            public ButtonClickListener(Activity activity, RutaFragment rutaFragment)
            {
                this.activity = activity;
                RutaFragment = rutaFragment;
            }
            public void OnClick(View v)
            {
                switch (v.Id)
                {
                    case Resource.Id.AccesoCarrito:
                        if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
                        {
                            Toast.MakeText(this.activity, "No estas conectado a internet para realizar una venta", ToastLength.Short).Show();
                            return;
                        }
                        if (ProveedorGlobal.Cliente == null)
                        {
                            Toast.MakeText(this.activity, "Debes seleccionar un cliente de tu ruta.", ToastLength.Short).Show();
                            return;
                        }
                        //if (ProveedorGlobal.Cliente.Bloqueado)
                        //{
                        //    Toast.MakeText(this.activity, "El cliente está bloqueado.", ToastLength.Short).Show();
                        //    return;
                        //}
                        RutaFragment.callfragment(3);
                        break;
                    case Resource.Id.AccesoDegustacion:
                        if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
                        {
                            Toast.MakeText(this.activity, "No estas conectado a internet para realizar una degustación.", ToastLength.Short).Show();
                            return;
                        }
                        if (ProveedorGlobal.Cliente == null)
                        {
                            Toast.MakeText(this.activity, "Debes seleccionar un cliente de tu ruta.", ToastLength.Short).Show();
                            return;
                        }
                        RutaFragment.callfragment(2);
                        break;
                    case Resource.Id.locationGoogle:
                        if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
                        {
                            Toast.MakeText(this.activity, "No estas conectado a internet para realizar esta acción", ToastLength.Short).Show();
                            return;
                        }
                        if (ProveedorGlobal.Cliente == null)
                        {
                            Toast.MakeText(this.activity, "Debes seleccionar un cliente de tu ruta.", ToastLength.Short).Show();
                            return;
                        }
                        var geoUri = Android.Net.Uri.Parse("geo:" + ProveedorGlobal.Lan.Latitud + "," + ProveedorGlobal.Lan.Longitud + "?q=" + ProveedorGlobal.Cliente.IM_COORDENADAS_Y + "," + ProveedorGlobal.Cliente.IM_COORDENADAS_X);
                        var mapIntent = new Intent(Intent.ActionView, geoUri);
                        mapIntent.SetPackage("com.google.android.apps.maps");
                        activity.StartActivity(mapIntent);
                        break;
                    case Resource.Id.CXC:
                        if (ProveedorGlobal.Cliente == null)
                        {
                            Toast.MakeText(this.activity, "Debes seleccionar un cliente de tu ruta.", ToastLength.Short).Show();
                            return;
                        }
                        RutaFragment.callfragment(4);
                        break;
                    //case Resource.Id.AccesoSeguimientoVenta:
                    //    if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
                    //    {
                    //        Toast.MakeText(this.activity, "No estas conectado a internet para realizar esta acción", ToastLength.Short).Show();
                    //        return;
                    //    }
                    //    if (ProveedorGlobal.Cliente == null)
                    //    {
                    //        Toast.MakeText(this.activity, "Debes seleccionar un cliente de tu ruta.", ToastLength.Short).Show();
                    //        return;
                    //    }                       
                    //     RutaFragment.callfragment(4);                                                  

                    //    break;
                    //case Resource.Id.AccesoSeguimientoVisita:
                    //    if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
                    //    {
                    //        Toast.MakeText(this.activity, "No estas conectado a internet para realizar esta acción", ToastLength.Short).Show();
                    //        return;
                    //    }
                    //    if (ProveedorGlobal.Cliente == null)
                    //    {
                    //        Toast.MakeText(this.activity, "Debes seleccionar un cliente de tu ruta.", ToastLength.Short).Show();
                    //        return;
                    //    }
                    //    RutaFragment.callfragment(5);
                    //   break;
                    case Resource.Id.BajaCliente:
                        if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
                        {
                            Toast.MakeText(this.activity, "No estas conectado a internet para realizar esta acción", ToastLength.Short).Show();
                            return;
                        }
                        if (ProveedorGlobal.Cliente == null)
                        {
                            Toast.MakeText(this.activity, "Debes seleccionar un cliente de tu ruta.", ToastLength.Short).Show();
                            return;
                        }
                        RutaFragment.callfragment(1);
                        break;
                }

            }
        }       
        public class ServiceViewHolderCliente : Java.Lang.Object
        {
            public TextView ClienteId { get; set; }
            public TextView ClienteNombre { get; set; }            
            public TextView ClienteSecuencia { get; set; }            
            public TextView ClienteVisitado { get; set; }            
            public AutofitTextView DireccionCliente { get; set; }
            public TextView MicroCredtio { get; set; }
            public ImageView location { get; set; }
            public ImageView locationGoogleMaps { get; set; }
            public ImageView AccesoDegustacion { get; set; }
            public ImageView AccesoCarrito { get; set; }
            public ImageView CXC { get; set; }
            public TextView TipoDeVisita { get; set; }
           // public ImageView AccesoSeguimientoVenta { get; set; }
            //    public ImageView AccesoSeguimientoVisita { get; set; }
            public ImageView BajaCliente { get; set; }
            public ImageView ClienteSeleccionado { get; set; }   
           // public ImageView AccesoSeguimientoCierreVenta { get; set; }
            public TextView Telefono { get; set; }
        }              

    }
}