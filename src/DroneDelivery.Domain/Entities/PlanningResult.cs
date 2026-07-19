namespace DroneDelivery.Domain.Entities;

public class PlanningResult
{
    public IReadOnlyList<Trip> Trips { get; }

    public IReadOnlyList<ImpossibleOrder> ImpossibleOrders { get; }

    public PlanningResult(
        IEnumerable<Trip> trips,
        IEnumerable<ImpossibleOrder> impossibleOrders)
    {
        ArgumentNullException.ThrowIfNull(trips);
        ArgumentNullException.ThrowIfNull(impossibleOrders);

        var tripList = trips.ToList();
        var impossibleOrderList = impossibleOrders.ToList();

        if (tripList.Any(trip => trip is null))
        {
            throw new ArgumentException(
                "Trips cannot contain null items.",
                nameof(trips));
        }

        if (impossibleOrderList.Any(impossibleOrder => impossibleOrder is null))
        {
            throw new ArgumentException(
                "Impossible orders cannot contain null items.",
                nameof(impossibleOrders));
        }

        Trips = tripList.AsReadOnly();
        ImpossibleOrders = impossibleOrderList.AsReadOnly();
    }
}