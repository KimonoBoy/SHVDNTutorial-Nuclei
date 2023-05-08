using System;
using System.ComponentModel;
using System.Linq;
using GTA;
using Nuclei.Enums.UI;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Services.Player;
using Nuclei.UI.Menus.Base;

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
        if (Service.Character == null) return;
        if (Service.Character.Style.GetAllComponents().Length < 1 && Service.Character.Style.GetAllProps().Length < 1)
        {
            if (!Visible) return;
            NavigateToMenu(MenuTitle.Player);
            return;
        }

        Clear();

        GenerateComponents();
        AddHeader("Props");
        GenerateProps();
    }

    private void GenerateComponents()
    {
        foreach (var componentType in Enum.GetValues(typeof(PedComponentType)).Cast<PedComponentType>())
        {
            var currentIndex = Service.Character.Style[componentType].Index;
            var currentTextureIndex = Service.Character.Style[componentType].TextureIndex;

            var componentCount = Service.Character.Style[componentType].Count;
            if (componentCount < 1) continue;
            var listItemComponent = AddListItem(
                componentType.GetLocalizedDisplayNameFromHash() + " Style",
                $"Change {componentType} component",
                () => Service.Character.Style[componentType].Index,
                Service,
                (value, index) => Service.Character.Style[componentType].Index = value,
                Enumerable.Range(0, componentCount).ToArray()
            );
            listItemComponent.SelectedIndex = currentIndex;

            var textureCount = Service.Character.Style[componentType].TextureCount;
            var listItemVariation = AddListItem(
                componentType.GetLocalizedDisplayNameFromHash() + " Texture",
                "Change texture for " + componentType,
                () => Service.Character.Style[componentType].TextureIndex,
                Service,
                (value, index) => Service.Character.Style[componentType].TextureIndex = value,
                Enumerable.Range(0, textureCount).ToArray()
            );
            listItemVariation.SelectedIndex = currentTextureIndex;

            if (textureCount > 1)
                listItemVariation.Enabled = true;
            else
                listItemVariation.Enabled = false;

            listItemComponent.ItemChanged += (sender, args) =>
            {
                textureCount = Service.Character.Style[componentType].TextureCount;
                listItemVariation.Items.Clear();
                listItemVariation.Items.AddRange(Enumerable
                    .Range(0, textureCount).ToArray());
                if (textureCount > 1)
                {
                    listItemVariation.SelectedIndex = 0;
                    listItemVariation.Enabled = true;
                }
                else
                {
                    listItemVariation.Enabled = false;
                }
            };
        }
    }

    private void GenerateProps()
    {
        foreach (var propType in Enum.GetValues(typeof(PedPropType)).Cast<PedPropType>())
        {
            var currentIndex = Service.Character.Style[propType].Index;
            var currentTextureIndex = Service.Character.Style[propType].TextureIndex;

            var propCount = Service.Character.Style[propType].Count;
            if (propCount <= 1) continue;
            var listItemProp = AddListItem(
                propType.GetLocalizedDisplayNameFromHash() + " Style",
                $"Change {propType} component",
                () => Service.Character.Style[propType].Index,
                Service,
                (value, index) => Service.Character.Style[propType].Index = value,
                Enumerable.Range(0, propCount).ToArray()
            );
            if (listItemProp.Items.Count > 0) listItemProp.SelectedIndex = currentIndex;

            var textureCount = Service.Character.Style[propType].TextureCount;
            var listItemVariation = AddListItem(
                propType.GetLocalizedDisplayNameFromHash() + " Texture",
                "Change texture for " + propType,
                () => Service.Character.Style[propType].TextureIndex,
                Service,
                (value, index) => Service.Character.Style[propType].TextureIndex = value,
                Enumerable.Range(0, textureCount > 1 ? textureCount : 0).ToArray()
            );
            if (listItemVariation.Items.Count > 0) listItemVariation.SelectedIndex = currentTextureIndex;

            if (textureCount > 1)
                listItemVariation.Enabled = true;
            else
                listItemVariation.Enabled = false;

            listItemProp.ItemChanged += (sender, args) =>
            {
                textureCount = Service.Character.Style[propType].TextureCount;
                listItemVariation.Items.Clear();
                listItemVariation.Items.AddRange(Enumerable
                    .Range(0, textureCount).ToArray());
                if (textureCount > 1)
                {
                    listItemVariation.SelectedIndex = 0;
                    listItemVariation.Enabled = true;
                }
                else
                {
                    listItemVariation.Enabled = false;
                }
            };
        }
    }
}