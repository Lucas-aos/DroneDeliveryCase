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
    public void Plan_WhenSameDroneSupportsWeightAndRange_ShouldCreateTrip()
    {
        var drone = CreateDrone(
            capacityKg: 10,
            rangeKm: 20);

        var order = CreateOrder(
            weightKg: 8,
            x: 7.5,
            y: 0);

        var result = _planner.Plan(
            new[] { drone },
            new[] { order });

        Assert.Empty(result.ImpossibleOrders);

        var trip = Assert.Single(result.Trips);

        Assert.Same(drone, trip.Drone);

        var plannedOrder = Assert.Single(trip.Orders);
        Assert.Same(order, plannedOrder);
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
    [Fact]
    public void Plan_WhenThereIsOneFeasibleOrder_ShouldCreateSingleOrderTrip()
    {
        var drone = CreateDrone(
            capacityKg: 10,
            rangeKm: 20);

        var order = CreateOrder(
            weightKg: 5,
            x: 3,
            y: 4);

        var result = _planner.Plan(
            new[] { drone },
            new[] { order });

        var trip = Assert.Single(result.Trips);

        Assert.Empty(result.ImpossibleOrders);
        Assert.Same(drone, trip.Drone);

        var tripOrder = Assert.Single(trip.Orders);

        Assert.Same(order, tripOrder);
        Assert.Equal(5, trip.TotalWeightKg);
        Assert.Equal(10, trip.TotalDistanceKm, 10);
    }
    [Fact]
    public void Plan_WhenThereIsOneFeasibleOrderAndOneImpossibleOrder_ShouldReturnBoth()
    {
        var drone = CreateDrone(
            capacityKg: 10,
            rangeKm: 20);

        var feasibleOrder = CreateOrder(
            id: "ORDER-FEASIBLE",
            weightKg: 5,
            x: 3,
            y: 4,
            inputOrder: 0);

        var impossibleOrder = CreateOrder(
            id: "ORDER-IMPOSSIBLE",
            weightKg: 15,
            x: 1,
            y: 0,
            inputOrder: 1);

        var result = _planner.Plan(
            new[] { drone },
            new[]
            {
            feasibleOrder,
            impossibleOrder
            });

        var trip = Assert.Single(result.Trips);
        var deliveredOrder = Assert.Single(trip.Orders);

        Assert.Same(feasibleOrder, deliveredOrder);

        var impossibleResult = Assert.Single(
            result.ImpossibleOrders);

        Assert.Same(
            impossibleOrder,
            impossibleResult.Order);

        Assert.Equal(
            ImpossibleReason.WeightExceeded,
            impossibleResult.Reason);
    }
    [Fact]
    public void Plan_WhenMultipleCompatibleDronesExist_ShouldSelectHighestCapacity()
    {
        var lowerCapacityDrone = CreateDrone(
            id: "DRONE-LOWER-CAPACITY",
            capacityKg: 10,
            rangeKm: 100,
            inputOrder: 0);

        var higherCapacityDrone = CreateDrone(
            id: "DRONE-HIGHER-CAPACITY",
            capacityKg: 20,
            rangeKm: 20,
            inputOrder: 1);

        var order = CreateOrder(
            weightKg: 5,
            x: 3,
            y: 4);

        var result = _planner.Plan(
            new[]
            {
            lowerCapacityDrone,
            higherCapacityDrone
            },
            new[] { order });

        var trip = Assert.Single(result.Trips);

        Assert.Same(higherCapacityDrone, trip.Drone);
    }
    [Fact]
    public void Plan_WhenDroneCapacitiesAreEqual_ShouldSelectHighestRange()
    {
        var lowerRangeDrone = CreateDrone(
            id: "DRONE-LOWER-RANGE",
            capacityKg: 20,
            rangeKm: 30,
            inputOrder: 0);

        var higherRangeDrone = CreateDrone(
            id: "DRONE-HIGHER-RANGE",
            capacityKg: 20,
            rangeKm: 40,
            inputOrder: 1);

        var order = CreateOrder(
            weightKg: 5,
            x: 3,
            y: 4);

        var result = _planner.Plan(
            new[]
            {
            lowerRangeDrone,
            higherRangeDrone
            },
            new[] { order });

        var trip = Assert.Single(result.Trips);

        Assert.Same(higherRangeDrone, trip.Drone);
    }
    [Fact]
    public void Plan_WhenCapacityAndRangeAreEqual_ShouldSelectLowestInputOrder()
    {
        var laterDrone = CreateDrone(
            id: "DRONE-LATER",
            capacityKg: 20,
            rangeKm: 40,
            inputOrder: 1);

        var earlierDrone = CreateDrone(
            id: "DRONE-EARLIER",
            capacityKg: 20,
            rangeKm: 40,
            inputOrder: 0);

        var order = CreateOrder(
            weightKg: 5,
            x: 3,
            y: 4);

        var result = _planner.Plan(
            new[]
            {
            laterDrone,
            earlierDrone
            },
            new[] { order });

        var trip = Assert.Single(result.Trips);

        Assert.Same(earlierDrone, trip.Drone);
    }
    [Fact]
    public void Plan_WhenHighestCapacityDroneIsIncompatible_ShouldSelectCompatibleDrone()
    {
        var incompatibleDrone = CreateDrone(
            id: "DRONE-INCOMPATIBLE",
            capacityKg: 100,
            rangeKm: 9,
            inputOrder: 0);

        var compatibleDrone = CreateDrone(
            id: "DRONE-COMPATIBLE",
            capacityKg: 10,
            rangeKm: 20,
            inputOrder: 1);

        var order = CreateOrder(
            weightKg: 5,
            x: 3,
            y: 4);

        var result = _planner.Plan(
            new[]
            {
            incompatibleDrone,
            compatibleDrone
            },
            new[] { order });

        var trip = Assert.Single(result.Trips);

        Assert.Same(compatibleDrone, trip.Drone);
    }
    [Fact]
    public void Plan_WhenMultipleFeasibleOrdersFitOneTrip_ShouldCreateSingleTrip()
    {
        var drone = CreateDrone(
            capacityKg: 20,
            rangeKm: 100);

        var firstOrder = CreateOrder(
            id: "ORDER-FIRST",
            weightKg: 5,
            x: 1,
            y: 0,
            priority: Priority.High,
            inputOrder: 0);

        var secondOrder = CreateOrder(
            id: "ORDER-SECOND",
            weightKg: 4,
            x: 3,
            y: 0,
            priority: Priority.Medium,
            inputOrder: 1);

        var thirdOrder = CreateOrder(
            id: "ORDER-THIRD",
            weightKg: 3,
            x: 0,
            y: 4,
            priority: Priority.Low,
            inputOrder: 2);

        var result = _planner.Plan(
            new[] { drone },
            new[]
            {
            thirdOrder,
            secondOrder,
            firstOrder
            });

        var trip = Assert.Single(result.Trips);

        Assert.Empty(result.ImpossibleOrders);
        Assert.Same(drone, trip.Drone);

        Assert.Equal(3, trip.Orders.Count);
        Assert.Equal(12, trip.TotalWeightKg);

        Assert.Same(firstOrder, trip.Orders[0]);
        Assert.Same(secondOrder, trip.Orders[1]);
        Assert.Same(thirdOrder, trip.Orders[2]);

        Assert.Equal(12, trip.TotalDistanceKm, 10);
    }
    [Fact]
    public void Plan_WhenFeasibleOrdersFitOneTripAndThereIsAnImpossibleOrder_ShouldReturnBoth()
    {
        var drone = CreateDrone(
            capacityKg: 20,
            rangeKm: 100);

        var firstFeasibleOrder = CreateOrder(
            id: "ORDER-FEASIBLE-01",
            weightKg: 5,
            x: 1,
            y: 0,
            priority: Priority.High,
            inputOrder: 0);

        var secondFeasibleOrder = CreateOrder(
            id: "ORDER-FEASIBLE-02",
            weightKg: 4,
            x: 3,
            y: 0,
            priority: Priority.Medium,
            inputOrder: 1);

        var impossibleOrder = CreateOrder(
            id: "ORDER-IMPOSSIBLE",
            weightKg: 25,
            x: 1,
            y: 1,
            priority: Priority.Low,
            inputOrder: 2);

        var result = _planner.Plan(
            new[] { drone },
            new[]
            {
            impossibleOrder,
            secondFeasibleOrder,
            firstFeasibleOrder
            });

        var trip = Assert.Single(result.Trips);

        Assert.Equal(2, trip.Orders.Count);
        Assert.Contains(firstFeasibleOrder, trip.Orders);
        Assert.Contains(secondFeasibleOrder, trip.Orders);

        var impossibleResult = Assert.Single(
            result.ImpossibleOrders);

        Assert.Same(
            impossibleOrder,
            impossibleResult.Order);

        Assert.Equal(
            ImpossibleReason.WeightExceeded,
            impossibleResult.Reason);
    }
    [Fact]
    public void Plan_WhenSelectingFirstOrder_ShouldPreferHighestPriority()
    {
        var highCapacityShortRangeDrone = CreateDrone(
            id: "DRONE-SHORT-RANGE",
            capacityKg: 20,
            rangeKm: 20,
            inputOrder: 0);

        var lowerCapacityLongRangeDrone = CreateDrone(
            id: "DRONE-LONG-RANGE",
            capacityKg: 15,
            rangeKm: 100,
            inputOrder: 1);

        var highPriorityFarOrder = CreateOrder(
            id: "ORDER-HIGH",
            weightKg: 5,
            x: 20,
            y: 0,
            priority: Priority.High,
            inputOrder: 1);

        var lowPriorityNearOrder = CreateOrder(
            id: "ORDER-LOW",
            weightKg: 5,
            x: 1,
            y: 0,
            priority: Priority.Low,
            inputOrder: 0);

        var result = _planner.Plan(
            new[]
            {
            highCapacityShortRangeDrone,
            lowerCapacityLongRangeDrone
            },
            new[]
            {
            lowPriorityNearOrder,
            highPriorityFarOrder
            });

        var trip = Assert.Single(result.Trips);

        Assert.Same(
            lowerCapacityLongRangeDrone,
            trip.Drone);

        Assert.Equal(2, trip.Orders.Count);
    }
    [Fact]
    public void Plan_WhenPrioritiesAreEqual_ShouldSelectHeaviestOrderFirst()
    {
        var highCapacityShortRangeDrone = CreateDrone(
            id: "DRONE-SHORT-RANGE",
            capacityKg: 20,
            rangeKm: 20,
            inputOrder: 0);

        var lowerCapacityLongRangeDrone = CreateDrone(
            id: "DRONE-LONG-RANGE",
            capacityKg: 15,
            rangeKm: 100,
            inputOrder: 1);

        var heavierFarOrder = CreateOrder(
            id: "ORDER-HEAVIER",
            weightKg: 8,
            x: 20,
            y: 0,
            priority: Priority.High,
            inputOrder: 1);

        var lighterNearOrder = CreateOrder(
            id: "ORDER-LIGHTER",
            weightKg: 5,
            x: 1,
            y: 0,
            priority: Priority.High,
            inputOrder: 0);

        var result = _planner.Plan(
            new[]
            {
            highCapacityShortRangeDrone,
            lowerCapacityLongRangeDrone
            },
            new[]
            {
            lighterNearOrder,
            heavierFarOrder
            });

        var trip = Assert.Single(result.Trips);

        Assert.Same(
            lowerCapacityLongRangeDrone,
            trip.Drone);

        Assert.Equal(13, trip.TotalWeightKg);
    }
    [Fact]
    public void Plan_WhenPriorityAndWeightAreEqual_ShouldSelectLowestInputOrderFirst()
    {
        var highCapacityShortRangeDrone = CreateDrone(
            id: "DRONE-SHORT-RANGE",
            capacityKg: 20,
            rangeKm: 20,
            inputOrder: 0);

        var lowerCapacityLongRangeDrone = CreateDrone(
            id: "DRONE-LONG-RANGE",
            capacityKg: 15,
            rangeKm: 100,
            inputOrder: 1);

        var earlierFarOrder = CreateOrder(
            id: "ORDER-EARLIER",
            weightKg: 5,
            x: 20,
            y: 0,
            priority: Priority.High,
            inputOrder: 0);

        var laterNearOrder = CreateOrder(
            id: "ORDER-LATER",
            weightKg: 5,
            x: 1,
            y: 0,
            priority: Priority.High,
            inputOrder: 1);

        var result = _planner.Plan(
            new[]
            {
            highCapacityShortRangeDrone,
            lowerCapacityLongRangeDrone
            },
            new[]
            {
            laterNearOrder,
            earlierFarOrder
            });

        var trip = Assert.Single(result.Trips);

        Assert.Same(
            lowerCapacityLongRangeDrone,
            trip.Drone);

        Assert.Equal(2, trip.Orders.Count);
    }
    [Fact]
    public void Plan_WhenAnOrderDoesNotFitFirstTripByWeight_ShouldThrowNotSupportedException()
    {
        var drone = CreateDrone(
            capacityKg: 10,
            rangeKm: 100);

        var firstOrder = CreateOrder(
            id: "ORDER-01",
            weightKg: 6,
            x: 1,
            y: 0,
            priority: Priority.High,
            inputOrder: 0);

        var remainingOrder = CreateOrder(
            id: "ORDER-02",
            weightKg: 5,
            x: 2,
            y: 0,
            priority: Priority.Medium,
            inputOrder: 1);

        Assert.Throws<NotSupportedException>(
            () => _planner.Plan(
                new[] { drone },
                new[]
                {
                firstOrder,
                remainingOrder
                }));
    }
    [Fact]
    public void Plan_WhenAnOrderDoesNotFitFirstTripByRange_ShouldThrowNotSupportedException()
    {
        var drone = CreateDrone(
            capacityKg: 20,
            rangeKm: 22);

        var firstOrder = CreateOrder(
            id: "ORDER-01",
            weightKg: 5,
            x: 10,
            y: 0,
            priority: Priority.High,
            inputOrder: 0);

        var remainingOrder = CreateOrder(
            id: "ORDER-02",
            weightKg: 5,
            x: -10,
            y: 0,
            priority: Priority.Medium,
            inputOrder: 1);

        Assert.Throws<NotSupportedException>(
            () => _planner.Plan(
                new[] { drone },
                new[]
                {
                firstOrder,
                remainingOrder
                }));
    }
}