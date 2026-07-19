namespace DroneDelivery.Api.Contracts.Requests;

public sealed record OrderRequest
{
    public string Id { get; init; } = string.Empty;

    public double WeightKg { get; init; }

    public string Priority { get; init; } = string.Empty;

    public double X { get; init; }

    public double Y { get; init; }
}