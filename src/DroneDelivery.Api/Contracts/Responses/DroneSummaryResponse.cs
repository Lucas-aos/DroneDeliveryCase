namespace DroneDelivery.Api.Contracts.Responses;

public sealed record DroneSummaryResponse
{
    public required string DroneId { get; init; }

    public required double CapacityKg { get; init; }

    public required double RangeKm { get; init; }

    public required bool WasUsed { get; init; }

    public required int TripCount { get; init; }

    public required int DeliveredOrders { get; init; }

    public required double TotalDeliveredWeightKg { get; init; }

    public required double TotalDistanceKm { get; init; }
}