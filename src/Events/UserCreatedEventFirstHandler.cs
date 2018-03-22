using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace MediatRConsole.Events
{
    public class UserCreatedEventFirstHandler : INotificationHandler<UserCreatedEvent>
    {
        public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"FirstHandler received event. UserId {notification.NewUserId}");
            return Task.CompletedTask;
        }
    }
}