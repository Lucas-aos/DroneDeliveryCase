namespace DroneDelivery.Api.Contracts.Requests;

public sealed record DroneRequest
{
    public string Id { get; init; } = string.Empty;

    public double CapacityKg { get; init; }

    public double RangeKm { get; init; }
}