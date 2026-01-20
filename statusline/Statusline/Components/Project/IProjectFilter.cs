namespace Statusline.Components.Project;

public interface IProjectFilter
{
    string Name { get; }
    string Icon { get; }

    /// <summary>
    /// Try to detect project in the given directory
    /// </summary>
    /// <returns>Project name if detected, null otherwise</returns>
    string? Detect(string directory);
}
