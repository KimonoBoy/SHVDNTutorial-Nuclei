using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Nuclei.Services.Observable;

public class ObservableService : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}