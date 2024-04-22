﻿using System;
using System.Windows;
using System.Windows.Media;
using KC.WPF_Kanban.Utils;

namespace KC.WPF_Kanban;

/// <summary>
/// A simple card displayed on a <see cref="KanbanBoard"/>
/// </summary>
public class KanbanCard : KanbanCardBase
{
    #region Override DP Metadata

    static KanbanCard()
    {
        // Enable Themes for this Control
        DefaultStyleKeyProperty.OverrideMetadata(typeof(KanbanCard), new FrameworkPropertyMetadata(typeof(KanbanCard)));
    }

    #endregion

    #region Visual DP

    /// <summary>
    /// Gets the left part of the title
    /// </summary>
    public string LeftTitle
    {
        get => (string)GetValue(LeftTitleProperty);
        set => SetValue(LeftTitleProperty, value);
    }
    public static readonly DependencyProperty LeftTitleProperty =
        DependencyProperty.Register(nameof(LeftTitle), typeof(string), typeof(KanbanCard),
            new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Gets the right part of the title
    /// </summary>
    public string RightTitle
    {
        get => (string)GetValue(RightTitleProperty);
        set => SetValue(RightTitleProperty, value);
    }
    public static readonly DependencyProperty RightTitleProperty =
        DependencyProperty.Register(nameof(RightTitle), typeof(string), typeof(KanbanCard),
            new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Gets the background brush used for the title bar
    /// </summary>
    public Brush TitleBackground
    {
        get => (Brush)GetValue(TitleBackgroundProperty);
        set => SetValue(TitleBackgroundProperty, value);
    }
    public static readonly DependencyProperty TitleBackgroundProperty =
        DependencyProperty.Register(nameof(TitleBackground), typeof(Brush), typeof(KanbanCard),
            new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 224, 204))));

    /// <summary>
    /// Gets or sets a foreground brush for the title bar
    /// </summary>
    public Brush TitleForeground
    {
        get => (Brush)GetValue(TitleForegroundProperty);
        set => SetValue(TitleForegroundProperty, value);
    }
    public static readonly DependencyProperty TitleForegroundProperty =
        DependencyProperty.Register(nameof(TitleForeground), typeof(Brush), typeof(KanbanCard),
            new FrameworkPropertyMetadata(Brushes.Black));

    /// <summary>
    /// Gets or sets a descriptive texts
    /// </summary>
    public string DescriptionText
    {
        get => (string)GetValue(DescriptionTextProperty);
        set => SetValue(DescriptionTextProperty, value);
    }
    public static readonly DependencyProperty DescriptionTextProperty =
        DependencyProperty.Register(nameof(DescriptionText), typeof(string), typeof(KanbanCard),
            new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Gets or sets the moment the card was created
    /// </summary>
    public DateTime CreationTime
    {
        get => (DateTime)GetValue(CreationTimeProperty);
        set => SetValue(CreationTimeProperty, value);
    }
    public static readonly DependencyProperty CreationTimeProperty =
        DependencyProperty.Register(nameof(CreationTime), typeof(DateTime), typeof(KanbanCard),
            new FrameworkPropertyMetadata(DateTime.MinValue));

    /// <summary>
    /// Gets or sets the time worked on the card in minutes
    /// </summary>
    public int WorkedMinutes
    {
        get => (int)GetValue(WorkedMinutesProperty);
        set => SetValue(WorkedMinutesProperty, value);
    }
    public static readonly DependencyProperty WorkedMinutesProperty =
        DependencyProperty.Register(nameof(WorkedMinutes), typeof(int), typeof(KanbanCard),
            new FrameworkPropertyMetadata(-1));

    /// <summary>
    /// Gets or sets the <see cref="Brush"/> of a colored tile on the card
    /// </summary>
    public Brush TileBrush
    {
        get => (Brush)GetValue(TileBrushProperty);
        set => SetValue(TileBrushProperty, value);
    }
    public static readonly DependencyProperty TileBrushProperty =
        DependencyProperty.Register(nameof(TileBrush), typeof(Brush), typeof(KanbanCard),
            new FrameworkPropertyMetadata(Brushes.Transparent));

    /// <summary>
    /// Gets or sets a text displayed inside the colored tile
    /// </summary>
    public string TileText
    {
        get => (string)GetValue(TileTextProperty);
        set => SetValue(TileTextProperty, value);
    }
    public static readonly DependencyProperty TileTextProperty =
        DependencyProperty.Register(nameof(TileText), typeof(string), typeof(KanbanCard),
            new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Gets or sets a <see cref="Brush"/> used to display text inside the colored tile
    /// </summary>
    public Brush TileForeground
    {
        get => (Brush)GetValue(TileForegroundProperty);
        set => SetValue(TileForegroundProperty, value);
    }
    public static readonly DependencyProperty TileForegroundProperty =
        DependencyProperty.Register(nameof(TileForeground), typeof(Brush), typeof(KanbanCard),
            new FrameworkPropertyMetadata(Brushes.Black));

    /// <summary>
    /// Gets or sets a size string. Like T-Shirt sizes: S, M, L, XL. This may be mapped to story points.
    /// </summary>
    public string CardSize
    {
        get => (string)GetValue(CardSizeProperty);
        set => SetValue(CardSizeProperty, value);
    }
    public static readonly DependencyProperty CardSizeProperty =
        DependencyProperty.Register(nameof(CardSize), typeof(string), typeof(KanbanCard),
            new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Gets or sets the time units used to display the elapsed time value
    /// </summary>
    public TimeUnit ElapsedTimeUnits
    {
        get => (TimeUnit)GetValue(ElapsedTimeUnitsProperty);
        set => SetValue(ElapsedTimeUnitsProperty, value);
    }
    public static readonly DependencyProperty ElapsedTimeUnitsProperty =
        DependencyProperty.Register("ElapsedTimeUnits", typeof(TimeUnit), typeof(KanbanCard),
            new FrameworkPropertyMetadata(TimeUnit.All));

    #endregion

}
