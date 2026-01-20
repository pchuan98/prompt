using System.Drawing;
using System.Text.Json;
using System.Text.Json.Nodes;
using Statusline.Extensions;

namespace Statusline.Services;

public static class Installer
{
    public static async Task RunAsync()
    {
        var exePath = (Environment.ProcessPath ?? Path.GetFullPath(Environment.GetCommandLineArgs()[0]))
            .Replace('\\', '/');

        "StatusLine Installation\n".Fg(Color.Cyan).WriteLine();
        "Executable: ".Fg(Color.Gray).Write();
        $"{exePath}\n".Fg(Color.White).WriteLine();

        "Select installation scope:".Fg(Color.Yellow).WriteLine();
        "  [0] ".Fg(Color.Green).Write();
        "Global  (~/.claude/settings.json)".Fg(Color.White).Opacity(0.8).WriteLine();
        "  [1] ".Fg(Color.Green).Write();
        "Current directory (./.claude/settings.json)".Fg(Color.White).Opacity(0.8).WriteLine();

        "\nChoice [0]: ".Fg(Color.Yellow).Write();

        var input = Console.ReadLine()?.Trim();
        var choice = string.IsNullOrEmpty(input) ? 0 : int.TryParse(input, out var n) ? n : -1;

        var targetFile = choice switch
        {
            0 => GetGlobalSettingsPath(),
            1 => GetLocalSettingsPath(),
            _ => null
        };

        if (targetFile == null)
        {
            "Invalid choice.".Fg(Color.Red).WriteLine();
            return;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(targetFile)!);

        var exists = File.Exists(targetFile);
        var settings = exists
            ? JsonNode.Parse(await File.ReadAllTextAsync(targetFile))?.AsObject() ?? []
            : [];

        (exists ? "Updating existing settings.json..." : "Creating new settings.json...")
            .Fg(Color.Gray).WriteLine();

        settings["statusLine"] = new JsonObject
        {
            ["type"] = "command",
            ["command"] = exePath,
            ["padding"] = 0
        };

        await File.WriteAllTextAsync(targetFile, settings.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));

        "\nDone! ".Fg(Color.Green).Write();
        $"Configured at: {targetFile}".Fg(Color.White).Opacity(0.8).WriteLine();
    }

    private static string GetGlobalSettingsPath()
        => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".claude", "settings.json");

    private static string GetLocalSettingsPath()
        => Path.Combine(Environment.CurrentDirectory, ".claude", "settings.json");
}
