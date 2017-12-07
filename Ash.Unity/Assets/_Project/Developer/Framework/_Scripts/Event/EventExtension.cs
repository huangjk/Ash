using Ash.Event;
using System;
using AshUnity;

namespace Framework
{
    public static class EventExtension
    {
        public static bool Check(this EventComponent eventComponent, Venipuncture.EventId eventId, EventHandler<AshEventArgs> handler)
        {
            return eventComponent.Check((AshUnity.EventId)eventId, handler);
        }

        public static void Subscribe(this EventComponent eventComponent, Venipuncture.EventId eventId, EventHandler<AshEventArgs> handler)
        {
            eventComponent.Subscribe((AshUnity.EventId)eventId, handler);
        }

        public static void Unsubscribe(this EventComponent eventComponent, Venipuncture.EventId eventId, EventHandler<AshEventArgs> handler)
        {
            eventComponent.Unsubscribe((AshUnity.EventId)eventId, handler);
        }
    }
}
