using Audit.Core;
using MediatR;

namespace Books.Application.Behaviours;

internal class AuditLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public AuditLoggingBehavior()
    {
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using var scope = AuditScope.Create(new AuditScopeOptions { CreationPolicy = EventCreationPolicy.Manual });
        scope.SetCustomField("UserId", "Testiarvo");

        TResponse response;

        try
        {
            response = await next();
            scope.SetCustomField("Success", "true");
        }
        catch (Exception)
        {
            scope.SetCustomField("Success", "false");
            throw;
        }
        finally
        {
            scope.Save();
        }

        return response;
    }
}