namespace DroneDelivery.Api.Contracts.Responses;

public sealed record ErrorResponse
{
    public string Message { get; init; } = string.Empty;

    public IReadOnlyList<string> Errors { get; init; } = [];
}