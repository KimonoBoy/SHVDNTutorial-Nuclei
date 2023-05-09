using System;
using System.Collections.ObjectModel;
using GTA;
using Nuclei.Services.Generics;
using Nuclei.Services.Player.Dtos;

namespace Nuclei.Services.Player;

public class ModelChangerService : GenericService<ModelChangerService>
{
    private PedHash _currentPedHash;
    private ObservableCollection<CustomPedDto> _customModels = new();

    private ObservableCollection<PedHash> _favoriteModels = new();

    public ObservableCollection<CustomPedDto> CustomModels
    {
        get => _customModels;
        set
        {
            if (Equals(value, _customModels)) return;
            _customModels = value;
            OnPropertyChanged();
        }
    }

    public PedHash CurrentPedHash
    {
        get => _currentPedHash;
        set
        {
            if (value == _currentPedHash) return;
            _currentPedHash = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<PedHash> FavoriteModels
    {
        get => _favoriteModels;
        set
        {
            if (Equals(value, _favoriteModels)) return;
            _favoriteModels = value;
            OnPropertyChanged();
        }
    }

    public event EventHandler<PedHash> ModelChangeRequested;

    public void RequestChangeModel(PedHash pedHash)
    {
        ModelChangeRequested?.Invoke(this, pedHash);
    }

    public event EventHandler<CustomPedDto> CustomModelChangeRequested;

    public void RequestChangeModel(CustomPedDto customPedDto)
    {
        CustomModelChangeRequested?.Invoke(this, customPedDto);
    }
}