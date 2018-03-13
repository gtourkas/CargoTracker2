using System.Threading.Tasks;
using Domain.Shipping.Cargo;

namespace App.Monitoring.OnCargoLoading
{
    public class Handler
    {
        public async Task Handle(HandlingEvent @event)
        {
            if (@event.Type != HandlingType.Load)
                return;


        }
    }
}
