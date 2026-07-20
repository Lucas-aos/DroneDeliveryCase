namespace DroneDelivery.Api.Contracts.Responses;

public sealed record PlanningCreatedResponse
{
    public required Guid PlanningId { get; init; }

    public required DateTimeOffset CreatedAtUtc { get; init; }

    public required PlanningResponse Planning { get; init; }
}