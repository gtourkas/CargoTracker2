using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public interface IEventEmitter
    {
        Task EmitAsync<T>(T message) where T : class, IEvent;

    }
}
