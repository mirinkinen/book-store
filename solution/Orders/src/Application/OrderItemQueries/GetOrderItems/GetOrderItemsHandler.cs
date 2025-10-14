using GreenDonut.Data;
using MediatR;

namespace Application.OrderItemQueries.GetOrderItems;

public record GetOrderItemsQuery : IRequest<Page<OrderItemNode>>
{
    public PagingArguments PagingArguments { get; }
    public QueryContext<OrderItemNode> QueryContext { get; }

    public GetOrderItemsQuery(PagingArguments pagingArguments, QueryContext<OrderItemNode> queryContext)
    {
        PagingArguments = pagingArguments;
        QueryContext = queryContext;
    }
}

public class GetOrderItemsHandler : IRequestHandler<GetOrderItemsQuery, Page<OrderItemNode>>
{
    private readonly IOrderItemReadRepository _readRepository;

    public GetOrderItemsHandler(IOrderItemReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public Task<Page<OrderItemNode>> Handle(GetOrderItemsQuery request, CancellationToken cancellationToken)
    {
        return _readRepository.GetPage(request.PagingArguments, request.QueryContext, cancellationToken).AsTask();
    }
}