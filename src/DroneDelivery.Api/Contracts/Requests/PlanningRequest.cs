namespace DroneDelivery.Api.Contracts.Requests;

public sealed record PlanningRequest
{
    public IReadOnlyList<DroneRequest> Drones { get; init; } = [];

    public IReadOnlyList<OrderRequest> Orders { get; init; } = [];
}