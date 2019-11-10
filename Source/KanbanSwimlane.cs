using KC.WPF_Kanban.Model;
using KC.WPF_Kanban.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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

        /// <summary>
        /// Gets the count of all cards in expanded columns assigned to the <see cref="KanbanSwimlane"/>
        /// </summary>
        public int VisibleCardCount => Cells.Where(cell => cell.Column.IsColumnContentVisible).Sum(cell => cell.Cards.Count);

        private void Cell_CardsChanged(object sender, RoutedEventArgs e)
        {
            OnPropertyChanged(nameof(CardCount));
            OnPropertyChanged(nameof(VisibleCardCount));
        }


        /// <summary>
        /// Handles swimlane specific action, when a collumn in collapsed or expanded
        /// </summary>
        /// <param name="column">The column that was changed</param>
        public virtual void OnColumnCollapsedChanged(KanbanColumn column)
        {
            // Visible CardCount may change, when a column is collapsed or expanded
            OnPropertyChanged(nameof(VisibleCardCount));
        }

        #endregion

        #region Visual DPs

        /// <summary>
        /// Gets or sets a caption
        /// </summary>
        public string Caption
        {
            get => (string)GetValue(CaptionProperty);
            set => SetValue(CaptionProperty, value);
        }
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register(nameof(Caption), typeof(string), typeof(KanbanSwimlane),
                new FrameworkPropertyMetadata(DefaultCaption));

        /// <summary>
        /// Gets or sets whether the <see cref="KanbanSwimlane"/> is collapsed
        /// </summary>
        public bool IsCollapsed
        {
            get => (bool)GetValue(IsCollapsedProperty);
            set => SetValue(IsCollapsedProperty, value);
        }
        public static readonly DependencyProperty IsCollapsedProperty =
            DependencyProperty.Register(nameof(IsCollapsed), typeof(bool), typeof(KanbanSwimlane),
                new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsCollapsedChanged)));

        private static void OnIsCollapsedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is KanbanSwimlane lane)
            {
                var panel = FrameworkUtils.FindVisualParent<KanbanBoardGridPanel>(lane);
                panel?.OnSwimlaneCollapsedChanged(lane);
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets a unique value that is used to assign cards to the <see cref="KanbanSwimlane"/>
        /// </summary>
        public object LaneValue { get; set; }

        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Json Model

        internal JsonSwimlane ToJson()
        {
            return new JsonSwimlane()
            {
                Caption = Caption,
                Color = BrushSerianization.SerializeBrush(Background),
                IsCollapsed = IsCollapsed,
                LaneValue = LaneValue,
                Foreground = BrushSerianization.SerializeBrush(Foreground)
            };
        }

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
            if (!string.IsNullOrWhiteSpace(model.Foreground))
            {
                lane.Foreground = BrushSerianization.DeserializeBrush(model.Foreground);
            }
            return lane;
        }

        #endregion
    }
}
