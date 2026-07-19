using DroneDelivery.Domain.Calculations;
using DroneDelivery.Domain.Entities;
using DroneDelivery.Domain.Enums;
using DroneDelivery.Domain.ValueObjects;

namespace DroneDelivery.Domain.Planning;

public class TripPlanner
{
    private static readonly Coordinate BaseCoordinate = new(0, 0);

    public PlanningResult Plan(
        IEnumerable<Drone> drones,
        IEnumerable<Order> orders)
    {
        ArgumentNullException.ThrowIfNull(drones);
        ArgumentNullException.ThrowIfNull(orders);

        var droneList = drones.ToList();
        var orderList = orders.ToList();

        ValidateNullItems(droneList, orderList);
        ValidateDuplicateDroneIds(droneList);
        ValidateDuplicateOrderIds(orderList);
        ValidateDuplicateDroneInputOrders(droneList);
        ValidateDuplicateOrderInputOrders(orderList);

        if (orderList.Count == 0)
        {
            return new PlanningResult(
                Array.Empty<Trip>(),
                Array.Empty<ImpossibleOrder>());
        }

        if (droneList.Count == 0)
        {
            throw new ArgumentException(
                "At least one drone is required when there are orders to plan.",
                nameof(drones));
        }

        var impossibleOrders = IdentifyImpossibleOrders(
            droneList,
            orderList);

        if (impossibleOrders.Count == orderList.Count)
        {
            return new PlanningResult(
                Array.Empty<Trip>(),
                impossibleOrders);
        }

        throw new NotSupportedException(
            "Trip formation for feasible orders has not been implemented yet.");
    }

    private static List<ImpossibleOrder> IdentifyImpossibleOrders(
        IReadOnlyCollection<Drone> drones,
        IEnumerable<Order> orders)
    {
        var impossibleOrders = new List<ImpossibleOrder>();

        foreach (var order in orders)
        {
            var outboundDistanceKm = DistanceCalculator.Calculate(
                BaseCoordinate,
                order.Destination);

            var requiredRangeKm = outboundDistanceKm * 2;

            var hasDroneWithSufficientWeight = drones.Any(
                drone => drone.CapacityKg >= order.WeightKg);

            var hasDroneWithSufficientRange = drones.Any(
                drone => drone.RangeKm >= requiredRangeKm);

            var hasCompatibleDrone = drones.Any(
                drone =>
                    drone.CapacityKg >= order.WeightKg
                    && drone.RangeKm >= requiredRangeKm);

            if (hasCompatibleDrone)
            {
                continue;
            }

            var reason = DetermineImpossibleReason(
                hasDroneWithSufficientWeight,
                hasDroneWithSufficientRange);

            impossibleOrders.Add(
                new ImpossibleOrder(order, reason));
        }

        return impossibleOrders;
    }

    private static ImpossibleReason DetermineImpossibleReason(
        bool hasDroneWithSufficientWeight,
        bool hasDroneWithSufficientRange)
    {
        if (!hasDroneWithSufficientWeight
            && hasDroneWithSufficientRange)
        {
            return ImpossibleReason.WeightExceeded;
        }

        if (hasDroneWithSufficientWeight
            && !hasDroneWithSufficientRange)
        {
            return ImpossibleReason.RangeExceeded;
        }

        return ImpossibleReason.WeightAndRangeExceeded;
    }

    private static void ValidateNullItems(
        IReadOnlyCollection<Drone> drones,
        IReadOnlyCollection<Order> orders)
    {
        if (drones.Any(drone => drone is null))
        {
            throw new ArgumentException(
                "Drones cannot contain null items.",
                nameof(drones));
        }

        if (orders.Any(order => order is null))
        {
            throw new ArgumentException(
                "Orders cannot contain null items.",
                nameof(orders));
        }
    }

    private static void ValidateDuplicateDroneIds(
        IEnumerable<Drone> drones)
    {
        var hasDuplicateId = drones
            .GroupBy(
                drone => drone.Id,
                StringComparer.Ordinal)
            .Any(group => group.Count() > 1);

        if (hasDuplicateId)
        {
            throw new ArgumentException(
                "Drone IDs must be unique.",
                nameof(drones));
        }
    }

    private static void ValidateDuplicateOrderIds(
        IEnumerable<Order> orders)
    {
        var hasDuplicateId = orders
            .GroupBy(
                order => order.Id,
                StringComparer.Ordinal)
            .Any(group => group.Count() > 1);

        if (hasDuplicateId)
        {
            throw new ArgumentException(
                "Order IDs must be unique.",
                nameof(orders));
        }
    }

    private static void ValidateDuplicateDroneInputOrders(
        IEnumerable<Drone> drones)
    {
        var hasDuplicateInputOrder = drones
            .GroupBy(drone => drone.InputOrder)
            .Any(group => group.Count() > 1);

        if (hasDuplicateInputOrder)
        {
            throw new ArgumentException(
                "Drone InputOrder values must be unique.",
                nameof(drones));
        }
    }

    private static void ValidateDuplicateOrderInputOrders(
        IEnumerable<Order> orders)
    {
        var hasDuplicateInputOrder = orders
            .GroupBy(order => order.InputOrder)
            .Any(group => group.Count() > 1);

        if (hasDuplicateInputOrder)
        {
            throw new ArgumentException(
                "Order InputOrder values must be unique.",
                nameof(orders));
        }
    }
}