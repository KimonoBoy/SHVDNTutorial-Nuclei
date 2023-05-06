using System;
using System.Linq;
using GTA;

namespace Nuclei.Scripts.Player;

public class OnePunchManScript : PlayerScriptBase
{
    private DateTime _lastEntityCheck = DateTime.UtcNow;

    protected override void OnTick(object sender, EventArgs e)
    {
        ProcessOnePunchMan();
    }

    /// <summary>
    ///     Processes the OnePunchMan feature. When active, hitting an entity with a melee weapon will apply immense force,
    ///     pushing it away.
    /// </summary>
    private void ProcessOnePunchMan()
    {
        if (!Service.IsOnePunchMan) return;
        if (!Character.IsInMeleeCombat) return;

        // Check for damaged entities only once every 100 milliseconds
        if ((DateTime.UtcNow - _lastEntityCheck).TotalMilliseconds < 100) return;

        _lastEntityCheck = DateTime.UtcNow;

        var targetedEntity = GetClosestDamagedEntity();

        if (targetedEntity != null) ApplyOnePunchManForce(targetedEntity, 30.0f, 1000.0f);
    }

    /// <summary>
    ///     Gets the closest damaged entity to the player character that is in front of the player and within a certain
    ///     distance.
    /// </summary>
    /// <returns>The closest damaged Entity object, or null if no such entity is found.</returns>
    private Entity GetClosestDamagedEntity()
    {
        var maxDistance = 20.0f;
        return GTA.World.GetAllEntities()
            .Where(entity => entity.Position.DistanceTo(Character.Position) <= maxDistance &&
                             IsEntityInFrontOfPlayer(entity))
            .OrderBy(entity => entity.Position.DistanceTo(Character.Position))
            .FirstOrDefault(entity => entity != Character && (entity.HasBeenDamagedBy(Character) ||
                                                              entity.IsTouching(Character)));
    }

    /// <summary>
    ///     Determines if the specified entity is in front of the player character.
    /// </summary>
    /// <param name="entity">The Entity object to check.</param>
    /// <returns>True if the entity is in front of the player character, false otherwise.</returns>
    private bool IsEntityInFrontOfPlayer(Entity entity)
    {
        var playerToEntityDirection = (entity.Position - Character.Position).Normalized;
        var dotProduct = playerToEntityDirection.X * Character.ForwardVector.X +
                         playerToEntityDirection.Y * Character.ForwardVector.Y +
                         playerToEntityDirection.Z * Character.ForwardVector.Z;

        // If dotProduct is greater than 0, the entity is in front of the player
        return dotProduct > 0;
    }

    /// <summary>
    ///     Applies OnePunchMan force to the target entity, pushing it away with immense force.
    /// </summary>
    /// <param name="target">The Entity object to apply the force to.</param>
    /// <param name="forceUpwards">The upward force to apply to the target.</param>
    /// <param name="forceForwards">The forward force to apply to the target.</param>
    private void ApplyOnePunchManForce(Entity target, float forceUpwards, float forceForwards)
    {
        target.ApplyForce(Character.UpVector * forceUpwards);
        target.ApplyForce(Character.ForwardVector * forceForwards);

        if (target is Ped ped)
            ped.Kill();

        target.ClearLastWeaponDamage();
    }
}