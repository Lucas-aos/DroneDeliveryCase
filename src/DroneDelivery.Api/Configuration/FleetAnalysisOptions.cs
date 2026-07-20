namespace DroneDelivery.Api.Configuration;

public sealed class FleetAnalysisOptions
{
    public double DroneSpeedKmPerHour { get; init; } = 40;

    public double UnderutilizedFleetThresholdPercentage { get; init; } = 50;

    public double HighAverageLoadThresholdPercentage { get; init; } = 85;

    public double HighRangeUsageThresholdPercentage { get; init; } = 90;
}