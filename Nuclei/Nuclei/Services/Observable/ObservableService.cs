using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Nuclei.Services.Observable;

public abstract class ObservableService : INotifyPropertyChanged
{
    protected Dictionary<string, List<Action>> PropertyActions { get; } = new();
    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        if (propertyName != null && PropertyActions.TryGetValue(propertyName, out var actions))
            foreach (var action in actions)
                action?.Invoke();
    }

    /// <summary>
    ///     Registers an action to be executed when the property changes.
    /// </summary>
    /// <typeparam name="T">The type</typeparam>
    /// <param name="propertyExpression">The property to register an action for when changed.</param>
    /// <param name="action">The action to perform when the property changes.</param>
    public void RegisterAction<T>(Expression<Func<T>> propertyExpression, Action action)
    {
        var propertyName = GetPropertyName(propertyExpression);
        if (!PropertyActions.ContainsKey(propertyName)) PropertyActions[propertyName] = new List<Action>();
        PropertyActions[propertyName].Add(action);
    }

    private string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
    {
        if (propertyExpression == null) throw new ArgumentNullException(nameof(propertyExpression));

        var body = propertyExpression.Body as MemberExpression;
        if (body == null) throw new ArgumentException("Invalid expression.");

        return body.Member.Name;
    }
}