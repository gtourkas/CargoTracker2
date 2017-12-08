using System.Collections.Generic;

namespace Domain.Shipping.Voyage
{
    public class Schedule
    {
        public ICollection<CarrierMovement> CarrierMovements { get; private set; }

        public Schedule(ICollection<CarrierMovement> carrierMovements)
        {
            // TODO: validation

            CarrierMovements = carrierMovements;
        }
    }
}
