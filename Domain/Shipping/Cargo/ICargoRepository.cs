using System.Threading.Tasks;

namespace Domain.Shipping.Cargo
{
    public interface ICargoRepository
    {
        Task<Cargo> FindByIDAsync(TrackingId trackingId);

        Task SaveAsync(Cargo cargo);

    }
}
