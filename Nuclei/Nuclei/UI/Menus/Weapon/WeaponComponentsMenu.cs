using System;
using System.Linq;
using GTA.UI;
using Nuclei.Enums.UI;
using Nuclei.Services.Weapon;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.Weapon;

public class WeaponComponentsMenu : GenericMenu<WeaponComponentsService>
{
    public WeaponComponentsMenu(Enum @enum) : base(@enum)
    {
        Shown += OnShown;
    }

    private void OnShown(object sender, EventArgs e)
    {
        Notification.Show($"{Service.CurrentWeapon == null}");
        if (Service.CurrentWeapon == null)
        {
            NavigateToMenu(MenuTitle.Weapons);
            return;
        }

        GenerateValidWeaponComponents();
    }

    private void GenerateValidWeaponComponents()
    {
        Clear();
        Barrels();
        Scopes();
        ClipSize();
        GunRoot();
        Suppressor();
    }

    private void Suppressor()
    {
        if (Service.CurrentWeapon.Components.SuppressorAndMuzzleBrakeVariationsCount <= 0) return;

        AddListItem("Suppressor", "Select Suppressor", null, null,
            (value, index) =>
            {
                Service.CurrentWeapon.Components.GetSuppressorOrMuzzleBrakeComponent(index).Active = true;
            }, Enumerable
                .Range(-1, Service.CurrentWeapon.Components.SuppressorAndMuzzleBrakeVariationsCount + 1).Select(
                    index =>
                    {
                        var supressorComponent =
                            Service.CurrentWeapon.Components.GetSuppressorOrMuzzleBrakeComponent(index);
                        var componentName = !string.IsNullOrEmpty(supressorComponent.LocalizedName)
                            ? supressorComponent.LocalizedName
                            : supressorComponent.DisplayName;
                        var componentNameAndIndex =
                            $"{componentName} ({index} / {Service.CurrentWeapon.Components.GunRootVariationsCount - 1})";

                        return componentNameAndIndex;
                    }).ToArray());
    }

    private void GunRoot()
    {
        if (Service.CurrentWeapon.Components.GunRootVariationsCount <= 0) return;

        AddListItem("Skin", "Select Skin", null, null,
            (value, index) => { Service.CurrentWeapon.Components.GetGunRootComponent(index).Active = true; }, Enumerable
                .Range(0, Service.CurrentWeapon.Components.GunRootVariationsCount).Select(
                    index =>
                    {
                        var gunRootComponent = Service.CurrentWeapon.Components.GetGunRootComponent(index);
                        var componentName = !string.IsNullOrEmpty(gunRootComponent.LocalizedName)
                            ? gunRootComponent.LocalizedName
                            : gunRootComponent.DisplayName;
                        var componentNameAndIndex =
                            $"{componentName} ({index} / {Service.CurrentWeapon.Components.GunRootVariationsCount - 1})";

                        return componentNameAndIndex;
                    }).ToArray());
    }

    private void ClipSize()
    {
        if (Service.CurrentWeapon.Components.ClipVariationsCount <= 0) return;

        AddListItem("Clip", "Select Magasine", null, null, null, Enumerable
            .Range(0, Service.CurrentWeapon.Components.ClipVariationsCount).Select(
                index =>
                {
                    var clipComponent = Service.CurrentWeapon.Components.GetClipComponent(index);
                    var componentName = !string.IsNullOrEmpty(clipComponent.LocalizedName)
                        ? clipComponent.LocalizedName
                        : clipComponent.DisplayName;

                    var componentNameAndIndex =
                        $"{componentName} ({index} / {Service.CurrentWeapon.Components.ClipVariationsCount - 1})";
                    return componentNameAndIndex;
                }).ToArray());
    }

    private void Scopes()
    {
        if (Service.CurrentWeapon.Components.ScopeVariationsCount <= 0) return;

        AddListItem("Scope", "Select Scope", null, null, null, Enumerable
            .Range(0, Service.CurrentWeapon.Components.BarrelVariationsCount).Select(
                index =>
                {
                    var scopeComponent = Service.CurrentWeapon.Components.GetScopeComponent(index);
                    var componentName = !string.IsNullOrEmpty(scopeComponent.LocalizedName)
                        ? scopeComponent.LocalizedName
                        : scopeComponent.DisplayName;
                    var componentNameAndIndex =
                        $"{componentName} ({index} / {Service.CurrentWeapon.Components.BarrelVariationsCount - 1})";

                    return componentNameAndIndex;
                }).ToArray());
    }

    private void Barrels()
    {
        if (Service.CurrentWeapon.Components.BarrelVariationsCount <= 0) return;

        AddListItem("Barrel", "Select Barrel", null, null, null, Enumerable
            .Range(0, Service.CurrentWeapon.Components.BarrelVariationsCount).Select(
                index =>
                {
                    var barrelComponent = Service.CurrentWeapon.Components.GetBarrelComponent(index);
                    var componentName = !string.IsNullOrEmpty(barrelComponent.LocalizedName)
                        ? barrelComponent.LocalizedName
                        : barrelComponent.DisplayName;
                    var componentNameAndIndex =
                        $"{componentName} ({index} / {Service.CurrentWeapon.Components.BarrelVariationsCount - 1})";
                    return componentNameAndIndex;
                }).ToArray());
    }
}