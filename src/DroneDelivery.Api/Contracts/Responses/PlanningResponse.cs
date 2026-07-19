namespace DroneDelivery.Api.Contracts.Responses;

public sealed record PlanningResponse
{
    public IReadOnlyList<TripResponse> Trips { get; init; } = [];

    public IReadOnlyList<ImpossibleOrderResponse> ImpossibleOrders { get; init; } = [];
}