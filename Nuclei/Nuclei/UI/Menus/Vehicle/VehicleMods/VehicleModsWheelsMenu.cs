using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using GTA;
using Nuclei.Enums.UI;
using Nuclei.Enums.Vehicle;
using Nuclei.Helpers.ExtensionMethods;

namespace Nuclei.UI.Menus.Vehicle.VehicleMods;

public class VehicleModsWheelsMenu : VehicleModsMenuBase
{
    public VehicleModsWheelsMenu(Enum @enum) : base(@enum)
    {
        Shown += OnShown;
        Closed += OnClosed;
    }

    private void OnClosed(object sender, EventArgs e)
    {
        Service.PropertyChanged -= OnPropertyChanged;
    }

    private void OnShown(object sender, EventArgs e)
    {
        Service.PropertyChanged += OnPropertyChanged;
        UpdateMenuItems();
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

        if (e.PropertyName == nameof(Service.CurrentWheelType)) UpdateMenuItems();
    }

    protected override void UpdateMenuItems()
    {
        Clear();
        WheelTypes();
        RimColors();
        base.UpdateMenuItems();
        AddHeader("Tires");
        CustomTires();
        TireSmokeColor();
    }

    private void TireSmokeColor()
    {
        var listItemTireSmokeColor = AddListItem(VehicleModsItemTitles.TireSmokeColor,
            () =>
            {
                var color = Service.CurrentTireSmokeColor;
                var index = Service.TireSmokeColorDictionary.ToList()
                    .IndexOf(Service.TireSmokeColorDictionary.FirstOrDefault(s => s.Value == color));

                return index;
            }, Service,
            (selected, index) => { Service.CurrentTireSmokeColor = Service.TireSmokeColorDictionary[selected]; },
            Service.TireSmokeColorDictionary.Keys.ToArray());

        // listItemTireSmokeColor.SelectedItem = Service.TireSmokeColorDictionary
        //     .FirstOrDefault(s => s.Value == Service.CurrentTireSmokeColor).Key;
    }

    private void CustomTires()
    {
        var checkBoxCustomTires = AddCheckbox(VehicleModsItemTitles.CustomTires, () => Service.CurrentCustomTires,
            Service, @checked => { Service.CurrentCustomTires = @checked; });
        checkBoxCustomTires.Checked = Service.CurrentCustomTires;
    }

    private void RimColors()
    {
        var listItemRimColor =
            AddListItem(VehicleModsItemTitles.RimColor,
                () => (int)Service.CurrentRimColor,
                Service,
                (selected, index) => { Service.CurrentRimColor = selected.GetHashFromDisplayName<VehicleColor>(); },
                typeof(VehicleColor).ToDisplayNameArray());

        // listItemRimColor.SelectedItem = Service.CurrentVehicle.Mods.RimColor.GetLocalizedDisplayNameFromHash();
    }

    private void WheelTypes()
    {
        var allowedWheelTypes = typeof(VehicleWheelType).ToDisplayNameArray().Where(wheelType =>
            Service.CurrentVehicle.Mods.AllowedWheelTypes.Contains(
                wheelType.GetHashFromDisplayName<VehicleWheelType>())).ToArray();

        var listItemWheelTypes = AddListItem(VehicleModsItemTitles.WheelType,
            () => (int)Service.CurrentWheelType, Service,
            (selected, index) => { Service.CurrentWheelType = selected.GetHashFromDisplayName<VehicleWheelType>(); },
            allowedWheelTypes);

        // if (allowedWheelTypes.Contains(Service.CurrentWheelType.GetLocalizedDisplayNameFromHash()))
        //     listItemWheelTypes.SelectedItem = Service.CurrentWheelType.GetLocalizedDisplayNameFromHash();
        // else
        //     listItemWheelTypes.SelectedIndex = 0;
    }

    protected override ObservableCollection<VehicleModType> GetValidModTypes()
    {
        return new ObservableCollection<VehicleModType>(Service.ValidVehicleModTypes.Where(modType =>
            modType is VehicleModType.FrontWheel or VehicleModType.RearWheel));
    }
}