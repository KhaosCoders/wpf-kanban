using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
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
            MessageBox.Show(string.Format("Kanban Json: {0}", kanBoard.SaveModel()));
        }
    }
}
