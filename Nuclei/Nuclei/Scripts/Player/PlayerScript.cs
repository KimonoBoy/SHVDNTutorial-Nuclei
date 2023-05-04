using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using Nuclei.Enums.Player;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Exception;
using Nuclei.Services.Exception.CustomExceptions;
using Nuclei.Services.Player;
using Control = GTA.Control;

namespace Nuclei.Scripts.Player;

public class PlayerScript : GenericScript<PlayerService>
{
    private const int DrownsInWater = 3;
    private DateTime _lastEntityCheck = DateTime.UtcNow;

    protected override void SubscribeToEvents()
    {
        Tick += OnTick;
        KeyDown += OnKeyDown;
        Service.PlayerFixRequested += OnPlayerFixRequested;
        Service.CashInputRequested += OnCashInputRequested;
        Service.AddCashRequested += OnAddCashRequested;
        Service.PropertyChanged += OnPropertyChanged;
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Service.WantedLevel)) Game.Player.WantedLevel = Service.WantedLevel;

        if (e.PropertyName == nameof(Service.IsInvisible)) Game.Player.IsInvincible = Service.IsInvisible;
    }

    protected override void UnsubscribeOnExit()
    {
        Tick -= OnTick;
        KeyDown -= OnKeyDown;
        Service.PlayerFixRequested -= OnPlayerFixRequested;
        Service.CashInputRequested -= OnCashInputRequested;
        Service.AddCashRequested -= OnAddCashRequested;
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (Character == null) return;

        ProcessNoiseless();
        ProcessSuperJump();
        ProcessOnePunchMan();
        ProcessSuperSpeed();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.T && e.Control) CurrentEntity.TeleportToBlip(BlipSprite.Waypoint);
    }

    private void OnPlayerFixRequested(object sender, EventArgs e)
    {
        Character.Health = Character.MaxHealth;
        Character.Armor = Game.Player.MaxArmor;
    }

    private void OnCashInputRequested(object sender, EventArgs e)
    {
        RequestCashInput();
    }

    private void OnAddCashRequested(object sender, CashHash cashHash)
    {
        AddCash(cashHash);
    }

    protected override void ProcessGameStatesTimer(object sender, EventArgs e)
    {
        if (Character == null) return;

        ProcessInvincible();
        ProcessInvisible();
        ProcessInfiniteBreath();
        ProcessRideOnCars();
        ProcessInfiniteStamina();
        ProcessInfiniteSpecialAbility();
        ProcessLockedWantedLevel();
    }

    private void ProcessLockedWantedLevel()
    {
        if (Service.IsWantedLevelLocked)
            Game.Player.WantedLevel = Service.LockedWantedLevel;
    }

    protected override void UpdateServiceStatesTimer(object sender, EventArgs e)
    {
        UpdateWantedLevel();
    }

    private void ProcessInvincible()
    {
        if (Character.IsInvincible == Service.IsInvincible) return;

        Character.IsInvincible = Service.IsInvincible;
    }

    private void ProcessInvisible()
    {
        Character.IsVisible = !Service.IsInvisible;
        Character.CanBeTargetted = !Service.IsInvisible;
        Game.Player.IgnoredByEveryone = Service.IsInvisible;
    }

    private void ProcessInfiniteBreath()
    {
        if (Character.GetConfigFlag(DrownsInWater) == !Service.HasInfiniteBreath) return;

        /*
         * False: Can't drown in water. (InfiniteBreath)
         * True: Can drown in water. (Not InfiniteBreath)  
         */
        Character.SetConfigFlag(DrownsInWater, !Service.HasInfiniteBreath);
    }

    private void ProcessRideOnCars()
    {
        if (Character.CanRagdoll == !Service.CanRideOnCars) return;

        // False means the player won't fall over.
        Character.CanRagdoll = !Service.CanRideOnCars;
    }

    private void UpdateWantedLevel()
    {
        if (Service.WantedLevel == Game.Player.WantedLevel) return;

        Service.WantedLevel = Game.Player.WantedLevel;
    }

    /// <summary>
    ///     The noise level increases slowly over time. This prevents that.
    /// </summary>
    private void ProcessNoiseless()
    {
        if (!Service.IsNoiseless) return;

        Function.Call(Hash.SET_PLAYER_NOISE_MULTIPLIER, Game.Player, 0.0f);
        Function.Call(Hash.SET_PLAYER_SNEAKING_NOISE_MULTIPLIER, Game.Player, 0.0f);
    }

    /// <summary>
    ///     When sprinting or swimming, if the amount of time you can sprint for
    ///     drops below 5 seconds, RESET STAMINA to FULL.
    /// </summary>
    private void ProcessInfiniteStamina()
    {
        if (!Service.HasInfiniteStamina) return;
        if (!Character.IsRunning && !Character.IsSprinting &&
            !Character.IsSwimming) return;

        if (Game.Player.RemainingSprintTime <= 5.0f)
            Function.Call(Hash.RESET_PLAYER_STAMINA, Game.Player);
    }

    /// <summary>
    ///     Restores Special Ability Meter to Full.
    /// </summary>
    private void ProcessInfiniteSpecialAbility()
    {
        if (!Service.HasInfiniteSpecialAbility) return;
        var isAbilityMeterFull = Function.Call<bool>(Hash.IS_SPECIAL_ABILITY_METER_FULL, Game.Player);
        if (isAbilityMeterFull) return;

        Function.Call(Hash.SPECIAL_ABILITY_FILL_METER, Game.Player, true);
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
        return World.GetAllEntities()
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
        ProcessSuperSpeedTicks(maxSpeed, entityForceMultiplier);
    }

    /// <summary>
    ///     Processes the different SuperSpeeds.
    ///     If the player is not sprinting, the SuperSpeed is disabled.
    ///     If the entityForceMultiplier is 0.0f, the entities the player is touching will not be affected.
    /// </summary>
    /// <param name="maxSpeed">The maximum movement speed.</param>
    /// <param name="entityForceMultiplier">The force of which entities the player is touching will be pushed away.</param>
    private void ProcessSuperSpeedTicks(int maxSpeed, float entityForceMultiplier = 0.0f)
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

    /// <summary>
    ///     Allows the player to jump as high as a building.
    /// </summary>
    private void ProcessSuperJump()
    {
        if (!Service.CanSuperJump) return;

        Game.Player.SetSuperJumpThisFrame();
    }

    /// <summary>
    ///     Requests a UserInput Window for setting Player Money.
    /// </summary>
    private void RequestCashInput()
    {
        try
        {
            var maxLength = ulong.MaxValue.ToString().Length;
            var cashInput = Game.GetUserInput(WindowTitle.EnterMessage20, "", 20);
            if (cashInput.Length > maxLength)
                cashInput = cashInput.Substring(0, maxLength);

            if (string.IsNullOrEmpty(cashInput)) return;

            var cashInputAsULong = ulong.TryParse(cashInput, out var result);

            if (cashInputAsULong)
                Game.Player.Money = result > int.MaxValue ? int.MaxValue : (int)result;
            else
                throw new InvalidCashInputException();
        }
        catch (CustomExceptionBase cashInputException)
        {
            ExceptionService.Instance.RaiseError(cashInputException);
        }
        catch (Exception ex)
        {
            ExceptionService.Instance.RaiseError(ex);
        }
    }

    /// <summary>
    ///     Adds the CashHash value to the player's current money amount.
    /// </summary>
    /// <param name="cashHash">The cashHash.</param>
    private static void AddCash(CashHash cashHash)
    {
        var descriptionToInt = new string(cashHash.GetDescription().Where(char.IsDigit).ToArray());

        var parseSuccess = int.TryParse(descriptionToInt, out var result);

        if (!parseSuccess)
            return;

        var newMoney = (long)Game.Player.Money + result;

        if (newMoney > int.MaxValue)
            Game.Player.Money = int.MaxValue;
        else
            Game.Player.Money = (int)newMoney;
    }
}