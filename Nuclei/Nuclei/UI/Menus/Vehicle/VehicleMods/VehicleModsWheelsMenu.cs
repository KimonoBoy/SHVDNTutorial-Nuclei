using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GTA;
using Nuclei.Enums.Vehicle;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.UI.Menus.Base.ItemFactory;

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
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (Service.CurrentVehicle == null) return;
        if (e.PropertyName == nameof(Service.WheelType))
            GenerateMenu();
    }

    protected override void PreModTypeMods()
    {
        WheelTypes();
    }

    protected override void PostModTypeMods()
    {
        RimColors();
        AddHeader("Tires");
        CustomTires();
        TireSmokeColor();
    }

    private void CustomTires()
    {
        var checkBoxCustomTires = AddCheckbox(VehicleModsItemTitle.CustomTires, () => Service.CustomTires, Service,
            @checked => { Service.CustomTires = @checked; });
        checkBoxCustomTires.Checked = Service.CustomTires;
    }

    private void TireSmokeColor()
    {
        var listItemTireSmokeColor = AddListItem(VehicleModsItemTitle.TireSmokeColor,
            () => (int)Service.TireSmokeColor,
            Service,
            (value, index) => { Service.TireSmokeColor = (TireSmokeColor)index; },
            Enumerable.Range(0, Service.TireSmokeColorDictionary.Keys.Count)
                .Select(index =>
                    $"{((TireSmokeColor)index).GetLocalizedDisplayNameFromHash()} {index} / {Service.TireSmokeColorDictionary.Count - 1}")
                .ToArray());
        listItemTireSmokeColor.SetSelectedIndexSafe((int)Service.TireSmokeColor);
    }

    private void RimColors()
    {
        var listItemRimColor = AddListItem(VehicleModsItemTitle.RimColor,
            () => (int)Service.RimColor,
            Service,
            (value, index) =>
            {
                Service.RimColor =
                    (VehicleColor)index;
            },
            Enumerable.Range(0, Enum.GetValues(typeof(VehicleColor)).Length)
                .Select(index =>
                    $"{((VehicleColor)index).GetLocalizedDisplayNameFromHash()} {index} / {Enum.GetValues(typeof(VehicleColor)).Length - 1}")
                .ToArray());
        listItemRimColor.SetSelectedIndexSafe((int)Service.RimColor);
    }

    private void WheelTypes()
    {
        var listItemWheelType = AddListItem(VehicleModsItemTitle.WheelType, () => (int)Service.WheelType,
            Service,
            (value, index) => { Service.WheelType = (VehicleWheelType)index; },
            typeof(VehicleWheelType).ToDisplayNameArray());
        listItemWheelType.SetSelectedIndexSafe((int)Service.WheelType);
    }

    protected override IEnumerable<VehicleMod> GetValidMods()
    {
        if (Service.CurrentVehicle.IsMotorcycle)
            return Service.VehicleMods.Where(vehicleMod =>
                vehicleMod.Type == VehicleModType.FrontWheel || vehicleMod.Type == VehicleModType.RearWheel);
        return Service.VehicleMods.Where(vehicleMod => vehicleMod.Type == VehicleModType.FrontWheel);
    }
}