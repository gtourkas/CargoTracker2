using System.Threading.Tasks;

namespace Domain.Shipping.Voyage
{
    public interface IVoyageRepository
    {
        Task<Voyage> FindAsync(VoyageNumber voyageNumber);

        Task SaveAsync(Voyage voyage);
    }
}
