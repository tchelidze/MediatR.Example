using System;
using MediatR;

namespace MediatRConsole.Commands
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public string Name { get; set; }
    }
}