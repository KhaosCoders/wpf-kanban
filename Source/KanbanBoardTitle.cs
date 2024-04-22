using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KC.WPF_Kanban;

/// <summary>
/// The default title control of a Kanban Board
/// </summary>
public class KanbanBoardTitle : Control
{
    static KanbanBoardTitle()
    {
        // Enable Themes for this Control
        DefaultStyleKeyProperty.OverrideMetadata(typeof(KanbanBoardTitle), new FrameworkPropertyMetadata(typeof(KanbanBoardTitle)));
    }

    /// <summary>
    /// Gets or sets the title
    /// </summary>
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public static string GetTitle(DependencyObject obj)
    {
        return (string)obj.GetValue(TitleProperty);
    }
    public static void SetTitle(DependencyObject obj, string value)
    {
        obj.SetValue(TitleProperty, value);
    }
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.RegisterAttached("Title", typeof(string), typeof(KanbanBoardTitle),
            new FrameworkPropertyMetadata("Kanban Board",
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

    /// <summary>
    /// Gets or sets whether a reload button is shown in the title
    /// </summary>
    public bool ShowReloadButton
    {
        get => (bool)GetValue(ShowReloadButtonProperty);
        set => SetValue(ShowReloadButtonProperty, value);
    }
    public static bool GetShowReloadButton(DependencyObject obj)
    {
        return (bool)obj.GetValue(ShowReloadButtonProperty);
    }
    public static void SetShowReloadButton(DependencyObject obj, bool value)
    {
        obj.SetValue(ShowReloadButtonProperty, value);
    }
    public static readonly DependencyProperty ShowReloadButtonProperty =
        DependencyProperty.RegisterAttached("ShowReloadButton", typeof(bool), typeof(KanbanBoardTitle),
            new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
}
