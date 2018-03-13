using System.Threading.Tasks;
using Domain.Shipping.Cargo;

namespace App.RegisterHandlingEvent
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
            var cargo = await CargoRepository.GetAsync(cmd.HandlingEvent.TrackingId);

            // apply command
            cargo.RegisterHandlingEvent(cmd.HandlingEvent);

            // emit events 
            await EventDispatcher.DispatchAsync(cargo.Events);

            // persist state
            await CargoRepository.SaveAsync(cargo);
        }

    }
}
