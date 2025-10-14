using Common.Domain;
using MediatR;

namespace Application.OrderItemQueries.GetOrderItemById;

public record GetOrderItemByIdQuery(Guid Id) : IRequest<OrderItemNode>;

public class GetOrderItemByIdHandler : IRequestHandler<GetOrderItemByIdQuery, OrderItemNode>
{
    private readonly IOrderItemReadRepository _readRepository;

    public GetOrderItemByIdHandler(IOrderItemReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<OrderItemNode> Handle(GetOrderItemByIdQuery request, CancellationToken cancellationToken)
    {
        var orderItem = await _readRepository.GetFirstOrDefaultAsync(request.Id, cancellationToken);

        if (orderItem is null)
        {
            throw new EntityNotFoundException("OrderItem not found", "order-item-not-found");
        }

        return orderItem;
    }
}