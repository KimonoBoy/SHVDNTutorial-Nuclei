using System;
using System.Linq;
using GTA;
using Nuclei.Enums.Player;

namespace Nuclei.Scripts.Player;

public class SuperSpeedScript : PlayerScriptBase
{
    protected override void OnTick(object sender, EventArgs e)
    {
        ProcessSuperSpeed();
    }


    /// <summary>
    ///     Processes the different SuperSpeeds.
    ///     If the player is not sprinting, the SuperSpeed is disabled.
    ///     If the entityForceMultiplier is 0.0f, the entities the player is touching will not be affected.
    /// </summary>
    private void ProcessSuperSpeed()
    {
        int maxSpeed;
        float entityForceMultiplier;

        Game.Player.SetRunSpeedMultThisFrame(1.49f);

        switch (Service.SuperSpeed)
        {
            case SuperSpeedHash.Fast:
                return; // Do nothing, only apply the run speed multiplier.
            case SuperSpeedHash.Faster:
                maxSpeed = 30;
                entityForceMultiplier = 0.0f;
                break;
            case SuperSpeedHash.Sonic:
                maxSpeed = 70;
                entityForceMultiplier = 300.0f;
                break;
            case SuperSpeedHash.TheFlash:
                maxSpeed = 120;
                entityForceMultiplier = 1000.0f;
                break;
            default:
                Game.Player.SetRunSpeedMultThisFrame(1.0f); // 1.0f is the default speed.
                return;
        }

        if (!Character.IsRunning && !Character.IsSprinting) return;
        ApplySuperSpeedForce(maxSpeed, entityForceMultiplier);
    }

    /// <summary>
    ///     Processes the different SuperSpeeds.
    ///     If the player is not sprinting, the SuperSpeed is disabled.
    ///     If the entityForceMultiplier is 0.0f, the entities the player is touching will not be affected.
    /// </summary>
    /// <param name="maxSpeed">The maximum movement speed.</param>
    /// <param name="entityForceMultiplier">The force of which entities the player is touching will be pushed away.</param>
    private void ApplySuperSpeedForce(int maxSpeed, float entityForceMultiplier = 0.0f)
    {
        if (!Game.IsControlPressed(Control.Sprint) || Character.IsJumping) return;

        Character.MaxSpeed = maxSpeed;
        Character.ApplyForce(Character.ForwardVector * maxSpeed);

        var distanceToGround = Character.HeightAboveGround;

        // Apply a force proportional to the distance to the ground to keep the character on the ground
        if (distanceToGround >= 1.5f)
            Character.ApplyForce(Character.UpVector * (-maxSpeed * (1 + distanceToGround)));

        ApplyForceOnImpact(maxSpeed, entityForceMultiplier);
    }

    private void ApplyForceOnImpact(int maxSpeed, float entityForceMultiplier)
    {
        if (entityForceMultiplier <= 0.0f) return;

        // Gets all entities that are touching the player.
        var touchingEntities = World.GetAllEntities()
            .OrderBy(entity => entity.Position.DistanceTo(Character.Position))
            .Where(entity =>
                entity != Character && entity.IsTouching(Character));

        // Pushes the entities away from the player.
        touchingEntities.ToList().ForEach(entity =>
            entity.ApplyForce(Character.ForwardVector * maxSpeed * entityForceMultiplier));
    }
}