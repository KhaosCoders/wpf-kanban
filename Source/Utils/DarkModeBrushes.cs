using System;
using System.Windows.Media;
using KC.WPF_Kanban.Converter;

namespace KC.WPF_Kanban.Utils;

internal static class DarkModeBrushes
{
    internal static Brush Convert(Brush source, DarkModeBrushRole role)
    {
        if (source is SolidColorBrush solid)
        {
            var result = solid.CloneCurrentValue();
            result.Color = ConvertColor(solid.Color, role);
            result.Freeze();
            return result;
        }
        if (source is GradientBrush gradient)
        {
            var result = gradient.CloneCurrentValue();
            foreach (GradientStop stop in result.GradientStops)
            {
                stop.Color = ConvertColor(stop.Color, role);
            }
            result.Freeze();
            return result;
        }
        return source;
    }

    private static Color ConvertColor(Color color, DarkModeBrushRole role)
    {
        if (color.A == 0)
        {
            return color;
        }

        double r = ToLinear(color.R), g = ToLinear(color.G), b = ToLinear(color.B);
        double luminance = 0.2126 * r + 0.7152 * g + 0.0722 * b;
        double target;
        switch (role)
        {
            case DarkModeBrushRole.Background:
                target = 0.015 + (1 - luminance) * 0.07;
                break;
            case DarkModeBrushRole.Foreground:
                // Every dark solid surface has L <= .085. L >= .75 gives
                // text contrast > 5.9:1, including arbitrary custom color pairs.
                target = Math.Max(0.75, luminance);
                break;
            case DarkModeBrushRole.Accent:
                target = Math.Max(0.3, luminance);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(role));
        }

        double Adjust(double channel) => luminance > target
            ? channel * target / luminance
            : channel + (1 - channel) * (target - luminance) / (1 - luminance);

        if (target == luminance)
        {
            return color;
        }
        return Color.FromArgb(color.A, ToSrgb(Adjust(r)), ToSrgb(Adjust(g)), ToSrgb(Adjust(b)));
    }

    private static double ToLinear(byte value)
    {
        double channel = value / 255.0;
        return channel <= 0.04045 ? channel / 12.92 : Math.Pow((channel + 0.055) / 1.055, 2.4);
    }

    private static byte ToSrgb(double value) =>
        (byte)Math.Round(255 * (value <= 0.0031308 ? 12.92 * value : 1.055 * Math.Pow(value, 1 / 2.4) - 0.055));
}
