namespace DroneDelivery.Api.Contracts.Responses;

public sealed record RouteResponse
{
    public required int TripSequence { get; init; }

    public required string DroneId { get; init; }

    public required double TotalWeightKg { get; init; }

    public required double TotalDistanceKm { get; init; }

    public required IReadOnlyList<RouteStopResponse> Stops { get; init; }
}