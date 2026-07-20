namespace DroneDelivery.Api.Contracts.Responses;

public sealed record FleetRecommendationResponse
{
    public required string Type { get; init; }

    public required string Severity { get; init;}

    public required string Title { get; init; }

    public required string Description { get; init; }

    public double? SuggestedMinimumCapacityKg { get; init; }

    public double? SuggestedMinimumRangeKm { get; init; }
}