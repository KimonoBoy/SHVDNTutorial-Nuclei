using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Nuclei.Services.Observable;

public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged,
    INotifyPropertyChanged
{
    public new TValue this[TKey key]
    {
        get => base[key];
        set
        {
            var oldValue = base[key];
            base[key] = value;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace,
                new KeyValuePair<TKey, TValue>(key, value), new KeyValuePair<TKey, TValue>(key, oldValue)));
        }
    }

    public event NotifyCollectionChangedEventHandler CollectionChanged;
    public event PropertyChangedEventHandler PropertyChanged;

    public new void Add(TKey key, TValue value)
    {
        base.Add(key, value);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
            new KeyValuePair<TKey, TValue>(key, value)));
        OnPropertyChanged("Count");
    }

    public new bool Remove(TKey key)
    {
        if (TryGetValue(key, out var value))
        {
            base.Remove(key);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove,
                new KeyValuePair<TKey, TValue>(key, value)));
            OnPropertyChanged("Count");
            return true;
        }

        return false;
    }

    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        CollectionChanged?.Invoke(this, e);
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}