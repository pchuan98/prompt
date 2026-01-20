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
        Demo();
        break;
    case "--help" or "-h":
        ShowHelp();
        break;
    default:
        Console.WriteLine($"Unknown option: {args[0]}");
        ShowHelp();
        break;
}

return;

void RunStatusLine()
{
    // TODO: Implement statusline output
    Console.WriteLine("StatusLine running...");
}

void Demo()
{
    // TODO: Implement demo preview
    Console.WriteLine("Demo mode");
}

void ShowHelp()
{
    Console.WriteLine("""
        StatusLine - Claude Code Status Line Plugin

        Usage: StatusLine [options]

        Options:
          --install, -i    Install statusline configuration
          --demo, -d       Preview statusline output
          --help, -h       Show this help message
        """);
}
