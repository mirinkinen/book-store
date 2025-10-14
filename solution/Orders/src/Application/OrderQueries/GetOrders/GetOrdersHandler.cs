using GreenDonut.Data;
using MediatR;

namespace Application.OrderQueries.GetOrders;

public record GetOrdersQuery : IRequest<Page<OrderNode>>
{
    public PagingArguments PagingArguments { get; }
    public QueryContext<OrderNode> QueryContext { get; }

    public GetOrdersQuery(PagingArguments pagingArguments, QueryContext<OrderNode> queryContext)
    {
        PagingArguments = pagingArguments;
        QueryContext = queryContext;
    }
}

public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, Page<OrderNode>>
{
    private readonly IOrderReadRepository _readRepository;

    public GetOrdersHandler(IOrderReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public Task<Page<OrderNode>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        return _readRepository.GetPage(request.PagingArguments, request.QueryContext, cancellationToken).AsTask();
    }
}