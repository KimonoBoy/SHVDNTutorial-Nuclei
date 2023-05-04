using System;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Settings;

namespace Nuclei.Scripts.Settings;

public class StorageScript : GenericScript<StorageService>
{
    public StorageScript()
    {
        Service.AutoSave = State.GetState().AutoSave;
        Service.AutoLoad = State.GetState().AutoLoad;
    }

    protected override void ProcessGameStatesTimer(object sender, EventArgs e)
    {
    }

    protected override void UpdateServiceStatesTimer(object sender, EventArgs e)
    {
    }

    protected override void SubscribeToEvents()
    {
    }

    protected override void UnsubscribeOnExit()
    {
    }
}