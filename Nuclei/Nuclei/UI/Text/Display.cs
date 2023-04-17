using System.Drawing;
using GTA.UI;
using Font = GTA.UI.Font;

namespace Nuclei.UI.Text;

public class Display
{
    public static void DrawTextElement(string message, float x, float y,
        Color color,
        float scale = 0.5f,
        Font font = Font.ChaletComprimeCologne,
        bool outline = false, bool shadow = false)
    {
        var textElement = new TextElement(message, new PointF(x, y), scale)
        {
            Color = color,
            Font = font,
            Outline = outline,
            Shadow = shadow
        };
        textElement.Draw();
    }

    public static void Notify(string message, string activationMessage = "", bool state = true,
        bool hidePrevious = true)
    {
        var color = state ? "~g~" : "~r~";
        var text = $"~b~{message}";
        var activationText = $"{color}{activationMessage}";
        var notification = $"~h~{text}~h~: {activationText}";
        var handle = Notification.Show(notification);

        if (hidePrevious)
            Notification.Hide(handle - 1);
    }
}