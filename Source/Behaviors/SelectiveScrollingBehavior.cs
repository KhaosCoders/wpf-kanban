using KC.WPF_Kanban.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace KC.WPF_Kanban.Behaviors;

/// <summary>
/// This Behavior fixes a Bug in .NET
/// The SelectiveScrollingOrientation property only works, when the UIElement has been added to a ScrollViewer
/// if the property is set before that happened, it is ignored
/// </summary>
public class SelectiveScrollingBehavior
{
    #region Orientation

    public static SelectiveScrollingOrientation GetOrientation(DependencyObject obj) => (SelectiveScrollingOrientation)obj.GetValue(OrientationProperty);
    public static void SetOrientation(DependencyObject obj, SelectiveScrollingOrientation value) => obj.SetValue(OrientationProperty, value);

    public static readonly DependencyProperty OrientationProperty =
        DependencyProperty.RegisterAttached("Orientation", typeof(SelectiveScrollingOrientation), typeof(SelectiveScrollingBehavior),
            new PropertyMetadata(SelectiveScrollingOrientation.Both, OnOrientationChanged));

    private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not FrameworkElement oElement
            || e.NewValue is not SelectiveScrollingOrientation oOrientation)
        {
            return;
        }

        SelectiveScrollingGrid.SetSelectiveScrollingOrientation(oElement, oOrientation);
        if (oOrientation == SelectiveScrollingOrientation.Both)
        {
            return;
        }

        if (BindingOperations.GetBinding(oElement.RenderTransform, TranslateTransform.XProperty) == null)
        {
            // setting the property didn't work, propably because it's not inside a ScrollViewer jet
            oElement.IsVisibleChanged += OElement_IsVisibleChanged;
        }
    }

    private static void OElement_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is not FrameworkElement oElement)
        {
            return;
        }

        if (FrameworkUtils.FindVisualParent<ScrollViewer>(oElement) is ScrollViewer scorllViewer)
        {
            oElement.IsVisibleChanged -= OElement_IsVisibleChanged;
            var oOrientation = SelectiveScrollingGrid.GetSelectiveScrollingOrientation(oElement);
            // Property has to be reset before setting it again
            SelectiveScrollingGrid.SetSelectiveScrollingOrientation(oElement, SelectiveScrollingOrientation.Both);
            SelectiveScrollingGrid.SetSelectiveScrollingOrientation(oElement, oOrientation);
        }
    }

    #endregion
}
