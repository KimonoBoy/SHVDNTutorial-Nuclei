using System.Drawing;
using Font = GTA.UI.Font;

namespace Nuclei.UI.Menus.Base.Configuration;

public class MenuConfiguration
{
    public MenuConfiguration()
    {
        SubtitleFont = Font.Pricedown;
        BannerColor = Color.Black;
        MaxItems = 12;
    }

    public MenuConfiguration(int width, Font subtitleFont, Color bannerColor, int maxItems)
    {
        Width = Width;
        SubtitleFont = subtitleFont;
        BannerColor = bannerColor;
        MaxItems = maxItems;
    }

    public int Width { get; set; }
    public Font SubtitleFont { get; set; }
    public Color BannerColor { get; set; }
    public int MaxItems { get; set; }
}