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
    /// A panel, that arranges all columns uniformly except when they are collapsed
    /// Columns may also span over multiple columns
    /// </summary>
    public class UniformKanbanPanel : Panel
    {

        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        public static Orientation GetOrientation(DependencyObject obj)
        {
            return (Orientation)obj.GetValue(OrientationProperty);
        }
        public static void SetOrientation(DependencyObject obj, Orientation value)
        {
            obj.SetValue(OrientationProperty, value);
        }
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.RegisterAttached("Orientation", typeof(Orientation), typeof(UniformKanbanPanel),
                new FrameworkPropertyMetadata(Orientation.Horizontal,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

        public int ColumnSpacing { get; set; } = 4;

        protected override Size MeasureOverride(Size availableSize)
        {
            Size size = GetOrientatedSize(0d, availableSize.Width, 0d, availableSize.Height);
            // Calculate different counts needed to calculate the uniform size
            SplotCount count = GetTotalSlotCount();
            if (count.TotalSlots == 0)
            {
                return size;
            }
            bool isHorizontal = Orientation == Orientation.Horizontal;

            // Space to split between columns
            double availableSpace = isHorizontal ? availableSize.Width : availableSize.Height;
            // Substract spacing between columns from available space
            double spacingSpace = (count.TotalSlots - 1) * ColumnSpacing;
            availableSpace -= spacingSpace;

            // First measure the collapsed controls
            // These are fix in size and must therefor substracted from available space
            Size collapsedSize = GetOrientatedSize(1d, availableSize.Width, 1d, availableSize.Height);
            double collapsedSpace = 0d;
            foreach (FrameworkElement element in InternalChildren)
            {
                if (element is ICollapsible collapsible && collapsible.IsCollapsed)
                {
                    if (isHorizontal)
                    {
                        collapsedSize.Width = element.MinWidth > 0 ? element.MinWidth : 1d;
                        element.Measure(collapsedSize);
                        collapsedSpace += element.DesiredSize.Width;
                    }
                    else
                    {
                        collapsedSize.Height = element.MinHeight > 0 ? element.MinHeight : 1d;
                        element.Measure(collapsedSize);
                        collapsedSpace += element.DesiredSize.Height;
                    }
                }
            }
            availableSpace -= collapsedSpace;

            // Then meausure all the other controls
            uniformSpace = availableSpace / count.UsedSlots;
            Size uniformSize = GetOrientatedSize(uniformSpace, availableSize.Width, uniformSpace, availableSize.Height);
            Size controlSpanSize;
            double usedSpace = 0;
            foreach (UIElement element in InternalChildren)
            {
                controlSpanSize = new Size(uniformSize.Width, uniformSize.Height);
                if (!(element is ICollapsible collapsible && collapsible.IsCollapsed))
                {
                    if (element is IColumnSpan columnSpan && columnSpan.ColumnSpan > 1)
                    {
                        if (isHorizontal)
                        {
                            controlSpanSize.Width *= columnSpan.ColumnSpan;
                            controlSpanSize.Width += (columnSpan.ColumnSpan - 1) * ColumnSpacing;
                        }
                        else
                        {
                            controlSpanSize.Height *= columnSpan.ColumnSpan;
                            controlSpanSize.Height += (columnSpan.ColumnSpan - 1) * ColumnSpacing;
                        }
                    }

                    element.Measure(controlSpanSize);
                    if (isHorizontal)
                    {
                        usedSpace += element.DesiredSize.Width;
                    }
                    else
                    {
                        usedSpace += element.DesiredSize.Height;
                    }
                }
            }

            // Finally add all used spaces together
            if (isHorizontal)
            {
                size.Width += spacingSpace + collapsedSpace + usedSpace;
            }
            else
            {
                size.Height += spacingSpace + collapsedSpace + usedSpace;
            }

            return size;
        }

        private double uniformSpace;

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (InternalChildren.Count == 0)
            {
                return finalSize;
            }
            bool isHorizontal = Orientation == Orientation.Horizontal;

            double position = 0;
            double spanSpace;
            Rect controlRect;
            foreach(UIElement element in InternalChildren)
            {
                if (element is ICollapsible collapsible && collapsible.IsCollapsed)
                {
                    // Collapsed element
                    if (isHorizontal)
                    {
                        controlRect = new Rect(position, 0, element.DesiredSize.Width, finalSize.Height);
                    }
                    else
                    {
                        controlRect = new Rect(0, position, finalSize.Width, element.DesiredSize.Height);
                    }
                }
                else if (element is IColumnSpan columnspan && columnspan.ColumnSpan > 1)
                {
                    // Multi-Span element
                    spanSpace = (columnspan.ColumnSpan - 1) * ColumnSpacing;
                    if (isHorizontal)
                    {
                        controlRect = new Rect(position, 0, (uniformSpace * columnspan.ColumnSpan) + spanSpace, finalSize.Height);
                    }
                    else
                    {
                        controlRect = new Rect(0, position, finalSize.Width, (uniformSpace * columnspan.ColumnSpan) + spanSpace);
                    }
                }
                else
                {
                    // Basic 1 Spot rect
                    if (isHorizontal)
                    {
                        controlRect = new Rect(position, 0, uniformSpace, finalSize.Height);
                    }
                    else
                    {
                        controlRect = new Rect(0, position, finalSize.Width, uniformSpace);
                    }
                }
                if (isHorizontal)
                {
                    position += controlRect.Width;
                }
                else
                {
                    position += controlRect.Height;
                }
                position += ColumnSpacing;
                element.Arrange(controlRect);
            }
            if (position > 0)
            {
                position -= ColumnSpacing;
            }
            return GetOrientatedSize(position, finalSize.Width, position, finalSize.Height);
        }

        /// <summary>
        /// Returns a new orientated <see cref="Size"/> where the short side matches the <see cref="Orientation"/> of the panel
        /// </summary>
        private Size GetOrientatedSize(double width, double widthMax, double height, double heightMax)
        {
            if (Orientation == Orientation.Horizontal)
            {
                return new Size(width, heightMax);
            }
            return new Size(widthMax, height);
        }

        private SplotCount GetTotalSlotCount()
        {
            SplotCount count = new SplotCount();
            foreach (UIElement element in InternalChildren)
            {
                if (element is ICollapsible collapsible && collapsible.IsCollapsed)
                {
                    count.CollapsedSlots++;
                }
                else if (element is IColumnSpan columnspan)
                {
                    count.UsedSlots += columnspan.ColumnSpan;
                }
                else
                {
                    count.UsedSlots++;
                }
            }
            return count;
        }

        private class SplotCount
        {
            public int TotalSlots => CollapsedSlots + UsedSlots;

            public int CollapsedSlots { get; set; }

            public int UsedSlots { get; set; }
        }
    }
}
