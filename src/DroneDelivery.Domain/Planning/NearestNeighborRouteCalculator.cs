using DroneDelivery.Domain.Calculations;
using DroneDelivery.Domain.Entities;
using DroneDelivery.Domain.Enums;
using DroneDelivery.Domain.ValueObjects;

namespace DroneDelivery.Domain.Planning;

public class NearestNeighborRouteCalculator
{
    private static readonly Coordinate BaseCoordinate = new(0, 0);

    public RouteCalculationResult Calculate(
        IEnumerable<Order> orders)
    {
        ArgumentNullException.ThrowIfNull(orders);

        var remainingOrders = orders.ToList();

        if (remainingOrders.Any(order => order is null))
        {
            throw new ArgumentException(
                "Orders cannot contain null items.",
                nameof(orders));
        }

        if (remainingOrders.Count == 0)
        {
            return new RouteCalculationResult(
                Array.Empty<Order>(),
                totalDistanceKm: 0);
        }

        var orderedRoute = new List<Order>(
            capacity: remainingOrders.Count);

        var currentCoordinate = BaseCoordinate;
        var totalDistanceKm = 0.0;

        while (remainingOrders.Count > 0)
        {
            var nextCandidate = remainingOrders
                .Select(order => new
                {
                    Order = order,
                    DistanceKm = DistanceCalculator.Calculate(
                        currentCoordinate,
                        order.Destination)
                })
                .OrderBy(candidate => candidate.DistanceKm)
                .ThenBy(candidate => GetPriorityOrder(candidate.Order.Priority))
                .ThenBy(candidate => candidate.Order.InputOrder)
                .ThenByDescending(candidate => candidate.Order.WeightKg)
                .ThenBy(
                    candidate => candidate.Order.Id,
                    StringComparer.Ordinal)
                .First();

            orderedRoute.Add(nextCandidate.Order);
            totalDistanceKm += nextCandidate.DistanceKm;
            currentCoordinate = nextCandidate.Order.Destination;

            remainingOrders.Remove(nextCandidate.Order);
        }

        totalDistanceKm += DistanceCalculator.Calculate(
            currentCoordinate,
            BaseCoordinate);

        return new RouteCalculationResult(
            orderedRoute,
            totalDistanceKm);
    }

    private static int GetPriorityOrder(Priority priority)
    {
        return priority switch
        {
            Priority.High => 0,
            Priority.Medium => 1,
            Priority.Low => 2,

            _ => throw new ArgumentOutOfRangeException(
                nameof(priority),
                priority,
                "The order priority is invalid.")
        };
    }
}