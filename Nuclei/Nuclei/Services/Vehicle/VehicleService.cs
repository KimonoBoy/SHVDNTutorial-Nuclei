using System;
using Nuclei.Helpers.Utilities;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Vehicle;

public class VehicleService : GenericService<VehicleService>
{
    public BindableProperty<bool> Indestructible { get; set; } = new();

    public event EventHandler RepairRequested;

    public void Repair()
    {
        RepairRequested?.Invoke(this, EventArgs.Empty);
    }
}