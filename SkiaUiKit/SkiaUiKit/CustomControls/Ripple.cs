using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using System;

namespace SkiaUiKit.CustomControls
{
    public class Ripple : SKCanvasView
    {
        float _currentRadius;
        double _colorAlphaPercentage;

        private float _pressedAreaX;
        private float _pressedAreaY;
        private Easing _customEasing;
        private SKImageInfo _info;

        public Ripple()
        {
            this.EnableTouchEvents = true;
        }

        public static readonly BindableProperty ElevationProperty
            = BindableProperty.Create(nameof(Elevation), typeof(float), typeof(CardView), 4f);

        public float Elevation
        {
            get => (float)GetValue(ElevationProperty);
            set => SetValue(ElevationProperty, value);
        }

        protected override void OnTouch(SKTouchEventArgs e)
        {
            switch (e.ActionType)
            {
                case SKTouchAction.Pressed:
                    _pressedAreaX = e.Location.X;
                    _pressedAreaY = e.Location.Y;

                    var area = Math.Max(_info.Width, _info.Height) * 1.25;

                    this.Animate("ScaleAnim", new Animation((s) =>
                    {
                        _colorAlphaPercentage = _currentRadius / area;
                        this.InvalidateSurface();
                        _currentRadius = (float)s;
                    }, _currentRadius = (float)(area * 0.04), area), 16, length: 350, easing: Easing.SinOut, finished: (d, b) => 
                    {
                        this.Animate("FadeAnim", new Animation((s) =>
                        {
                            _colorAlphaPercentage = s;
                            this.InvalidateSurface();
                        }, _colorAlphaPercentage, 0), 16, 350, Easing.SinOut);
                    });
                    break;
            }

            e.Handled = true;
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            _info = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;
            canvas.Clear();

            var touchLocation = new SKPoint(_pressedAreaX, _pressedAreaY);
            var rippleColor = Color.FromHex("#52000000").MultiplyAlpha(_colorAlphaPercentage).ToSKColor();
            var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = rippleColor,
                IsStroke = false,
                IsAntialias = true,
            };

            canvas.DrawCircle(touchLocation, _currentRadius, paint);
        }
    }
}
