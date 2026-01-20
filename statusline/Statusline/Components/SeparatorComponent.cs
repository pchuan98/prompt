using System.Drawing;
using Statusline.Extensions;

namespace Statusline.Components;

public class SeparatorComponent(string text = " | ", double opacity = 0.5) : IComponent
{
    public void Render() => text.Fg(Color.White).Opacity(opacity).Write();
}
