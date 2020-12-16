using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace KC.WPF_Kanban
{
    /// <summary>
    /// A empty card with variable content
    /// </summary>
    [ContentProperty("Content")]
    public class KanbanPlainCard : KanbanCardBase
    {
        #region Override DP Metadata

        static KanbanPlainCard()
        {
            // Enable Themes for this Control
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KanbanPlainCard), new FrameworkPropertyMetadata(typeof(KanbanPlainCard)));
        }

        #endregion

        /// <summary>
        /// Content of the Card
        /// </summary>
        public UIElement Content
        {
            get { return (UIElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(UIElement), typeof(KanbanPlainCard), new UIPropertyMetadata(null));

    }
}
