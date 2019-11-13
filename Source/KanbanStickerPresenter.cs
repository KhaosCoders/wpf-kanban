using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KC.WPF_Kanban
{
    public class KanbanStickerPresenter : Control
    {
        #region Override DP Metadata

        static KanbanStickerPresenter()
        {
            // Enable Themes for this Control
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KanbanStickerPresenter), new FrameworkPropertyMetadata(typeof(KanbanStickerPresenter)));
        }

        #endregion

        /// <summary>
        /// Gets or sets a collection of Stickers (<see cref="KanbanStickerBase"/>)
        /// </summary>
        public IList<KanbanStickerBase> Stickers
        {
            get => (IList<KanbanStickerBase>)GetValue(StickersProperty);
            set => SetValue(StickersProperty, value);
        }
        public static readonly DependencyProperty StickersProperty =
            DependencyProperty.Register(nameof(Stickers), typeof(IList<KanbanStickerBase>), typeof(KanbanStickerPresenter),
                new FrameworkPropertyMetadata(new ReadOnlyCollection<KanbanStickerBase>(new List<KanbanStickerBase>())));

    }
}
