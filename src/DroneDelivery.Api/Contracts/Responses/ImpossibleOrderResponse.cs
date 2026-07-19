namespace DroneDelivery.Api.Contracts.Responses;

public sealed record ImpossibleOrderResponse
{
    public string OrderId { get; init; } = string.Empty;

    public string Reason { get; init; } = string.Empty;
}