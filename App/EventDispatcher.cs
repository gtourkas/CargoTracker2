using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public interface IEventDispatcher
    {
        Task DispatchAsync(IEvent @event);
        Task DispatchAsync(IEnumerable<IEvent> events);
    }

    public class EventDispatcher : IEventDispatcher
    {
        private readonly IDictionary<Type, IList<Action<object>>> _handlers = new Dictionary<Type, IList<Action<object>>>();

        public void Register<T>(IEventHandlerFactory<T> handlerFactory) where T : class, IEvent
        {
            _ensureKey<T>();

            var t = typeof(T);

            _handlers[t].Add(async e =>
            {
                var handler = handlerFactory.Create();

                await handler.HandleAsync((T)e);
            });
        }

        public void Register<T>(IEventHandler<T> handler) where T : class, IEvent
        {
            _ensureKey<T>();

            var t = typeof(T);

            _handlers[t].Add(async e =>
            {
                await handler.HandleAsync((T)e);
            });
        }

        public void Register<T>(Func<T,Task> handlerAsync) where T : class, IEvent
        {
            _ensureKey<T>();

            var t = typeof(T);

            _handlers[t].Add(async e =>
            {
                await handlerAsync((T)e);
            });
        }

        public void Register<T>(Action<T> handler) where T : class, IEvent
        {
            _ensureKey<T>();

            var t = typeof(T);

            _handlers[t].Add( e =>
            {
                handler((T)e);
            });
        }

        private void _ensureKey<T>() where T : class, IEvent
        {
            var t = typeof(T);
            if (!_handlers.ContainsKey(t))
                _handlers[t] = new List<Action<object>>();
        }


        public async Task DispatchAsync(IEvent @event)
        {
            var t = @event.GetType();
            if (_handlers.ContainsKey(t)) {
                foreach (var h in _handlers[t])
                    await Task.Run(() => { h(@event); });
            }
        }


        public async Task DispatchAsync(IEnumerable<IEvent> events)
        {
            foreach (var e in events)
                await DispatchAsync(e);
        }
    }

}
