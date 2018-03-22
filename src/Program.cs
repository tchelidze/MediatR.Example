using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using FluentValidation;
using MediatR;
using MediatRConsole.Behaviors;
using MediatRConsole.Commands;
using MediatRConsole.Events;
using MediatRConsole.Exceptions;
using MediatRConsole.Validators;

namespace MediatRConsole
{
    internal class Program
    {
        private static void WriteLine(string text, ConsoleColor color)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = previousColor;
        }

        private static async Task Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            containerBuilder
                .Register<SingleInstanceFactory>(context =>
                {
                    var componentContext = context.Resolve<IComponentContext>();
                    return type => componentContext.TryResolve(type, out var o) ? o : null;
                })
                .InstancePerLifetimeScope();

            containerBuilder
                .Register<MultiInstanceFactory>(context =>
                {
                    var componentContext = context.Resolve<IComponentContext>();
                    return type =>
                        (IEnumerable<object>)componentContext.Resolve(typeof(IEnumerable<>).MakeGenericType(type));
                })
                .InstancePerLifetimeScope();

            // Register Command validators
            containerBuilder
                .RegisterAssemblyTypes(typeof(CreateUserCommandValidator).Assembly)
                .AsClosedTypesOf(typeof(IValidator<>));

            // Register Command handlers
            containerBuilder
                .RegisterAssemblyTypes(typeof(CreateUserCommandHandler).Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            // Register Notification Handlers
            containerBuilder.RegisterAssemblyTypes(typeof(UserCreatedEvent).Assembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>));


            // Register Pipeline behaviors
            containerBuilder
                .RegisterGeneric(typeof(RequestValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));


            using (var container = containerBuilder.Build())
            {
                var mediator = container.Resolve<IMediator>();

                var newOrderGuid = await mediator.Send(new CreateUserCommand
                {
                    Name = "Tchelidze"
                });

                WriteLine($"New order created. Guid : {newOrderGuid}", ConsoleColor.Green);

                try
                {
                    await mediator.Send(new CreateUserCommand
                    {
                        Name = string.Empty
                    });
                }
                catch (InvalidCommandException<CreateUserCommand> ex)
                {
                    WriteLine($"Invalid command was sent. Validation error(s) \n\t " +
                              $"{string.Join("\n\t", ex.FailedValidationResults.SelectMany(it => it.Errors).Select(it => it.ErrorMessage).ToArray())}",
                        ConsoleColor.Red);
                }
            }
        }
    }
}