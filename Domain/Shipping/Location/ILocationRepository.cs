using System.Threading.Tasks;

namespace Domain.Shipping.Location
{
    public interface ILocationRepository
    {
        Task<Location> FindByIDAsync(UnLocode unLocode);

        Task SaveAsync(Location location);
    }
}
