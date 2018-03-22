using FluentValidation;
using MediatRConsole.Commands;

namespace MediatRConsole.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(createUserCommand => createUserCommand.Name)
                .NotEmpty()
                .WithMessage("No name found");
        }
    }
}