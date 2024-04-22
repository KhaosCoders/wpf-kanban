using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace KC.WPF_Kanban;

public class KanbanBlockerPresenter : Control
{
    #region Override DP Metadata

    static KanbanBlockerPresenter()
    {
        // Enable Themes for this Control
        DefaultStyleKeyProperty.OverrideMetadata(typeof(KanbanBlockerPresenter), new FrameworkPropertyMetadata(typeof(KanbanBlockerPresenter)));
    }

    #endregion

    /// <summary>
    /// Gets or sets a collection of <see cref="KanbanBlocker"/>
    /// </summary>
    public IList<KanbanBlocker> Blockers
    {
        get => (IList<KanbanBlocker>)GetValue(BlockersProperty);
        set => SetValue(BlockersProperty, value);
    }
    public static readonly DependencyProperty BlockersProperty =
        DependencyProperty.Register(nameof(Blockers), typeof(IList<KanbanBlocker>), typeof(KanbanBlockerPresenter),
            new FrameworkPropertyMetadata(new ReadOnlyCollection<KanbanBlocker>(new List<KanbanBlocker>())));

}
