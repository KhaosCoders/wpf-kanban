using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KC.WPF_Kanban
{
    /// <summary>
    /// This WrapPanel fixes a bug with ItemsPresenter and WrapPanels
    /// </summary>
    public class KanbanCellWrapPanel : WrapPanel
    {
        protected override Size MeasureOverride(Size constraint)
        {
            Size size = base.MeasureOverride(constraint);
            // Invalidate ItemsPresenter, when ever Height doesn't match
            if (size.Height>2 && this.VisualParent is UIElement parent && parent.DesiredSize.Height != size.Height)
            {
                parent.InvalidateMeasure();
            }
            return size;
        }
    }
}
