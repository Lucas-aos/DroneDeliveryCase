namespace DroneDelivery.Api.Contracts.Responses;

public sealed record TripOrderResponse
{
    public string Id { get; init; } = string.Empty;

    public int Sequence { get; init; }
}