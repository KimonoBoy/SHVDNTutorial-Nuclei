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
        {
            if (Service.CurrentVehicle == null)
            {
                Back();
                return;
            }

            UpdateMenuItems();
        }
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
            modType != VehicleModType.FrontWheel && modType != VehicleModType.RearWheel));
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