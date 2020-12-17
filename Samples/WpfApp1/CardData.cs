using System;
using System.Collections.Generic;
using System.Windows.Media;
using KC.WPF_Kanban;

namespace WpfApp1
{
    public class CardData
    {
        public string Id { get; set; }
        public string Column { get; set; }
        public string Lane { get; set; }

        public string Description { get; set; }

        public int Number { get; set; }

        public string Assignee { get; set; }

        public Color TileColor { get; set; }

        public DateTime CreationTime { get; set; }

        public int Duration { get; set; } = -1;

        public string this[string indexer]
        {
            get
            {
                if (indexer == "Column")
                    return Column;
                if (indexer == "Lane")
                    return Lane;
                return null;
            }
        }

        public int TileRgb => KC.WPF_Kanban.Converter.BrushConverter.ToInteger(TileColor);

        public List<KanbanBlocker> Blocker { get; set; }
        public List<KanbanStickerBase> Stickers { get; set; }

        public string Size { get; set; }

        public string TileText { get; set; }
    }
}