using System.Drawing;
using Statusline.Extensions;
using Statusline.Services;

if (args.Length == 0)
{
    RunStatusLine();
    return;
}

switch (args[0].ToLower())
{
    case "--install" or "-i":
        await Installer.RunAsync();
        break;
    case "--demo" or "-d":
        await Demo.RunAsync();
        break;
    case "--help" or "-h":
        ShowHelp();
        break;
    default:
        $"Unknown option: {args[0]}".Fg(Color.Red).WriteLine();
        ShowHelp();
        break;
}

return;

void RunStatusLine()
{
    // TODO: Implement statusline output
    "StatusLine running...".Fg(Color.Gray).WriteLine();
}

void ShowHelp()
{
    "StatusLine".Fg(Color.Cyan).Write();
    " - Claude Code Status Line Plugin\n".Fg(Color.White).Opacity(0.7).WriteLine();

    "Usage: ".Fg(Color.Yellow).Write();
    "StatusLine [options]\n".Fg(Color.White).Opacity(0.7).WriteLine();

    "Options:".Fg(Color.Yellow).WriteLine();
    "  --install, -i    ".Fg(Color.Green).Write();
    "Install statusline configuration".Fg(Color.White).Opacity(0.7).WriteLine();
    "  --demo, -d       ".Fg(Color.Green).Write();
    "Preview statusline output".Fg(Color.White).Opacity(0.7).WriteLine();
    "  --help, -h       ".Fg(Color.Green).Write();
    "Show this help message".Fg(Color.White).Opacity(0.7).WriteLine();
}
