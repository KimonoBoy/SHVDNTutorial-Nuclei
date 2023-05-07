using System;
using System.Collections.ObjectModel;
using GTA;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Player;

public class ModelChangerService : GenericService<ModelChangerService>
{
    private PedHash _currentPedHash;

    private ObservableCollection<PedHash> _favoriteModels = new();

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
}