using System;
using Nuclei.Helpers.Utilities;

namespace Nuclei.Services.Vehicle;

public interface IVehicleService
{
    BindableProperty<bool> Indestructible { get; }

    event EventHandler RepairRequested;
    void OnRepairRequested();
}