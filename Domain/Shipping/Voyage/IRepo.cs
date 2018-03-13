using System.Threading.Tasks;

namespace Domain.Shipping.Voyage
{
    public interface IRepo
    {
        Task<Voyage> GetAsync(VoyageNumber voyageNumber);

        Task SaveAsync(Voyage voyage);

        Task DeleteAsyng(VoyageNumber voyageNumber);
    }
}
