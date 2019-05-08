using SkiaSharp;
using SkiaSharp.Views.Forms;
using SkiaUiKit.CustomControls;
using SkiaUiKit.Enums;
using Xamarin.Forms;

namespace SkiaUiKit
{
    public class Gradient : Ripple
    {
        public static readonly BindableProperty GradientStyleProperty 
            = BindableProperty.Create(nameof(Enums.GradientType), typeof(GradientType), typeof(Gradient));

        public GradientType GradientStyle
        {
            get => (GradientType)GetValue(GradientStyleProperty);
            set => SetValue(GradientStyleProperty, value);
        }

        public static new readonly BindableProperty BackgroundColorProperty 
            = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(Gradient), Color.WhiteSmoke);

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public static readonly BindableProperty SecondaryColorProperty 
            = BindableProperty.Create(nameof(SecondaryColor), typeof(Color), typeof(Gradient));

        public Color SecondaryColor
        {
            get => (Color)GetValue(SecondaryColorProperty);
            set => SetValue(SecondaryColorProperty, value);
        }

        public SKShader GetShader(
            GradientType gradientType,
            SKRect rect)
        {
            switch (gradientType)
            {
                case GradientType.CornerToCorner:
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
    }
}
