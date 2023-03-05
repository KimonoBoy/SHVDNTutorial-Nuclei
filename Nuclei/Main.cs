using System;
using System.Windows.Forms;
using GTA;
using LemonUI;
using LemonUI.Menus;

namespace Nuclei;

public class Main : Script
{
    private readonly NativeCheckboxItem _checkBoxInvincible = new("Invincible", "Makes the Player invincible.");
    private readonly NativeItem _itemFixPlayer = new("Fix Player", "Restores Player's Health and Armor.");

    private readonly NativeListItem<int> _listItemWantedLevel =
        new("Wanted Level", "Adjust the Player's Wanted Level.", 0, 1, 2, 3, 4, 5);

    private readonly NativeMenu _menu = new("Nuclei", "Main Menu");

    private readonly NativeMenu _playerMenu = new("Nuclei", "Player Menu");
    private readonly ObjectPool _pool = new();
    private readonly NativeSubmenuItem _subMenuItemPlayer;

    public Main()
    {
        _pool.Add(_menu);

        _itemFixPlayer.Activated += OnFixPlayerActivated;
        _menu.Add(_itemFixPlayer);

        _checkBoxInvincible.CheckboxChanged += OnInvincibleCheckboxChanged;
        _menu.Add(_checkBoxInvincible);

        _listItemWantedLevel.ItemChanged += OnWantedLevelItemChanged;
        _menu.Add(_listItemWantedLevel);

        _subMenuItemPlayer = new NativeSubmenuItem(_playerMenu, _menu);
        _menu.Add(_subMenuItemPlayer);
        _pool.Add(_playerMenu);

        KeyDown += OnKeyDown;
        Tick += OnTick;
    }

    private void OnWantedLevelItemChanged(object sender, ItemChangedEventArgs<int> wantedLevel)
    {
        Game.Player.WantedLevel = wantedLevel.Object;
    }

    private void OnInvincibleCheckboxChanged(object sender, EventArgs e)
    {
        Game.Player.Character.IsInvincible = _checkBoxInvincible.Checked;
    }

    private void OnFixPlayerActivated(object sender, EventArgs e)
    {
        Game.Player.Character.Health = Game.Player.Character.MaxHealth;
        Game.Player.Character.Armor = Game.Player.MaxArmor;
    }

    private void OnTick(object sender, EventArgs e)
    {
        _pool.Process();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F5) _menu.Visible = !_menu.Visible;
    }
}