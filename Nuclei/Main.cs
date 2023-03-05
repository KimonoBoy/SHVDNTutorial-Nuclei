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
    private readonly NativeMenu _menu = new("Nuclei", "Main Menu");
    private readonly ObjectPool _pool = new();

    public Main()
    {
        _pool.Add(_menu);

        _itemFixPlayer.Activated += OnFixPlayerActivated;
        _menu.Add(_itemFixPlayer);

        _checkBoxInvincible.CheckboxChanged += OnInvincibleCheckboxChanged;
        _menu.Add(_checkBoxInvincible);
        KeyDown += OnKeyDown;
        Tick += OnTick;
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