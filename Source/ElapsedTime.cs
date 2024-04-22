﻿using KC.WPF_Kanban.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace KC.WPF_Kanban;

/// <summary>
/// Displays the time elapsed since a given point in time
/// </summary>
public class ElapsedTime : Control
{
    #region Override DP Metadata

    static ElapsedTime()
    {
        // Enable Themes for this Control
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ElapsedTime), new FrameworkPropertyMetadata(typeof(ElapsedTime)));
    }

    #endregion

    /// <summary>
    /// Gets or sets the point in time, from which the elapsed time is calculated
    /// </summary>
    public DateTime Time
    {
        get => (DateTime)GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }
    public static readonly DependencyProperty TimeProperty =
        DependencyProperty.Register(nameof(Time), typeof(DateTime), typeof(ElapsedTime),
            new FrameworkPropertyMetadata(DateTime.MinValue, new PropertyChangedCallback(OnTimeChanged)));

    private static void OnTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ElapsedTime t)
        {
            t.ConvertTimeStr();
        }
    }

    /// <summary>
    /// Gets or sets the elapsed time as short string
    /// </summary>
    public string ElapsedTimeStr
    {
        get => (string)GetValue(ElapsedTimeStrProperty);
        set => SetValue(ElapsedTimeStrProperty, value);
    }
    public static readonly DependencyProperty ElapsedTimeStrProperty =
        DependencyProperty.Register(nameof(ElapsedTimeStr), typeof(string), typeof(ElapsedTime),
            new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Calculates the elapsed time string
    /// </summary>
    private void ConvertTimeStr()
    {
        _timer?.Stop();
        if (Time == DateTime.MinValue)
        {
            ElapsedTimeStr = string.Empty;
            return;
        }
        TimeSpan duration = DateTime.UtcNow - Time.ToUniversalTime();
        ElapsedTimeStr = duration.AsShortStr(TimeUnits);

        SetTimerInterval(duration);
    }

    /// <summary>
    /// Gets or sets the time units used to display the elapsed time
    /// </summary>
    public TimeUnit TimeUnits
    {
        get => GetTimeUnits(this);
        set => SetTimeUnits(this, value);
    }
    public static TimeUnit GetTimeUnits(DependencyObject obj)
    {
        return (TimeUnit)obj.GetValue(TimeUnitsProperty);
    }
    public static void SetTimeUnits(DependencyObject obj, TimeUnit value)
    {
        obj.SetValue(TimeUnitsProperty, value);
    }
    public static readonly DependencyProperty TimeUnitsProperty =
        DependencyProperty.RegisterAttached(nameof(TimeUnits), typeof(TimeUnit), typeof(ElapsedTime),
            new FrameworkPropertyMetadata(TimeUnit.All, new PropertyChangedCallback(OnTimeUnitsChanged)));

    private static void OnTimeUnitsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
        (d as ElapsedTime)?.ConvertTimeStr();

    private DispatcherTimer _timer;

    /// <summary>
    /// Sets the update timer to a fitting interval
    /// </summary>
    private void SetTimerInterval(TimeSpan duration)
    {
        if (_timer == null)
        {
            _timer = new DispatcherTimer();
            _timer.Tick += _timer_Tick;
        }

        // Set the best interval for updating the control
        if (duration.TotalSeconds <= 60 && TimeUnits.IsSet(TimeUnit.Seconds))
        {
            if (_timer.Interval.TotalSeconds != 1)
            {
                _timer.Interval = TimeSpan.FromSeconds(1);
            }
        }
        else if (duration.TotalMinutes <= 10 && TimeUnits.IsSet(TimeUnit.Minutes))
        {
            if (_timer.Interval.TotalSeconds != 6)
            {
                _timer.Interval = TimeSpan.FromSeconds(6);
            }
        }
        else if (duration.TotalMinutes <= 60 && TimeUnits.IsSet(TimeUnit.Minutes))
        {
            if (_timer.Interval.TotalMinutes != 1)
            {
                _timer.Interval = TimeSpan.FromMinutes(1);
            }
        }
        else if (duration.TotalHours <= 10 && TimeUnits.IsSet(TimeUnit.Hours))
        {
            if (_timer.Interval.TotalMinutes != 6)
            {
                _timer.Interval = TimeSpan.FromMinutes(6);
            }
        }
        else if (duration.TotalHours <= 240 && TimeUnits.IsSet(TimeUnit.Days))
        {
            if (_timer.Interval.TotalHours != 1)
            {
                _timer.Interval = TimeSpan.FromHours(1);
            }
        }
        else
        {
            if (_timer.Interval.TotalDays != 1)
            {
                _timer.Interval = TimeSpan.FromDays(1);
            }
        }
        _timer.Start();
    }

    private void _timer_Tick(object sender, EventArgs e)
    {
        ConvertTimeStr();
    }
}
