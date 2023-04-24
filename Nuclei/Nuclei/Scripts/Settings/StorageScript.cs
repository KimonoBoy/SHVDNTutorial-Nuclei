using Nuclei.Scripts.Generics;
using Nuclei.Services.Settings;

namespace Nuclei.Scripts.Settings;

public class StorageScript : GenericScriptBase<StorageService>
{
    public StorageScript()
    {
        Service.AutoSave = State.GetState().AutoSave;
        Service.AutoLoad = State.GetState().AutoLoad;
    }

    public override void UnsubscribeOnExit()
    {
    }
}