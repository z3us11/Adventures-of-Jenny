using System;
using System.Collections.Generic;
using UnityEngine;


namespace CodeArchitecture
{
    public static class EventManager
    {
        // Using a dictionary to store events. Since we want to store events with different parameter types,
        // we store them as objects and cast later.
        private static Dictionary<EventName, object> eventDictionary = new Dictionary<EventName, object>();

        // Registers a new generic event if it doesn't already exist.
        public static void RegisterEvent<T>(EventName eventName)
        {
            if (!eventDictionary.ContainsKey(eventName))
            {
                eventDictionary.Add(eventName, new GameEvent<T>(eventName));
            }
        }

        // Registers a listener for an event with parameter type T.
        public static void StartListening<T>(EventName eventName, Action<T> listener)
        {
            if (!eventDictionary.ContainsKey(eventName))
            {
                RegisterEvent<T>(eventName);
            }
            (eventDictionary[eventName] as GameEvent<T>).AddListener(listener);
        }

        // Unregisters a listener for an event with parameter type T.
        public static void StopListening<T>(EventName eventName, Action<T> listener)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                (eventDictionary[eventName] as GameEvent<T>).RemoveListener(listener);
            }
        }

        // Triggers the event and passes the parameter to all registered listeners.
        public static void TriggerEvent<T>(EventName eventName, T param)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                (eventDictionary[eventName] as GameEvent<T>).Invoke(param);
            }
            else
            {
                Debug.LogWarning("GenericEventManager: No event found with name " + eventName);
            }
        }
    }

}
