using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp1
{
    public class DummyData
    {
        public DataTable Table { get; set; }

        public ObservableCollection<CardData> Collection { get; set; } = new ObservableCollection<CardData>();

        public DummyData()
        {
            Table = new DataTable();
            Table.Columns.Add(new DataColumn("Column", typeof(string)));
            Table.Columns.Add(new DataColumn("Title", typeof(string)));
            Table.Rows.Add("Backlog", "BL 1");
            Table.Rows.Add("Requested", "R 1");
            Table.Rows.Add("Requested", "R 2");
            Table.Rows.Add("Design", "De 1");
            Table.Rows.Add("Doing", "Doi 1");
            Table.Rows.Add("Doing", "Doi 2");
            Table.Rows.Add("Done", "Don 1");
            Table.Rows.Add("Backlog", "BL 2");

            Collection.Add(new CardData() { Column = "Backlog", Title = "Something in the backlog Part 1/2", Color = Colors.LightBlue });
            Collection.Add(new CardData() { Column = "Requested", Title = "This is something requested, not started jet" });
            Collection.Add(new CardData() { Column = "Requested", Title = "This also is something requested ;)" });
            Collection.Add(new CardData() { Column = "Design", Title = "We're designing the hell out of this", Color = Colors.LightBlue });
            Collection.Add(new CardData() { Column = "Doing", Title = "Something we're working on right now", Color = Colors.LightGreen });
            Collection.Add(new CardData() { Column = "Doing", Title = "The other thing we're working on right now", Color = Colors.LightBlue });
            Collection.Add(new CardData() { Column = "Done", Title = "This is done, yeah!" });
            Collection.Add(new CardData() { Column = "Backlog", Title = "Something in the backlog Part 2/2" });
        }
    }
}
