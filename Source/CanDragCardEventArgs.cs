using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KC.WPF_Kanban
{
    /// <summary>
    /// The delegate type for handling a card drag event
    /// </summary>
    public delegate void CanDragCardEventHandler(
        object sender,
        CanDragCardEventArgs e);

    /// <summary>
    /// The inputs to a card drag event handler
    /// </summary>
    public class CanDragCardEventArgs : RoutedEventArgs
    {
        public CanDragCardEventArgs(RoutedEvent id, KanbanCardBase card)
        {
            RoutedEvent = id;
            Card = card;
        }

        /// <summary>
        /// The dragged card
        /// </summary>
        public KanbanCardBase Card { get; set; }

        /// <summary>
        /// Gets or sets whether the card can be dragged
        /// </summary>
        public bool CanDrag { get; set; }


        /// <summary>
        /// This method is used to perform the proper type casting in order to
        /// call the type-safe SelectionChangedEventHandler delegate for the SelectionChangedEvent event.
        /// </summary>
        /// <param name="genericHandler">The handler to invoke.</param>
        /// <param name="genericTarget">The current object along the event's route.</param>
        protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
        {
            CanDragCardEventHandler handler = (CanDragCardEventHandler)genericHandler;

            handler(genericTarget, this);
        }
    }
}
