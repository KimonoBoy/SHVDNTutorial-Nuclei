using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GTA;
using Nuclei.Constants;
using Nuclei.Enums.Hotkey;
using Nuclei.Enums.UI;
using Nuclei.Enums.Vehicle;
using Control = GTA.Control;

namespace Nuclei.Services.Settings;

public class HotkeysService
{
    public static HotkeysService Instance = new(Paths.HotkeysPath);

    private readonly Dictionary<SectionName, IEnumerable<Enum>> _sectionMapping = new()
    {
        { SectionName.Player, Enum.GetValues(typeof(PlayerItemTitle)).Cast<Enum>() },
        { SectionName.Vehicle, Enum.GetValues(typeof(VehicleItemTitle)).Cast<Enum>() },
        { SectionName.VehicleSpawner, Enum.GetValues(typeof(VehicleSpawnerItemTitle)).Cast<Enum>() },
        { SectionName.VehicleWeapon, Enum.GetValues(typeof(VehicleWeaponsItemTitle)).Cast<Enum>() },
        { SectionName.VehicleMod, Enum.GetValues(typeof(VehicleModsItemTitle)).Cast<Enum>() },
        { SectionName.Weapon, Enum.GetValues(typeof(WeaponItemTitle)).Cast<Enum>() },
        { SectionName.World, Enum.GetValues(typeof(WorldItemTitle)).Cast<Enum>() }
    };

    private readonly ScriptSettings settings;

    public HotkeysService(string hotkeysPath)
    {
        settings = ScriptSettings.Load(hotkeysPath);
    }


    public bool IsKeyPressed(Tuple<Keys, Control?, Keys[]> keys)
    {
        var isMainKeyPressed = Game.IsKeyPressed(keys.Item1);

        var isMainControlPressed = Game.IsControlPressed(keys.Item2.HasValue ? keys.Item2.Value : (Control)(-1));

        var areModifiersPressed = keys.Item3.All(k => System.Windows.Forms.Control.ModifierKeys.HasFlag(k));

        return (isMainKeyPressed || isMainControlPressed) && areModifiersPressed;
    }


    private Tuple<Keys, Control?, Keys[]> GetValueInternal(string section, string keyName)
    {
        var mainKeyString = settings.GetValue(section, keyName + "Key", "");
        Enum.TryParse(mainKeyString, out Keys mainKey);

        var mainControlString = settings.GetValue(section, keyName + "Control", "");
        Enum.TryParse(mainControlString, out Control mainControl);

        var modifierStrings = settings.GetValue(section, keyName + "Modifiers", "").Split(',').Select(s => s.Trim())
            .ToArray();
        var modifierKeys = modifierStrings.Select(s => Enum.TryParse(s, out Keys result) ? result : Keys.None)
            .ToArray();
        return new Tuple<Keys, Control?, Keys[]>(mainKey, mainControl, modifierKeys);
    }

    public Tuple<Keys, Control?, Keys[]> GetValue(SectionName section, Enum keyName)
    {
        return GetValueInternal(section.ToString(), keyName.ToString());
    }

    public Tuple<Keys, Control?, Keys[]> GetValue(string section, string keyName)
    {
        return GetValueInternal(section, keyName);
    }

    private void SetValueInternal(string section, string keyName, Keys key, Control? control, Keys[] modifiers)
    {
        settings.SetValue(section, keyName + "Key", key);
        settings.SetValue(section, keyName + "Control", control.HasValue ? control.Value : (Control)(-1));
        settings.SetValue(section, keyName + "Modifiers", string.Join(", ", modifiers));
        settings.Save();
    }

    public void SetValue(SectionName section, Enum keyName, Keys key, Control? control, Keys[] modifiers)
    {
        SetValueInternal(section.ToString(), keyName.ToString(), key, control, modifiers);
    }

    public void SetValue(string section, string keyName, Keys key, Control? control, Keys[] modifiers)
    {
        SetValueInternal(section, keyName, key, control, modifiers);
    }
}