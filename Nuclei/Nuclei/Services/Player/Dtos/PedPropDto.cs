using GTA;

namespace Nuclei.Services.Player.Dtos;

public class PedPropDto
{
    public PedPropDto(PedPropType propType, int drawableIndex, int textureIndex)
    {
        PedPropType = propType;
        DrawableIndex = drawableIndex;
        TextureIndex = textureIndex;
    }

    public int TextureIndex { get; set; }
    public int DrawableIndex { get; set; }
    public PedPropType PedPropType { get; set; }
}