using System;
using System.Windows.Forms;
using GTA;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Services.Player;

namespace Nuclei.Scripts.Player;

public class PlayerScript : Script
{
    private readonly PlayerService _playerService = PlayerService.Instance;

    public PlayerScript()
    {
        Tick += OnTick;
        KeyDown += OnKeyDown;
        _playerService.PlayerFixed += OnPlayerFixed;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.T && e.Control)
            Game.Player.Character.TeleportToBlip(BlipSprite.Waypoint);
    }

    private void OnPlayerFixed(object sender, EventArgs e)
    {
        Game.Player.Character.Health = Game.Player.Character.MaxHealth;
        Game.Player.Character.Armor = Game.Player.MaxArmor;
    }

    private void OnTick(object sender, EventArgs e)
    {
        Invincible();
        AdjustWantedLevel();
        GTA.UI.Notification.Show($"Okay: {_playerService.GetType}");
    }

    private void AdjustWantedLevel()
    {
        Game.Player.WantedLevel = _playerService.WantedLevel;
    }

    private void Invincible()
    {
        Game.Player.Character.IsInvincible = _playerService.IsInvincible;
    }
}