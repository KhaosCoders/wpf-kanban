using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class KanbanCardWindow : Window
    {
        public KanbanCardWindow()
        {
            DataContext = new DummyData();
            InitializeComponent();

            /*
            kanBoard.LoadJsonModel(
@"{
  ""Title"": ""My Board"",
  ""ColumnPath"": ""[Column]"",
  ""SwimlanePath"": ""[Lane]"",
  ""Columns"": [
    {
      ""Caption"": ""Backlog"",
      ""CardLimit"": 5,
      ""ColumnSpan"": 1,
      ""ColumnValue"": ""Backlog"",
      ""IsCollapsed"": true,
      ""Color"": ""#FF000000""
    },
    {
      ""Caption"": ""Requested"",
      ""CardLimit"": 4,
      ""ColumnSpan"": 1,
      ""ColumnValue"": ""Requested"",
      ""IsCollapsed"": false,
      ""Color"": ""#FF0000FF""
    },
    {
      ""Caption"": ""In progress"",
      ""CardLimit"": 2,
      ""ColumnSpan"": 3,
      ""ColumnValue"": null,
      ""IsCollapsed"": false,
      ""Color"": ""#FFFFA500""
    },
    {
      ""Caption"": ""Done"",
      ""CardLimit"": -1,
      ""ColumnSpan"": 1,
      ""ColumnValue"": ""Done"",
      ""IsCollapsed"": false,
      ""Color"": ""#FF008000""
    },
    {
      ""Caption"": ""Archiv"",
      ""CardLimit"": -1,
      ""ColumnSpan"": 1,
      ""ColumnValue"": ""Archiv"",
      ""IsCollapsed"": true,
      ""Color"": ""#00FFFFFF""
    }
  ],
  ""Swimlanes"": [
    {
      ""Color"": ""#FF7FFFD4"",
      ""Caption"": ""Lane 1"",
      ""LaneValue"": ""lane1"",
      ""IsCollapsed"": false
    },
    {
      ""Color"": ""#FFF0F8FF"",
      ""Caption"": ""Lane 2"",
      ""LaneValue"": ""lane2"",
      ""IsCollapsed"": false
    },
    {
      ""Color"": ""#FFB8860B"",
      ""Caption"": ""Lane 3"",
      ""LaneValue"": ""lane3"",
      ""IsCollapsed"": true
    }
  ]
}");
*/
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(string.Format("Kanban Json: {0}", kanBoard.SaveModel()));
        }

        private void kanBoard_CanDragCard(object sender, KC.WPF_Kanban.CanDragCardEventArgs e)
        {
            e.CanDrag = e.Card.Id != null;
        }

        private void kanBoard_CanDropCard(object sender, KC.WPF_Kanban.CanDropCardEventArgs e)
        {
            e.CanDrop = e.TargetColumn.ColumnValue.ToString()[0] == 'D';
        }

        private void kanBoard_CardMoved(object sender, KC.WPF_Kanban.CardMovedEventArgs e)
        {
            MessageBox.Show($"Moved card {e.Card.Id} to Colum {e.TargetColumn.ColumnValue} / Swimlane {e.TargetSwimlane.LaneValue}");
        }

        private void AddCard_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is DummyData oData)
            {
                oData.AddCard();
            }
        }
    }
}
