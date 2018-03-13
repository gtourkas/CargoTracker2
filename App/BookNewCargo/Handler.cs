using System.Threading.Tasks;
using Domain.Shipping.Cargo;

namespace App.BookNewCargo
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
            // apply command
            // var cargo = new Cargo(cmd.TrackingId, cmd.RouteSpec);

            // emit events 
            // EventDispatcher.DispatchAsync(cargo.Events);

            // persist state
            // await CargoRepository.SaveAsync(cargo);
        }
    }
}
