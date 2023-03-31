using System.Drawing;
using System.Windows.Forms;
using GTA;
using LemonUI.Menus;
using Font = GTA.UI.Font;

namespace Nuclei.UI.Items;

public class NativeHeaderItem : NativeItem
{
    /// <summary>
    /// Creates a new NativeHeaderItem.
    /// 
    /// The purpose of the HeaderItem is to group items together.
    /// </summary>
    /// <param name="title">The DisplayTitle of the HeaderItem</param>
    public NativeHeaderItem(string title) : base(title)
    {
        Colors.BackgroundNormal = Color.FromArgb(230, 0, 0, 0);
        Colors.TitleNormal = Color.FromArgb(230, 255, 255, 255);
        UseCustomBackground = true;
        UpdateColors();

        TitleFont = Font.Pricedown;
    }
}