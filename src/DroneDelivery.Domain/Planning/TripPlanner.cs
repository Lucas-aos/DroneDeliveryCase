using DroneDelivery.Domain.Entities;

namespace DroneDelivery.Domain.Planning;

public class TripPlanner
{
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

        throw new NotSupportedException(
            "Planning trips for non-empty drone and order collections " +
            "has not been implemented yet.");
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