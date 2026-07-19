using DroneDelivery.Domain.Enums;

namespace DroneDelivery.Domain.Entities;

public class ImpossibleOrder
{
    public Order Order { get; }

    public ImpossibleReason Reason { get; }

    public ImpossibleOrder(
        Order order,
        ImpossibleReason reason)
    {
        ArgumentNullException.ThrowIfNull(order);

        if (!Enum.IsDefined(reason))
        {
            throw new ArgumentOutOfRangeException(
                nameof(reason),
                reason,
                "The impossible order reason is invalid.");
        }

        Order = order;
        Reason = reason;
    }
}