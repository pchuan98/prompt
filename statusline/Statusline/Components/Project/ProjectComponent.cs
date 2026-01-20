using System.Diagnostics;
using System.Drawing;
using Statusline.Components.Project.Filters;
using Statusline.Extensions;

namespace Statusline.Components.Project;

public class ProjectComponent : IComponent
{
    private readonly List<IProjectFilter> _filters = [new SolutionFilter()];

    public string? Directory { get; set; }
    public int MaxLength { get; set; } = 30;
    public Color IconColor { get; set; } = Color.Purple;
    public double Opacity { get; set; } = 0.8;

    public void Render()
    {
        var directory = Directory ?? Environment.CurrentDirectory;

        // 1. Try filters first
        foreach (var filter in _filters)
        {
            var result = filter.Detect(directory);
            if (result != null)
            {
                filter.Icon.Fg(IconColor).Opacity(Opacity).Write();
                $" {Truncate(result)}".Fg(Color.White).Opacity(Opacity).Write();
                return;
            }
        }

        // 2. Try git root
        var gitRoot = GetGitRoot(directory);
        if (gitRoot != null)
        {
            "".Fg(IconColor).Opacity(Opacity).Write();
            $" {Truncate(Path.GetFileName(gitRoot))}".Fg(Color.White).Opacity(Opacity).Write();
            return;
        }

        // 3. Fallback to current folder
        var folderName = Path.GetFileName(directory);
        if (!string.IsNullOrEmpty(folderName))
        {
            "".Fg(IconColor).Opacity(Opacity).Write();
            $" {Truncate(folderName)}".Fg(Color.White).Opacity(Opacity).Write();
        }
    }

    private string Truncate(string text)
    {
        if (text.Length <= MaxLength) return text;

        var half = (MaxLength - 3) / 2;
        return $"{text[..half]}...{text[^half..]}";
    }

    private static string? GetGitRoot(string directory)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = "rev-parse --show-toplevel",
                WorkingDirectory = directory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            if (process == null) return null;

            var output = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit(2000);

            return process.ExitCode == 0 ? output : null;
        }
        catch
        {
            return null;
        }
    }

    public void AddFilter(IProjectFilter filter) => _filters.Insert(0, filter);
}
