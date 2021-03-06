﻿using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using System;
using System.Threading.Tasks;

namespace SkiaUiKit.CustomControls
{
    public class Ripple : DropShadow
    {
        private float _currentRadius;
        private double _colorAlphaPercentage;
        private float _pressedAreaX;
        private float _pressedAreaY;
        private SKImageInfo _info;
        private SKCanvasView _canvas;

        private TaskCompletionSource<object> _releasedTask = new TaskCompletionSource<object>();

        public Ripple()
        {
            _canvas = new SKCanvasView
            {
                EnableTouchEvents = true
            };

            _canvas.Touch += _canvas_Touch;
            _canvas.PaintSurface += _canvas_PaintSurface;

            this.Content = _canvas;
        }

        private void _canvas_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            _info = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;
            canvas.Clear();

            var touchLocation = new SKPoint(_pressedAreaX, _pressedAreaY);
            var rippleColor = RippleColor.MultiplyAlpha(0.32).MultiplyAlpha(_colorAlphaPercentage).ToSKColor();
            var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = rippleColor,
                IsStroke = false,
                IsAntialias = true,
                IsDither = true
            };

            var innerRect = new SKRect(0, 0, _info.Width, _info.Height);
            var baseRect = new SKRoundRect(innerRect, this.CornerRadius, this.CornerRadius);

            canvas.ClipRoundRect(baseRect, antialias: true);
            canvas.DrawCircle(touchLocation, _currentRadius, paint);
        }

        private void _canvas_Touch(object sender, SKTouchEventArgs e)
        {
            switch (e.ActionType)
            {
                case SKTouchAction.Pressed:
                    _pressedAreaX = e.Location.X;
                    _pressedAreaY = e.Location.Y;

                    var area = Math.Max(_info.Width, _info.Height) * 1.50;

                    this.Animate("ScaleAnim", new Animation((s) =>
                    {
                        _colorAlphaPercentage = _currentRadius / area;
                        _canvas.InvalidateSurface();
                        _currentRadius = (float)s;
                    }, _currentRadius = (float)(area * 0.04), area), 16, length: 350, easing: Easing.SinOut, finished: async (d, b) =>
                    {
                        await _releasedTask.Task;
                        _releasedTask = new TaskCompletionSource<object>();

                        this.Animate("FadeAnim", new Animation((s) =>
                        {
                            _colorAlphaPercentage = s;
                            _canvas.InvalidateSurface();
                        }, _colorAlphaPercentage, 0), 16, 350, Easing.SinOut);
                    });
                    break;
                case SKTouchAction.Released:
                    if (_releasedTask.Task.Status != TaskStatus.RanToCompletion)
                    {
                        _releasedTask.SetResult(null);
                    }
                    break;
            }

            e.Handled = true;
        }

        public static readonly BindableProperty RippleColorProperty
            = BindableProperty.Create(nameof(RippleColor), typeof(Color), typeof(Ripple), Color.Black);

        public Color RippleColor
        {
            get => (Color)GetValue(RippleColorProperty);
            set => SetValue(RippleColorProperty, value);
        }
    }
}
