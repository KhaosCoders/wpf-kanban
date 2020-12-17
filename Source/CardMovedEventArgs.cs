using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KC.WPF_Kanban
{
    /// <summary>
    /// The delegate type for handling a card moved event
    /// </summary>
    public delegate void CardMovedEventHandler(
        object sender,
        CardMovedEventArgs e);

    /// <summary>
    /// The inputs to a card moved event handler
    /// </summary>
    public class CardMovedEventArgs : RoutedEventArgs
    {
        public CardMovedEventArgs(RoutedEvent id, KanbanCardBase card, KanbanColumn column, KanbanSwimlane swimlane)
        {
            RoutedEvent = id;
            Card = card;
            TargetColumn = column;
            TargetSwimlane = swimlane;
        }

        /// <summary>
        /// The moved card
        /// </summary>
        public KanbanCardBase Card { get; set; }

        /// <summary>
        /// The column the card is moved to
        /// </summary>
        public KanbanColumn TargetColumn { get; set; }

        /// <summary>
        /// The swimlane the card is moved to
        /// </summary>
        public KanbanSwimlane TargetSwimlane { get; set; }

        /// <summary>
        /// This method is used to perform the proper type casting in order to
        /// call the type-safe SelectionChangedEventHandler delegate for the SelectionChangedEvent event.
        /// </summary>
        /// <param name="genericHandler">The handler to invoke.</param>
        /// <param name="genericTarget">The current object along the event's route.</param>
        protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
        {
            CardMovedEventHandler handler = (CardMovedEventHandler)genericHandler;

            handler(genericTarget, this);
        }
    }
}
