using Domain.Shipping.Cargo;
using Domain.Shipping.Location;
using Domain.Shipping.Voyage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shipping.RegisterHandlingEvent
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
