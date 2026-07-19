namespace DroneDelivery.Api.Contracts.Responses;

public sealed record TripResponse
{
    public string DroneId { get; init; } = string.Empty;

    public IReadOnlyList<TripOrderResponse> Orders { get; init; } = [];

    public double TotalWeightKg { get; init; }

    public double TotalDistanceKm { get; init; }
}