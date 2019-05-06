using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using System;

namespace SkiaUiKit.CustomControls
{
    public class Button : SKCanvasView
    {
        bool _isAnimating;
        float _currentRadius;
        double _colorAlphaPercentage = 100;

        SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke
        };
        private float _pressedAreaX;
        private float _pressedAreaY;

        public Button()
        {
            this.EnableTouchEvents = true;
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
                        _isAnimating = true;
                        this.InvalidateSurface();
                        _currentRadius = (float)s;
                        _colorAlphaPercentage = _currentRadius / (this.Width * 1.25);
                    }, _currentRadius = (float)((this.Width * 1.25) * 0.05), this.Width * 1.25), 16, length: 300, easing: Easing.SinOut);
                    break;
                case SKTouchAction.Released:
                    this.AbortAnimation("ScaleAnim");
                    this.Animate("FadeAnim", new Animation((s) =>
                    {
                        this.InvalidateSurface();
                        _currentRadius = (float)s;
                        _colorAlphaPercentage = Math.Abs(_currentRadius / (this.Width * 1.25) - 1);
                    }, _currentRadius, this.Width * 1.25), 16, length: 300, easing: Easing.SinOut, finished: (d, b) =>
                    {
                        _currentRadius = (float)((this.Width * 1.25) * 0.05);
                        _colorAlphaPercentage = 100;
                        _isAnimating = false;
                        this.InvalidateSurface();
                    });
                    break;
            }

            e.Handled = true;
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            var canvas = surface.Canvas;
            canvas.Clear();

            if (_isAnimating)
            {
                var center = new SKPoint(_pressedAreaX, _pressedAreaY);
                var rippleColor = Color.FromHex("#52FFFFFF").MultiplyAlpha(_colorAlphaPercentage).ToSKColor();
                paint.Color = rippleColor;
                paint.IsStroke = false;
                paint.IsAntialias = true;
                canvas.DrawCircle(center, _currentRadius, paint);
            }
            else
            {
                var center = new SKPoint(_pressedAreaX, _pressedAreaY);
                var rippleColor = Color.FromHex("#52FFFFFF").MultiplyAlpha(_colorAlphaPercentage).ToSKColor();
                paint.Color = rippleColor;
                paint.IsStroke = false;
                paint.IsAntialias = true;
                canvas.DrawCircle(center, _currentRadius, paint);
                canvas.Clear();
            }
        }
    }
}
