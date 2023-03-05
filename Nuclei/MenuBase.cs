using LemonUI.Menus;

namespace Nuclei;

public abstract class MenuBase : NativeMenu
{
    protected MenuBase(string subtitle, string description) : base("Nuclei", subtitle, description)
    {
    }
}