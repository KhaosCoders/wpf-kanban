using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KC.WPF_Kanban
{
    public abstract class KanbanStickerBase : ContentControl
    {
        #region Override DP Metadata

        static KanbanStickerBase()
        {
            // Enable Themes for this Control
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KanbanStickerBase), new FrameworkPropertyMetadata(typeof(KanbanStickerBase)));
        }

        #endregion

        /// <summary>
        /// Gets or sets a description text for the sticker
        /// </summary>
        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(nameof(Description), typeof(string), typeof(KanbanStickerBase),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the source of the sticker icon
        /// </summary>
        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register(nameof(Image), typeof(ImageSource), typeof(KanbanStickerBase),
                new FrameworkPropertyMetadata(null));

    }
}
