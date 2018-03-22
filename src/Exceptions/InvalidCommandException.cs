using System;
using System.Collections.Generic;
using FluentValidation.Results;

namespace MediatRConsole.Exceptions
{
    public class InvalidCommandException<TCommand> : Exception
    {
        public InvalidCommandException(TCommand invalidCommand, IReadOnlyList<ValidationResult> failedValidationResults)
        {
            InvalidCommand = invalidCommand;
            FailedValidationResults = failedValidationResults;
        }

        public TCommand InvalidCommand { get; }

        public IReadOnlyList<ValidationResult> FailedValidationResults { get; }
    }
}