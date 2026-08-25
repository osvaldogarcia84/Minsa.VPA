using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Minsa.VPA.Fragments;
using Minsa.VPA.Modelos;
using Minsa.VPA.Proveedores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minsa.VPA.Adaptadores
{
    public class ClientesPronosticoAdapter : Adaptador<ClientePronostico>
    {
        private readonly List<ClientePronostico> Items;
        private readonly Activity _context;
        public PronosticoFragment PronosticoFragment;
        ResultadoDeOperacionGenerico<string> resultadoEliminar;
        ProveedorDePronostico proveedorDePronostico = new ProveedorDePronostico();
        public ClientesPronosticoAdapter(Activity context, List<ClientePronostico> clientePronosticos, PronosticoFragment pronosticoFragment)
            : base(context, clientePronosticos)
        {
            _context = context;
            Items = clientePronosticos;
            if (ProveedorGlobal.Cliente == null)
                ProveedorGlobal.PosicionClientePronostico = -1;
            PronosticoFragment = pronosticoFragment;
          
        }


        public override long GetItemId(int position)
        {

            return position;
        }
        public void setSelectedIndex(int ind)
        {
            ProveedorGlobal.PosicionClientePronostico = ind;
            NotifyDataSetChanged();
        }

        public override ClientePronostico this[int position]
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
            ClientesPronosticoAdapterViewHolder holder = null;
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.card_ctePronostico, null);

            if (!Items.Any())
                return view;
            ClientePronostico item = Items[position];
            if (item.Cliente == null)
                return view;

            holder = new ClientesPronosticoAdapterViewHolder();

            holder.ClienteIdPronostico = view.FindViewById<TextView>(Resource.Id.ClienteIdPronostico);
            holder.NombreCtePronostico = view.FindViewById<TextView>(Resource.Id.NombreCtePronostico);
            holder.Fecha = view.FindViewById<TextView>(Resource.Id.Fecha);
            holder.IdRecurso = view.FindViewById<TextView>(Resource.Id.IdRecurso);
            holder.Recurso = view.FindViewById<TextView>(Resource.Id.Recurso);
            holder.Volumen = view.FindViewById<TextView>(Resource.Id.Volumen);
            holder.AcessoEditar = view.FindViewById<ImageView>(Resource.Id.AcessoEditar);
            holder.AccesoEliminar = view.FindViewById<ImageView>(Resource.Id.AccesoEliminar);
            holder.ClienteSeleccionadoPronostico = view.FindViewById<ImageView>(Resource.Id.ClienteSeleccionadoPronostico);

            holder.ClienteIdPronostico.Text = "Socio: " + item.Cliente;
            holder.NombreCtePronostico.Text = "Nombre: " + item.NombreCte;
            holder.Fecha.Text = "Fecha: " + item.Fecha;
            holder.IdRecurso.Text = "IdRecurso: " + item.IdRecurso;
            holder.Recurso.Text = "Recurso: " + item.NombreRecurso;
            holder.Volumen.Text = "Volumen: " + item.Volumen;
            holder.AcessoEditar.SetOnClickListener(new ButtonClickListenerPro(this._context, PronosticoFragment));
            holder.AccesoEliminar.SetOnClickListener(new ButtonClickListenerPro(this._context, PronosticoFragment));
            //holder.AccesoEliminar.SetTag(Resource.Id.AccesoEliminar, position);
            //holder.AccesoEliminar.Click -= MyClickEventPronostico;
            //holder.AccesoEliminar.Click += MyClickEventPronostico;

            if (ProveedorGlobal.PosicionClientePronostico != -1 && position == ProveedorGlobal.PosicionClientePronostico)
            {
                holder.ClienteSeleccionadoPronostico.SetImageResource(Resource.Drawable.bookmarkSuccess);
            }
            else
            {
                holder.ClienteSeleccionadoPronostico.SetImageResource(Resource.Drawable.bookmark);
            }

            return view;
        }

        //Fill in cound here, currently 0
        //void MyClickEventPronostico(object sender, EventArgs e)
        //{
        //    int i = (int)((ImageButton)sender).GetTag(Resource.Id.AccesoEliminar);
        //    var posicion = Items[i];
        //    var pronostico = Items[i];
        //    PronosticoFragment.clientesPronostico.RemoveAt(i);
        //    Items.RemoveAt(i);
        //    var eliminar = Task.Run(async () =>
        //    {
        //        resultadoEliminar = await proveedorDePronostico.EliminaPronostico(new DataEliminaPronostico(pronostico.Fecha,PronosticoFragment.vendedor.Valor.Almacen,pronostico.Cliente,PronosticoFragment.vendedor.Valor.Id,pronostico.IdRecurso));
        //    });
        //    eliminar.Wait();
        //    Toast.MakeText(this._context, "Se elimino correctamente tu registro", ToastLength.Short).Show();
        //    return;           
        //    NotifyDataSetChanged();
            
        //}

    }
    

    public class ButtonClickListenerPro : Java.Lang.Object, View.IOnClickListener
    {
        private Activity activity;
        private PronosticoFragment PronosticoFragment;
        public ButtonClickListenerPro(Activity activity, PronosticoFragment pronosticoFragment)
        {
            this.activity = activity;
            PronosticoFragment = pronosticoFragment;
        }
        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.AcessoEditar:
                    if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
                    {
                        Toast.MakeText(this.activity, "No estas conectado a internet para realizar una venta", ToastLength.Short).Show();
                        return;
                    }
                    if (ProveedorGlobal.ClientePronostico == null)
                    {
                        Toast.MakeText(this.activity, "Debes seleccionar un cliente de tu pronostico", ToastLength.Short).Show();
                        return;
                    }
                    //if (ProveedorGlobal.Cliente.Bloqueado)
                    //{
                    //    Toast.MakeText(this.activity, "El cliente está bloqueado.", ToastLength.Short).Show();
                    //    return;
                    //}
                    PronosticoFragment.callfragmentPronostico(1);
                    break;
                case Resource.Id.AccesoEliminar:
                    if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
                    {
                        Toast.MakeText(this.activity, "No estas conectado a internet para realizar esta accion.", ToastLength.Short).Show();
                        return;
                    }
                    if (ProveedorGlobal.ClientePronostico == null)
                    {
                        Toast.MakeText(this.activity, "Debes seleccionar un cliente de tu pronostico.", ToastLength.Short).Show();
                        return;
                    }
                    //PronosticoFragment.callfragmentPronostico(2);
                    //var eliminar = Task.Run(async () =>
                    //        {
                    //            resultadoEliminar = await proveedorDePronostico.EliminaPronostico(new DataEliminaPronostico(ProveedorGlobal.ClientePronostico.Fecha,
                    //                                                                                    vendedor.Valor.Almacen, ProveedorGlobal.ClientePronostico.Cliente,
                    //                                                                                    vendedor.Valor.Id, ProveedorGlobal.ClientePronostico.IdRecurso));
                    //            resultadoCtePronosticoEliminar = await proveedorDePronostico.ConsultaClientePronostico(new DataClientePronostico(ProveedorGlobal.ClientePronostico.Cliente, Configuracion.Compania, vendedor.Valor.Almacen, ObtieneFiltros));
                    //        });
                    //eliminar.Wait();
                    //if (resultadoEliminar.Tipo == TipoDeResultado.Exito)
                    //{
                    //    clientesPronostico = null;
                    //    clientesPronostico = resultadoCtePronosticoEliminar.Valor;
                    //    ListAdapter = new ClientesPronosticoAdapter(Activity, clientesPronostico, this);
                    //    ((BaseAdapter)ListAdapter).NotifyDataSetChanged();
                    //    Snackbar.Make(View, "Se elimino correctamente tu información", Snackbar.LengthLong);
                    //}
                    break;
            }

        }
    }

    public class ClientesPronosticoAdapterViewHolder : Java.Lang.Object
    {
        public TextView ClienteIdPronostico { get; set; }
        public TextView NombreCtePronostico { get; set; }
        public TextView Fecha { get; set; }
        public TextView IdRecurso { get; set; }
        public TextView Recurso { get; set; }
        public TextView Volumen { get; set; }
        public ImageView AcessoEditar { get; set; }
        public ImageView AccesoEliminar { get; set; }
        public ImageView ClienteSeleccionadoPronostico { get; set; }
    }
}