using DroneDelivery.Domain.Calculations;
using DroneDelivery.Domain.Entities;
using DroneDelivery.Domain.Enums;
using DroneDelivery.Domain.ValueObjects;

namespace DroneDelivery.Domain.Planning;

public class TripPlanner
{
    private static readonly Coordinate BaseCoordinate = new(0, 0);

    private static readonly NearestNeighborRouteCalculator RouteCalculator = new();


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

        var impossibleOrderIds = impossibleOrders
            .Select(item => item.Order.Id)
            .ToHashSet(StringComparer.Ordinal);

        var feasibleOrders = orderList
            .Where(order => !impossibleOrderIds.Contains(order.Id))
            .ToList();

        if (feasibleOrders.Count == 0)
        {
            return new PlanningResult(
                Array.Empty<Trip>(),
                impossibleOrders);
        }

        var trips = BuildTrips(
            droneList,
            feasibleOrders);

        return new PlanningResult(
            trips,
            impossibleOrders);
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

    private static IReadOnlyList<Trip> BuildTrips(
    IReadOnlyCollection<Drone> drones,
    IReadOnlyCollection<Order> feasibleOrders)
    {
        var remainingOrders = feasibleOrders.ToList();
        var trips = new List<Trip>();

        while (remainingOrders.Count > 0)
        {
            var trip = CreateTrip(
                drones,
                remainingOrders);

            trips.Add(trip);

            var plannedOrderIds = trip.Orders
                .Select(order => order.Id)
                .ToHashSet(StringComparer.Ordinal);

            remainingOrders.RemoveAll(order =>
                plannedOrderIds.Contains(order.Id));
        }

        return trips;
    }

    private static Trip CreateTrip(
    IReadOnlyCollection<Drone> drones,
    IReadOnlyCollection<Order> remainingOrders)
    {
        var firstOrder = SelectFirstOrder(
            remainingOrders);

        var initialRoute = RouteCalculator.Calculate(
            new[] { firstOrder });

        var selectedDrone = SelectDrone(
            drones,
            firstOrder.WeightKg,
            initialRoute.TotalDistanceKm);

        return BuildTrip(
            selectedDrone,
            firstOrder,
            remainingOrders);
    }

    private static Trip BuildTrip(
    Drone drone,
    Order firstOrder,
    IReadOnlyCollection<Order> feasibleOrders)
    {
        var selectedOrders = new List<Order>
    {
        firstOrder
    };

        var remainingOrders = feasibleOrders
            .Where(order =>
                !string.Equals(
                    order.Id,
                    firstOrder.Id,
                    StringComparison.Ordinal))
            .ToList();

        var currentRoute = RouteCalculator.Calculate(
            selectedOrders);

        while (remainingOrders.Count > 0)
        {
            var candidate = FindBestInsertionCandidate(
                drone,
                selectedOrders,
                remainingOrders,
                currentRoute.TotalDistanceKm);

            if (candidate is null)
            {
                break;
            }

            selectedOrders.Add(candidate.Order);
            currentRoute = candidate.Route;

            remainingOrders.RemoveAll(order =>
                string.Equals(
                    order.Id,
                    candidate.Order.Id,
                    StringComparison.Ordinal));

        }

        var totalWeightKg = currentRoute.OrderedOrders
            .Sum(order => order.WeightKg);

        return new Trip(
            drone,
            currentRoute.OrderedOrders,
            totalWeightKg,
            currentRoute.TotalDistanceKm);
    }
    private static InsertionCandidate? FindBestInsertionCandidate(
    Drone drone,
    IReadOnlyCollection<Order> selectedOrders,
    IReadOnlyCollection<Order> remainingOrders,
    double currentDistanceKm)
    {
        var currentWeightKg = selectedOrders
            .Sum(order => order.WeightKg);

        InsertionCandidate? bestCandidate = null;

        foreach (var order in remainingOrders)
        {
            var simulatedWeightKg =
                currentWeightKg + order.WeightKg;

            if (simulatedWeightKg > drone.CapacityKg)
            {
                continue;
            }

            var simulatedOrders = selectedOrders
                .Append(order)
                .ToList();

            var simulatedRoute = RouteCalculator.Calculate(
                simulatedOrders);

            if (simulatedRoute.TotalDistanceKm > drone.RangeKm)
            {
                continue;
            }

            var distanceIncreaseKm =
                simulatedRoute.TotalDistanceKm
                - currentDistanceKm;

            if (bestCandidate is null
                || distanceIncreaseKm
                    < bestCandidate.DistanceIncreaseKm)
            {
                bestCandidate = new InsertionCandidate(
                    order,
                    simulatedRoute,
                    distanceIncreaseKm);
            }
        }

        return bestCandidate;
    }
    private static Drone SelectDrone(
    IReadOnlyCollection<Drone> drones,
    double requiredWeightKg,
    double requiredRangeKm)
    {
        return drones
            .Where(drone =>
                drone.CapacityKg >= requiredWeightKg
                && drone.RangeKm >= requiredRangeKm)
            .OrderByDescending(drone => drone.CapacityKg)
            .ThenByDescending(drone => drone.RangeKm)
            .ThenBy(drone => drone.InputOrder)
            .ThenBy(
                drone => drone.Id,
                StringComparer.Ordinal)
            .First();
    }

    private static Order SelectFirstOrder(
    IEnumerable<Order> orders)
    {
        return orders
            .OrderByDescending(
                order => GetPriorityRank(order.Priority))
            .ThenByDescending(order => order.WeightKg)
            .ThenBy(order => order.InputOrder)
            .ThenBy(
                order => order.Id,
                StringComparer.Ordinal)
            .First();
    }
    private static int GetPriorityRank(Priority priority)
    {
        return priority switch
        {
            Priority.High => 3,
            Priority.Medium => 2,
            Priority.Low => 1,

            _ => throw new ArgumentOutOfRangeException(
                nameof(priority),
                priority,
                "Unsupported priority.")
        };
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


    private record InsertionCandidate(
    Order Order,
    RouteCalculationResult Route,
    double DistanceIncreaseKm);

}