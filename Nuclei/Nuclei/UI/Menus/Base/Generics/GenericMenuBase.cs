using System;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Services.Generics;

namespace Nuclei.UI.Menus.Abstracts.Generics;

public abstract class GenericMenuBase<TService> : MenuBase where TService : GenericService<TService>, new()
{
    protected TService Service = GenericService<TService>.Instance;


    protected GenericMenuBase(string subtitle, string description) : base(subtitle,
        description)
    {
    }

    protected GenericMenuBase(Enum @enum) : this(@enum.ToPrettyString(),
        @enum.GetDescription())
    {
    }
}