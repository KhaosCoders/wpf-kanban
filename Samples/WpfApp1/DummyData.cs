using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Table.Rows.Add("Done", "Don 1");
            Table.Rows.Add("Backlog", "BL 2");



            Collection.Add(new CardData() { Column = "Backlog", Title = "BL 1" });
            Collection.Add(new CardData() { Column = "Requested", Title = "R 1" });
            Collection.Add(new CardData() { Column = "Requested", Title = "R 2" });
            Collection.Add(new CardData() { Column = "Design", Title = "De 1" });
            Collection.Add(new CardData() { Column = "Doing", Title = "Doi 1" });
            Collection.Add(new CardData() { Column = "Done", Title = "Don 1" });
            Collection.Add(new CardData() { Column = "Backlog", Title = "BL 2" });

        }




    }
}
