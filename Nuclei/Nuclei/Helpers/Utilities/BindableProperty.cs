using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Nuclei.Helpers.Utilities;

public class BindableProperty<T>
{
    private T _value;

    public BindableProperty()
    {
    }

    public BindableProperty(T defaultValue)
    {
        _value = defaultValue;
    }

    public T Value
    {
        get => _value;
        set
        {
            if (EqualityComparer<T>.Default.Equals(_value, value)) return;

            if (_value is ObservableCollection<T> oldCollection)
                oldCollection.CollectionChanged -= CollectionChangedHandler;

            _value = value;

            if (_value is ObservableCollection<T> newCollection)
                newCollection.CollectionChanged += CollectionChangedHandler;

            ValueChanged?.Invoke(this, new ValueEventArgs<T>(value));
        }
    }

    public event EventHandler<ValueEventArgs<T>> ValueChanged;

    private void CollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
    {
        ValueChanged?.Invoke(this, new ValueEventArgs<T>(_value));
    }
}

public class ValueEventArgs<T> : EventArgs
{
    public ValueEventArgs(T value)
    {
        Value = value;
    }

    public T Value { get; }
}