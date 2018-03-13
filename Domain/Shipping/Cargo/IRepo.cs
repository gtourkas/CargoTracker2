using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shipping.Cargo
{
    public interface IRepo
    {
        Task<Cargo> GetAsync(TrackingId id);

        Task SaveAsync(Cargo container);

        Task DeleteAsync(TrackingId id);
    }
}
