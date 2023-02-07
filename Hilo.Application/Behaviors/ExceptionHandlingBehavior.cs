namespace Hilo.Application.Behaviors;

using System.Threading;
using System.Threading.Tasks;
using Hilo.Application.Model;
using Hilo.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> logger;

    public ExceptionHandlingBehavior(ILogger<TRequest> logger)
    {
        this.logger = logger;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (DomainException ex)
        {
            this.logger.LogInformation(ex.Message, ex);
            throw new ExpectedException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex.Message, ex);
            throw new UnexpectedException(ex.Message, ex);
        }
    }
}