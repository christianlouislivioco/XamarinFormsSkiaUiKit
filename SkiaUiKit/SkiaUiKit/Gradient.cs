using SkiaSharp;
using SkiaSharp.Views.Forms;
using SkiaUiKit.CustomControls;
using SkiaUiKit.Enums;
using Xamarin.Forms;

namespace SkiaUiKit
{
    public class Gradient : Ripple
    {
        public static readonly BindableProperty GradientProperty = BindableProperty.Create(nameof(Enums.GradientType), typeof(GradientType), typeof(Gradient));

        public GradientType GradientStyle
        {
            get => (GradientType)GetValue(GradientProperty);
            set => SetValue(GradientProperty, value);
        }

        public static readonly BindableProperty PrimaryColorProperty = BindableProperty.Create(nameof(PrimaryColor), typeof(Color), typeof(Gradient), Color.WhiteSmoke);

        public Color PrimaryColor
        {
            get => (Color)GetValue(PrimaryColorProperty);
            set => SetValue(PrimaryColorProperty, value);
        }

        public static readonly BindableProperty SecondaryColorProperty = BindableProperty.Create(nameof(SecondaryColor), typeof(Color), typeof(Gradient));

        public Color SecondaryColor
        {
            get => (Color)GetValue(SecondaryColorProperty);
            set => SetValue(SecondaryColorProperty, value);
        }

        public SKShader GetGradient(
            GradientType gradientType,
            SKRect rect)
        {
            switch (gradientType)
            {
                case GradientType.CornerToCorner:
                    return SKShader.CreateLinearGradient(
                                 new SKPoint(rect.Left, rect.Top),
                                 new SKPoint(rect.Right, rect.Bottom),
                                 new SKColor[] { this.PrimaryColor.ToSKColor(), this.SecondaryColor.ToSKColor() },
                                 new float[] { 0, 1 },
                                 SKShaderTileMode.Repeat);
                default:
                    return null;
            }
        }
    }
}
