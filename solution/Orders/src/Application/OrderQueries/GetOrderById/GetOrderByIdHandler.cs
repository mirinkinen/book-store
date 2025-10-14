using Common.Domain;
using MediatR;

namespace Application.OrderQueries.GetOrderById;

public record GetOrderByIdQuery(Guid Id) : IRequest<OrderNode>;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderNode>
{
    private readonly IOrderReadRepository _readRepository;

    public GetOrderByIdHandler(IOrderReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<OrderNode> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _readRepository.GetFirstOrDefaultAsync(request.Id, cancellationToken);

        if (order is null)
        {
            throw new EntityNotFoundException("Order not found", "order-not-found");
        }

        return order;
    }
}