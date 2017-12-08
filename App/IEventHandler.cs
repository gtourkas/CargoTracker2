using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public interface IEventHandlerFactory<T> : IHandlerFactory<T> where T : class, IEvent
    {
    }

    public interface IEventHandler<T> : IHandler<T> where T : class, IEvent
    {
    }
}
