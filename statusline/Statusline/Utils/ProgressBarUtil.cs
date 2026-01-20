using System.Drawing;

namespace Statusline.Utils;

public static class ProgressBarUtil
{
    public const string EmptyBlock = "░";
    public const string FillBlock = "▓";
    public const string FullBlock = "█";

    public static string Render(double value, int width = 20, Func<double, Color>? colorMap = null,
        string fillChar = FullBlock, string emptyChar = EmptyBlock, double opacity = 1.0)
    {
        value = Math.Clamp(value, 0, 1);
        var result = "";

        for (var i = 0; i < width; i++)
        {
            var pos = (double)i / (width - 1);
            var isFilled = pos <= value;
            var color = colorMap?.Invoke(pos) ?? Color.Green;
            var ch = isFilled ? fillChar : emptyChar;

            result += isFilled
                ? ColorUtil.Format(ch, color, opacity: opacity)
                : ColorUtil.Format(ch, Color.Gray, opacity: opacity * 0.5);
        }

        return result;
    }

    public static void Write(double value, int width = 20, Func<double, Color>? colorMap = null,
        string fillChar = FullBlock, string emptyChar = EmptyBlock, double opacity = 1.0)
        => Console.Write(Render(value, width, colorMap, fillChar, emptyChar, opacity));

    // Preset color maps
    public static Color Green2Red(double d)
        => Color.FromArgb(255, 160, 255 - (int)(250 * d), 0);

    public static Color Red(double d)
        => Color.FromArgb(255, (int)(200 * d) + 50, 10, 10);
}
