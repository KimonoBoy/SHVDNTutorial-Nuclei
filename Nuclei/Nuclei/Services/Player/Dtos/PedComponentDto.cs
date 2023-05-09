using GTA;

namespace Nuclei.Services.Player.Dtos;

public class PedComponentDto
{
    public PedComponentDto(PedComponentType componentType, int drawableIndex, int textureIndex)
    {
        PedComponentType = componentType;
        DrawableIndex = drawableIndex;
        TextureIndex = textureIndex;
    }

    public int TextureIndex { get; set; }

    public int DrawableIndex { get; set; }
    public PedComponentType PedComponentType { get; set; }
}