using Statusline.Components.Project;

namespace Statusline.Components;

public static class ComponentExtensions
{
    public static List<IComponent> AddSeparator(this List<IComponent> list, string text = " | ", double opacity = 0.5)
    {
        list.Add(new SeparatorComponent(text, opacity));
        return list;
    }

    public static List<IComponent> AddProject(this List<IComponent> list, Action<ProjectComponent>? configure = null)
    {
        var component = new ProjectComponent();
        configure?.Invoke(component);
        list.Add(component);
        return list;
    }

    public static void Render(this List<IComponent> list)
    {
        foreach (var component in list)
            component.Render();
    }
}
