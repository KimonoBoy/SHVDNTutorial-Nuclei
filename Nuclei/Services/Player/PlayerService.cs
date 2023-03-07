namespace Nuclei.Services.Player;

/// <summary>
///     This class represents the PlayerService and serves as a layer between the PlayerMenu and the PlayerScript.
///     The primary responsibility of this class is state management.
/// </summary>
public class PlayerService : IPlayerService
{
    /// <summary>
    ///     A singleton instance of the PlayerService class.
    /// </summary>
    public static readonly PlayerService Instance = new();

    /// <summary>
    ///     A property that determines whether the player is invincible or not.
    /// </summary>
    public bool IsInvincible { get; private set; }

    /// <summary>
    ///     A property that represents the wanted level of the player.
    /// </summary>
    public int WantedLevel { get; private set; }

    /// <summary>
    ///     A method that fixes the player. The implementation of this method will be added later.
    /// </summary>
    public void FixPlayer()
    {
        // We'll implement this a bit later.
    }

    /// <summary>
    ///     A method that sets the value of the `IsInvincible` property.
    /// </summary>
    /// <param name="isInvincible">A boolean value that determines whether the player is invincible or not.</param>
    public void SetInvincible(bool isInvincible)
    {
        IsInvincible = isInvincible;
    }

    /// <summary>
    ///     A method that sets the value of the `WantedLevel` property.
    /// </summary>
    /// <param name="wantedLevel">An integer value that represents the wanted level of the player.</param>
    public void SetWantedLevel(int wantedLevel)
    {
        WantedLevel = wantedLevel;
    }
}