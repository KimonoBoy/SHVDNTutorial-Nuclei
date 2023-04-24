using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using GTA;
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
                Back();
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
            (selected, index) => { Service.CurrentTireSmokeColor = Service.TireSmokeColorDictionary[selected]; }, null,
            index =>
            {
                var keyValuePair = Service.TireSmokeColorDictionary.ElementAt(index);
                return keyValuePair.Key;
            }, Service,
            Service.TireSmokeColorDictionary.Keys.ToArray());

        listItemTireSmokeColor.SelectedItem = Service.TireSmokeColorDictionary
            .FirstOrDefault(s => s.Value == Service.CurrentTireSmokeColor).Key;
    }

    private void CustomTires()
    {
        var checkBoxCustomTires = AddCheckbox(VehicleModsItemTitles.CustomTires, () => Service.CurrentCustomTires,
            @checked => { Service.CurrentCustomTires = @checked; }, Service);
        checkBoxCustomTires.Checked = Service.CurrentCustomTires;
    }

    private void RimColors()
    {
        var listItemRimColor =
            AddListItem(VehicleModsItemTitles.RimColor,
                (selected, index) => { Service.CurrentRimColor = selected.GetHashFromDisplayName<VehicleColor>(); },
                null,
                value => Service.CurrentRimColor.GetLocalizedDisplayNameFromHash(),
                Service,
                typeof(VehicleColor).ToDisplayNameArray());

        listItemRimColor.SelectedItem = Service.CurrentVehicle.Mods.RimColor.GetLocalizedDisplayNameFromHash();
    }

    private void WheelTypes()
    {
        var allowedWheelTypes = typeof(VehicleWheelType).ToDisplayNameArray().Where(wheelType =>
            Service.CurrentVehicle.Mods.AllowedWheelTypes.Contains(
                wheelType.GetHashFromDisplayName<VehicleWheelType>())).ToArray();

        var listItemWheelTypes = AddListItem(VehicleModsItemTitles.WheelType,
            (selected, index) => { Service.CurrentWheelType = selected.GetHashFromDisplayName<VehicleWheelType>(); },
            null, value => Service.CurrentWheelType.GetLocalizedDisplayNameFromHash(), Service,
            allowedWheelTypes);

        if (allowedWheelTypes.Contains(Service.CurrentWheelType.GetLocalizedDisplayNameFromHash()))
            listItemWheelTypes.SelectedItem = Service.CurrentWheelType.GetLocalizedDisplayNameFromHash();
        else
            listItemWheelTypes.SelectedIndex = 0;
    }

    protected override ObservableCollection<VehicleModType> GetValidModTypes()
    {
        return new ObservableCollection<VehicleModType>(Service.ValidVehicleModTypes.Where(modType =>
            modType is VehicleModType.FrontWheel or VehicleModType.RearWheel));
    }
}