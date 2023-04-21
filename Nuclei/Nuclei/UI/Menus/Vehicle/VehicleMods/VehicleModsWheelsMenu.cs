using System;
using System.Collections.Generic;
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
    }

    protected override void UpdateMenuItems()
    {
        Clear();
        WheelTypes();
        base.UpdateMenuItems();
    }

    private void WheelTypes()
    {
        var listItemWheelTypes = AddListItem(VehicleModsItemTitles.WheelType,
            (selected, index) =>
            {
                Service.CurrentWheelType.Value = selected.GetHashFromDisplayName<VehicleWheelType>();
            }, null,
            typeof(VehicleWheelType).ToDisplayNameArray().Where(wheelType =>
                Service.CurrentVehicle.Value.Mods.AllowedWheelTypes.Contains(
                    wheelType.GetHashFromDisplayName<VehicleWheelType>())).ToArray());
        listItemWheelTypes.SelectedItem = Service.CurrentWheelType.Value.GetLocalizedDisplayNameFromHash();
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