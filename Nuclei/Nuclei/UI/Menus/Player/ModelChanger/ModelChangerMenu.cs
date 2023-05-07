using System;
using System.Linq;
using GTA;
using Nuclei.Helpers;
using Nuclei.Services.Player;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.Player.ModelChanger;

public class ModelChangerMenu : GenericMenu<ModelChangerService>
{
    public ModelChangerMenu(Enum @enum) : base(@enum)
    {
        Protagonists();
        GenerateModels();
    }

    private void Protagonists()
    {
        var itemFranklin = AddItem("Franklin", "", () => { Service.RequestChangeModel(PedHash.Franklin); });
        var itemMichael = AddItem("Michael", "", () => { Service.RequestChangeModel(PedHash.Michael); });
        var itemTrevor = AddItem("Trevor", "", () => { Service.RequestChangeModel(PedHash.Trevor); });
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