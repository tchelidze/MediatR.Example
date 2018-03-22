using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MediatRConsole.Exceptions;

namespace MediatRConsole.Behaviors
{
    public class RequestValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IValidator<TRequest>[] _validators;

        public RequestValidatorBehavior(IValidator<TRequest>[] validators) => _validators = validators;

        public async Task<TResponse> Handle(TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var failedValidationResults =
                _validators
                    .Select(validator => validator.Validate(request))
                    .Where(validationResult => !validationResult.IsValid)
                    .ToList();

            if (failedValidationResults.Any())
            {
                throw new InvalidCommandException<TRequest>(request, failedValidationResults);
            }

            return await next();
        }
    }
}