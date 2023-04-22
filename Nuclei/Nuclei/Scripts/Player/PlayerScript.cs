using System;
using System.Linq;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using Nuclei.Enums.Player;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Helpers.Utilities.BindableProperty;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Exception;
using Nuclei.Services.Exception.CustomExceptions;
using Nuclei.Services.Player;
using Control = GTA.Control;

namespace Nuclei.Scripts.Player;

public class PlayerScript : GenericScriptBase<PlayerService>
{
    private DateTime _lastEntityCheck = DateTime.UtcNow;

    protected override void SubscribeToEvents()
    {
        Tick += OnTick;
        KeyDown += OnKeyDown;
        Service.PlayerFixed += OnPlayerFixed;
        Service.WantedLevel.ValueChanged += OnWantedLevelChanged;
        Service.CashInputRequested += OnCashInputRequested;
        Service.AddCashRequested += OnAddCashRequested;
        GameStateTimer.SubscribeToTimerElapsed(UpdatePlayer);
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (Character == null) return;

        UpdateFeature(Service.WantedLevel, UpdateWantedLevel);
        UpdateFeature(Service.IsNoiseless, ProcessNoiseless);
        UpdateFeature(Service.CanSuperJump, ProcessSuperJump);
        UpdateFeature(Service.IsOnePunchMan, ProcessOnePunchMan);
        UpdateFeature(Service.SuperSpeed, ProcessSuperSpeed);
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.T && e.Control) CurrentEntity.TeleportToBlip(BlipSprite.Waypoint);
    }

    private void OnPlayerFixed(object sender, EventArgs e)
    {
        Character.Health = Character.MaxHealth;
        Character.Armor = Game.Player.MaxArmor;
    }

    private void OnWantedLevelChanged(object sender, ValueEventArgs<int> wantedLevel)
    {
        Game.Player.WantedLevel = wantedLevel.Value;
    }

    private void OnCashInputRequested(object sender, EventArgs e)
    {
        RequestCashInput();
    }

    private void OnAddCashRequested(object sender, CashHash cashHash)
    {
        AddCash(cashHash);
    }

    private void UpdatePlayer(object sender, EventArgs e)
    {
        if (Character == null) return;

        UpdateFeature(Service.IsInvincible, UpdateInvincible);
        UpdateFeature(Service.IsInvisible, UpdateInvisible);
        UpdateFeature(Service.HasInfiniteBreath, UpdateInfiniteBreath);
        UpdateFeature(Service.CanRideOnCars, UpdateRideOnCars);

        /*
         * Process Functions that doesn't need to be called every tick.
         */
        UpdateFeature(Service.HasInfiniteStamina, ProcessInfiniteStamina);
        UpdateFeature(Service.HasInfiniteSpecialAbility, ProcessInfiniteSpecialAbility);
    }

    private void UpdateInvincible(bool invincible)
    {
        if (Character.IsInvincible == invincible) return;

        Character.IsInvincible = invincible;
    }

    private static void UpdateInvisible(bool invisible)
    {
        Character.IsVisible = !invisible;
        Character.CanBeTargetted = !invisible;
        Game.Player.IgnoredByEveryone = invisible;
    }

    private static void UpdateInfiniteBreath(bool infiniteBreath)
    {
        if (Character.GetConfigFlag(DrownsInWater) == !infiniteBreath) return;

        /*
         * False: Can't drown in water. (InfiniteBreath)
         * True: Can drown in water. (Not InfiniteBreath)  
         */
        Character.SetConfigFlag(DrownsInWater, !infiniteBreath);
    }

    private void UpdateRideOnCars(bool rideOnCars)
    {
        if (Character.CanRagdoll == !rideOnCars) return;

        // False means the player won't fall over.
        Character.CanRagdoll = !rideOnCars;
    }

    private void UpdateWantedLevel(int wantedLevel)
    {
        if (Service.IsWantedLevelLocked.Value)
            Game.Player.WantedLevel = Service.LockedWantedLevel.Value;

        if (wantedLevel != Game.Player.WantedLevel)
            Service.WantedLevel.Value = Game.Player.WantedLevel;
    }

    /// <summary>
    ///     The noise level increases slowly over time. This prevents that.
    /// </summary>
    private void ProcessNoiseless(bool noiseless)
    {
        if (!noiseless) return;

        Function.Call(Hash.SET_PLAYER_NOISE_MULTIPLIER, Game.Player, 0.0f);
        Function.Call(Hash.SET_PLAYER_SNEAKING_NOISE_MULTIPLIER, Game.Player, 0.0f);
    }

    /// <summary>
    ///     When sprinting or swimming, if the amount of time you can sprint for
    ///     drops below 5 seconds, RESET STAMINA to FULL.
    /// </summary>
    private void ProcessInfiniteStamina(bool infiniteStamina)
    {
        if (!infiniteStamina) return;
        if (!Character.IsRunning && !Character.IsSprinting &&
            !Character.IsSwimming) return;

        if (Game.Player.RemainingSprintTime <= 5.0f)
            Function.Call(Hash.RESET_PLAYER_STAMINA, Game.Player);
    }

    /// <summary>
    ///     Restores Special Ability Meter to Full.
    /// </summary>
    private void ProcessInfiniteSpecialAbility(bool infiniteSpecialAbility)
    {
        if (!infiniteSpecialAbility) return;
        var isAbilityMeterFull = Function.Call<bool>(Hash.IS_SPECIAL_ABILITY_METER_FULL, Game.Player);

        if (isAbilityMeterFull) return;

        Function.Call(Hash.SPECIAL_ABILITY_FILL_METER, Game.Player, true);
    }

    /// <summary>
    ///     Processes the OnePunchMan feature. When active, hitting an entity with a melee weapon will apply immense force,
    ///     pushing it away.
    /// </summary>
    private void ProcessOnePunchMan(bool onePunchMan)
    {
        if (!onePunchMan) return;
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
    private void ProcessSuperSpeed(SuperSpeedHash superSpeed)
    {
        int maxSpeed;
        float entityForceMultiplier;

        Game.Player.SetRunSpeedMultThisFrame(1.49f);

        switch (superSpeed)
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
        /*
         * We will rework this code later. We will add a few more functionality, such as WallRunning, FlashTime (SlowMotion when Flash), Improved SuperJump.
         *
         * Should also prevent the player from falling over, and make him collision proof.
         * For now you can activate: RideOnCars and Invincible
         *
         * We'll also add a menu, where the user can select the different values of forces and speeds.
         */

        if (!Game.IsControlPressed(Control.Sprint) || Character.IsJumping) return;

        Character.MaxSpeed = maxSpeed;
        Character.ApplyForce(Character.ForwardVector * maxSpeed);

        // Raycast to find ground position below character (more accurate than using Z coordinate (HeightAboveGround))
        var characterPosition = Character.Position;
        var raycastResult = World.Raycast(characterPosition, characterPosition - new Vector3(0, 0, 50.0f),
            IntersectFlags.Everything);

        if (raycastResult.DidHit)
        {
            // Calculate the distance to the ground
            var distanceToGround = characterPosition.Z - raycastResult.HitPosition.Z;

            // Apply a force proportional to the distance to the ground to keep the character on the ground
            if (distanceToGround >= 0.2f)
                Character.ApplyForce(Character.UpVector *
                                     (-maxSpeed * (1 + distanceToGround)));
        }

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
    private void ProcessSuperJump(bool superJump)
    {
        if (!superJump) return;

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