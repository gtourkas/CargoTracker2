using System.Threading.Tasks;

namespace Domain.Shipping.Location
{
    public interface IRepo
    {
        Task<Location> GetAsync(UnLocode unLocode);

        Task SaveAsync(Location location);

        Task DeleteAsyng(UnLocode unLocode);
    }
}
