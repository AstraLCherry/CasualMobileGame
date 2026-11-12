using JetBrains.Annotations;
using System;

public static class EventBus<T> where T : IEvent
{
    //public delegate void Event(T eventData);

    //public static event Event OnEvent;

    public static event Action<T> OnAction;

    public static void Raise([CanBeNull] T eventData) => OnAction?.Invoke(eventData);
}

public interface IEvent { }
