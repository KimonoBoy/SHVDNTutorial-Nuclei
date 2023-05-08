using GTA;
using Newtonsoft.Json;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Player;

public class AppearanceService : GenericService<AppearanceService>
{
    [JsonIgnore] public Style Style { get; set; }
}