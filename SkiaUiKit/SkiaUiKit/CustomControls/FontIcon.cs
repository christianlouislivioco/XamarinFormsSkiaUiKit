using System.Runtime.CompilerServices;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace SkiaUiKit.CustomControls
{
    public class FontIcon : ContentView
    {
        private const double DefaultFontIconSize = 24;
        private SKCanvasView _canvasView = new SKCanvasView();

        public FontIcon()
        {
            this.WidthRequest = DefaultFontIconSize;
            this.HeightRequest = DefaultFontIconSize;

            _canvasView.PaintSurface += Canvas_PaintSurface;
            this.Content = _canvasView;
        }

        private void Canvas_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var info = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();

            using (var textPaint = new SKPaint())
            {
                textPaint.IsAntialias = true;
                textPaint.Color = this.Color.ToSKColor();
                textPaint.Typeface = SKTypeface.FromFile(this.Typeface);

                var textWidth = textPaint.MeasureText(this.Glyph);
                textPaint.TextSize = this.GlyphSize;

                var textBounds = new SKRect();
                textPaint.MeasureText(this.Glyph, ref textBounds);

                float xText = info.Width / 2 - textBounds.MidX;
                float yText = info.Height / 2 - textBounds.MidY;

                canvas.DrawText(this.Glyph, xText, yText, textPaint);
            }
        }

        public static readonly BindableProperty GlyphProperty
            = BindableProperty.Create(nameof(Glyph), typeof(string), typeof(FontIcon), default(string));

        public string Glyph
        {
            get => (string)GetValue(GlyphProperty);
            set => SetValue(GlyphProperty, value);
        }

        public static readonly BindableProperty TypefaceProperty
            = BindableProperty.Create(nameof(Typeface), typeof(string), typeof(FontIcon), "Assets/fontello.ttf");

        public string Typeface
        {
            get => (string)GetValue(TypefaceProperty);
            set => SetValue(TypefaceProperty, value);
        }

        public static readonly BindableProperty ColorProperty
            = BindableProperty.Create(nameof(Color), typeof(Color), typeof(FontIcon), Color.Black);

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public static readonly BindableProperty GlyphSizeProperty 
            = BindableProperty.Create(nameof(GlyphSize), typeof(float), typeof(FontIcon), (float)DefaultFontIconSize);

        public float GlyphSize
        {
            get => (float)GetValue(GlyphSizeProperty);
            set => SetValue(GlyphSizeProperty, value);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(this.Glyph))
            {
                _canvasView.InvalidateSurface();
            }
        }
    }
}
