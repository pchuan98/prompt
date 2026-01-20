using System.Text.Json;
using System.Text.Json.Nodes;

namespace Statusline.Services;

public static class Installer
{
    public static async Task RunAsync()
    {
        var exePath = (Environment.ProcessPath ?? Path.GetFullPath(Environment.GetCommandLineArgs()[0]))
            .Replace('\\', '/');

        Console.WriteLine("StatusLine Installation\n");
        Console.WriteLine($"Executable: {exePath}\n");
        Console.WriteLine("Select installation scope:");
        Console.WriteLine("  [0] Global  (~/.claude/settings.json)");
        Console.WriteLine("  [1] Current directory (./.claude/settings.json)");
        Console.Write("\nChoice [0]: ");

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
            Console.WriteLine("Invalid choice.");
            return;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(targetFile)!);

        var settings = File.Exists(targetFile)
            ? JsonNode.Parse(await File.ReadAllTextAsync(targetFile))?.AsObject() ?? []
            : [];

        Console.WriteLine(File.Exists(targetFile) ? "Updating existing settings.json..." : "Creating new settings.json...");

        settings["statusLine"] = new JsonObject
        {
            ["type"] = "command",
            ["command"] = exePath,
            ["padding"] = 0
        };

        await File.WriteAllTextAsync(targetFile, settings.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
        Console.WriteLine($"\nDone! Configured at: {targetFile}");
    }

    private static string GetGlobalSettingsPath()
        => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".claude", "settings.json");

    private static string GetLocalSettingsPath()
        => Path.Combine(Environment.CurrentDirectory, ".claude", "settings.json");
}
