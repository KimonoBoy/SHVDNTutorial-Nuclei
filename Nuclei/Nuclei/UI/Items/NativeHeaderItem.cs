using System.Drawing;
using System.Windows.Forms;
using GTA;
using LemonUI.Menus;
using Font = GTA.UI.Font;

namespace Nuclei.UI.Items;

/// <summary>
///      Creates a new header item.
///      The purpose of the header item is to group items together.
/// </summary>
public sealed class NativeHeaderItem : NativeItem
{
    public NativeHeaderItem(string title) : base(title)
    {
        Colors.BackgroundNormal = Color.FromArgb(230, 0, 0, 0);
        Colors.TitleNormal = Color.FromArgb(230, 255, 255, 255);
        UseCustomBackground = true;
        UpdateColors();

        TitleFont = Font.Pricedown;
    }
}