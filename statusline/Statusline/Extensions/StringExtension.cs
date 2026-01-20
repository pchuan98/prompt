using System.Drawing;
using Statusline.Utils;

namespace Statusline.Extensions;

public readonly struct StyledText(string content, Color fg = default, Color bg = default, double opacity = 1.0)
{
    public string Content { get; } = content;
    public Color Fg { get; } = fg == default ? Color.White : fg;
    public Color Bg { get; } = bg;
    public double Opacity { get; } = opacity;

    public StyledText SetFg(Color color) => new(Content, color, Bg, Opacity);
    public StyledText SetBg(Color color) => new(Content, Fg, color, Opacity);
    public StyledText SetOpacity(double value) => new(Content, Fg, Bg, value);

    public string Render() => ColorUtil.Format(Content, Fg, Bg == default ? null : Bg, Opacity);
    public void Write() => Console.Write(Render());
    public void WriteLine() => Console.WriteLine(Render());

    public static StyledText operator +(StyledText a, StyledText b) => new(a.Render() + b.Render());
    public static implicit operator StyledText(string s) => new(s);
}

public static class StringExtension
{
    public static StyledText Fg(this string s, Color color) => new StyledText(s).SetFg(color);
    public static StyledText Bg(this string s, Color color) => new StyledText(s).SetBg(color);
    public static StyledText Opacity(this string s, double value) => new StyledText(s).SetOpacity(value);

    public static StyledText Fg(this StyledText t, Color color) => t.SetFg(color);
    public static StyledText Bg(this StyledText t, Color color) => t.SetBg(color);
    public static StyledText Opacity(this StyledText t, double value) => t.SetOpacity(value);

    public static void Write(this StyledText[] texts)
    {
        foreach (var t in texts) t.Write();
    }

    public static void WriteLine(this StyledText[] texts)
    {
        foreach (var t in texts) t.Write();
        Console.WriteLine();
    }
}
