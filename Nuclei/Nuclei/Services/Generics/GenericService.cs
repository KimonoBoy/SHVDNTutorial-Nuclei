using System;
using Nuclei.Helpers.Utilities;

namespace Nuclei.Services.Generics;

public class GenericService<T> where T : class, new()
{
    private static readonly Lazy<T> _instance = new(() => new T());
    public static T Instance => _instance.Value;

    /// <summary>
    ///     Sets the state of the T service to the state of the new state parameter.
    /// </summary>
    /// <param name="newState">The new state to apply to the service.</param>
    public void SetState(T newState)
    {
        // if (newState == null) return;
        //
        // AddCash.Value = newState.AddCash.Value;
        // SuperSpeed.Value = newState.SuperSpeed.Value;
        // IsOnePunchMan.Value = newState.IsOnePunchMan.Value;
        // IsInvisible.Value = newState.IsInvisible.Value;
        // CanRideOnCars.Value = newState.CanRideOnCars.Value;
        // IsNoiseless.Value = newState.IsNoiseless.Value;
        // IsWantedLevelLocked.Value = newState.IsWantedLevelLocked.Value;
        // LockedWantedLevel.Value = newState.LockedWantedLevel.Value;
        // HasInfiniteSpecialAbility.Value = newState.HasInfiniteSpecialAbility.Value;
        // HasInfiniteStamina.Value = newState.HasInfiniteStamina.Value;
        // HasInfiniteBreath.Value = newState.HasInfiniteBreath.Value;
        // IsInvincible.Value = newState.IsInvincible.Value;
        // WantedLevel.Value = newState.WantedLevel.Value;
        // CanSuperJump.Value = newState.CanSuperJump.Value;

        ReflectionUtilities.CopyBindableProperties(newState, this);
    }
}