using System.Collections.Generic;
using GTA;

namespace Nuclei.Services.Player;

public class CustomPedDto
{
    public string Title { get; set; }
    public PedHash PedHash { get; set; }
    public List<PedComponentDto> PedComponents { get; set; } = new();
    public List<PedPropDto> PedProps { get; set; } = new();
}

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