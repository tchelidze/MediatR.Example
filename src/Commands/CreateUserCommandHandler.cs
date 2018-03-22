using System;
using System.Threading.Tasks;
using MediatR;
using MediatRConsole.Events;

namespace MediatRConsole.Commands
{
    public class CreateUserCommandHandler : AsyncRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IMediator _mediator;

        public CreateUserCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected override async Task<Guid> HandleCore(CreateUserCommand request)
        {
            var createdUserId = Guid.NewGuid();
            await _mediator.Publish(new UserCreatedEvent(createdUserId));

            return createdUserId;
        }
    }
}