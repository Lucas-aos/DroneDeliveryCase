using DroneDelivery.Domain.Entities;

namespace DroneDelivery.Domain.Entities;

public class Trip
{
    public Drone Drone { get; }

    public IReadOnlyList<Order> Orders { get; }

    public double TotalWeightKg { get; }

    public double TotalDistanceKm { get; }

    public Trip(
        Drone drone,
        IEnumerable<Order> orders,
        double totalWeightKg,
        double totalDistanceKm)
    {
        ArgumentNullException.ThrowIfNull(drone);
        ArgumentNullException.ThrowIfNull(orders);

        var orderList = orders.ToList();

        if (orderList.Count == 0)
        {
            throw new ArgumentException(
                "A trip must contain at least one order.",
                nameof(orders));
        }

        if (orderList.Any(o => o is null))
        {
            throw new ArgumentException(
                "Orders cannot contain null items.",
                nameof(orders));
        }

        if (orderList.Select(o => o.Id).Distinct().Count() != orderList.Count)
        {
            throw new ArgumentException(
                "Orders cannot contain duplicate IDs.",
                nameof(orders));
        }

        if (!double.IsFinite(totalWeightKg) || totalWeightKg <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(totalWeightKg),
                totalWeightKg,
                "Total weight must be a finite value greater than zero.");
        }

        if (!double.IsFinite(totalDistanceKm) || totalDistanceKm < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(totalDistanceKm),
                totalDistanceKm,
                "Total distance must be a finite value greater than or equal to zero.");
        }

        Drone = drone;
        Orders = orderList.AsReadOnly();
        TotalWeightKg = totalWeightKg;
        TotalDistanceKm = totalDistanceKm;
    }
}