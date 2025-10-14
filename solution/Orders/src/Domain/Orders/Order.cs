using Common.Domain;
using Domain.OrderItems;
using FluentValidation;
using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Orders;

public class Order : Entity
{
    public required string CustomerName { get; set; }

    public required DateOnly OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public required Guid OrganizationId { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new();

    [Obsolete("Only for serialization", true)]
    public Order()
    {
    }

    [SetsRequiredMembers]
    public Order(string customerName, DateOnly orderDate, Guid organizationId)
    {
        CustomerName = customerName;
        OrderDate = orderDate;
        OrganizationId = organizationId;
        TotalAmount = 0;

        Validate();
    }

    public void Update(string customerName, DateOnly orderDate)
    {
        CustomerName = customerName;
        OrderDate = orderDate;

        Validate();
        RecalculateTotal();
    }

    public void AddOrderItem(OrderItem orderItem)
    {
        OrderItems.Add(orderItem);
        RecalculateTotal();
    }

    public OrderItem RemoveOrderItem(Guid orderItemId)
    {
        var orderItem = OrderItems.FirstOrDefault(oi => oi.Id == orderItemId);
        if (orderItem == null)
        {
            throw new KeyNotFoundException($"OrderItem with ID '{orderItemId}' not found for this order.");
        }

        OrderItems.Remove(orderItem);
        RecalculateTotal();
        return orderItem;
    }

    private void RecalculateTotal()
    {
        TotalAmount = OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);
    }

    private void Validate()
    {
        var failures = new List<ValidationFailure>();

        if (string.IsNullOrWhiteSpace(CustomerName))
        {
            failures.Add(new ValidationFailure(nameof(CustomerName), $"'{nameof(CustomerName)}' cannot be null or whitespace."));
        }

        if (OrderDate == DateOnly.MinValue)
        {
            failures.Add(new ValidationFailure(nameof(OrderDate), $"'{nameof(OrderDate)}' cannot be min value."));
        }

        if (OrderDate.ToDateTime(TimeOnly.MinValue) > DateTime.UtcNow)
        {
            failures.Add(new ValidationFailure(nameof(OrderDate), $"'{nameof(OrderDate)}' cannot be in future."));
        }

        if (OrganizationId == Guid.Empty)
        {
            failures.Add(new ValidationFailure(nameof(OrganizationId), $"'{nameof(OrganizationId)}' cannot be empty."));
        }

        if (failures.Count != 0)
        {
            throw new ValidationException("Order validation failed.", failures);
        }
    }
}