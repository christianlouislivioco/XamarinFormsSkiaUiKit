using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace SkiaUiKit.CustomControls
{
    [ContentProperty("MainContent")]
    public class Card : Gradient
    {
        private SKCanvasView _dropShadowView;
        private Grid _grid;
        private ContentView _mainContent;
        private float _shadowArea;

        public Card()
        {
            _mainContent = new ContentView();
            _dropShadowView = new SKCanvasView();
            _dropShadowView.PaintSurface += CanvasView_PaintSurface;

            HandleShadowArea();
            _mainContent.Margin = this.Padding;

            _grid = new Grid();
            _grid.Children.Add(_dropShadowView);
            _grid.Children.Add(_mainContent);

            this.Content = _grid;
        }

        public View MainContent
        {
            get { return _mainContent.Content; }
            set { _mainContent.Content = value; }
        }

        public static readonly BindableProperty IsClickableProperty = BindableProperty.Create(nameof(IsClickable), typeof(bool), typeof(Card), false);

        public bool IsClickable
        {
            get => (bool)GetValue(IsClickableProperty);
            set => SetValue(IsClickableProperty, value);
        }

        public new static readonly BindableProperty PaddingProperty = BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(Card), new Thickness(20));

        public new Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(this.Elevation))
            {
                HandleShadowArea();
            }
            if (propertyName == nameof(this.Padding))
            {
                _mainContent.Margin = this.Padding;
            }
            if (propertyName == nameof(this.IsClickable))
            {
                if (this.IsClickable)
                {
                    _grid = new Grid();
                    _grid.Children.Add(_dropShadowView);
                    _grid.Children.Add(new Ripple());
                    _grid.Children.Add(_mainContent);
                    this.Content = _grid;
                }
            }
        }

        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var info = e.Info;
            canvas.Clear();

            var shadow = SKImageFilter.CreateDropShadow(
                0f,
                this.Elevation,
                this.Elevation,
                this.Elevation,
                this.ShadowColor,
                SKDropShadowImageFilterShadowMode.DrawShadowAndForeground);

            var innerRect = new SKRect(_shadowArea, _shadowArea, info.Width - _shadowArea, info.Height - _shadowArea);
            var baseRect = new SKRoundRect(innerRect, this.CornerRadius, this.CornerRadius);

            var bgPaint = new SKPaint
            {
                Shader = GetGradient(GradientStyle, innerRect),
                Style = SKPaintStyle.Fill,
                Color = this.PrimaryColor.ToSKColor(),
                BlendMode = SKBlendMode.SrcOver,
                IsAntialias = true,
                ImageFilter = shadow
            };

            canvas.DrawRoundRect(baseRect, bgPaint);
        }

        private void HandleShadowArea()
        {
            if (this.Elevation < 0)
            {
                this.ShadowColor = SKColors.Transparent;
                return;
            }

            _shadowArea = this.Elevation + 8;
            _dropShadowView.Margin = -_shadowArea;
        }
    }
}
