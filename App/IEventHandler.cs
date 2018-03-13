using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public interface IEventHandlerFactory<T> where T : class, IEvent
    {
        IEventHandler<T> Create();
    }

    public interface IEventHandler<T> where T : class, IEvent
    {
        Task HandleAsync(T @event);
    }
}
