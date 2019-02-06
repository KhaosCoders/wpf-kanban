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

        #region Cells

        /// <summary>
        /// Gets or sets a <see cref="List{T}"/> or <see cref="KanbanBoardCell"/>
        /// </summary>
        protected List<KanbanBoardCell> Cells { get; set; } = new List<KanbanBoardCell>();

        /// <summary>
        /// Adds a <see cref="KanbanBoardCell"/> to the swimlane
        /// </summary>
        public void AddCell(KanbanBoardCell cell)
        {
            cell.Swimlane = this;
            Cells.Add(cell);
        }

        /// <summary>
        /// Removes a <see cref="KanbanBoardCell"/> from the swimlane
        /// </summary>
        public void RemoveCell(KanbanBoardCell cell)
        {
            if (cell.Swimlane == this)
            {
                cell.Swimlane = null;
            }
            Cells.Remove(cell);
        }

        #endregion

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
