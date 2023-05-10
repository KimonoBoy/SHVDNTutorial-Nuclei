using System;
using System.Linq;
using System.Windows.Forms;
using GTA;
using Control = GTA.Control;

namespace Nuclei.Services.Settings;

public class HotkeysService
{
    private readonly ScriptSettings settings;

    public HotkeysService(string hotkeysPath)
    {
        settings = ScriptSettings.Load(hotkeysPath);
    }

    public Tuple<Keys, Control, Keys[]> GetValue(string section, string keyName)
    {
        var mainKeyString = settings.GetValue(section, keyName + "Key", "");
        Enum.TryParse(mainKeyString, out Keys mainKey);

        var mainControlString = settings.GetValue(section, keyName + "Control", "");
        Enum.TryParse(mainControlString, out Control mainControl);

        var modifierStrings = settings.GetValue(section, keyName + "Modifiers", "").Split(',').Select(s => s.Trim())
            .ToArray();
        var modifierKeys = modifierStrings.Select(s => Enum.TryParse(s, out Keys result) ? result : Keys.None)
            .ToArray();
        return new Tuple<Keys, Control, Keys[]>(mainKey, mainControl, modifierKeys);
    }

    public bool IsKeyPressed(Tuple<Keys, Control, Keys[]> keys)
    {
        // Check main key
        var isMainKeyPressed = Game.IsKeyPressed(keys.Item1);

        // Check main control
        var isMainControlPressed = Game.IsControlPressed(keys.Item2);

        // Check modifiers
        var areModifiersPressed = keys.Item3.All(k => System.Windows.Forms.Control.ModifierKeys.HasFlag(k));

        return (isMainKeyPressed || isMainControlPressed) && areModifiersPressed;
    }

    public void SetValue(string section, string keyName, Keys key, Control control, Keys[] modifiers)
    {
        settings.SetValue(section, keyName + "Key", key);
        settings.SetValue(section, keyName + "Control", control);
        settings.SetValue(section, keyName + "Modifiers", string.Join(", ", modifiers));
        settings.Save();
    }
}