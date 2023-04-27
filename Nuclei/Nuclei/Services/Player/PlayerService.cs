using System;
using Nuclei.Enums.Player;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Player;

public class PlayerService : GenericService<PlayerService>
{
    private CashHash _addCash;

    private bool _canRideOnCars;

    private bool _canSuperJump;

    private bool _hasInfiniteBreath;

    private bool _hasInfiniteSpecialAbility;

    private bool _hasInfiniteStamina;

    private bool _isInvincible;

    private bool _isInvisible;

    private bool _isNoiseless;

    private bool _isOnePunchMan;

    private bool _isWantedLevelLocked;

    private int _lockedWantedLevel;

    private SuperSpeedHash _superSpeed;

    private int _wantedLevel;

    public CashHash AddCash
    {
        get => _addCash;
        set
        {
            if (_addCash == value) return;
            _addCash = value;
            OnPropertyChanged();
        }
    }

    public SuperSpeedHash SuperSpeed
    {
        get => _superSpeed;
        set
        {
            if (_superSpeed == value) return;
            _superSpeed = value;
            OnPropertyChanged();
        }
    }

    public bool IsOnePunchMan
    {
        get => _isOnePunchMan;
        set
        {
            if (_isOnePunchMan == value) return;
            _isOnePunchMan = value;
            OnPropertyChanged();
        }
    }

    public bool IsInvisible
    {
        get => _isInvisible;
        set
        {
            if (_isInvisible == value) return;
            _isInvisible = value;
            OnPropertyChanged();
        }
    }

    public bool CanRideOnCars
    {
        get => _canRideOnCars;
        set
        {
            if (_canRideOnCars == value) return;
            _canRideOnCars = value;
            OnPropertyChanged();
        }
    }

    public bool IsNoiseless
    {
        get => _isNoiseless;
        set
        {
            if (_isNoiseless == value) return;
            _isNoiseless = value;
            OnPropertyChanged();
        }
    }

    public bool IsWantedLevelLocked
    {
        get => _isWantedLevelLocked;
        set
        {
            if (_isWantedLevelLocked == value) return;
            _isWantedLevelLocked = value;
            OnPropertyChanged();
        }
    }

    public int LockedWantedLevel
    {
        get => _lockedWantedLevel;
        set
        {
            if (_lockedWantedLevel == value) return;
            _lockedWantedLevel = value;
            OnPropertyChanged();
        }
    }

    public bool HasInfiniteSpecialAbility
    {
        get => _hasInfiniteSpecialAbility;
        set
        {
            if (_hasInfiniteSpecialAbility == value) return;
            _hasInfiniteSpecialAbility = value;
            OnPropertyChanged();
        }
    }

    public bool HasInfiniteStamina
    {
        get => _hasInfiniteStamina;
        set
        {
            if (_hasInfiniteStamina == value) return;
            _hasInfiniteStamina = value;
            OnPropertyChanged();
        }
    }

    public bool HasInfiniteBreath
    {
        get => _hasInfiniteBreath;
        set
        {
            if (_hasInfiniteBreath == value) return;
            _hasInfiniteBreath = value;
            OnPropertyChanged();
        }
    }

    public bool IsInvincible
    {
        get => _isInvincible;
        set
        {
            if (_isInvincible == value) return;
            _isInvincible = value;
            OnPropertyChanged();
        }
    }

    public int WantedLevel
    {
        get => _wantedLevel;
        set
        {
            if (_wantedLevel == value) return;
            _wantedLevel = value;
            OnPropertyChanged();
        }
    }

    public bool CanSuperJump
    {
        get => _canSuperJump;
        set
        {
            if (_canSuperJump == value) return;
            _canSuperJump = value;
            OnPropertyChanged();
        }
    }

    public event EventHandler PlayerFixRequested;

    public void RequestFixPlayer()
    {
        PlayerFixRequested?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler CashInputRequested;

    public void RequestCashInput()
    {
        CashInputRequested?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler<CashHash> AddCashRequested;

    public void RequestCashResult(CashHash cashHash)
    {
        AddCashRequested?.Invoke(this, cashHash);
    }
}