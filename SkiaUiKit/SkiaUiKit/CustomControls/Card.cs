using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace SkiaUiKit.CustomControls
{
    [ContentProperty("MainContent")]
    public class Card : Gradient
    {
        private ContentView _mainContent;
        private float _shadowArea;
        private SKCanvasView _dropShadowView;
        private Grid _grid;

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

        public View MainContent
        {
            get { return _mainContent.Content; }
            set { _mainContent.Content = value; }
        }

        public static readonly BindableProperty ElevationProperty
            = BindableProperty.Create(nameof(Elevation), typeof(float), typeof(Card), 4f);

        public float Elevation
        {
            get => (float)GetValue(ElevationProperty);
            set => SetValue(ElevationProperty, value);
        }

        public static readonly BindableProperty CornerRadiusProperty
            = BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(Card), 20f);

        public float CornerRadius
        {
            get => (float)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly BindableProperty ShadowColorProperty
            = BindableProperty.Create(nameof(ShadowColor), typeof(SKColor), typeof(Card), new SKColor(0, 0, 0, 102));

        public SKColor ShadowColor
        {
            get => (SKColor)GetValue(ShadowColorProperty);
            set => SetValue(ShadowColorProperty, value);
        }

        public new static readonly BindableProperty PaddingProperty
         = BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(Card), new Thickness(20));

        public new Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

        public static readonly BindableProperty IsClickableProperty
          = BindableProperty.Create(nameof(IsClickable), typeof(bool), typeof(Card), false);

        public bool IsClickable
        {
            get => (bool)GetValue(IsClickableProperty);
            set => SetValue(IsClickableProperty, value);
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
    }
}
