namespace DroneDelivery.Api.Contracts.Responses;

public sealed record DroneAnalysisResponse
{
    public required string DroneId { get; init; }

    public required bool WasUsed { get; init; }

    public required int TripCount { get; init; }

    public required int DeliveredOrders { get; init; }

    public required double DeliveredWeightKg { get; init; }

    public required double DistanceKm { get; init; }

    public required double EfficiencyKgPerKm { get; init; }

    public required double AverageLoadFactorPercentage { get; init; }

    public required double AverageBatteryUsagePerTripPercentage { get; init; }

    public required double MaximumBatteryUsagePerTripPercentage { get; init; }

    public required double EstimatedTimeMinutes { get; init; }
}