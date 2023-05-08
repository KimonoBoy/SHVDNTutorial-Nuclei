using System;
using System.Linq;
using System.Windows.Forms;
using GTA;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Player;
using Control = GTA.Control;

namespace Nuclei.Scripts.Player;

public class ModelChangerScript : GenericScript<ModelChangerService>
{
    protected override void OnTick(object sender, EventArgs e)
    {
        PreventInfiniteLoop();
    }

    private void PreventInfiniteLoop()
    {
        if ((PedHash)Character.Model.Hash == PedHash.Franklin || (PedHash)Character.Model.Hash == PedHash.Michael ||
            (PedHash)Character.Model.Hash == PedHash.Trevor) return;

        if (Character.IsDead || Game.IsCutsceneActive) ChangeModel(PedHash.Franklin);
    }

    protected override void SubscribeToEvents()
    {
        Service.ModelChangeRequested += OnModelChangeRequested;
        Service.CustomModelChangeRequested += OnCustomModelChangeRequested;
        KeyDown += OnKeyDown;
    }

    private void OnCustomModelChangeRequested(object sender, CustomPedDto customPedDto)
    {
        ChangeModel(customPedDto.PedHash);
        foreach (var customPedAppearanceDto in customPedDto.PedComponents)
        {
            Character.Style[customPedAppearanceDto.PedComponentType].Index = customPedAppearanceDto.DrawableIndex;
            Character.Style[customPedAppearanceDto.PedComponentType].TextureIndex = customPedAppearanceDto.TextureIndex;
        }

        foreach (var pedPropDto in customPedDto.PedProps)
        {
            Character.Style[pedPropDto.PedPropType].Index = pedPropDto.DrawableIndex;
            Character.Style[pedPropDto.PedPropType].TextureIndex = pedPropDto.TextureIndex;
        }
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (Game.IsControlPressed(Control.Jump))
        {
            if (Service.FavoriteModels.Contains(Service.CurrentPedHash))
                Service.FavoriteModels.Remove(Service.CurrentPedHash);
            else
                Service.FavoriteModels.Add(Service.CurrentPedHash);

            var customPedDto =
                Service.CustomModels.FirstOrDefault(
                    pedDto => pedDto.PedHash == Service.CurrentPedHash);
            if (Service.CustomModels.Contains(customPedDto))
                Service.CustomModels.Remove(customPedDto);
        }
    }

    private void OnModelChangeRequested(object sender, PedHash pedHash)
    {
        ChangeModel(pedHash);
    }

    private void ChangeModel(PedHash pedHash)
    {
        // Request Devin Weston's character model
        var characterModel = new Model(pedHash);
        characterModel.Request(500);

        // Check the model is valid
        if (characterModel is { IsInCdImage: true, IsValid: true })
        {
            // If the model isn't loaded, wait until it is
            while (!characterModel.IsLoaded) Wait(100);

            // Set the player's model (a new ped for player will be created)
            Game.Player.ChangeModel(characterModel);
            Wait(100);
            Character.Style.SetDefaultClothes();
        }

        // Let the game release the model from memory after we've assigned it.
        characterModel.MarkAsNoLongerNeeded();
    }

    protected override void UnsubscribeOnExit()
    {
    }
}