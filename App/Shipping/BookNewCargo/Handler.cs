﻿using Domain.Shipping.Cargo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shipping.BookNewCargo
{

    public class Handler : ICommandHandler<Command>
    {
        public IEventDispatcher EventDispatcher { get; set; }

        public ICargoRepository CargoRepository { get; set; }

        public Handler(IEventDispatcher eventDispatcher, ICargoRepository cargoRepository)
        {
            EventDispatcher = eventDispatcher;
            CargoRepository = cargoRepository;
        }

        public async Task HandleAsync(Command cmd)
        {
            // apply command
            var cargo = new Cargo(cmd.TrackingId, cmd.RouteSpec);

            // emit events 
            EventDispatcher.Dispatch(cargo.Events);

            // persist state
            await CargoRepository.SaveAsync(cargo);
        }
    }
}