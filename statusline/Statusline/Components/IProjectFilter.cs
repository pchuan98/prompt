namespace Statusline.Components;

public interface IProjectFilter
{
    string Name { get; }
    string Icon { get; }

    /// <summary>
    /// Try to detect project in the given directory
    /// </summary>
    /// <param name="directory">Directory to search</param>
    /// <returns>Project name if detected, null otherwise</returns>
    string? Detect(string directory);
}
