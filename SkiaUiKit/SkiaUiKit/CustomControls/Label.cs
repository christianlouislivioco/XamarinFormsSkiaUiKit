using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace SkiaUiKit.CustomControls
{
    public class Label : Gradient
    {
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


        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var info = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();

            using (SKPaint paint = new SKPaint())
            {
                paint.TextSize = 14;

                if (this.IsFitToScale)
                {
                    float width = paint.MeasureText(this.Text);
                    float scale = 0.9f * info.Width / width;
                    paint.TextSize *= scale;
                }
   
                paint.IsAntialias = true;

                var textBounds = new SKRect();
                paint.MeasureText(this.Text, ref textBounds);

                float xText = info.Width / 2 - textBounds.MidX;
                float yText = info.Height / 2 - textBounds.MidY;

                textBounds.Offset(xText, yText);

                paint.Shader = GetGradient(this.GradientStyle, textBounds);

                canvas.DrawText(this.Text, xText, yText, paint);
            }
        }
    }
}
