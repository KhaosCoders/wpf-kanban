using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace WpfApp1
{
    public class DummyData
    {
        //public DataTable Table { get; set; }

        public ObservableCollection<CardData> Collection { get; set; } = new ObservableCollection<CardData>();

        public DummyData()
        {
            //Table = new DataTable();
            //Table.Columns.Add(new DataColumn("Column", typeof(string)));
            //Table.Columns.Add(new DataColumn("Lane", typeof(string)));
            //Table.Columns.Add(new DataColumn("Title", typeof(string)));
            //Table.Rows.Add("Backlog", "lane1", "BL 1");
            //Table.Rows.Add("Requested", "lane1", "R 1");
            //Table.Rows.Add("Requested", "lane1", "R 2");
            //Table.Rows.Add("Design", "lane1", "De 1");
            //Table.Rows.Add("Doing", "lane1", "Doi 1");
            //Table.Rows.Add("Doing", "lane2", "Doi 2");
            //Table.Rows.Add("Done", "lane1", "Don 1");
            //Table.Rows.Add("Backlog", "lane2", "BL 2");

            Collection.Add(new CardData() { Column = "Backlog", Lane = "lane1", Size = "S", CreationTime = DateTime.Now, Duration = 30, Number = 1, Assignee = "User1", Description = "Something in the backlog Part 1/2", TileColor = Colors.Yellow, TileText = "C" });
            Collection.Add(new CardData() { Column = "Requested", Lane = "lane1", Size = "M", CreationTime = DateTime.Today, Duration = 90, Number = 2, Assignee = "User2", Description = "This is something requested, not started jet" });
            Collection.Add(new CardData()
            {
                Column = "Requested",
                Lane = "lane1",
                Size = "XXL",
                CreationTime = DateTime.MinValue,
                Duration = 200,
                Number = 99999,
                Assignee = "User1",
                Description = "This also is something requested ;)",
                Stickers = new List<KC.WPF_Kanban.KanbanStickerBase>() {
                   new KC.WPF_Kanban.Stickers.OkSticker()
                }
            });
            Collection.Add(new CardData()
            {
                Id = "CardBlocked",
                Column = "Design",
                Lane = "lane1",
                CreationTime = DateTime.UtcNow,
                Duration = 650,
                Number = 4,
                Size = "S",
                Assignee = "User1",
                Description = "We're designing the hell out of this",
                TileColor = Colors.LightBlue,
                Stickers = new List<KC.WPF_Kanban.KanbanStickerBase>() {
                   new KC.WPF_Kanban.Stickers.OkSticker()
                },
                Blocker = new List<KC.WPF_Kanban.KanbanBlocker>() {
                   new KC.WPF_Kanban.KanbanBlocker()  { BlockType="X", Description="Some sort of block (1)", Background=Brushes.Red, BorderBrush=Brushes.DarkRed, Foreground=Brushes.White, BorderThickness=new System.Windows.Thickness(10)},
                   new KC.WPF_Kanban.KanbanBlocker()  { BlockType="Y", Description="Some sort of block (2)", Background=Brushes.Red, BorderBrush=Brushes.DarkRed, Foreground=Brushes.White, BorderThickness=new System.Windows.Thickness(10)},
                   new KC.WPF_Kanban.KanbanBlocker()  { BlockType="Z", Description="Some sort of block (3)", Background=Brushes.Red, BorderBrush=Brushes.DarkRed, Foreground=Brushes.White, BorderThickness=new System.Windows.Thickness(10)}
                }
            });
            Collection.Add(new CardData() { Column = "Doing", Lane = "lane2", Size = "L", CreationTime = DateTime.Parse("12.02.2019 08:00"), Duration = 1500, Number = 5, Assignee = "User2", Description = "Something we're working on right now", TileColor = Colors.LightGreen, TileText = "A" });
            Collection.Add(new CardData() { Column = "Doing", Lane = "lane2", Size = "XL", CreationTime = DateTime.Now, Duration = 10000, Number = 6, Assignee = "User2", Description = "The other thing we're working on right now", TileColor = Colors.LightBlue, TileText = "B" });
            Collection.Add(new CardData() { Column = "Done", Lane = "lane1", Size = "S", CreationTime = DateTime.Now, Duration = 100000, Number = 7, Assignee = "User1", Description = "This is done, yeah!" });
            Collection.Add(new CardData() { Column = "Backlog", Lane = "lane2", Size = "S", CreationTime = DateTime.Now, Number = 8, Assignee = "User1", Description = "Something in the backlog Part 2/2" });
        }

        public void AddCard()
        {
            Collection.Add(new CardData() { Column = "Requested", Lane = "lane2", Size = "L", CreationTime = DateTime.Parse("12.02.2019 08:00"), Duration = 1500, Number = 5, Assignee = "User2", Description = "Something we're working on right now", TileColor = Colors.LightGreen, TileText = "A" });
        }
    }
}
