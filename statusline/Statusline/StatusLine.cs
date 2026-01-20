namespace Statusline;

public interface IComponent
{
    void Render();
}

public static class StatusLine
{
    public static List<IComponent> Create() => [];
}
