using Nuclei.Scripts.Generics;
using Nuclei.Services.Settings;

namespace Nuclei.Scripts.Settings;

public class StorageScript : GenericScriptBase<StorageService>
{
    public StorageScript()
    {
        Service.AutoSave.Value = State.GetState().AutoSave.Value;
        Service.AutoLoad.Value = State.GetState().AutoLoad.Value;
    }
}