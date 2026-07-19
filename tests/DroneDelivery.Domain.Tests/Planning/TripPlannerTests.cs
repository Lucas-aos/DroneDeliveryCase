using DroneDelivery.Domain.Entities;
using DroneDelivery.Domain.Enums;
using DroneDelivery.Domain.Planning;
using DroneDelivery.Domain.ValueObjects;

namespace DroneDelivery.Domain.Tests.Planning;

public class TripPlannerTests
{
    private readonly TripPlanner _planner = new();

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
            destination: new Coordinate(3, 4),
            priority: Priority.High,
            inputOrder);

    [Fact]
    public void Plan_WithNullDrones_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => _planner.Plan(
                null!,
                Array.Empty<Order>()));

        Assert.Equal("drones", exception.ParamName);
    }

    [Fact]
    public void Plan_WithNullOrders_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => _planner.Plan(
                Array.Empty<Drone>(),
                null!));

        Assert.Equal("orders", exception.ParamName);
    }

    [Fact]
    public void Plan_WithNullDroneItem_ShouldThrowArgumentException()
    {
        var drones = new Drone[]
        {
            CreateDrone(),
            null!
        };

        var exception = Assert.Throws<ArgumentException>(
            () => _planner.Plan(
                drones,
                Array.Empty<Order>()));

        Assert.Equal("drones", exception.ParamName);
    }

    [Fact]
    public void Plan_WithNullOrderItem_ShouldThrowArgumentException()
    {
        var orders = new Order[]
        {
            CreateOrder(),
            null!
        };

        var exception = Assert.Throws<ArgumentException>(
            () => _planner.Plan(
                new[] { CreateDrone() },
                orders));

        Assert.Equal("orders", exception.ParamName);
    }

    [Fact]
    public void Plan_WithDuplicateDroneIds_ShouldThrowArgumentException()
    {
        var drones = new[]
        {
            CreateDrone(
                id: "DRONE-01",
                inputOrder: 0),

            CreateDrone(
                id: "DRONE-01",
                inputOrder: 1)
        };

        var exception = Assert.Throws<ArgumentException>(
            () => _planner.Plan(
                drones,
                Array.Empty<Order>()));

        Assert.Equal("drones", exception.ParamName);
    }

    [Fact]
    public void Plan_WithDuplicateOrderIds_ShouldThrowArgumentException()
    {
        var orders = new[]
        {
            CreateOrder(
                id: "ORDER-01",
                inputOrder: 0),

            CreateOrder(
                id: "ORDER-01",
                inputOrder: 1)
        };

        var exception = Assert.Throws<ArgumentException>(
            () => _planner.Plan(
                new[] { CreateDrone() },
                orders));

        Assert.Equal("orders", exception.ParamName);
    }

    [Fact]
    public void Plan_WithDuplicateDroneInputOrders_ShouldThrowArgumentException()
    {
        var drones = new[]
        {
            CreateDrone(
                id: "DRONE-01",
                inputOrder: 0),

            CreateDrone(
                id: "DRONE-02",
                inputOrder: 0)
        };

        var exception = Assert.Throws<ArgumentException>(
            () => _planner.Plan(
                drones,
                Array.Empty<Order>()));

        Assert.Equal("drones", exception.ParamName);
    }

    [Fact]
    public void Plan_WithDuplicateOrderInputOrders_ShouldThrowArgumentException()
    {
        var orders = new[]
        {
            CreateOrder(
                id: "ORDER-01",
                inputOrder: 0),

            CreateOrder(
                id: "ORDER-02",
                inputOrder: 0)
        };

        var exception = Assert.Throws<ArgumentException>(
            () => _planner.Plan(
                new[] { CreateDrone() },
                orders));

        Assert.Equal("orders", exception.ParamName);
    }

    [Fact]
    public void Plan_WithNoOrdersAndNoDrones_ShouldReturnEmptyResult()
    {
        var result = _planner.Plan(
            Array.Empty<Drone>(),
            Array.Empty<Order>());

        Assert.Empty(result.Trips);
        Assert.Empty(result.ImpossibleOrders);
    }

    [Fact]
    public void Plan_WithNoOrdersAndAvailableDrones_ShouldReturnEmptyResult()
    {
        var drones = new[]
        {
            CreateDrone(
                id: "DRONE-01",
                inputOrder: 0),

            CreateDrone(
                id: "DRONE-02",
                inputOrder: 1)
        };

        var result = _planner.Plan(
            drones,
            Array.Empty<Order>());

        Assert.Empty(result.Trips);
        Assert.Empty(result.ImpossibleOrders);
    }

    [Fact]
    public void Plan_WithOrdersAndNoDrones_ShouldThrowArgumentException()
    {
        var orders = new[]
        {
            CreateOrder()
        };

        var exception = Assert.Throws<ArgumentException>(
            () => _planner.Plan(
                Array.Empty<Drone>(),
                orders));

        Assert.Equal("drones", exception.ParamName);
    }

    [Fact]
    public void Plan_WithValidDronesAndOrders_ShouldNotReturnProvisionalResult()
    {
        var drones = new[]
        {
            CreateDrone()
        };

        var orders = new[]
        {
            CreateOrder()
        };

        Assert.Throws<NotSupportedException>(
            () => _planner.Plan(drones, orders));
    }
}