using System.Windows.Media;

namespace WpfApp1
{
    public class CardData
    {
        public string Column { get; set; }
        public string Lane { get; set; }

        public string Title { get; set; }

        public Color Color { get; set; }

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