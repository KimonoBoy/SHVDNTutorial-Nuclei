using System;
using System.Collections.Generic;

namespace Nuclei.Helpers.Utilities;

public class BindableProperty<T>
{
    private T _value;

    public T Value
    {
        get => _value;
        set
        {
            if (EqualityComparer<T>.Default.Equals(_value, value)) return;

            _value = value;
            ValueChanged?.Invoke(this, new ValueEventArgs<T>(value));
        }
    }

    public event EventHandler<ValueEventArgs<T>> ValueChanged;
}

public class ValueEventArgs<T> : EventArgs
{
    public ValueEventArgs(T value)
    {
        Value = value;
    }

    public T Value { get; }
}