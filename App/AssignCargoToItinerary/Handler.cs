using System.Threading.Tasks;
using Domain.Shipping.Cargo;

namespace App.AssignCargoToItinerary
{
    public class Handler : ICommandHandler<Command>
    {
        public IEventDispatcher EventDispatcher { get; set; }

        public IRepo CargoRepository { get; set; }

        public Handler(IEventDispatcher eventDispatcher, IRepo cargoRepository)
        {
            EventDispatcher = eventDispatcher;
            CargoRepository = cargoRepository;
        }

        public async Task HandleAsync(Command cmd)
        {
            // restore aggregate's state
            var cargo = await CargoRepository.GetAsync(cmd.TrackingId);

            // apply command
            cargo.AssignToItinerary(cmd.Itinerary);

            // emit events 
            EventDispatcher.DispatchAsync(cargo.Events);

            // persist state
            await CargoRepository.SaveAsync(cargo);
        }
    }
}
