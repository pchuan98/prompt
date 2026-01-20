using System.Drawing;
using Statusline.Components;
using Statusline.Components.Project;
using Statusline.Extensions;
using Statusline.Utils;

namespace Statusline.Services;

public static class Demo
{
    public static async Task RunAsync()
    {
        Console.CursorVisible = false;
        Console.Clear();

        DisplayColors();
        Console.WriteLine();

        var progressLine = Console.CursorTop;

        try
        {
            while (!Console.KeyAvailable)
            {
                for (var i = 0; i <= 100 && !Console.KeyAvailable; i += 2)
                {
                    DrawProgress(progressLine, i / 100.0);
                    await Task.Delay(30);
                }

                for (var i = 100; i >= 0 && !Console.KeyAvailable; i -= 2)
                {
                    DrawProgress(progressLine, i / 100.0);
                    await Task.Delay(30);
                }
            }
        }
        finally
        {
            Console.CursorVisible = true;
            Console.WriteLine("\n");
        }
    }

    private static void DrawProgress(int line, double value)
    {
        Console.SetCursorPosition(0, line);
        "Progress: ".Fg(Color.White).Opacity(0.7).Write();
        ProgressBarUtil.Write(value, width: 30, colorMap: ProgressBarUtil.Green2Red, opacity: 0.7);
        $" {value * 100,3:F0}%".Fg(Color.White).Opacity(0.6).Write();
    }

    private static void DisplayColors()
    {
        "=== StatusLine Demo ===\n".Fg(Color.Cyan).WriteLine();

        "Basic Colors:".Fg(Color.White).WriteLine();
        Color[] basics = [Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Cyan, Color.Magenta];
        foreach (var c in basics)
            $" ██ {c.Name,-8}".Fg(c).Write();
        Console.WriteLine("\n");

        "Gradient (Green -> Red):".Fg(Color.White).WriteLine();
        for (var i = 0; i <= 40; i++)
            "█".Fg(ProgressBarUtil.Green2Red(i / 40.0)).Write();
        Console.WriteLine("\n");

        "Opacity Levels:".Fg(Color.White).WriteLine();
        for (var i = 20; i >= 1; i--)
            "██".Fg(Color.Cyan).Opacity(i / 20.0).Write();
        Console.WriteLine("\n");

        // Project Component
        "Project Component:".Fg(Color.White).WriteLine();
        new ProjectComponent().Render();
        Console.WriteLine("\n");

        // StatusLine Builder
        "StatusLine Builder:".Fg(Color.White).WriteLine();
        StatusLine.Create()
            .AddProject()
            .AddSeparator()
            .AddProject(p => p.MaxLength = 15)
            .Render();
        Console.WriteLine("\n");

        "Press any key to exit...\n".Fg(Color.Gray).Opacity(0.5).WriteLine();
    }
}
