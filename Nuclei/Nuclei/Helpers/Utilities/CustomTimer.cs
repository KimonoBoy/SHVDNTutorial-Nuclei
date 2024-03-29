﻿using System;
using System.Linq;
using System.Windows.Forms;

namespace Nuclei.Helpers.Utilities;

public class CustomTimer
{
    private readonly Timer _timer = new();

    public CustomTimer(int interval)
    {
        _timer.Interval = interval;
        _timer.Tick += OnTimerElapsed;
        _timer.Start();
    }

    public event EventHandler TimerElapsed;

    private void OnTimerElapsed(object sender, EventArgs e)
    {
        TimerElapsed?.Invoke(this, EventArgs.Empty);
    }

    internal void SubscribeToTimerElapsed(EventHandler eventHandler)
    {
        if (TimerElapsed == null || !TimerElapsed.GetInvocationList().Contains(eventHandler))
            TimerElapsed += eventHandler;
    }

    private void UnsubscribeFromTimerElapsed(EventHandler eventHandler)
    {
        if (TimerElapsed != null && TimerElapsed.GetInvocationList().Contains(eventHandler))
            TimerElapsed -= eventHandler;
    }

    public void Stop()
    {
        _timer.Stop();
        _timer.Dispose();

        // Unsubscribe all custom event handlers
        if (TimerElapsed == null) return;

        foreach (var handler in TimerElapsed.GetInvocationList())
            UnsubscribeFromTimerElapsed((EventHandler)handler);
    }
}