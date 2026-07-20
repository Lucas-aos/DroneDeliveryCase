using DroneDelivery.Api.Contracts.Requests;
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
}