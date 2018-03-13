using System.Threading.Tasks;
using Domain.Shipping.Cargo;

namespace App.Monitoring.OnCargoUnloading
{
    public class Handler
    {
        public async Task Handle(HandlingEvent @event)
        {
            if (@event.Type != HandlingType.Unload)
                return;

        }
    }
}
