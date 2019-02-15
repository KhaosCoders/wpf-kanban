using System;
using System.Windows.Media;

namespace WpfApp1
{
    public class CardData
    {
        public string Column { get; set; }
        public string Lane { get; set; }

        public string Description { get; set; }

        public int Number { get; set; }

        public string Assignee { get; set; }

        public Color TitleColor { get; set; }

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
    }
}