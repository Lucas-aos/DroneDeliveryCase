using DroneDelivery.Domain.Entities;
using DroneDelivery.Domain.Planning;

namespace DroneDelivery.Api.Storage;

public sealed record StoredPlanningScenario
{
    public required Guid PlanningId { get; init; }

    public required DateTimeOffset CreatedAtUtc { get; init; }

    public required IReadOnlyList<Drone> Drones { get; init; }

    public required IReadOnlyList<Order> Orders { get; init; }

    public required PlanningResult Result { get; init; }
}