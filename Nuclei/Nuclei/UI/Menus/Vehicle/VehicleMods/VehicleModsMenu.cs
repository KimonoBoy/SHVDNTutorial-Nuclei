using System;
using System.Collections.ObjectModel;
using System.Linq;
using GTA;
using Nuclei.Enums.UI;
using Nuclei.Enums.Vehicle;
using Nuclei.Helpers.ExtensionMethods;

namespace Nuclei.UI.Menus.Vehicle.VehicleMods;

public class VehicleModsMenu : VehicleModsMenuBase
{
    public VehicleModsMenu(Enum @enum) : base(@enum)
    {
    }

    protected override void UpdateMenuItems()
    {
        Clear();

        RandomizeMods();
        WheelsMenu();
        base.UpdateMenuItems();
        LicensePlate();
    }

    private void RandomizeMods()
    {
        var itemRandomizeMods = AddItem(VehicleModsItemTitles.RandomizeMods,
            () =>
            {
                Service.RequestRandomizeMods(Service.ValidVehicleModTypes.Value);
                UpdateMenuItems();
            });
    }

    private void WheelsMenu()
    {
        var wheelsMenu = new VehicleModsWheelsMenu(MenuTitles.Wheels);
        AddMenu(wheelsMenu);
    }

    protected override ObservableCollection<VehicleModType> GetValidModTypes()
    {
        return new ObservableCollection<VehicleModType>(Service.ValidVehicleModTypes.Value.Where(modType =>
            modType != VehicleModType.FrontWheel && modType != VehicleModType.RearWheel));
    }

    private void LicensePlate()
    {
        if (Service.CurrentVehicle.Value == null) return;

        var itemLicensePlate =
            AddItem(VehicleModsItemTitles.LicensePlate, () => Service.RequestLicensePlateInput());
        itemLicensePlate.AltTitle = Service.CurrentVehicle.Value.Mods.LicensePlate;
        Service.LicensePlate.ValueChanged += (sender, args) => { itemLicensePlate.AltTitle = args.Value; };

        var listItemLicensePlateStyle =
            AddListItem(VehicleModsItemTitles.LicensePlateStyle,
                (selected, index) =>
                {
                    Service.LicensePlateStyle.Value = selected.GetHashFromDisplayName<LicensePlateStyle>();
                }, null,
                typeof(LicensePlateStyle).ToDisplayNameArray());

        listItemLicensePlateStyle.SelectedItem =
            Service.LicensePlateStyle.Value.GetLocalizedDisplayNameFromHash();
    }
}