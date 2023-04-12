using System.Linq;
using GTA;
using GTA.Math;

namespace Nuclei.Helpers.ExtensionMethods;

public static class EntityExtensions
{
    /// <summary>
    ///     Teleport an entity to a map blip.
    /// </summary>
    /// <param name="entity">The entity to teleport.</param>
    /// <param name="blipSprite">The blip to teleport to (e.g., BlipSprite.Waypoint).</param>
    /// <returns>Whether or not the teleport was successful.</returns>
    public static bool TeleportToBlip(this Entity entity, BlipSprite blipSprite)
    {
        // Find the target blip
        var blip = World.GetAllBlips().FirstOrDefault(b => b.Sprite == blipSprite);

        // If no matching blip is found, return false
        if (blip == null) return false;

        // Get blip position and initial z-coordinate
        var blipPos = blip.Position;
        entity.Position = new Vector3(blipPos.X, blipPos.Y, 0.0f);

        // Wait for 200 milliseconds to allow the game to update
        Script.Wait(200);

        // If entity is in water, return true
        if (entity.IsInWater) return true;

        // Elevate the entity's position until it is above ground
        var attempt = 0;
        while (true)
        {
            attempt++;
            var groundHeight = World.GetGroundHeight(entity.Position);

            // If the entity is above ground, break the loop
            if (entity.Position.Z >= groundHeight && groundHeight != 0) break;

            // Increment the entity's Z-coordinate adaptively based on the ground height difference
            var zCoordIncrement = 1.0f;
            entity.Position = new Vector3(entity.Position.X, entity.Position.Y, entity.Position.Z + zCoordIncrement);

            // Yield if necessary
            if (attempt % 20 == 0) Script.Yield();
        }

        return true;
    }
}