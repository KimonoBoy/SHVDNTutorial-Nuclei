using GTA;
using GTA.Math;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Weapon;

namespace Nuclei.Scripts.Weapon;

public abstract class WeaponScriptBase : GenericScript<WeaponsService>
{
    /// <summary>
    ///     Gets the aimed position, if the crosshair hits a target return that position, otherwise if the crosshair does not
    ///     intersect with anything return the camera hit position at max cameraDistance.
    /// </summary>
    /// <param name="cameraDistance">The distance when camera is used as the return value</param>
    /// <returns>The hitCoords</returns>
    public Vector3 GetAimedPosition(float cameraDistance)
    {
        Vector3 aimedPosition;

        var crosshairCoords = World.GetCrosshairCoordinates(IntersectFlags.Everything);

        if (crosshairCoords.DidHit)
            aimedPosition = crosshairCoords.HitPosition;
        else
            aimedPosition = GameplayCamera.Position + GameplayCamera.Direction * cameraDistance;
        return aimedPosition;
    }

    protected override void SubscribeToEvents()
    {
    }

    protected override void UnsubscribeOnExit()
    {
    }
}