using System.Drawing;

namespace Statusline.Utils;

public static class ColorUtil
{
    private const string Reset = "\e[0m";

    public static void WriteReset() => Console.Write(Reset);

    public static string Format(string text, Color fg, Color? bg = null, double opacity = 1.0)
    {
        fg = fg == Color.Empty ? Color.White : fg;

        if (opacity < 1.0)
            fg = Blend(fg, bg ?? Color.Black, opacity);

        var result = $"\e[38;2;{fg.R};{fg.G};{fg.B}m";

        if (bg.HasValue && bg.Value != Color.Empty)
            result += $"\e[48;2;{bg.Value.R};{bg.Value.G};{bg.Value.B}m";

        return result + text + Reset;
    }

    public static void Write(string text, Color fg, Color? bg = null, double opacity = 1.0)
        => Console.Write(Format(text, fg, bg, opacity));

    public static void WriteLine(string text, Color fg, Color? bg = null, double opacity = 1.0)
        => Console.WriteLine(Format(text, fg, bg, opacity));

    private static Color Blend(Color fg, Color bg, double alpha)
    {
        alpha = Math.Clamp(alpha, 0, 1);
        return Color.FromArgb(
            (int)(bg.R * (1 - alpha) + fg.R * alpha),
            (int)(bg.G * (1 - alpha) + fg.G * alpha),
            (int)(bg.B * (1 - alpha) + fg.B * alpha)
        );
    }
}
