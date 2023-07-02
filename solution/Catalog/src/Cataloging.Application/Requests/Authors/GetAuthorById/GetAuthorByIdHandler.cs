using Cataloging.Application.Services;
using Cataloging.Domain.Authors;
using MediatR;
using Shared.Application.Auditing;
using Shared.Application.Authentication;
using System.Diagnostics.CodeAnalysis;

namespace Cataloging.Application.Requests.Authors.GetAuthorById;

public record GetAuthorByIdQuery(Guid AuthorId, User Actor) : IAuditableQuery<IQueryable<Author>>;

public class GetAuthorByIdHandler : IRequestHandler<GetAuthorByIdQuery, IQueryable<Author>>
{
    private readonly IQueryAuthorizer _queryAuthorizer;

    public GetAuthorByIdHandler(IQueryAuthorizer queryAuthorizer)
    {
        _queryAuthorizer = queryAuthorizer;
    }

    public Task<IQueryable<Author>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_queryAuthorizer.GetAuthorizedEntities<Author>(cancellationToken).Where(a => a.Id == request.AuthorId));
    }
}