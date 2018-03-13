using System;
using Domain.Shipping.Cargo;

namespace App.RegisterHandlingEvent
{
    public class Command : ICommand
    {
        public HandlingEvent HandlingEvent { get; private set; }

        public Command(HandlingEvent handlingEvent)
        {
            if (handlingEvent == null)
                throw new ArgumentNullException("handlingEvent");

            HandlingEvent = handlingEvent;
        }
    }
}
