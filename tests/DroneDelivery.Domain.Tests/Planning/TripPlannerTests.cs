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
        double capacityKg = 20,
        double rangeKm = 100,
        int inputOrder = 0)
        => new(
            id,
            capacityKg,
            rangeKm,
            inputOrder);

    private static Order CreateOrder(
        string id = "ORDER-01",
        double weightKg = 5,
        double x = 3,
        double y = 4,
        Priority priority = Priority.High,
        int inputOrder = 0)
        => new(
            id,
            weightKg,
            new Coordinate(x, y),
            priority,
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
    [Fact]
    public void Plan_WhenNoDroneSupportsOrderWeight_ShouldReturnWeightExceeded()
    {
        var drones = new[]
        {
        CreateDrone(
            capacityKg: 5,
            rangeKm: 100)
    };

        var order = CreateOrder(
            weightKg: 10,
            x: 3,
            y: 4);

        var result = _planner.Plan(
            drones,
            new[] { order });

        Assert.Empty(result.Trips);

        var impossibleOrder = Assert.Single(
            result.ImpossibleOrders);

        Assert.Same(order, impossibleOrder.Order);
        Assert.Equal(
            ImpossibleReason.WeightExceeded,
            impossibleOrder.Reason);
    }
    [Fact]
    public void Plan_WhenRoundTripExceedsEveryDroneRange_ShouldReturnRangeExceeded()
    {
        var drones = new[]
        {
        CreateDrone(
            capacityKg: 20,
            rangeKm: 9)
    };

        var order = CreateOrder(
            weightKg: 5,
            x: 3,
            y: 4);

        var result = _planner.Plan(
            drones,
            new[] { order });

        Assert.Empty(result.Trips);

        var impossibleOrder = Assert.Single(
            result.ImpossibleOrders);

        Assert.Same(order, impossibleOrder.Order);
        Assert.Equal(
            ImpossibleReason.RangeExceeded,
            impossibleOrder.Reason);
    }
    [Fact]
    public void Plan_WhenNoDroneSupportsWeightOrRange_ShouldReturnWeightAndRangeExceeded()
    {
        var drones = new[]
        {
        CreateDrone(
            capacityKg: 5,
            rangeKm: 9)
    };

        var order = CreateOrder(
            weightKg: 10,
            x: 3,
            y: 4);

        var result = _planner.Plan(
            drones,
            new[] { order });

        Assert.Empty(result.Trips);

        var impossibleOrder = Assert.Single(
            result.ImpossibleOrders);

        Assert.Same(order, impossibleOrder.Order);
        Assert.Equal(
            ImpossibleReason.WeightAndRangeExceeded,
            impossibleOrder.Reason);
    }
    [Fact]
    public void Plan_WhenWeightAndRangeAreSupportedByDifferentDrones_ShouldReturnWeightAndRangeExceeded()
    {
        var drones = new[]
        {
        CreateDrone(
            id: "DRONE-WEIGHT",
            capacityKg: 10,
            rangeKm: 10,
            inputOrder: 0),

        CreateDrone(
            id: "DRONE-RANGE",
            capacityKg: 5,
            rangeKm: 20,
            inputOrder: 1)
    };

        var order = CreateOrder(
            weightKg: 8,
            x: 7.5,
            y: 0);

        var result = _planner.Plan(
            drones,
            new[] { order });

        Assert.Empty(result.Trips);

        var impossibleOrder = Assert.Single(
            result.ImpossibleOrders);

        Assert.Same(order, impossibleOrder.Order);
        Assert.Equal(
            ImpossibleReason.WeightAndRangeExceeded,
            impossibleOrder.Reason);
    }
    [Fact]
    public void Plan_WhenSameDroneSupportsWeightAndRange_ShouldNotClassifyOrderAsImpossible()
    {
        var drones = new[]
        {
        CreateDrone(
            capacityKg: 10,
            rangeKm: 20)
    };

        var order = CreateOrder(
            weightKg: 8,
            x: 7.5,
            y: 0);

        Assert.Throws<NotSupportedException>(
            () => _planner.Plan(
                drones,
                new[] { order }));
    }
    [Fact]
    public void Plan_WhenAllOrdersAreImpossible_ShouldReturnEachReason()
    {
        var drones = new[]
        {
        CreateDrone(
            capacityKg: 10,
            rangeKm: 20)
    };

        var weightExceededOrder = CreateOrder(
            id: "ORDER-WEIGHT",
            weightKg: 15,
            x: 1,
            y: 0,
            inputOrder: 0);

        var rangeExceededOrder = CreateOrder(
            id: "ORDER-RANGE",
            weightKg: 5,
            x: 11,
            y: 0,
            inputOrder: 1);

        var result = _planner.Plan(
            drones,
            new[]
            {
            weightExceededOrder,
            rangeExceededOrder
            });

        Assert.Empty(result.Trips);
        Assert.Equal(2, result.ImpossibleOrders.Count);

        Assert.Contains(
            result.ImpossibleOrders,
            item =>
                item.Order.Id == "ORDER-WEIGHT"
                && item.Reason
                    == ImpossibleReason.WeightExceeded);

        Assert.Contains(
            result.ImpossibleOrders,
            item =>
                item.Order.Id == "ORDER-RANGE"
                && item.Reason
                    == ImpossibleReason.RangeExceeded);
    }
}