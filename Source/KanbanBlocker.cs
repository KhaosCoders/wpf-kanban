using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KC.WPF_Kanban
{
    public class KanbanBlocker : Control
    {
        #region Override DP Metadata

        static KanbanBlocker()
        {
            // Enable Themes for this Control
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KanbanBlocker), new FrameworkPropertyMetadata(typeof(KanbanBlocker)));
        }

        #endregion

        #region Visual DP

        /// <summary>
        /// Gets or sets a description text for the blocker
        /// </summary>
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(KanbanBlocker),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets or sets a one or two character string, representing the type of block
        /// </summary>
        public string BlockType
        {
            get { return (string)GetValue(BlockTypeProperty); }
            set { SetValue(BlockTypeProperty, value); }
        }
        public static readonly DependencyProperty BlockTypeProperty =
            DependencyProperty.Register("BlockType", typeof(string), typeof(KanbanBlocker),
                new FrameworkPropertyMetadata(null));

        #endregion
    }
}
