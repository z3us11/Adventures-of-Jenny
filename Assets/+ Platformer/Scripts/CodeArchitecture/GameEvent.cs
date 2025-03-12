using System;

namespace CodeArchitecture
{
    public class GameEvent<T>
    {
        public EventName eventName;
        private Action<T> eventAction;

        public GameEvent(EventName name)
        {
            eventName = name;
        }

        // Register a listener that accepts a parameter of type T.
        public void AddListener(Action<T> listener)
        {
            eventAction += listener;
        }

        // Unregister a listener.
        public void RemoveListener(Action<T> listener)
        {
            eventAction -= listener;
        }

        // Invoke all registered listeners with the provided parameter.
        public void Invoke(T param)
        {
            eventAction?.Invoke(param);
        }
    }

    public enum EventName
    {
        TimeChanged
    }
}

