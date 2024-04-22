using System.Windows;
using System;
using System.Windows.Controls;

namespace KC.WPF_Kanban;

public class KanbanBoardReloadButton : Button
{
    static KanbanBoardReloadButton()
    {
        Type ownerType = typeof(KanbanBoardReloadButton);
        DefaultStyleKeyProperty.OverrideMetadata(ownerType, new FrameworkPropertyMetadata(ownerType));
    }

    /// <summary>
    /// A event that is fired when the reload board button is clicked
    /// </summary>
    public event EventHandler ReloadBoardClicked
    {
        add => AddHandler(ReloadBoardClickedEvent, value);
        remove => RemoveHandler(ReloadBoardClickedEvent, value);
    }
    public static readonly RoutedEvent ReloadBoardClickedEvent = EventManager
        .RegisterRoutedEvent(nameof(ReloadBoardClicked), RoutingStrategy.Bubble, typeof(EventHandler), typeof(KanbanBoardReloadButton));

    protected override void OnClick()
    {
        RaiseEvent(new RoutedEventArgs(ReloadBoardClickedEvent, this));
    }
}
