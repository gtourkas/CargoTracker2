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
        void Dispatch(IEvent domainEvent);
        void Dispatch(IEnumerable<IEvent> domainEvents);
    }

    public class EventDispatcher : IEventDispatcher
    {
        private IDictionary<Type, IList<Action<object>>> _handlers = new Dictionary<Type, IList<Action<object>>>();

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


        public void Dispatch(IEvent @event)
        {
            var t = @event.GetType();
            if (_handlers.ContainsKey(t)) {
                foreach (var h in _handlers[t])
                    h(@event);
                return;
            }
        }


        public void Dispatch(IEnumerable<IEvent> domainEvents)
        {
            foreach (var e in domainEvents)
                Dispatch(e);
        }
    }

}
