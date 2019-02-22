using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using KC.WPF_Kanban.Model;
using KC.WPF_Kanban.Utils;

namespace KC.WPF_Kanban
{
    public class KanbanSwimlane : Control, INotifyPropertyChanged
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
            cell.CardsChanged += Cell_CardsChanged;
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
                cell.CardsChanged -= Cell_CardsChanged;
            }
            Cells.Remove(cell);
        }

        /// <summary>
        /// Gets the count of all cards assigned to the <see cref="KanbanSwimlane"/>
        /// </summary>
        public int CardCount => Cells.Sum(cell => cell.Cards.Count);

        private void Cell_CardsChanged(object sender, RoutedEventArgs e) =>
            OnPropertyChanged(nameof(CardCount));

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

        /// <summary>
        /// Gets or sets whether the <see cref="KanbanSwimlane"/> is collapsed
        /// </summary>
        public bool IsCollapsed
        {
            get { return (bool)GetValue(IsCollapsedProperty); }
            set { SetValue(IsCollapsedProperty, value); }
        }
        public static readonly DependencyProperty IsCollapsedProperty =
            DependencyProperty.Register("IsCollapsed", typeof(bool), typeof(KanbanSwimlane),
                new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsCollapsedChanged)));

        private static void OnIsCollapsedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Relayout kanban panel, when swimlane is expanded or collapsed
            if (d is KanbanSwimlane lane)
            {
                var panel = FrameworkUtils.FindVisualParent<KanbanBoardGridPanel>(lane);
                if (panel != null)
                {
                    panel.InvalidateMeasure();
                }
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets a unique value that is used to assign cards to the <see cref="KanbanSwimlane"/>
        /// </summary>
        public object LaneValue { get; set; }

        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        #region Json Model

        internal JsonSwimlane ToJson() => new JsonSwimlane()
        {
            Caption = this.Caption,
            Color = BrushSerianization.SerializeBrush(this.Background),
            IsCollapsed = this.IsCollapsed,
            LaneValue = this.LaneValue
        };

        internal static KanbanSwimlane FromModel(JsonSwimlane model)
        {
            KanbanSwimlane lane = new KanbanSwimlane()
            {
                Caption = model.Caption,
                IsCollapsed = model.IsCollapsed,
                LaneValue = model.LaneValue
            };
            if (!string.IsNullOrWhiteSpace(model.Color))
            {
                lane.Background = BrushSerianization.DeserializeBrush(model.Color);
            }
            return lane;
        }

        #endregion
    }
}
