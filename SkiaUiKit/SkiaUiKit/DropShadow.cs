using SkiaSharp;
using Xamarin.Forms;

namespace SkiaUiKit
{
    public class DropShadow : ContentView
    {
        public static readonly BindableProperty ElevationProperty = BindableProperty.Create(nameof(Elevation), typeof(float), typeof(DropShadow), 4f);

        public float Elevation
        {
            get => (float)GetValue(ElevationProperty);
            set => SetValue(ElevationProperty, value);
        }

        public static readonly BindableProperty CornerRadiusProperty
            = BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(DropShadow), 20f);

        public float CornerRadius
        {
            get => (float)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly BindableProperty ShadowColorProperty
            = BindableProperty.Create(nameof(ShadowColor), typeof(SKColor), typeof(DropShadow), new SKColor(0, 0, 0, 102));

        public SKColor ShadowColor
        {
            get => (SKColor)GetValue(ShadowColorProperty);
            set => SetValue(ShadowColorProperty, value);
        }
    }
}
