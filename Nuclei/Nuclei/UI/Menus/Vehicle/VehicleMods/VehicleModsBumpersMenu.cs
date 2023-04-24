using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using GTA;
using Nuclei.Enums.UI;

namespace Nuclei.UI.Menus.Vehicle.VehicleMods;

public class VehicleModsBumpersMenu : VehicleModsMenuBase
{
    public VehicleModsBumpersMenu(Enum @enum) : base(@enum)
    {
        Shown += OnShown;
        Closed += OnClosed;
    }

    private void OnShown(object sender, EventArgs e)
    {
        Service.PropertyChanged += OnPropertyChanged;
        UpdateMenuItems();
    }

    private void OnClosed(object sender, EventArgs e)
    {
        Service.PropertyChanged -= OnPropertyChanged;
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Service.CurrentVehicle))
        {
            if (Service.CurrentVehicle == null)
            {
                NavigateToMenu(MenuTitles.Vehicle);
                return;
            }

            if (!Service.ValidVehicleModTypes.Contains(VehicleModType.FrontBumper) &&
                !Service.ValidVehicleModTypes.Contains(VehicleModType.RearBumper))
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
        base.UpdateMenuItems();
    }

    protected override ObservableCollection<VehicleModType> GetValidModTypes()
    {
        return new ObservableCollection<VehicleModType>(Service.ValidVehicleModTypes.Where(modType =>
            modType is VehicleModType.FrontBumper or VehicleModType.RearBumper));
    }
}