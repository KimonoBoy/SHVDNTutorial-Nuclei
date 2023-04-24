using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using GTA;
using Nuclei.Enums.UI;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Services.Vehicle.VehicleMods;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.Vehicle.VehicleMods;

public abstract class VehicleModsMenuBase : GenericMenuBase<VehicleModsService>
{
    protected VehicleModsMenuBase(Enum @enum) : base(@enum)
    {
        Width = 550;
        Shown += OnShown;
        Closed += OnClosed;
    }

    private void OnClosed(object sender, EventArgs e)
    {
        Service.PropertyChanged -= OnPropertyChanged;
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Service.CurrentVehicle))
        {
            if (!Visible) return;
            if (Service.CurrentVehicle == null)
            {
                NavigateToMenu(MenuTitles.Vehicle);
                return;
            }

            UpdateMenuItems();
        }
    }

    private void OnShown(object sender, EventArgs e)
    {
        if (!Visible) return;
        if (Service.CurrentVehicle == null)
        {
            NavigateToMenu(MenuTitles.Vehicle);
            return;
        }

        UpdateMenuItems();
        Service.PropertyChanged += OnPropertyChanged;
    }

    protected abstract ObservableCollection<VehicleModType> GetValidModTypes();

    protected virtual void UpdateMenuItems()
    {
        GenerateModsMenu();
    }

    private void GenerateModsMenu()
    {
        foreach (var modType in GetValidModTypes())
        {
            var currentMod = Service.CurrentVehicle.Mods[modType];
            var currentIndex = currentMod.Index;
            var listItem = AddListItem(modType.GetLocalizedDisplayNameFromHash(), "",
                (value, index) => { currentMod.Index = index == currentMod.Count ? -1 : index; },
                null,
                null, Service,
                Enumerable.Range(0, currentMod.Count + 1).ToList().Select(i =>
                {
                    if (i == currentMod.Count) return "Stock" + $" {0}/{currentMod.Count}";
                    currentMod.Index = i;
                    var localizedString = currentMod.LocalizedName + $" {i + 1}/{currentMod.Count}";
                    return localizedString;
                }).ToArray());

            if (currentIndex == 0)
            {
                /*
                 * Weird fix, but works.
                 */
                listItem.SelectedIndex++;
                listItem.SelectedIndex--;
            }
            else
            {
                listItem.SelectedIndex = currentIndex == -1 ? currentMod.Count : currentIndex;
            }
        }
    }
}