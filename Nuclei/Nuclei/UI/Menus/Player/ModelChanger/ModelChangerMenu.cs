using System;
using System.Collections.Specialized;
using System.Linq;
using GTA;
using LemonUI.Elements;
using LemonUI.Menus;
using Nuclei.Enums.UI;
using Nuclei.Helpers;
using Nuclei.Helpers.ExtensionMethods;

namespace Nuclei.UI.Menus.Player.ModelChanger;

public class ModelChangerMenu : ModelChangerMenuBase
{
    public ModelChangerMenu(Enum @enum) : base(@enum)
    {
        Protagonists();
        SavedModelsMenu();
        FavoritesMenu();
        GenerateModels();
    }

    private void SavedModelsMenu()
    {
        var savedModelsMenu = new ModelChangerSavedModelsMenu(MenuTitle.SavedModels);
        AddMenu(savedModelsMenu);
    }

    protected override void OnShown(object sender, EventArgs e)
    {
        if (Service.FavoriteModels.Contains(PedHash.Franklin))
            GetItem<NativeItem>(PedHash.Franklin).RightBadge = new ScaledTexture("commonmenu", "shop_new_star");
        if (Service.FavoriteModels.Contains(PedHash.Michael))
            GetItem<NativeItem>(PedHash.Michael).RightBadge = new ScaledTexture("commonmenu", "shop_new_star");
        if (Service.FavoriteModels.Contains(PedHash.Trevor))
            GetItem<NativeItem>(PedHash.Trevor).RightBadge = new ScaledTexture("commonmenu", "shop_new_star");
    }

    protected override void UpdateSelectedItem(string title)
    {
        Service.CurrentPedHash = title.GetHashFromDisplayName<PedHash>();
    }

    protected override void OnModelCollectionChanged<T>(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add when e.NewItems != null:
                e.NewItems.Cast<PedHash>().ToList().ForEach(pedHash =>
                {
                    var displayName = pedHash.GetLocalizedDisplayNameFromHash();
                    var item = Items.FirstOrDefault(i => i.Title == displayName);
                    if (item != null) item.RightBadge = new ScaledTexture("commonmenu", "shop_new_star");
                });
                break;
            case NotifyCollectionChangedAction.Remove when e.OldItems != null:
                e.OldItems.Cast<PedHash>().ToList().ForEach(pedHash =>
                {
                    var displayName = pedHash.GetLocalizedDisplayNameFromHash();
                    var item = Items.FirstOrDefault(i => i.Title == displayName);
                    if (item != null) item.RightBadge = null;
                });
                break;
        }
    }

    private void FavoritesMenu()
    {
        var favoritesMenu = new ModelChangerFavoritesMenu(MenuTitle.FavoriteModels);
        AddMenu(favoritesMenu);
    }

    private void Protagonists()
    {
        var itemFranklin = AddItem(PedHash.Franklin, () => { Service.RequestChangeModel(PedHash.Franklin); });
        itemFranklin.Selected += (sender, args) =>
        {
            UpdateSelectedItem(PedHash.Franklin.GetLocalizedDisplayNameFromHash());
        };

        var itemMichael = AddItem(PedHash.Michael, () => { Service.RequestChangeModel(PedHash.Michael); });
        itemMichael.Selected += (sender, args) =>
        {
            UpdateSelectedItem(PedHash.Michael.GetLocalizedDisplayNameFromHash());
        };

        var itemTrevor = AddItem(PedHash.Trevor, () => { Service.RequestChangeModel(PedHash.Trevor); });
        itemTrevor.Selected += (sender, args) =>
        {
            UpdateSelectedItem(PedHash.Trevor.GetLocalizedDisplayNameFromHash());
        };
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