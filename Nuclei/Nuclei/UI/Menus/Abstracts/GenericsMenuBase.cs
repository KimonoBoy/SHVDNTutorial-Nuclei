using System;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Services.Generics;

namespace Nuclei.UI.Menus.Abstracts;

public abstract class GenericsMenuBase<TService> : MenuBase where TService : GenericService<TService>, new()
{
    protected TService Service = GenericService<TService>.Instance;

    protected GenericsMenuBase(string subtitle, string description) : base(subtitle, description)
    {
    }

    protected GenericsMenuBase(Enum @enum) : this(@enum.ToPrettyString(), @enum.GetDescription())
    {
    }
}