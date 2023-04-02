using System;
using System.Windows.Forms;
using GTA;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Helpers.Utilities;
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
        _playerService.IsInvincible.ValueChanged += OnInvincibleChanged;
        _playerService.WantedLevel.ValueChanged += OnWantedLevelChanged;
    }

    private void OnTick(object sender, EventArgs e)
    {
        /*
         * Updates the different states in the PlayerService, when
         * changes in the game happens.
         */
        UpdateInvincible();
        UpdateWantedLevel();
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

    private void OnInvincibleChanged(object sender, ValueEventArgs<bool> e)
    {
        Game.Player.Character.IsInvincible = e.Value;
    }

    private void OnWantedLevelChanged(object sender, ValueEventArgs<int> e)
    {
        Game.Player.WantedLevel = e.Value;
    }

    private void UpdateWantedLevel()
    {
        if (_playerService.WantedLevel.Value != Game.Player.WantedLevel)
            _playerService.WantedLevel.Value = Game.Player.WantedLevel;
    }

    private void UpdateInvincible()
    {
        if (_playerService.IsInvincible.Value != Game.Player.IsInvincible)
            _playerService.IsInvincible.Value = Game.Player.Character.IsInvincible;
    }
}