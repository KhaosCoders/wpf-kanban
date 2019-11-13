using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KC.WPF_Kanban.Stickers
{
    public class OkSticker : KanbanStickerBase
    {
        #region Override DP Metadata

        static OkSticker()
        {
            // Enable Themes for this Control
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OkSticker), new FrameworkPropertyMetadata(typeof(OkSticker)));

            DescriptionProperty.OverrideMetadata(typeof(OkSticker), new FrameworkPropertyMetadata("Ok"));

            ImageProperty.OverrideMetadata(typeof(OkSticker), new FrameworkPropertyMetadata(new BitmapImage(new Uri("pack://application:,,,/WPF-Kanban;component/Res/ok.png"))));
        }

        #endregion
    }
}
