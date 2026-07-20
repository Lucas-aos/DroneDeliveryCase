using DroneDelivery.Api.Analysis;

namespace DroneDelivery.Api.Models;

public sealed record FleetRecommendation
{
    public required RecommendationType Type { get; init; }

    public required RecommendationSeverity Severity { get; init; }

    public required string Title{ get; init; }

    public required string Description {get;init;}

    public double? SuggestedMinimumCapacityKg { get; init;}

    public double? SuggestedMinimumRangeKm { get; init; }
}