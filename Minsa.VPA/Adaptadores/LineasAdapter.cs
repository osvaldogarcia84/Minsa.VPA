using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Minsa.VPA.Fragments;
using Minsa.VPA.Modelos;
using Minsa.VPA.Proveedores;
using Minsa.VPA.Servicios;

namespace Minsa.VPA.Adaptadores
{
    public class LineasAdapter : BaseAdapter<LineaMovil>
    {
        public List<LineaMovil> Items;
        private readonly Activity _context;
        public CarritoFragment SetearDatos;
        ProveedorDeClientes proveedorDeClientes = new ProveedorDeClientes();
        ProveedorDeEstrategia proveedorDeEstrategia = new ProveedorDeEstrategia();
        ResultadoDeOperacionGenerico<GpoArticulos> validaGpoArticulos;
        ResultadoDeOperacionGenerico<PromocionBultos> ResultadoPromocionBultos;
        ResultadoDeOperacionGenerico<ArticulosSinDescuentos> resultadoArticulosSinDescuento;
        Cliente Cliente;
        Vendedor Vendedor;
        decimal OperacionPromo = 0;
        decimal SumaCantidad = 0;
        //decimal SumaCantidadAcumulada;
        decimal DescuentoTotal = 0;
  //      decimal TotalNeto;
      //  double ConversionTonelada = 0.02;
        decimal SumaCantidadRemover = 0;
        public LineasAdapter(Activity context, IEnumerable<LineaMovil> items, CarritoFragment setear, Cliente cliente, Vendedor vendedor) : base()
        {
            _context = context;
            Items = items.ToList();
            SetearDatos = setear;
            Cliente = cliente;
            Vendedor = vendedor;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override LineaMovil this[int position]
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
            ServiceViewHolder holder = null;
            OperacionPromo = 0;
           
            LineaMovil tempServiceItem = Items[position];
            if (Items.Count == 0)
                return null;

            View view = convertView;
            if (view == null)
            {
                view = _context.LayoutInflater.Inflate(Resource.Layout.Linea, null);
            }
            //SumaCantidadAcumulada +=  Items.Cantidad;        
            //OperacionPromo = tempServiceItem.Cantidad * ResultadoPromocionBultos.Valor.Descuento;
            //  OperacionPromo = (tempServiceItem.TotalNeto() - OperacionPromo) * tempServiceItem.Cantidad;
            // OperacionPromo = 0;

            //SumaCantidad = 0;
            //for (var i = 0; i < Items.Count; i++)
            //{
            //    var ArtiSinDescuento = Task.Run(async () =>
            //    {
            //        resultadoArticulosSinDescuento = await proveedorDeEstrategia.ArticulosSinDescuentos(new DataArticulosSinDescuentos(Items[i].ArticuloId));
            //    });
            //    ArtiSinDescuento.Wait();
               
            //    if (resultadoArticulosSinDescuento.Valor == null)
            //    {
            //        Promocion(Cliente, Items[i].ArticuloId);
            //    }               
            //}

            //var promo = Task.Run(async () =>
            //{
            //    ResultadoPromocionBultos = await proveedorDeEstrategia.PromocionBultos(new DataPromocionBultos(Cliente.ClienteId, SumaCantidad));
            //});
            //promo.Wait();
            //if (ResultadoPromocionBultos.Tipo == TipoDeResultado.Exito)
            //{
            //    OperacionPromo = ResultadoPromocionBultos.Valor.Descuento;
            //    DescuentoTotal = OperacionPromo * (SumaCantidad * Convert.ToDecimal(ConversionTonelada));
            //    ProveedorGlobal.DescuentoTotalView = DescuentoTotal;
            //    ProveedorGlobal.DescuentoXOrden = ResultadoPromocionBultos.Valor.Descuento;
            //}
            //else 
            //{
            //    DescuentoTotal = 0;
            //    ProveedorGlobal.DescuentoTotalView = 0;
            //    ProveedorGlobal.DescuentoXOrden = 0;
            //}

            
            // Instancias
            holder = new ServiceViewHolder();
            holder.ArticuloId = view.FindViewById<TextView>(Resource.Id.IdProducto);
            holder.Nombre = view.FindViewById<TextView>(Resource.Id.LineasNombreProducto);
            holder.Cantidad = view.FindViewById<TextView>(Resource.Id.CantidadLinea);
            holder.Almacen = view.FindViewById<TextView>(Resource.Id.Almacen);
            holder.Fecha = view.FindViewById<TextView>(Resource.Id.FechaEntrega);
            holder.PrecioNeto = view.FindViewById<TextView>(Resource.Id.PrecioNeto);
            holder.Impuestos = view.FindViewById<TextView>(Resource.Id.Impuestos);
            holder.Importe = view.FindViewById<TextView>(Resource.Id.Importe);
            holder.DeleteButton = view.FindViewById<ImageButton>(Resource.Id.buttonDeleteService);        
            holder.LineaImagen = view.FindViewById<ImageView>(Resource.Id.LineasImagen);


            // Asignacion
            holder.ArticuloId.Text = tempServiceItem.ArticuloId;
            holder.Nombre.Text = tempServiceItem.ArticuloNombre;
            holder.Cantidad.Text = "Cantidad: " + tempServiceItem.Cantidad;
            holder.Almacen.Text = "Almacen: " + tempServiceItem.AlmacenId;
            holder.Fecha.Text = "Fecha de entrega: " + tempServiceItem.FechaDeEntrega;
            
            holder.PrecioNeto.Text = "Precio neto: " + tempServiceItem.TotalNeto().ToString("##,###.##");           

            if (tempServiceItem.Image != null)
                holder.LineaImagen.SetImageDrawable(tempServiceItem.Image);

            var gpo = Task.Run(async () => {
                 validaGpoArticulos = await proveedorDeClientes.GpoArticulos(new GrupoArticulo(tempServiceItem.ArticuloId));
            });
            gpo.Wait();
           
            if (validaGpoArticulos.Tipo == TipoDeResultado.Exito)
            {
                if(validaGpoArticulos.Valor.GrupoArticulo == "110")
                {
                    // ====================================== Articulos especiales ===========================================

                    if (tempServiceItem.ArticuloId == "100110105" || tempServiceItem.ArticuloId == "250110100")
                    {
                        holder.Importe.Text = "Importe: " + tempServiceItem.TotalNeto().ToString("##,###.##");
                        holder.Impuestos.Text = "Impuestos:$ 0 ";
                    }
                    else if (tempServiceItem.ArticuloId == "100110103")
                    {
                        holder.Importe.Text = "Importe: " + tempServiceItem.PrecioFardo450gr().ToString("##,###.##");
                        holder.Impuestos.Text = "Impuestos:$ 0 ";
                    }
                    else
                    {
                        holder.Importe.Text = "Importe: " + tempServiceItem.PrecioFardo().ToString("##,###.##");
                        holder.Impuestos.Text = "Impuestos:$ 0 ";
                    }                     
                }
                else if (validaGpoArticulos.Valor.GrupoArticulo == "109")
                {
                    //view.FindViewById<TextView>(Resource.Id.LineaPrecioPorTonelada).Text = item.PrecioPorTM().ToString("##.###");
                    holder.Importe.Text = "Importe por TM: $ " + tempServiceItem.PrecioPorTM().ToString("##,###.##");
                    holder.Impuestos.Text = "Impuestos:$ 0 ";
                }
                else if (validaGpoArticulos.Valor.GrupoArticulo == "430" || validaGpoArticulos.Valor.GrupoArticulo == "139" || validaGpoArticulos.Valor.GrupoArticulo == "140")
                {
                    holder.Impuestos.Text = "Impuestos:$ " + (tempServiceItem.Impuesto != 0 ? tempServiceItem.Impuestos().ToString("##,###.##") : "0");
                    holder.Importe.Text = "Importe: $ " + tempServiceItem.TotalNetoIVA().ToString("##,###.##");
                }
                else
                {
                    holder.Importe.Text = "Importe: $ " + tempServiceItem.TotalNeto().ToString("##,###.##");
                    holder.Impuestos.Text = "Impuestos:$ 0 ";
                }
            }
            else
            {

                holder.Importe.Text = "Importe por TM: $ " + tempServiceItem.PrecioPorTM().ToString("##,###.##");
            }
           
            holder.DeleteButton.SetTag(Resource.Id.buttonDeleteService, position);
            holder.DeleteButton.Click -= MyClickEvent;
            holder.DeleteButton.Click += MyClickEvent;
            //SetearDatos.CalcularPrecio();
            // CarritoVacio(view, Items);
            return view;
        }

        void MyClickEvent(object sender, EventArgs e)
        {
            int i = (int)((ImageButton)sender).GetTag(Resource.Id.buttonDeleteService);
            var articulo = Items[i];
            SetearDatos._lineas.RemoveAt(i);
            Items.RemoveAt(i);
            var ArtiSinDescuento = Task.Run(async () =>
            {
                resultadoArticulosSinDescuento = await proveedorDeEstrategia.ArticulosSinDescuentos(new DataArticulosSinDescuentos(articulo.ArticuloId));
            });
            ArtiSinDescuento.Wait();

            if (resultadoArticulosSinDescuento.Valor == null)
            {
                RemueveDescuento(Cliente, articulo.ArticuloId, articulo.GrupoArticulo);
            }           
           
            //Promocion(Cliente);
            SetearDatos.Descuento.Text = "Descuento: " + Convert.ToString(ProveedorGlobal.DescuentoTotalView);
            //SetearDatos.DescuentoXOrden = ProveedorGlobal.DescuentoTotalView;
          
            NotifyDataSetChanged();
           // SetearDatos.CalcularPrecio();
        }

        public class ServiceViewHolder : Java.Lang.Object
        {
            public TextView ArticuloId { get; set; }
            public TextView Nombre { get; set; }
            public TextView Cantidad { get; set; }
            public TextView Almacen { get; set; }
            public TextView PrecioNeto { get; set; }
            public ImageView LineaImagen { get; set; }
            public TextView Fecha { get; set; }
            public TextView Impuestos { get; set; }
            public TextView Importe { get; set; }
            public ImageButton DeleteButton { get; set; }           
           
        }
        public void RemueveDescuento(Cliente cliente, string IdArticulo, string GrupoArticulo)
        {
            SumaCantidadRemover = 0;
            foreach (var sumaLineas in Items)
            {
                SumaCantidadRemover = SumaCantidadRemover + sumaLineas.Cantidad;
           //     TotalNeto = TotalNeto + (sumaLineas.PrecioNeto * sumaLineas.Cantidad);
            }

            var removeDescuento = Task.Run(async () =>
            {
                ResultadoPromocionBultos = await proveedorDeEstrategia.PromocionBultos(new DataPromocionBultos(cliente.ClienteId, SumaCantidadRemover, Vendedor.Almacen, Vendedor.Usuario));
            });
            removeDescuento.Wait();
            if (ResultadoPromocionBultos.Tipo == TipoDeResultado.Exito)
            {

                OperacionPromo = ResultadoPromocionBultos.Valor.Descuento;
                DescuentoTotal = OperacionPromo * (SumaCantidadRemover * Convert.ToDecimal(InformacionGeneral.ConversionTonelada));
                ProveedorGlobal.DescuentoTotalView = DescuentoTotal;
                ProveedorGlobal.DescuentoXOrden = ProveedorGlobal.DescuentoXOrden - ResultadoPromocionBultos.Valor.Descuento;
            }
            else
            {
                DescuentoTotal = 0;
                ProveedorGlobal.DescuentoTotalView = 0;
                ProveedorGlobal.DescuentoXOrden = 0;
            }

            //if (ResultadoPromocionBultos.Tipo == TipoDeResultado.Exito)
            //{                
            //        OperacionPromo = ResultadoPromocionBultos.Valor.Descuento;
            //        if (GrupoArticulo == "109" || GrupoArticulo == "110")
            //        {
            //            DescuentoTotal = OperacionPromo * (SumaCantidadRemover * Convert.ToDecimal(InformacionGeneral.ConversionTonelada));
            //        }
            //        else if(GrupoArticulo == "101")
            //        {
            //            DescuentoTotal = OperacionPromo * (SumaCantidadRemover * Convert.ToDecimal(InformacionGeneral.Sacos25KG));
            //        }
            //        else
            //        {
            //            DescuentoTotal = OperacionPromo * (SumaCantidadRemover * Convert.ToDecimal(InformacionGeneral.Pieza));
            //        }                        
            //        ProveedorGlobal.DescuentoTotalView = DescuentoTotal;
            //        ProveedorGlobal.DescuentoXOrden = ProveedorGlobal.DescuentoXOrden - ResultadoPromocionBultos.Valor.Descuento;             
            //}
            //else
            //{
            //    DescuentoTotal = 0;
            //    ProveedorGlobal.DescuentoTotalView = 0;
            //    ProveedorGlobal.DescuentoXOrden = 0;
            //}
        }
        //public void CarritoVacio(View convertView, List<LineaMovil> Items)
        //{
        //    if (Items.Count == 0)
        //    {
        //        ServiceViewHolder holderSN = null;
        //        holderSN = new ServiceViewHolder();
        //        View view = convertView;
        //        holderSN.VaciarCarrito = view.FindViewById<LinearLayout>(Resource.Id.SinArticulosC);
        //        holderSN.VaciarCarrito.Visibility = ViewStates.Visible;
        //    }

        //}

        //public void Add(LineaMovil service)
        //{
        //    Items.Add(service);
        //    this.NotifyDataSetChanged();
        //}

        //public override View GetView(int position, View convertView, ViewGroup parent)
        //{
        //    LineaMovil item = Items[position];
        //    View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.Linea, null);
        //    view.FindViewById<ImageView>(Resource.Id.LineasImagen).SetImageDrawable(item.Image);
        //    view.FindViewById<TextView>(Resource.Id.LineasNombreProductos).Text = item.Nombre;
        //    view.FindViewById<TextView>(Resource.Id.LineasPrecio).Text = "$ " + Convert.ToString(item.Precio);


        //    //var item = Items[position];
        //    //if (Items.Count == 0)
        //    //    return null;

        //    //View view = convertView;
        //    //if (view == null)
        //    //{
        //    //    view = _context.LayoutInflater.Inflate(Resource.Layout.Linea, null);

        //    //    //View view = convertView ?? Context.LayoutInflater.Inflate(Resource.Layout.SpinnerCustom, null);
        //    //    view.FindViewById<ImageView>(Resource.Id.LineasImagen).SetImageDrawable(item.Image);
        //    //    view.FindViewById<TextView>(Resource.Id.LineasNombreProductos).Text = item.Nombre;
        //    //    view.FindViewById<TextView>(Resource.Id.LineasPrecio).Text = "$ " + Convert.ToString(item.Precio);
        //    //    //view.FindViewById<Button>(Resource.Id.Remover).Text = "re";
        //    //    // btnDelete = view.FindViewById<Button>(Resource.Id.Remover);
        //    //    //  remover.SetCompoundDrawablesRelativeWithIntrinsicBounds(Resource.Drawable.remover,0,0,0);
        //    //    //btnDelete.Click += (object sender, EventArgs e) =>
        //    //    //{
        //    //    //    int delPost = (int)(((Button)sender).GetTag(Resource.Id.Remover));
        //    //    //    item.Remove(item[DelPos]);
        //    //    //    //   this.Remove(items[position]);
        //    //    //    //Items.RemoveAt(delPost);
        //    //    //    //  NotifyDataSetChanged();
        //    //    //    //    ////   Items.Remove(Items[position]);
        //    //    //    //    // //  Items.RemoveAt(delPost);    

        //    //    //    //Activity.RunOnUiThread(() =>
        //    //    //    //{
        //    //    //    //    //ListAdapter = new ArrayAdapter<string>(Resource.Layout.Carrito, _lineas);
        //    //    //    //    ((BaseAdapter)this.ListAdapter).NotifyDataSetChanged();
        //    //    //    //});


        //    //    //};
        //    //}

        //    return view;
        //}

    }
}