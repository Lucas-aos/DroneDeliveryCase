namespace DroneDelivery.Api.Models;

public sealed record FleetAnalysis
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

    public required IReadOnlyList<DroneAnalysis> Drones { get; init; }

    public required IReadOnlyList<FleetRecommendation> Recommendations
    {
        get;
        init;
    }
}