using System;
using System.Linq;
using GTA;
using Nuclei.Services.Player;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.Player.ModelChanger;

public class ModelChangerMenu : GenericMenu<ModelChangerService>
{
    public ModelChangerMenu(Enum @enum) : base(@enum)
    {
        GenerateModels();
    }

    private void GenerateModels()
    {
        var modelCategorizer = new ModelCategorizer();
        foreach (var pedHash in Enum.GetValues(typeof(PedHash)).Cast<PedHash>().OrderBy(pedHash => pedHash.ToString()))
        {
            var model = new Model(pedHash);
            modelCategorizer.Categorize(pedHash, model);
        }

        foreach (var category in modelCategorizer.Categories)
        {
            var modelTypeMenu = new ModelChangerTypeMenu(category.Key, "", category.Value);
            var modelTypeMenuItem = AddMenu(modelTypeMenu);
        }
    }
}