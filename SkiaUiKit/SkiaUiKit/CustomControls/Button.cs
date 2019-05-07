using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using System;

namespace SkiaUiKit.CustomControls
{
    public class Button : SKCanvasView
    {
        bool _isReleased;
        float _currentRadius;
        double _colorAlphaPercentage;

        private float _pressedAreaX;
        private float _pressedAreaY;
        private Easing _customEasing;

        public Button()
        {
            this.EnableTouchEvents = true;
            _customEasing = new Easing((val) =>
            {
                return (val * val) * (3.0 - 2.0 * val);
            });
        }

        protected override void OnTouch(SKTouchEventArgs e)
        {
            switch (e.ActionType)
            {
                case SKTouchAction.Pressed:
                    _pressedAreaX = e.Location.X;
                    _pressedAreaY = e.Location.Y;

                    this.Animate("ScaleAnim", new Animation((s) =>
                    {
                        //_colorAlphaPercentage = _currentRadius / (this.Width * 1.25);
                        _isReleased = true;
                        this.InvalidateSurface();
                        _currentRadius = (float)s;
                    }, _currentRadius = (float)((this.Width * 1.25) * 0.05), this.Width * 1.25), 16, length: 600, easing: _customEasing);
                    break;
                case SKTouchAction.Released:
                    //this.Animate("FadeAnim", new Animation((s) =>
                    //{
                    //    this.InvalidateSurface();
                    //    _currentRadius = (float)s;
                    //    _colorAlphaPercentage = Math.Abs(_currentRadius / (this.Width * 1.25) - 1);
                    //}, _currentRadius, this.Width * 1.25), 4, length: 500, easing: Easing.SinOut, finished: (d, b) =>
                    //{
                    //    _currentRadius = (float)((this.Width * 1.25) * 0.05);
                    //    _colorAlphaPercentage = 100;
                    //    _isReleased = false;
                    //    this.InvalidateSurface();
                    //});
                    break;
            }

            e.Handled = true;
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            var canvas = surface.Canvas;
            canvas.Clear();

            var touchLocation = new SKPoint(_pressedAreaX, _pressedAreaY);
            //var rippleColor = Color.FromHex("#52000000").MultiplyAlpha(_colorAlphaPercentage).ToSKColor();
            var rippleColor = Color.FromHex("#1A000000").ToSKColor();
            var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = rippleColor,
                IsStroke = false,
                IsAntialias = true,
            };

            canvas.DrawCircle(touchLocation, _currentRadius, paint);

            if (!_isReleased)
            {
                canvas.Clear();
            }
        }
    }
}
