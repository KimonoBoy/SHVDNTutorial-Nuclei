using System.Drawing;
using LemonUI.Menus;
using Font = GTA.UI.Font;

namespace Nuclei.UI.Menus.Base.CustomItems;

public class NativeHeaderItem : NativeItem
{
    /// <summary>
    ///     Creates a new NativeHeaderItem.
    ///     The purpose of the HeaderItem is to group items together.
    /// </summary>
    /// <param name="title">The DisplayTitle of the HeaderItem</param>
    public NativeHeaderItem(string title) : base(title)
    {
        Colors.TitleNormal = Color.FromArgb(255, 255, 255, 255);
        UpdateColors();

        TitleFont = Font.Pricedown;
    }
}