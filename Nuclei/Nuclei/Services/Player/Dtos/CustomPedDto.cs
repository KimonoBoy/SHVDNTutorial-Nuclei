using System.Collections.Generic;
using GTA;

namespace Nuclei.Services.Player.Dtos;

public class CustomPedDto
{
    public string Title { get; set; }
    public PedHash PedHash { get; set; }
    public List<PedComponentDto> PedComponents { get; set; } = new();
    public List<PedPropDto> PedProps { get; set; } = new();
}