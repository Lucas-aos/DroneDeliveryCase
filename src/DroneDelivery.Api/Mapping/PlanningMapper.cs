using DroneDelivery.Api.Contracts.Requests;
using DroneDelivery.Api.Contracts.Responses;
using DroneDelivery.Api.Storage;
using DroneDelivery.Domain.Entities;
using DroneDelivery.Domain.Enums;
using DroneDelivery.Domain.ValueObjects;

namespace DroneDelivery.Api.Mapping;

public static class PlanningMapper
{
    public static IReadOnlyList<Drone> ToDomainDrones(
        IReadOnlyList<DroneRequest> requests)
    {
        ArgumentNullException.ThrowIfNull(requests);

        return requests
            .Select((request, index) =>
                new Drone(
                    request.Id,
                    request.CapacityKg,
                    request.RangeKm,
                    index))
            .ToArray();
    }

    public static IReadOnlyList<Order> ToDomainOrders(
        IReadOnlyList<OrderRequest> requests)
    {
        ArgumentNullException.ThrowIfNull(requests);

        return requests
            .Select((request, index) =>
                new Order(
                    request.Id,
                    request.WeightKg,
                    new Coordinate(
                        request.X,
                        request.Y),
                    MapPriority(request.Priority),
                    index))
            .ToArray();
    }

    public static PlanningResponse ToResponse(
        PlanningResult planningResult)
    {
        ArgumentNullException.ThrowIfNull(planningResult);

        return new PlanningResponse
        {
            Trips = planningResult.Trips
                .Select(ToTripResponse)
                .ToArray(),

            ImpossibleOrders = planningResult.ImpossibleOrders
                .Select(ToImpossibleOrderResponse)
                .ToArray()
        };
    }

    private static TripResponse ToTripResponse(
        Trip trip)
    {
        return new TripResponse
        {
            DroneId = trip.Drone.Id,

            Orders = trip.Orders
                .Select((order, index) => new TripOrderResponse
                {
                    Id = order.Id,
                    Sequence = index + 1
                })
                .ToArray(),

            TotalWeightKg = trip.TotalWeightKg,
            TotalDistanceKm = trip.TotalDistanceKm
        };
    }

    private static ImpossibleOrderResponse ToImpossibleOrderResponse(
        ImpossibleOrder impossibleOrder)
    {
        return new ImpossibleOrderResponse
        {
            OrderId = impossibleOrder.Order.Id,
            Reason = impossibleOrder.Reason.ToString()
        };
    }

    private static Priority MapPriority(string priority)
    {
        return priority switch
        {
            "High" => Priority.High,
            "Medium" => Priority.Medium,
            "Low" => Priority.Low,

            _ => throw new ArgumentException(
                $"Invalid priority '{priority}'.",
                nameof(priority))
        };
    }

    public static IReadOnlyList<RouteResponse> ToRouteResponses(
        StoredPlanningScenario scenario)
    {
        ArgumentNullException.ThrowIfNull(scenario);

        return scenario.Result.Trips
            .Select((trip, tripIndex) =>
                new RouteResponse
                {
                    TripSequence = tripIndex + 1,
                    DroneId = trip.Drone.Id,
                    TotalWeightKg = trip.TotalWeightKg,
                    TotalDistanceKm = trip.TotalDistanceKm,

                    Stops = trip.Orders
                        .Select((order, orderIndex) =>
                            new RouteStopResponse
                            {
                                Sequence = orderIndex + 1,
                                OrderId = order.Id,
                                X = order.Destination.X,
                                Y = order.Destination.Y
                            })
                        .ToArray()
                })
            .ToArray();
    }

    public static IReadOnlyList<DroneSummaryResponse>
        ToDroneSummaryResponses(
            StoredPlanningScenario scenario)
    {
        ArgumentNullException.ThrowIfNull(scenario);

        var tripsByDrone = scenario.Result.Trips
            .GroupBy(
                trip => trip.Drone.Id,
                StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                group => group.Key,
                group => group.ToArray(),
                StringComparer.OrdinalIgnoreCase);

        return scenario.Drones
            .Select(drone =>
            {
                if (!tripsByDrone.TryGetValue(
                        drone.Id,
                        out var droneTrips))
                {
                    return new DroneSummaryResponse
                    {
                        DroneId = drone.Id,
                        CapacityKg = drone.CapacityKg,
                        RangeKm = drone.RangeKm,
                        WasUsed = false,
                        TripCount = 0,
                        DeliveredOrders = 0,
                        TotalDeliveredWeightKg = 0,
                        TotalDistanceKm = 0
                    };
                }

                return new DroneSummaryResponse
                {
                    DroneId = drone.Id,
                    CapacityKg = drone.CapacityKg,
                    RangeKm = drone.RangeKm,
                    WasUsed = true,
                    TripCount = droneTrips.Length,
                    DeliveredOrders = droneTrips.Sum(
                        trip => trip.Orders.Count),
                    TotalDeliveredWeightKg = droneTrips.Sum(
                        trip => trip.TotalWeightKg),
                    TotalDistanceKm = droneTrips.Sum(
                        trip => trip.TotalDistanceKm)
                };
            })
            .ToArray();
    }
}