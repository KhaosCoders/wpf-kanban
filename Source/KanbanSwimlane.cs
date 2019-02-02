using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KC.WPF_Kanban
{
    public class KanbanSwimlane : Control
    {
        private const string DefaultCaption = "SWIMLANE";

        #region Override DP Metadata

        static KanbanSwimlane()
        {
            // Enable Themes for this Control
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KanbanSwimlane), new FrameworkPropertyMetadata(typeof(KanbanSwimlane)));
        }

        #endregion

        public List<KanbanBoardCell> Cells { get; set; } = new List<KanbanBoardCell>();

        #region Visual DPs

        /// <summary>
        /// Gets or sets a caption
        /// </summary>
        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(KanbanSwimlane),
                new FrameworkPropertyMetadata(DefaultCaption));

        #endregion

        public string LaneValue { get; set; }
    }
}
