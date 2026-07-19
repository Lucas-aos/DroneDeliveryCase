using DroneDelivery.Domain.Entities;
using DroneDelivery.Domain.Enums;
using DroneDelivery.Domain.ValueObjects;

namespace DroneDelivery.Domain.Tests.Entities;

public class PlanningResultTests
{
    private static Drone CreateDrone(
        string id = "DRONE-01",
        int inputOrder = 0)
        => new(
            id,
            capacityKg: 20,
            rangeKm: 100,
            inputOrder);

    private static Order CreateOrder(
        string id = "ORDER-01",
        int inputOrder = 0)
        => new(
            id,
            weightKg: 5,
            destination: new Coordinate(10, 5),
            priority: Priority.High,
            inputOrder);

    private static Trip CreateTrip(
        string droneId = "DRONE-01",
        string orderId = "ORDER-01")
    {
        var drone = CreateDrone(droneId);
        var order = CreateOrder(orderId);

        return new Trip(
            drone,
            new[] { order },
            totalWeightKg: order.WeightKg,
            totalDistanceKm: 25);
    }

    private static ImpossibleOrder CreateImpossibleOrder(
        string orderId = "ORDER-02")
    {
        var order = CreateOrder(orderId);

        return new ImpossibleOrder(
            order,
            ImpossibleReason.WeightExceeded);
    }

    [Fact]
    public void Constructor_WithValidArguments_ShouldCreatePlanningResult()
    {
        var trip = CreateTrip();
        var impossibleOrder = CreateImpossibleOrder();

        var result = new PlanningResult(
            new[] { trip },
            new[] { impossibleOrder });

        Assert.Single(result.Trips);
        Assert.Single(result.ImpossibleOrders);
        Assert.Same(trip, result.Trips[0]);
        Assert.Same(impossibleOrder, result.ImpossibleOrders[0]);
    }

    [Fact]
    public void Constructor_WithEmptyCollections_ShouldCreatePlanningResult()
    {
        var result = new PlanningResult(
            Array.Empty<Trip>(),
            Array.Empty<ImpossibleOrder>());

        Assert.Empty(result.Trips);
        Assert.Empty(result.ImpossibleOrders);
    }

    [Fact]
    public void Constructor_WithNullTrips_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new PlanningResult(
                null!,
                Array.Empty<ImpossibleOrder>()));

        Assert.Equal("trips", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullImpossibleOrders_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new PlanningResult(
                Array.Empty<Trip>(),
                null!));

        Assert.Equal("impossibleOrders", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullItemInTrips_ShouldThrowArgumentException()
    {
        var trips = new Trip[]
        {
            CreateTrip(),
            null!
        };

        var exception = Assert.Throws<ArgumentException>(
            () => new PlanningResult(
                trips,
                Array.Empty<ImpossibleOrder>()));

        Assert.Equal("trips", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullItemInImpossibleOrders_ShouldThrowArgumentException()
    {
        var impossibleOrders = new ImpossibleOrder[]
        {
            CreateImpossibleOrder(),
            null!
        };

        var exception = Assert.Throws<ArgumentException>(
            () => new PlanningResult(
                Array.Empty<Trip>(),
                impossibleOrders));

        Assert.Equal("impossibleOrders", exception.ParamName);
    }

    [Fact]
    public void Constructor_ShouldCreateInternalCopyOfTrips()
    {
        var trips = new List<Trip>
        {
            CreateTrip()
        };

        var result = new PlanningResult(
            trips,
            Array.Empty<ImpossibleOrder>());

        trips.Clear();

        Assert.Single(result.Trips);
    }

    [Fact]
    public void Constructor_ShouldCreateInternalCopyOfImpossibleOrders()
    {
        var impossibleOrders = new List<ImpossibleOrder>
        {
            CreateImpossibleOrder()
        };

        var result = new PlanningResult(
            Array.Empty<Trip>(),
            impossibleOrders);

        impossibleOrders.Clear();

        Assert.Single(result.ImpossibleOrders);
    }

    [Fact]
    public void Trips_ShouldBeExposedAsReadOnlyList()
    {
        var result = new PlanningResult(
            new[] { CreateTrip() },
            Array.Empty<ImpossibleOrder>());

        Assert.IsAssignableFrom<IReadOnlyList<Trip>>(result.Trips);
    }

    [Fact]
    public void ImpossibleOrders_ShouldBeExposedAsReadOnlyList()
    {
        var result = new PlanningResult(
            Array.Empty<Trip>(),
            new[] { CreateImpossibleOrder() });

        Assert.IsAssignableFrom<IReadOnlyList<ImpossibleOrder>>(
            result.ImpossibleOrders);
    }

    [Fact]
    public void Constructor_WithMultipleTripsForSameDroneId_ShouldCreatePlanningResult()
    {
        var trips = new[]
        {
            CreateTrip(
                droneId: "DRONE-01",
                orderId: "ORDER-01"),

            CreateTrip(
                droneId: "DRONE-01",
                orderId: "ORDER-02")
        };

        var result = new PlanningResult(
            trips,
            Array.Empty<ImpossibleOrder>());

        Assert.Equal(2, result.Trips.Count);
    }

    [Fact]
    public void Constructor_WithRepeatedOrderAcrossCollections_ShouldCreatePlanningResult()
    {
        var deliveredOrder = CreateOrder("ORDER-01");

        var trip = new Trip(
            CreateDrone(),
            new[] { deliveredOrder },
            totalWeightKg: deliveredOrder.WeightKg,
            totalDistanceKm: 25);

        var impossibleOrder = new ImpossibleOrder(
            CreateOrder("ORDER-01"),
            ImpossibleReason.RangeExceeded);

        var result = new PlanningResult(
            new[] { trip },
            new[] { impossibleOrder });

        Assert.Single(result.Trips);
        Assert.Single(result.ImpossibleOrders);
    }
}