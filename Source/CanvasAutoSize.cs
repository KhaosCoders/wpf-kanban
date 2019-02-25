// Thanks to: https://stackoverflow.com/questions/855334/wpf-how-to-make-canvas-auto-resize
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
    /// A <see cref="Canvas"/> that fits to its content
    /// </summary>
    public class CanvasAutoSize : Canvas
    {
        protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint)
        {
            base.MeasureOverride(constraint);

            var children = base
                .InternalChildren
                .OfType<UIElement>();

            double width = 0;
            double height = 0;

            if (children.Any())
            {
                width = children.Max(i => i.DesiredSize.Width + (double)i.GetValue(Canvas.LeftProperty));
                height = children.Max(i => i.DesiredSize.Height + (double)i.GetValue(Canvas.TopProperty));

                if (double.IsNaN(width))
                {
                    width = 0;
                }
                if (double.IsNaN(height))
                {
                    height = 0;
                }
            }

            return new Size(width, height);
        }
    }
}
