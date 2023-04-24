using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            if (Service.CurrentVehicle == null)
                NavigateToMenu(MenuTitles.Vehicle);
    }

    protected override void UpdateMenuItems()
    {
        Clear();

        RandomizeMods();
        WheelsMenu();
        BumpersMenu();
        WindowTints();
        base.UpdateMenuItems();
        LicensePlate();
    }

    private void WindowTints()
    {
        var listItemWindowTints = AddListItem(VehicleModsItemTitles.WindowTint,
            (selected, index) => { Service.CurrentWindowTint = (VehicleWindowTint)index; }, null,
            value =>
            {
                if (Service.CurrentWindowTint != VehicleWindowTint.Invalid)
                    return Service.CurrentWindowTint.GetLocalizedDisplayNameFromHash();
                return VehicleWindowTint.None.GetLocalizedDisplayNameFromHash();
            }, Service,
            typeof(VehicleWindowTint).ToDisplayNameArray().Where(tint =>
                tint.GetHashFromDisplayName<VehicleWindowTint>() != VehicleWindowTint.Invalid).ToArray());

        if (Service.CurrentWindowTint == VehicleWindowTint.Invalid)
            listItemWindowTints.SelectedIndex = 0;
        else
            listItemWindowTints.SelectedItem = Service.CurrentWindowTint.GetLocalizedDisplayNameFromHash();
    }

    private void BumpersMenu()
    {
        if (!Service.ValidVehicleModTypes.Contains(VehicleModType.FrontBumper) &&
            !Service.ValidVehicleModTypes.Contains(VehicleModType.RearBumper)) return;

        var bumpersMenu = new VehicleModsBumpersMenu(MenuTitles.Bumpers);
        AddMenu(bumpersMenu);
    }

    private void RandomizeMods()
    {
        var itemRandomizeMods = AddItem(VehicleModsItemTitles.RandomizeMods,
            () =>
            {
                Service.RequestRandomizeMods(Service.ValidVehicleModTypes);
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
        return new ObservableCollection<VehicleModType>(Service.ValidVehicleModTypes.Where(modType =>
            modType != VehicleModType.FrontWheel && modType != VehicleModType.RearWheel &&
            modType != VehicleModType.FrontBumper && modType != VehicleModType.RearBumper));
    }

    private void LicensePlate()
    {
        if (Service.CurrentVehicle == null) return;

        var itemLicensePlate =
            AddItem(VehicleModsItemTitles.LicensePlate, () => Service.RequestLicensePlateInput());
        itemLicensePlate.AltTitle = Service.CurrentVehicle.Mods.LicensePlate;

        var listItemLicensePlateStyle =
            AddListItem(VehicleModsItemTitles.LicensePlateStyle,
                (selected, index) =>
                {
                    Service.LicensePlateStyle = selected.GetHashFromDisplayName<LicensePlateStyle>();
                }, null,
                value => Service.LicensePlateStyle.GetLocalizedDisplayNameFromHash(), Service,
                typeof(LicensePlateStyle).ToDisplayNameArray());

        listItemLicensePlateStyle.SelectedItem =
            Service.LicensePlateStyle.GetLocalizedDisplayNameFromHash();

        Service.LicensePlateInputRequested += (sender, args) => { itemLicensePlate.AltTitle = Service.LicensePlate; };
    }
}