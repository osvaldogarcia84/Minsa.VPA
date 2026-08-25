using System;
using System.Collections.Generic;
using System.Globalization;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Minsa.VPA.Views; // <-- GaugeView (ajusta a tu namespace)
using Android.App;
using Minsa.VPA.Modelos;

namespace Minsa.VPA.Adapters   // <-- Ajusta al namespace real
{
    // Modelo de ejemplo: ajusta a tu clase real
    //public class HistoricosVenta
    //{
    //    public string Sitio { get; set; }
    //    public string NombreVendedor { get; set; }
    //    public decimal VentaActual { get; set; }
    //    public decimal Presupuesto { get; set; }
    //    public decimal VentaMesAnterior { get; set; }
    //    public decimal VentaUltimosTresMeses { get; set; }
    //}

    public class HistoricoVentaAdapter : BaseAdapter<HistoricosVenta>
    {
        private readonly Context _context;
        public List<HistoricosVenta> Items { get; }

        public HistoricoVentaAdapter(Activity context, List<HistoricosVenta> items)
        {
            _context = context;
            Items = items ?? new List<HistoricosVenta>();
        }

        public override HistoricosVenta this[int position] => Items[position];
        public override int Count => Items.Count;
        public override long GetItemId(int position) => position;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder;
            var view = convertView;

            if (view == null)
            {
                view = LayoutInflater.From(_context).Inflate(Resource.Layout.cardHistoricoVenta, parent, false);
                holder = new ViewHolder(view);
                view.Tag = holder;
            }
            else
            {
                holder = (ViewHolder)view.Tag;
            }

            var item = Items[position];

            // ====== Textos base (ajusta IDs si difieren en tu XML) ======
            holder.txtSitios.Text = item.Sitio;
            holder.txtVendedorNombre.Text = item.NombreVendedor;
            holder.txtVentaActual.Text = $"Venta actual: {item.VentaActual.ToString("##,###.##")}";
            holder.txtPresupuesto.Text = $"Presupuesto: {item.Presupuesto.ToString("##,###.##")}";
            holder.txtVentaMesAnterior.Text = $"Venta mes anterior: {item.VentaMesAnterior.ToString("##,###.##")}";
            holder.txtVentaUltTresMeses.Text = $"Venta últimos tres meses: {item.VentaUltimosTresMeses.ToString("##,###.##")}";

            // ====== Gauge ======
            if (holder.gauge != null)
            {
                float cumplimiento = 0f;
                if (item.Presupuesto > 0)
                    cumplimiento = (float)(item.VentaActual / item.Presupuesto);

                // Para listas: anima solo cuando se crea la vista; en rebind pinta directo
                if (convertView == null)
                    holder.gauge.AnimateTo(cumplimiento, 700);
                else
                    holder.gauge.SetPercentage(cumplimiento);
            }

            // ====== Detalle numérico + Badge ======
            var mx = new CultureInfo("es-MX");
            string Mon(decimal v) => v.ToString("C0", mx); // usa C2 para centavos

            decimal venta = item.VentaActual;
            decimal presup = item.Presupuesto;

            if (presup <= 0)
            {
                holder.txtDetalleCumplimiento.Text = $"{Mon(venta)} de — (0%)";
                SetBadge(holder.txtEstadoCumplimiento, "Sin presupuesto", Color.ParseColor("#757575")); // gris
            }
            else
            {
                var pct = (venta / presup) * 100m;
                holder.txtDetalleCumplimiento.Text = $"{Mon(venta)} de {Mon(presup)} ({pct:0.#}%)";

                bool cumplido = pct >= 100m;
                if (cumplido)
                    SetBadge(holder.txtEstadoCumplimiento, "Cumplido", Color.ParseColor("#2E7D32")); // verde
                else
                    SetBadge(holder.txtEstadoCumplimiento, "Pendiente", Color.ParseColor("#E53935")); // rojo

                // (Opcional) mostrar faltante cuando esté pendiente:
                // if (!cumplido) holder.txtDetalleCumplimiento.Text += $"  |  Faltan {Mon(presup - venta)}";
            }

            return view;
        }

        // ====== ViewHolder ======
        private class ViewHolder : Java.Lang.Object
        {
            public TextView txtSitios;
            public TextView txtVendedorNombre;
            public TextView txtVentaActual;
            public TextView txtPresupuesto;
            public TextView txtVentaMesAnterior;
            public TextView txtVentaUltTresMeses;

            public GaugeView gauge;
            public TextView txtDetalleCumplimiento;
            public TextView txtEstadoCumplimiento;

            public ViewHolder(View view)
            {
                // Ajusta los IDs a los que tengas en tu cardHistoricoVenta.xml
                txtSitios = view.FindViewById<TextView>(Resource.Id.txtSitios);
                txtVendedorNombre = view.FindViewById<TextView>(Resource.Id.VendedorNombre);
                txtVentaActual = view.FindViewById<TextView>(Resource.Id.VentaActual);
                txtPresupuesto = view.FindViewById<TextView>(Resource.Id.Presupuesto);
                txtVentaMesAnterior = view.FindViewById<TextView>(Resource.Id.VentaMesAnterior);
                txtVentaUltTresMeses = view.FindViewById<TextView>(Resource.Id.VentaUltimosTresMeses);

                gauge = view.FindViewById<GaugeView>(Resource.Id.gaugeVenta);
                txtDetalleCumplimiento = view.FindViewById<TextView>(Resource.Id.txtDetalleCumplimiento);
                txtEstadoCumplimiento = view.FindViewById<TextView>(Resource.Id.txtEstadoCumplimiento);
            }
        }

        // ====== Helper de badge ======
        private void SetBadge(TextView badge, string text, Color backColor)
        {
            if (badge == null) return;

            badge.Text = text;
            badge.SetTextColor(Color.White);

            var gd = new GradientDrawable();
            gd.SetShape(ShapeType.Rectangle);
            gd.SetColor(backColor);

            float radius = TypedValue.ApplyDimension(ComplexUnitType.Dip, 12f, badge.Resources.DisplayMetrics);
            gd.SetCornerRadius(radius);

            badge.Background = gd;
        }
    }
}
