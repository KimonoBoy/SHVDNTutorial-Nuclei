﻿using GTA;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei;

public class PlayerMenu : MenuBase
{
    public PlayerMenu(string subtitle, string description) : base(subtitle, description)
    {
        AddItem("Fix Player", "Restores Player's Health and Armor back to full.", () =>
        {
            Game.Player.Character.Health = Game.Player.Character.MaxHealth;
            Game.Player.Character.Armor = Game.Player.MaxArmor;
        });

        AddCheckbox("Invincible", "Set the Player Invincible.", false,
            @checked => { Game.Player.Character.IsInvincible = @checked; });

        AddListItem("Wanted Level", "Adjust Player's Wanted Level.",
            (item, index) => { Game.Player.WantedLevel = item; }, 0, 1, 2, 3, 4, 5);
    }
}