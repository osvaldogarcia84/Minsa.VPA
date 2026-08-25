using System;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Animation;
using Android.Views.Animations;

namespace Minsa.VPA.Views   // <-- Cambia al namespace real de tu app
{
    public class GaugeView : View
    {
        private readonly Paint _bgPaint = new Paint(PaintFlags.AntiAlias);
        private readonly Paint _fgPaint = new Paint(PaintFlags.AntiAlias);
        private readonly Paint _tickPaint = new Paint(PaintFlags.AntiAlias);
        private readonly Paint _textPaint = new Paint(PaintFlags.AntiAlias) { TextAlign = Paint.Align.Center };

        private RectF _oval = new RectF();

        private float _percentage = 0f; // 0..1
        public float Percentage
        {
            get => _percentage;
            set { _percentage = Math.Max(0f, value); Invalidate(); }
        }

        public void SetPercentage(float value) => Percentage = value;

        private ValueAnimator _animator;

        public void AnimateTo(float target, int durationMs = 600)
        {
            target = Math.Max(0f, target);

            if (_animator != null && _animator.IsRunning)
                _animator.Cancel();

            _animator = ValueAnimator.OfFloat(Percentage, target);
            _animator.SetDuration(durationMs);
            _animator.SetInterpolator(new DecelerateInterpolator());
            _animator.Update += (s, e) => { Percentage = (float)e.Animation.AnimatedValue; };
            _animator.Start();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _animator != null)
            {
                if (_animator.IsRunning) _animator.Cancel();
                _animator.Dispose();
                _animator = null;
            }
            base.Dispose(disposing);
        }

        // Colores fijos
        public int ColorRed { get; set; } = Color.ParseColor("#E53935").ToArgb(); // rojo
        public int ColorYellow { get; set; } = Color.ParseColor("#FBC02D").ToArgb(); // amarillo
        public int ColorGreen { get; set; } = Color.ParseColor("#2E7D32").ToArgb(); // verde
        public int ColorBg { get; set; } = Color.ParseColor("#E0E0E0").ToArgb();
        public int ColorTicks { get; set; } = Color.ParseColor("#BDBDBD").ToArgb();
        public int ColorText { get; set; } = Color.Black.ToArgb();

        public float StrokeDp { get; set; } = 16f;
        public float StartAngle { get; set; } = 180f;
        public float SweepAngle { get; set; } = 180f;

        public GaugeView(Context context) : base(context) { Init(); }
        public GaugeView(Context context, IAttributeSet attrs) : base(context, attrs) { Init(); }
        public GaugeView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) { Init(); }

        private void Init()
        {
            float strokePx = TypedValue.ApplyDimension(ComplexUnitType.Dip, StrokeDp, Resources.DisplayMetrics);

            _bgPaint.SetStyle(Paint.Style.Stroke);
            _bgPaint.StrokeCap = Paint.Cap.Round;
            _bgPaint.StrokeJoin = Paint.Join.Round;
            _bgPaint.StrokeWidth = strokePx;
            _bgPaint.Color = new Color(ColorBg);

            _fgPaint.SetStyle(Paint.Style.Stroke);
            _fgPaint.StrokeCap = Paint.Cap.Round;
            _fgPaint.StrokeJoin = Paint.Join.Round;
            _fgPaint.StrokeWidth = strokePx;

            _tickPaint.SetStyle(Paint.Style.Stroke);
            _tickPaint.StrokeCap = Paint.Cap.Butt;
            _tickPaint.StrokeJoin = Paint.Join.Miter;
            _tickPaint.StrokeWidth = TypedValue.ApplyDimension(ComplexUnitType.Dip, 2f, Resources.DisplayMetrics);
            _tickPaint.Color = new Color(ColorTicks);

            _textPaint.Color = new Color(ColorText);
            _textPaint.TextSize = TypedValue.ApplyDimension(ComplexUnitType.Sp, 16f, Resources.DisplayMetrics);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);

            float strokeHalf = _bgPaint.StrokeWidth / 2f;

            float left = strokeHalf + PaddingLeft;
            float top = strokeHalf + PaddingTop;
            float right = w - strokeHalf - PaddingRight;
            float bottom = h - strokeHalf - PaddingBottom;

            float size = Math.Min(right - left, bottom - top);
            float cx = w / 2f;
            float cy = h - strokeHalf - PaddingBottom;
            _oval = new RectF(cx - size / 2f, cy - size, cx + size / 2f, cy);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            // Arco de fondo
            canvas.DrawArc(_oval, StartAngle, SweepAngle, false, _bgPaint);

            float clamped = Math.Max(0f, _percentage);
            float progSweep = SweepAngle * Math.Min(1f, clamped);

            // Selección de color por tramos
            float pct100 = clamped * 100f;
            int color;
            if (pct100 <= 70f) color = ColorRed;
            else if (pct100 <= 97f) color = ColorYellow;
            else color = ColorGreen;

            _fgPaint.Color = new Color(color);
            canvas.DrawArc(_oval, StartAngle, progSweep, false, _fgPaint);

            // Ticks (cada 20%)
            int ticks = 5;
            float centerX = _oval.CenterX();
            float centerY = _oval.Bottom;
            float radiusOuter = _oval.Width() / 2f;
            float radiusInner = radiusOuter - TypedValue.ApplyDimension(ComplexUnitType.Dip, 8f, Resources.DisplayMetrics);

            for (int i = 0; i <= ticks; i++)
            {
                float t = i / (float)ticks;
                float angRad = (float)((StartAngle + SweepAngle * t) * Math.PI / 180.0);
                float x1 = centerX + (float)Math.Cos(angRad) * radiusInner;
                float y1 = centerY + (float)Math.Sin(angRad) * radiusInner;
                float x2 = centerX + (float)Math.Cos(angRad) * radiusOuter;
                float y2 = centerY + (float)Math.Sin(angRad) * radiusOuter;
                canvas.DrawLine(x1, y1, x2, y2, _tickPaint);
            }

            // Aguja
            float needleAngRad = (float)((StartAngle + progSweep) * Math.PI / 180.0);
            float rx = centerX + (float)Math.Cos(needleAngRad) * (radiusOuter - _bgPaint.StrokeWidth);
            float ry = centerY + (float)Math.Sin(needleAngRad) * (radiusOuter - _bgPaint.StrokeWidth);
            using (var needle = new Paint(PaintFlags.AntiAlias) { Color = new Color(Color.ParseColor("#212121")), StrokeWidth = TypedValue.ApplyDimension(ComplexUnitType.Dip, 3f, Resources.DisplayMetrics) })
            {
                canvas.DrawLine(centerX, centerY, rx, ry, needle);
                canvas.DrawCircle(centerX, centerY, TypedValue.ApplyDimension(ComplexUnitType.Dip, 4f, Resources.DisplayMetrics), needle);
            }

            // Texto %
            string label = $"{Math.Min(clamped, 1f) * 100f:0.#}%";
            canvas.DrawText(label, centerX, _oval.Top + (_oval.Height() * 0.6f), _textPaint);
        }
    }
}
