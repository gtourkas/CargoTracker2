namespace Domain.Shipping.Voyage
{
    public class Voyage
    {
        public VoyageNumber VoyageNumber { get; private set; }

        public Schedule Schedule { get; private set; }

        public VesselMMSI VesselMMSI { get; private set; }
    }
}
