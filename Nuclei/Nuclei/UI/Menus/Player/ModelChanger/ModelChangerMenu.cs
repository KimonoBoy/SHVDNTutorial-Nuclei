using System;
using System.Linq;
using GTA;
using LemonUI.Elements;
using LemonUI.Menus;
using Nuclei.Enums.UI;
using Nuclei.Helpers;

namespace Nuclei.UI.Menus.Player.ModelChanger;

public class ModelChangerMenu : ModelChangerMenuBase
{
    public ModelChangerMenu(Enum @enum) : base(@enum)
    {
        Protagonists();
        FavoritesMenu();
        GenerateModels();
    }

    protected override void OnShown(object sender, EventArgs e)
    {
        base.OnShown(sender, e);
        if (Service.FavoriteModels.Contains(PedHash.Franklin))
            GetItem<NativeItem>(PedHash.Franklin).RightBadge = new ScaledTexture("commonmenu", "shop_new_star");
        if (Service.FavoriteModels.Contains(PedHash.Michael))
            GetItem<NativeItem>(PedHash.Michael).RightBadge = new ScaledTexture("commonmenu", "shop_new_star");
        if (Service.FavoriteModels.Contains(PedHash.Trevor))
            GetItem<NativeItem>(PedHash.Trevor).RightBadge = new ScaledTexture("commonmenu", "shop_new_star");
    }

    private void FavoritesMenu()
    {
        var favoritesMenu = new ModelChangerFavoritesMenu(MenuTitle.FavoriteModels);
        AddMenu(favoritesMenu);
    }

    private void Protagonists()
    {
        var itemFranklin = AddItem(PedHash.Franklin, () => { Service.RequestChangeModel(PedHash.Franklin); });

        var itemMichael = AddItem(PedHash.Michael, () => { Service.RequestChangeModel(PedHash.Michael); });

        var itemTrevor = AddItem(PedHash.Trevor, () => { Service.RequestChangeModel(PedHash.Trevor); });
    }

    private void GenerateModels()
    {
        var modelCategorizer = new ModelCategorizer();
        foreach (var pedHash in Enum.GetValues(typeof(PedHash)).Cast<PedHash>()
                     .OrderBy(pedHash => pedHash.ToString()))
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