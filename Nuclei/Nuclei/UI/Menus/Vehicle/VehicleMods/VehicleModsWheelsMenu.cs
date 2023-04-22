using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GTA;
using Nuclei.Enums.Vehicle;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Helpers.Utilities.BindableProperty;

namespace Nuclei.UI.Menus.Vehicle.VehicleMods;

public class VehicleModsWheelsMenu : VehicleModsMenuBase
{
    public VehicleModsWheelsMenu(Enum @enum) : base(@enum)
    {
        Service.CurrentWheelType.ValueChanged += OnCurrentWheelTypeChanged;
        Service.CurrentVehicle.ValueChanged += OnCurrentVehicleChanged;
    }

    private void OnCurrentVehicleChanged(object sender, ValueEventArgs<GTA.Vehicle> currentVehicle)
    {
        if (currentVehicle.Value == null) return;
        UpdateMenuItems();
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
        /*
         * This will be revisited later - there are a lot of colors in the Color struct. We will also implement
         * functionality for the user to provide his own color from ARGB values.
         */
        var listItemTireSmokeColor = AddListItem(VehicleModsItemTitles.TireSmokeColor,
            (selected, index) => { Service.CurrentTireSmokeColor.Value = selected; }, null,
            typeof(Color).GetAllKnownColors());
        if (listItemTireSmokeColor.Items.Contains(Service.CurrentTireSmokeColor.Value))
            listItemTireSmokeColor.SelectedItem = Service.CurrentTireSmokeColor.Value;
    }

    private void CustomTires()
    {
        var checkBoxCustomTires = AddCheckbox(VehicleModsItemTitles.CustomTires, Service.CurrentCustomTires,
            @checked => { Service.CurrentCustomTires.Value = @checked; });
    }

    private void RimColors()
    {
        var listItemRimColor =
            AddListItem(VehicleModsItemTitles.RimColor,
                (selected, index) =>
                {
                    Service.CurrentRimColor.Value = selected.GetHashFromDisplayName<VehicleColor>();
                }, null,
                typeof(VehicleColor).ToDisplayNameArray());

        listItemRimColor.SelectedItem = Service.CurrentVehicle.Value.Mods.RimColor.GetLocalizedDisplayNameFromHash();
    }

    private void WheelTypes()
    {
        var allowedWheelTypes = typeof(VehicleWheelType).ToDisplayNameArray().Where(wheelType =>
            Service.CurrentVehicle.Value.Mods.AllowedWheelTypes.Contains(
                wheelType.GetHashFromDisplayName<VehicleWheelType>())).ToArray();

        var listItemWheelTypes = AddListItem(VehicleModsItemTitles.WheelType,
            (selected, index) =>
            {
                Service.CurrentWheelType.Value = selected.GetHashFromDisplayName<VehicleWheelType>();
            }, null, allowedWheelTypes);

        var currentWheelTypeDisplayName = Service.CurrentWheelType.Value.GetLocalizedDisplayNameFromHash();
        if (allowedWheelTypes.Contains(currentWheelTypeDisplayName))
            listItemWheelTypes.SelectedItem = currentWheelTypeDisplayName;
    }

    private void OnCurrentWheelTypeChanged(object sender, ValueEventArgs<VehicleWheelType> e)
    {
        UpdateMenuItems();
    }

    protected override IEnumerable<VehicleModType> GetValidModTypes()
    {
        return Service.ValidVehicleModTypes.Value.Where(modType =>
            modType is VehicleModType.FrontWheel or VehicleModType.RearWheel);
    }
}