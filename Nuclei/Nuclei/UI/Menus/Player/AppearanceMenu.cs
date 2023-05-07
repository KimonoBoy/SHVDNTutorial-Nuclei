using System;
using System.ComponentModel;
using System.Linq;
using GTA;
using Nuclei.Services.Player;
using Nuclei.UI.Menus.Base;
using Nuclei.UI.Menus.Base.ItemFactory;

namespace Nuclei.UI.Menus.Player;

public class AppearanceMenu : GenericMenu<AppearanceService>
{
    public AppearanceMenu(Enum @enum) : base(@enum)
    {
        Shown += OnShown;
        Service.PropertyChanged += OnPropertyChanged;
    }

    private void OnShown(object sender, EventArgs e)
    {
        GenerateAppearanceItems();
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Service.Character)) GenerateAppearanceItems();
    }

    private void GenerateAppearanceItems()
    {
        Clear();

        if (Service.Character == null) return;

        foreach (PedComponentType pedComponentType in Enum.GetValues(typeof(PedComponentType)))
        {
            if (Service.Character.Style[pedComponentType].Count <= 1) continue;
            var listItemPedComponentType = AddListItem(pedComponentType,
                () => Service.Character.Style[pedComponentType].Index, Service,
                (value, index) => { Service.Character.Style[pedComponentType].Index = index; },
                Enumerable.Range(-1, Service.Character.Style[pedComponentType].Count)
                    .Select(index => { return index; })
                    .ToArray());
            listItemPedComponentType.SelectedIndex = Service.Character.Style[pedComponentType].Index;

            var listItemPedComponentTypeVariation = AddListItem("Style", "",
                () => Service.Character.Style[pedComponentType].TextureIndex, Service, (value, index) =>
                {
                    Service.Character.Style[pedComponentType]
                        .SetVariation(Service.Character.Style[pedComponentType].Index, index);
                },
                Enumerable.Range(-1, Service.Character.Style[pedComponentType].TextureCount).Select(index =>
                {
                    return index;
                }).ToArray());

            listItemPedComponentTypeVariation.SelectedIndex = Service.Character.Style[pedComponentType].TextureIndex;

            listItemPedComponentType.ItemChanged += (sender, args) =>
            {
                listItemPedComponentTypeVariation.Items.Clear();
                listItemPedComponentTypeVariation.Items.AddRange(Enumerable.Range(-1,
                    Service.Character.Style[pedComponentType].TextureCount).ToArray());
                listItemPedComponentTypeVariation.SetSelectedIndexSafe(Service.Character.Style[pedComponentType]
                    .TextureIndex);
            };
        }

        foreach (PedPropType pedPropType in Enum.GetValues(typeof(PedPropType)))
        {
            if (Service.Character.Style[pedPropType].Count <= 1) continue;
            var listItemPedComponentType = AddListItem(pedPropType,
                () => Service.Character.Style[pedPropType].Index, Service,
                (value, index) => { Service.Character.Style[pedPropType].Index = index; },
                Enumerable.Range(-1, Service.Character.Style[pedPropType].Count)
                    .Select(index => { return index; })
                    .ToArray());
            listItemPedComponentType.SelectedIndex = Service.Character.Style[pedPropType].Index;
        }
    }
}