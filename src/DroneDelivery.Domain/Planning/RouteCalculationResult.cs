using DroneDelivery.Domain.Entities;

namespace DroneDelivery.Domain.Planning;

public class RouteCalculationResult
{
    public IReadOnlyList<Order> OrderedOrders { get; }

    public double TotalDistanceKm { get; }

    public RouteCalculationResult(
        IEnumerable<Order> orderedOrders,
        double totalDistanceKm)
    {
        ArgumentNullException.ThrowIfNull(orderedOrders);

        var orderList = orderedOrders.ToList();

        if (orderList.Any(order => order is null))
        {
            throw new ArgumentException(
                "Ordered orders cannot contain null items.",
                nameof(orderedOrders));
        }

        if (!double.IsFinite(totalDistanceKm) || totalDistanceKm < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(totalDistanceKm),
                totalDistanceKm,
                "Total distance must be a finite value greater than or equal to zero.");
        }

        OrderedOrders = orderList.AsReadOnly();
        TotalDistanceKm = totalDistanceKm;
    }
}