using System;
using System.Collections.Specialized;
using System.Linq;
using GTA;
using GTA.UI;
using Nuclei.Services.Player.Dtos;

namespace Nuclei.UI.Menus.Player.ModelChanger;

public class ModelChangerSavedModelsMenu : ModelChangerMenuBase
{
    public ModelChangerSavedModelsMenu(Enum @enum) : base(@enum)
    {
    }

    protected override void OnShown(object sender, EventArgs e)
    {
        GenerateItems();
        Service.CustomModels.CollectionChanged += OnModelCollectionChanged<CustomPedDto>;
    }

    private void GenerateItems()
    {
        Clear();
        SaveCurrentModel();
        foreach (var customPedDto in Service.CustomModels)
        {
            var itemCustomModel = AddItem(customPedDto.Title, "",
                () => Service.RequestChangeModel(customPedDto));
        }
    }

    private void SaveCurrentModel()
    {
        AddItem("Save Current Model", "", () =>
        {
            var userInput = Game.GetUserInput();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                Notification.Show("Name cannot be empty.");
                return;
            }

            if (Service.CustomModels.Any(m => m.Title == userInput))
            {
                Notification.Show("Name already exists.");
                return;
            }

            var customCharacter = new CustomPedDto
            {
                Title = userInput,
                PedHash = (PedHash)Service.Character.Model.Hash
            };

            foreach (var pedComponent in Service.Character.Style.GetAllComponents())
            {
                var customPedComponent =
                    new PedComponentDto(pedComponent.Type, pedComponent.Index, pedComponent.TextureIndex);
                customCharacter.PedComponents.Add(customPedComponent);
            }

            foreach (var pedProp in Service.Character.Style.GetAllProps())
            {
                var customPedComponent =
                    new PedPropDto(pedProp.Type, pedProp.Index, pedProp.TextureIndex);
                customCharacter.PedProps.Add(customPedComponent);
            }

            Service.CustomModels.Add(customCharacter);
            Service.GetStorage().SetState(Service);
            Service.GetStorage().SaveState();
        });
    }

    protected override void UpdateSelectedItem(string title)
    {
        if (string.IsNullOrEmpty(title)) throw new ArgumentNullException(nameof(title));

        var pedHash = Service.CustomModels.FirstOrDefault(v => v.Title == title)?.PedHash;

        if (pedHash != null) Service.CurrentPedHash = (PedHash)pedHash;
    }

    protected override void OnModelCollectionChanged<T>(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e == null) throw new ArgumentNullException(nameof(e));

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add when e.NewItems != null:
                e.NewItems.Cast<CustomPedDto>().ToList().ForEach(AddCustomModelItem);
                break;
            case NotifyCollectionChangedAction.Remove when e.OldItems != null:
                e.OldItems.Cast<CustomPedDto>().ToList().ForEach(RemoveCustomPed);
                break;
        }
    }

    private void AddCustomModelItem(CustomPedDto customPed)
    {
        if (customPed == null) throw new ArgumentNullException(nameof(customPed));

        AddItem(customPed.Title, "",
            () => Service.RequestChangeModel(customPed));
    }

    private void RemoveCustomPed(CustomPedDto customPed)
    {
        if (customPed == null) throw new ArgumentNullException(nameof(customPed));

        var item = Items.FirstOrDefault(i => i.Title == customPed.Title);
        if (item != null)
        {
            var itemIndex = Items.IndexOf(item);
            Remove(item);
            if (Items.Count > 0) SelectedIndex = Math.Max(0, Math.Min(itemIndex, Items.Count - 1));
        }
    }
}