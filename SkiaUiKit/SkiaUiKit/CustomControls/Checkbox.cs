using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Label = Xamarin.Forms.Label;

namespace SkiaUiKit.CustomControls
{
    public class Checkbox : StackLayout
    {
        private double _colorAlphaPercentage;
        private double _currentRadius;
        private SKCanvasView _ripple;
        private TaskCompletionSource<object> _releasedTask = new TaskCompletionSource<object>();
        private FontIcon _fontIcon;

        public Checkbox()
        {
            this.Spacing = 0;
            this.Orientation = StackOrientation.Horizontal;

            var tap = new TapGestureRecognizer();
            this.GestureRecognizers.Add(tap);

            tap.Tapped += Tap_Tapped;

            _ripple = new SKCanvasView()
            {
                EnableTouchEvents = true
            };

            _ripple.PaintSurface += Ripple_PaintSurface;
            _ripple.Touch += Ripple_Touch;

            var grid = new Grid()
            {
                WidthRequest = 48,
                HeightRequest = 48,
            };

            _fontIcon = new FontIcon() { VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, Glyph = "\uE846" };
            grid.Children.Add(_fontIcon);
            grid.Children.Add(_ripple);

            this.Children.Add(grid);
            this.Children.Add(new Label() { Text = "Check me!", VerticalOptions = LayoutOptions.Center });
        }

        private void Tap_Tapped(object sender, System.EventArgs e)
        {
            if (IsChecked == true)
            {
                IsChecked = false;
                _fontIcon.Glyph = "\uE846";
            }
            else
            {
                IsChecked = true;
                _fontIcon.Glyph = "\uE845";
            }
        }

        private void Ripple_Touch(object sender, SKTouchEventArgs e)
        {
            switch (e.ActionType)
            {
                case SKTouchAction.Pressed:
                    var area = 24;

                    (this).Animate("ScaleAnim",
                                   new Animation((s) => RippleAction(s, area), _currentRadius = (float)(area * 0.08), area),
                                   16,
                                   length: 250,
                                   easing: Easing.SinOut,
                                   finished: async (d, b) => await FadeAsync());
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

        private async Task FadeAsync()
        {
            await _releasedTask.Task;
            _releasedTask = new TaskCompletionSource<object>();

            (this).Animate("FadeAnim",
                           new Animation((s) => FadeAction(s), _colorAlphaPercentage, 0),
                           16,
                           250,
                           Easing.SinOut);
        }

        private void RippleAction(double s, int area)
        {
            _colorAlphaPercentage = _currentRadius / area;
            _ripple.InvalidateSurface();
            _currentRadius = (float)s;
        }

        private void FadeAction(double s)
        {
            _colorAlphaPercentage = s;
            _ripple.InvalidateSurface();
        }

        private void Ripple_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            var canvas = surface.Canvas;
            canvas.Clear();

            var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.FromHex("#0060ac").MultiplyAlpha(0.32).MultiplyAlpha(_colorAlphaPercentage).ToSKColor(),
                IsStroke = false,
                IsAntialias = true,
                IsDither = true
            };

            canvas.DrawCircle(24, 24, (float)_currentRadius, paint);
        }

        public static readonly BindableProperty IsCheckedProperty
          = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(FontIcon), default(bool));

        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }
    }
}
