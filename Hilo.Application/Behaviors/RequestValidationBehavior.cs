namespace Hilo.Application.Behaviors;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR.Pipeline;

public class RequestValidationBehavior<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> validators;

    public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => this.validators = validators;

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        if (this.validators.Any())
        {
            var failures = this.validators.Select(v => v.Validate(request))
                                          .SelectMany(r => r.Errors)
                                          .Where(f => f != null).ToList();

            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }
        }

        return Task.CompletedTask;
    }
}
