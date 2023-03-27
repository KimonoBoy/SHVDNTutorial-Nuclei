using System.Linq;
using GTA;
using GTA.Math;

namespace Nuclei.Helpers.ExtensionMethods;

public static class EntityExtensions
{
    /// <summary>
    ///     Teleport to a map blip.
    /// </summary>
    /// <param name="entity">The entity to teleport.</param>
    /// <param name="blipSprite">The blip to teleport to. (e.g. BlipSprite.Waypoint)</param>
    /// <returns>Whether or not the teleport was successful.</returns>
    public static bool TeleportToBlip(this Entity entity, BlipSprite blipSprite)
    {
        var blip = World
            .GetAllBlips()
            .FirstOrDefault(b => b.Sprite == blipSprite);

        if (blip == null) return false;

        var blipPos = blip.Position;
        var zCoords = 0.0f;

        entity.Position = new Vector3(blipPos.X, blipPos.Y, zCoords);
        Script.Wait(200);

        if (entity.IsInWater) return true;

        var antiFreezeCounter = 0;

        while (entity.Position.Z < World.GetGroundHeight(entity.Position) ||
               World.GetGroundHeight(entity.Position) == 0)
        {
            antiFreezeCounter++;
            entity.Position = new Vector3(entity.Position.X, entity.Position.Y, zCoords += 3.0f);

            if (antiFreezeCounter <= 20) continue;
            Script.Yield();
            antiFreezeCounter = 0;
        }

        return true;
    }
}