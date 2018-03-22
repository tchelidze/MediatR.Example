using System;
using MediatR;

namespace MediatRConsole.Events
{
    public class UserCreatedEvent : INotification
    {
        public UserCreatedEvent(Guid newUserId)
        {
            NewUserId = newUserId;
        }

        public Guid NewUserId { get; }
    }
}