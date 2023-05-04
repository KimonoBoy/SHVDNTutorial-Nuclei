using System;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Services.Generics;

namespace Nuclei.UI.Menus.Base;

public abstract class GenericMenu<TService> : MenuBase where TService : GenericService<TService>, new()
{
    protected GenericMenu(string subtitle, string description) : base(subtitle, description)
    {
    }

    protected GenericMenu(Enum @enum) : this(@enum.ToPrettyString(), @enum.GetDescription())
    {
    }

    protected TService Service => GenericService<TService>.Instance;
}