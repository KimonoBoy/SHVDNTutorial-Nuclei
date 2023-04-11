using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using Nuclei.Enums.Player;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Helpers.Utilities;
using Nuclei.Services.Exception;
using Nuclei.Services.Exception.CustomExceptions;
using Nuclei.Services.Generics;
using Nuclei.Services.Player;
using Nuclei.UI.Text;
using Control = GTA.Control;

namespace Nuclei.Scripts.Player;

public class PlayerScript : Script
{
    private readonly PlayerService _playerService = PlayerService.Instance;

    private readonly GenericStateService<PlayerService> _playerServiceState =
        GenericStateService<PlayerService>.Instance;

    private DateTime _lastEntityCheck = DateTime.UtcNow;

    public PlayerScript()
    {
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        Tick += OnTick;
        KeyDown += OnKeyDown;
        _playerService.PlayerFixed += OnPlayerFixed;
        _playerService.IsInvincible.ValueChanged += OnInvincibleChanged;
        _playerService.WantedLevel.ValueChanged += OnWantedLevelChanged;
        _playerService.CashInputRequested += OnCashInputRequested;
        _playerService.HasInfiniteBreath.ValueChanged += OnInfiniteBreathChanged;
        _playerService.CanRideOnCars.ValueChanged += OnCanRideOnCarsChanged;
        _playerService.AddCashRequested += OnAddCashRequested;
        _playerService.IsInvisible.ValueChanged += OnIsInvisibleChanged;
    }

    private void OnIsInvisibleChanged(object sender, ValueEventArgs<bool> e)
    {
        Game.Player.Character.IsVisible = !e.Value;
        Game.Player.Character.CanBeTargetted = !e.Value;
        Game.Player.IgnoredByEveryone = e.Value;
    }

    private void OnTick(object sender, EventArgs e)
    {
        Display.DrawTextElement($"PlayerService SuperJump: {_playerService.CanSuperJump.Value}", 100.0f, 100.0f,
            Color.AntiqueWhite);
        Display.DrawTextElement($"PlayerServiceState SuperJump: {_playerServiceState.GetState().CanSuperJump.Value}",
            100.0f,
            120.0f, Color.AntiqueWhite);
        UpdateStates();
        ProcessFunctions();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.T && e.Control)
            Game.Player.Character.TeleportToBlip(BlipSprite.Waypoint);

        if (e.KeyCode == Keys.K && e.Control)
        {
            // Update the current state
            _playerServiceState.SetState(_playerService);

            // Save the updated state
            _playerServiceState.SaveState();
        }

        if (e.KeyCode == Keys.L && e.Control)
        {
            // Load the state from the file.
            var loadedPlayerService = _playerServiceState.LoadState();

            // Set the _playerService current state to the loaded state.
            if (loadedPlayerService != null) _playerService.SetState(loadedPlayerService);
        }
    }

    private void OnPlayerFixed(object sender, EventArgs e)
    {
        Game.Player.Character.Health = Game.Player.Character.MaxHealth;
        Game.Player.Character.Armor = Game.Player.MaxArmor;
    }

    private void OnInvincibleChanged(object sender, ValueEventArgs<bool> e)
    {
        Game.Player.Character.IsInvincible = e.Value;
    }

    private void OnWantedLevelChanged(object sender, ValueEventArgs<int> e)
    {
        Game.Player.WantedLevel =
            !_playerService.IsWantedLevelLocked.Value ? e.Value : _playerService.LockedWantedLevel.Value;
    }

    private void OnCashInputRequested(object sender, EventArgs e)
    {
        RequestCashInput();
    }

    private void OnInfiniteBreathChanged(object sender, ValueEventArgs<bool> e)
    {
        /*
         * PED_CONFIG_FLAG 3 is "DrownsInWater".
         *
         * False: Can't drown in water. (InfiniteBreath)
         * True: Can drown in water. (Not InfiniteBreath)  
         */
        Game.Player.Character.SetConfigFlag(3, !e.Value);
    }

    private void OnCanRideOnCarsChanged(object sender, ValueEventArgs<bool> e)
    {
        // False means the player won't fall over.
        Game.Player.Character.CanRagdoll = !e.Value;
    }

    private void OnAddCashRequested(object sender, CashHash cashHash)
    {
        AddCash(cashHash);
    }

    /// <summary>
    ///     Updates the different states in the service.
    /// </summary>
    private void UpdateStates()
    {
        /*
         * Until we add save & load functionality, we'll just update the states here.
         */

        UpdateInvincible();
        UpdateWantedLevel();
        UpdateInfiniteBreath();
        UpdateRideOnCars();
        UpdateInvisible();
    }

    private void UpdateInvisible()
    {
        if (_playerService.IsInvisible.Value == Game.Player.Character.IsVisible)
            _playerService.IsInvisible.Value = !Game.Player.Character.IsVisible;
    }

    private void UpdateRideOnCars()
    {
        if (_playerService.CanRideOnCars.Value == Game.Player.Character.CanRagdoll)
            _playerService.CanRideOnCars.Value = !Game.Player.Character.CanRagdoll;
    }

    private void UpdateInfiniteBreath()
    {
        if (_playerService.HasInfiniteBreath.Value == Game.Player.Character.GetConfigFlag(3))
            _playerService.HasInfiniteBreath.Value = !Game.Player.Character.GetConfigFlag(3);
    }

    private void UpdateInvincible()
    {
        // This needs to be implemented differently. We'll cover it later and you'll see why.
        if (_playerService.IsInvincible.Value != Game.Player.Character.IsInvincible)
            _playerService.IsInvincible.Value = Game.Player.Character.IsInvincible;
    }

    private void UpdateWantedLevel()
    {
        if (_playerService.WantedLevel.Value != Game.Player.WantedLevel)
            _playerService.WantedLevel.Value = Game.Player.WantedLevel;
    }

    /// <summary>
    ///     Game changes that need to be processed each frame.
    /// </summary>
    private void ProcessFunctions()
    {
        ProcessInfiniteStamina();
        ProcessInfiniteSpecialAbility();
        ProcessNoiseless();
        ProcessSuperJump();
        ProcessOnePunchMan();
        ProcessSuperSpeed();
    }

    /// <summary>
    ///     Processes the OnePunchMan feature. When active, hitting an entity with a melee weapon will apply immense force,
    ///     pushing it away.
    /// </summary>
    private void ProcessOnePunchMan()
    {
        if (!_playerService.IsOnePunchMan.Value || !Game.Player.Character.IsInMeleeCombat) return;

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
            .Where(entity => entity.Position.DistanceTo(Game.Player.Character.Position) <= maxDistance &&
                             IsEntityInFrontOfPlayer(entity))
            .OrderBy(entity => entity.Position.DistanceTo(Game.Player.Character.Position))
            .FirstOrDefault(entity => entity.HasBeenDamagedBy(Game.Player.Character) ||
                                      entity.IsTouching(Game.Player.Character));
    }

    /// <summary>
    ///     Determines if the specified entity is in front of the player character.
    /// </summary>
    /// <param name="entity">The Entity object to check.</param>
    /// <returns>True if the entity is in front of the player character, false otherwise.</returns>
    private bool IsEntityInFrontOfPlayer(Entity entity)
    {
        var playerToEntityDirection = (entity.Position - Game.Player.Character.Position).Normalized;
        var dotProduct = playerToEntityDirection.X * Game.Player.Character.ForwardVector.X +
                         playerToEntityDirection.Y * Game.Player.Character.ForwardVector.Y +
                         playerToEntityDirection.Z * Game.Player.Character.ForwardVector.Z;

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
        target.ApplyForce(Game.Player.Character.UpVector * forceUpwards);
        target.ApplyForce(Game.Player.Character.ForwardVector * forceForwards);
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

        switch (_playerService.SuperSpeed.Value)
        {
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

        if (!Game.IsControlPressed(Control.Sprint) || Game.Player.Character.IsJumping) return;

        Game.Player.Character.MaxSpeed = maxSpeed;
        Game.Player.Character.ApplyForce(Game.Player.Character.ForwardVector * maxSpeed);

        // Raycast to find ground position below character (more accurate than using Z coordinate (HeightAboveGround))
        var characterPosition = Game.Player.Character.Position;
        var raycastResult = World.Raycast(characterPosition, characterPosition - new Vector3(0, 0, 50.0f),
            IntersectFlags.Everything);

        if (raycastResult.DidHit)
        {
            // Calculate the distance to the ground
            var distanceToGround = characterPosition.Z - raycastResult.HitPosition.Z;

            // Apply a force proportional to the distance to the ground to keep the character on the ground
            if (distanceToGround >= 0.2f)
                Game.Player.Character.ApplyForce(Game.Player.Character.UpVector *
                                                 (-maxSpeed * (1 + distanceToGround)));
        }

        if (entityForceMultiplier <= 0.0f) return;

        // Gets all entities that are touching the player.
        var touchingEntities = World.GetAllEntities()
            .OrderBy(entity => entity.Position.DistanceTo(Game.Player.Character.Position))
            .Where(entity =>
                entity != Game.Player.Character && entity.IsTouching(Game.Player.Character));

        // Pushes the entities away from the player.
        touchingEntities.ToList().ForEach(entity =>
            entity.ApplyForce(Game.Player.Character.ForwardVector * maxSpeed * entityForceMultiplier));
    }

    /// <summary>
    ///     When sprinting or swimming, if the amount of time you can sprint for
    ///     drops below 5 seconds, RESET STAMINA to FULL.
    /// </summary>
    private void ProcessInfiniteStamina()
    {
        if (!_playerService.HasInfiniteStamina.Value) return;
        if (!Game.Player.Character.IsRunning && !Game.Player.Character.IsSprinting &&
            !Game.Player.Character.IsSwimming) return;

        if (Game.Player.RemainingSprintTime <= 5.0f)
            Function.Call(Hash.RESET_PLAYER_STAMINA, Game.Player);
    }

    /// <summary>
    ///     Allows the player to jump as high as a building.
    /// </summary>
    private void ProcessSuperJump()
    {
        if (!_playerService.CanSuperJump.Value) return;

        Game.Player.SetSuperJumpThisFrame();
    }

    /// <summary>
    ///     Restores Special Ability Meter to Full.
    /// </summary>
    private void ProcessInfiniteSpecialAbility()
    {
        if (!_playerService.HasInfiniteSpecialAbility.Value) return;
        var isAbilityMeterFull = Function.Call<bool>(Hash.IS_SPECIAL_ABILITY_METER_FULL, Game.Player);

        if (isAbilityMeterFull) return;

        Function.Call(Hash.SPECIAL_ABILITY_FILL_METER, Game.Player, true);
    }

    /// <summary>
    ///     The noise level increases slowly over time. This prevents that.
    /// </summary>
    private void ProcessNoiseless()
    {
        if (!_playerService.IsNoiseless.Value) return;

        Function.Call(Hash.SET_PLAYER_NOISE_MULTIPLIER, Game.Player, 0.0f);
        Function.Call(Hash.SET_PLAYER_SNEAKING_NOISE_MULTIPLIER, Game.Player, 0.0f);
    }

    /// <summary>
    ///     Requests a UserInput Window for setting Player Money.
    /// </summary>
    private void RequestCashInput()
    {
        try
        {
            var maxLength = int.MaxValue.ToString().Length;
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