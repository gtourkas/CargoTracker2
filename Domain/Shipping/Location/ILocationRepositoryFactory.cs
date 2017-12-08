namespace Domain.Shipping.Location
{
    public interface ILocationRepositoryFactory
    {
        ILocationRepository Create();
    }
}
