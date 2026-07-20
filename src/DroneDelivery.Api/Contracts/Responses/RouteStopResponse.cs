namespace DroneDelivery.Api.Contracts.Responses;

public sealed record RouteStopResponse
{
    public required int Sequence { get; init; }

    public required string OrderId { get; init; }

    public required double X { get; init; }

    public required double Y { get; init; }
}