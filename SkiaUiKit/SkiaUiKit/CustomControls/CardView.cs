using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace SkiaUiKit.CustomControls
{
    [ContentProperty("MainContent")]
    public class CardView : ContentView
    {
        private ContentView _mainContent;
        private float _shadowArea;
        private SKCanvasView _dropShadowView;

        public CardView()
        {
            _mainContent = new ContentView();
            _dropShadowView = new SKCanvasView();
            _dropShadowView.PaintSurface += CanvasView_PaintSurface;

            HandleShadowArea();
            _mainContent.Padding = this.Padding;

            var grid = new Grid();
            grid.Children.Add(_dropShadowView);
            grid.Children.Add(_mainContent);
            this.Content = grid;
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
            = BindableProperty.Create(nameof(Elevation), typeof(float), typeof(CardView), 4f);

        public float Elevation
        {
            get => (float)GetValue(ElevationProperty);
            set => SetValue(ElevationProperty, value);
        }

        public static readonly BindableProperty CornerRadiusProperty
            = BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(CardView), 20f);

        public float CornerRadius
        {
            get => (float)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public new static readonly BindableProperty BackgroundColorProperty
            = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(CardView), Color.WhiteSmoke);

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public static readonly BindableProperty SecondaryColorProperty
            = BindableProperty.Create(nameof(SecondaryColor), typeof(Color), typeof(CardView));

        public Color SecondaryColor
        {
            get => (Color)GetValue(SecondaryColorProperty);
            set => SetValue(SecondaryColorProperty, value);
        }

        public static readonly BindableProperty ShadowColorProperty
            = BindableProperty.Create(nameof(ShadowColor), typeof(SKColor), typeof(CardView), new SKColor(0, 0, 0, 102));

        public SKColor ShadowColor
        {
            get => (SKColor)GetValue(ShadowColorProperty);
            set => SetValue(ShadowColorProperty, value);
        }

        public new static readonly BindableProperty PaddingProperty
         = BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(CardView), new Thickness(20));

        public new Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

        public static readonly BindableProperty GradientProperty
           = BindableProperty.Create(nameof(GradientType), typeof(GradientType), typeof(CardView));

        public GradientType Gradient
        {
            get => (GradientType)GetValue(GradientProperty);
            set => SetValue(GradientProperty, value);
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
                Shader = GetGradient(Gradient, innerRect),
                Style = SKPaintStyle.Fill,
                Color = this.BackgroundColor.ToSKColor(),
                BlendMode = SKBlendMode.SrcOver,
                IsAntialias = true,
                ImageFilter = shadow
            };

            canvas.DrawRoundRect(baseRect, bgPaint);
        }

        private SKShader GetGradient(GradientType gradientType, SKRect rect)
        {
            switch (gradientType)
            {
                case GradientType.Linear:
                    return SKShader.CreateLinearGradient(
                                 new SKPoint(rect.Left, rect.Top),
                                 new SKPoint(rect.Right, rect.Bottom),
                                 new SKColor[] { this.BackgroundColor.ToSKColor(), this.SecondaryColor.ToSKColor() },
                                 new float[] { 0, 1 },
                                 SKShaderTileMode.Repeat);
                default:
                    return null;
            }
        }

        public enum GradientType
        {
            None,
            Linear
        }
    }
}
