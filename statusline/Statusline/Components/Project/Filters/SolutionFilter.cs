namespace Statusline.Components.Project.Filters;

public class SolutionFilter : IProjectFilter
{
    public string Name => "Solution";
    public string Icon => "󰌛";

    public string? Detect(string directory)
    {
        var files = Directory.GetFiles(directory, "*.sln")
            .Concat(Directory.GetFiles(directory, "*.slnx"))
            .ToArray();

        if (files.Length == 0) return null;

        return files.Length == 1
            ? Path.GetFileNameWithoutExtension(files[0])
            : $"{files.Length} solutions";
    }
}
