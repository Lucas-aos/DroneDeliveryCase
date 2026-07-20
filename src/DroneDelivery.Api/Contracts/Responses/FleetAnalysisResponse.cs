namespace DroneDelivery.Api.Contracts.Responses;

public sealed record FleetAnalysisResponse
{
    public required int TotalDrones { get; init; }

    public required int UsedDrones { get; init; }

    public required int TotalTrips { get; init; }

    public required int DeliveredOrders { get; init; }

    public required int ImpossibleOrders { get; init; }

    public required double TotalDeliveredWeightKg { get; init; }

    public required double TotalDistanceKm { get; init; }

    public required double FleetParticipationPercentage { get; init; }

    public required double AverageLoadFactorPercentage { get; init; }

    public required double FleetEfficiencyKgPerKm { get; init; }

    public required double EstimatedTotalTimeMinutes { get; init; }

    public required IReadOnlyList<DroneAnalysisResponse> Drones { get; init; }

    public required IReadOnlyList<FleetRecommendationResponse> Recommendations
    {
        get;
        init;
    }
}