using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GTA;
using Nuclei.Enums.UI;
using Nuclei.Enums.Vehicle;
using Nuclei.Helpers.ExtensionMethods;

namespace Nuclei.UI.Menus.Vehicle.VehicleMods;

public class VehicleModsLightsMenu : VehicleModsMenuBase
{
    public VehicleModsLightsMenu(Enum @enum) : base(@enum)
    {
        Shown += OnShown;
        Closed += OnClosed;
    }

    private void OnShown(object sender, EventArgs e)
    {
        Service.PropertyChanged += OnPropertyChanged;
        // UpdateMenuItems();
    }

    private void OnClosed(object sender, EventArgs e)
    {
        Service.PropertyChanged -= OnPropertyChanged;
    }

    protected override void UpdateMenuItems()
    {
        Clear();
        base.UpdateMenuItems();
        XenonHeadlights();
        NeonLightsLayout();
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

    private void NeonLightsLayout()
    {
        var listItemNeonLights = AddListItem(VehicleModsItemTitles.NeonLightsLayout,
            () => (int)Service.CurrentNeonLightsLayout, Service,
            (selected, index) => { Service.CurrentNeonLightsLayout = (NeonLightsLayout)index; },
            typeof(NeonLightsLayout).ToDisplayNameArray());
    }

    protected override ObservableCollection<VehicleModType> GetValidModTypes()
    {
        return new ObservableCollection<VehicleModType>();
    }

    private void XenonHeadlights()
    {
        var checkBoxXenonHeadLights = AddCheckbox(VehicleModsItemTitles.XenonHeadLights, () => Service.XenonHeadLights,
            Service, @checked => { Service.XenonHeadLights = @checked; });
    }
}