using System.Drawing;
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

    public static void Draw3DRectangleAroundObject(this Entity entity)
    {
        // Calculate the dimensions of the bounding box
        var dimensions = entity.Model.Dimensions;

        // Calculate the positions of the 8 corners of the 3D rectangle
        var corners = GetBoundingBoxCorners(entity, dimensions.rearBottomLeft, dimensions.frontTopRight);

        // Draw the lines connecting the corners of the 3D rectangle
        for (var i = 0; i < 4; i++)
        {
            World.DrawLine(corners[i], corners[(i + 1) % 4], Color.Aquamarine);
            World.DrawLine(corners[i + 4], corners[(i + 1) % 4 + 4], Color.Aquamarine);
            World.DrawLine(corners[i], corners[i + 4], Color.Aquamarine);
        }
    }

    public static Vector3[] GetBoundingBoxCorners(this Entity entity, Vector3 min, Vector3 max)
    {
        var corners = new Vector3[8];
        corners[0] = entity.GetOffsetPosition(new Vector3(min.X, min.Y, min.Z));
        corners[1] = entity.GetOffsetPosition(new Vector3(max.X, min.Y, min.Z));
        corners[2] = entity.GetOffsetPosition(new Vector3(max.X, max.Y, min.Z));
        corners[3] = entity.GetOffsetPosition(new Vector3(min.X, max.Y, min.Z));
        corners[4] = entity.GetOffsetPosition(new Vector3(min.X, min.Y, max.Z));
        corners[5] = entity.GetOffsetPosition(new Vector3(max.X, min.Y, max.Z));
        corners[6] = entity.GetOffsetPosition(new Vector3(max.X, max.Y, max.Z));
        corners[7] = entity.GetOffsetPosition(new Vector3(min.X, max.Y, max.Z));

        return corners;
    }
}