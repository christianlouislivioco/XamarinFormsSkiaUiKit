using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace SkiaUiKit.CustomControls
{
    public class Label : Gradient
    {
        private SKImageInfo _info;
        private SKRect _textBounds;

        public Label()
        {
            var canvasView = new SKCanvasView();
            canvasView.PaintSurface += CanvasView_PaintSurface;
            Content = canvasView;
        }

        public static readonly BindableProperty TextProperty
            = BindableProperty.Create(nameof(Text), typeof(string), typeof(Label));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty IsFitToScaleProperty
          = BindableProperty.Create(nameof(IsFitToScale), typeof(bool), typeof(Label), false);

        public bool IsFitToScale
        {
            get => (bool)GetValue(IsFitToScaleProperty);
            set => SetValue(IsFitToScaleProperty, value);
        }

        public static readonly BindableProperty FontSizeProperty
            = BindableProperty.Create(nameof(FontSize), typeof(float), typeof(Label), 14f);

        public float FontSize
        {
            get => (float)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static readonly BindableProperty HorizontalTextAlignmentProperty
            = BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(Label), TextAlignment.Start);

        public TextAlignment HorizontalTextAlignment
        {
            get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty);
            set => SetValue(HorizontalTextAlignmentProperty, value);
        }

        public static readonly BindableProperty VerticalTextAlignmentProperty
          = BindableProperty.Create(nameof(VerticalTextAlignment), typeof(TextAlignment), typeof(Label), TextAlignment.Start);

        public TextAlignment VerticalTextAlignment
        {
            get => (TextAlignment)GetValue(VerticalTextAlignmentProperty);
            set => SetValue(VerticalTextAlignmentProperty, value);
        }

        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            _info = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();

            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.TextSize = this.FontSize;

                if (this.IsFitToScale)
                {
                    var width = paint.MeasureText(this.Text);
                    var scale = 1f * _info.Width / width;
                    paint.TextSize *= scale;

                    this.HorizontalTextAlignment = TextAlignment.Center;
                    this.VerticalTextAlignment = TextAlignment.Center;
                }

                _textBounds = new SKRect();
                paint.MeasureText(this.Text, ref _textBounds);

                var xText = GetTextPositionX();
                var yText = GetTextPositionY();
                _textBounds.Offset(xText, yText);

                paint.Shader = GetGradient(this.GradientStyle, _textBounds);

                canvas.DrawText(this.Text, xText, yText, paint);

                this.WidthRequest = _textBounds.Width;
                this.HeightRequest = _textBounds.Height;
            }
        }

        private float GetTextPositionX()
        {
            switch (this.HorizontalTextAlignment)
            {
                case TextAlignment.Center:
                    return _info.Width / 2 - _textBounds.MidX;
                case TextAlignment.End:
                    return _info.Width - _textBounds.Width;
                default:
                    return 0;
            }
        }

        private float GetTextPositionY()
        {
            switch (this.VerticalTextAlignment)
            {
                case TextAlignment.Center:
                    return _info.Height / 2 - _textBounds.MidY;
                case TextAlignment.End:
                    return _info.Height - _textBounds.Height;
                default:
                    return 0;
            }
        }
    }
}
