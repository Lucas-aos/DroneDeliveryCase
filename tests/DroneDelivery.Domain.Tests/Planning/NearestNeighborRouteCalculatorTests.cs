using DroneDelivery.Domain.Calculations;
using DroneDelivery.Domain.Entities;
using DroneDelivery.Domain.Enums;
using DroneDelivery.Domain.Planning;
using DroneDelivery.Domain.ValueObjects;

namespace DroneDelivery.Domain.Tests.Planning;

public class NearestNeighborRouteCalculatorTests
{
    private readonly NearestNeighborRouteCalculator _calculator = new();

    private static Order CreateOrder(
        string id,
        double x,
        double y,
        Priority priority = Priority.Medium,
        double weightKg = 5,
        int inputOrder = 0)
        => new(
            id,
            weightKg,
            new Coordinate(x, y),
            priority,
            inputOrder);

    [Fact]
    public void Calculate_WithNullOrders_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => _calculator.Calculate(null!));

        Assert.Equal("orders", exception.ParamName);
    }

    [Fact]
    public void Calculate_WithNullItem_ShouldThrowArgumentException()
    {
        var orders = new Order[]
        {
            CreateOrder("ORDER-01", 1, 0),
            null!
        };

        var exception = Assert.Throws<ArgumentException>(
            () => _calculator.Calculate(orders));

        Assert.Equal("orders", exception.ParamName);
    }

    [Fact]
    public void Calculate_WithEmptyCollection_ShouldReturnEmptyRouteWithZeroDistance()
    {
        var result = _calculator.Calculate(
            Array.Empty<Order>());

        Assert.Empty(result.OrderedOrders);
        Assert.Equal(0, result.TotalDistanceKm);
    }

    [Fact]
    public void Calculate_WithSingleOrder_ShouldIncludeReturnToBase()
    {
        var order = CreateOrder(
            id: "ORDER-01",
            x: 3,
            y: 4);

        var result = _calculator.Calculate(
            new[] { order });

        Assert.Single(result.OrderedOrders);
        Assert.Same(order, result.OrderedOrders[0]);
        Assert.Equal(
            10,
            result.TotalDistanceKm,
            precision: 10);
    }

    [Fact]
    public void Calculate_ShouldSelectNearestOrderFromBase()
    {
        var nearOrder = CreateOrder(
            id: "NEAR",
            x: 1,
            y: 0,
            inputOrder: 1);

        var farOrder = CreateOrder(
            id: "FAR",
            x: 5,
            y: 0,
            inputOrder: 0);

        var result = _calculator.Calculate(
            new[] { farOrder, nearOrder });

        Assert.Equal(
            new[] { "NEAR", "FAR" },
            result.OrderedOrders.Select(order => order.Id));
    }

    [Fact]
    public void Calculate_ShouldRecalculateNearestNeighborFromCurrentDestination()
    {
        var firstOrder = CreateOrder(
            id: "FIRST",
            x: 1,
            y: 0,
            inputOrder: 0);

        var initiallySecondNearest = CreateOrder(
            id: "SECOND-FROM-BASE",
            x: 0,
            y: 1.5,
            inputOrder: 1);

        var nearestFromFirstDestination = CreateOrder(
            id: "NEAREST-FROM-FIRST",
            x: 2,
            y: 0,
            inputOrder: 2);

        var result = _calculator.Calculate(
            new[]
            {
                initiallySecondNearest,
                nearestFromFirstDestination,
                firstOrder
            });

        Assert.Equal(
            new[]
            {
                "FIRST",
                "NEAREST-FROM-FIRST",
                "SECOND-FROM-BASE"
            },
            result.OrderedOrders.Select(order => order.Id));
    }

    [Fact]
    public void Calculate_ShouldReturnTotalDistanceIncludingAllLegsAndReturnToBase()
    {
        var firstOrder = CreateOrder(
            id: "ORDER-01",
            x: 3,
            y: 0,
            inputOrder: 0);

        var secondOrder = CreateOrder(
            id: "ORDER-02",
            x: 3,
            y: 4,
            inputOrder: 1);

        var result = _calculator.Calculate(
            new[] { firstOrder, secondOrder });

        var baseCoordinate = new Coordinate(0, 0);

        var expectedDistance =
            DistanceCalculator.Calculate(
                baseCoordinate,
                firstOrder.Destination)
            +
            DistanceCalculator.Calculate(
                firstOrder.Destination,
                secondOrder.Destination)
            +
            DistanceCalculator.Calculate(
                secondOrder.Destination,
                baseCoordinate);

        Assert.Equal(
            expectedDistance,
            result.TotalDistanceKm,
            precision: 10);
    }

    [Fact]
    public void Calculate_WithEqualDistance_ShouldPreferHigherPriority()
    {
        var lowPriorityOrder = CreateOrder(
            id: "LOW",
            x: 1,
            y: 0,
            priority: Priority.Low,
            inputOrder: 0);

        var highPriorityOrder = CreateOrder(
            id: "HIGH",
            x: -1,
            y: 0,
            priority: Priority.High,
            inputOrder: 1);

        var result = _calculator.Calculate(
            new[] { lowPriorityOrder, highPriorityOrder });

        Assert.Equal(
            "HIGH",
            result.OrderedOrders[0].Id);
    }

    [Fact]
    public void Calculate_WithEqualDistanceAndPriority_ShouldPreferLowerInputOrder()
    {
        var heavierOrder = CreateOrder(
            id: "HEAVIER",
            x: 1,
            y: 0,
            priority: Priority.High,
            weightKg: 20,
            inputOrder: 1);

        var earlierOrder = CreateOrder(
            id: "EARLIER",
            x: -1,
            y: 0,
            priority: Priority.High,
            weightKg: 5,
            inputOrder: 0);

        var result = _calculator.Calculate(
            new[] { heavierOrder, earlierOrder });

        Assert.Equal(
            "EARLIER",
            result.OrderedOrders[0].Id);
    }

    [Fact]
    public void Calculate_WithEqualDistancePriorityAndInputOrder_ShouldPreferHigherWeight()
    {
        var lighterOrder = CreateOrder(
            id: "LIGHTER",
            x: 1,
            y: 0,
            priority: Priority.Medium,
            weightKg: 5,
            inputOrder: 0);

        var heavierOrder = CreateOrder(
            id: "HEAVIER",
            x: -1,
            y: 0,
            priority: Priority.Medium,
            weightKg: 10,
            inputOrder: 0);

        var result = _calculator.Calculate(
            new[] { lighterOrder, heavierOrder });

        Assert.Equal(
            "HEAVIER",
            result.OrderedOrders[0].Id);
    }

    [Fact]
    public void Calculate_WithAllPreviousCriteriaEqual_ShouldPreferLowerIdentifier()
    {
        var orderB = CreateOrder(
            id: "ORDER-B",
            x: 1,
            y: 0,
            priority: Priority.Medium,
            weightKg: 5,
            inputOrder: 0);

        var orderA = CreateOrder(
            id: "ORDER-A",
            x: -1,
            y: 0,
            priority: Priority.Medium,
            weightKg: 5,
            inputOrder: 0);

        var result = _calculator.Calculate(
            new[] { orderB, orderA });

        Assert.Equal(
            "ORDER-A",
            result.OrderedOrders[0].Id);
    }

    [Fact]
    public void Calculate_WithDifferentPhysicalCollectionOrder_ShouldProduceSameRoute()
    {
        var firstOrder = CreateOrder(
            id: "ORDER-01",
            x: 1,
            y: 0,
            priority: Priority.High,
            weightKg: 5,
            inputOrder: 0);

        var secondOrder = CreateOrder(
            id: "ORDER-02",
            x: 2,
            y: 0,
            priority: Priority.Medium,
            weightKg: 10,
            inputOrder: 1);

        var thirdOrder = CreateOrder(
            id: "ORDER-03",
            x: 0,
            y: 3,
            priority: Priority.Low,
            weightKg: 15,
            inputOrder: 2);

        var originalCollection = new[]
        {
            firstOrder,
            secondOrder,
            thirdOrder
        };

        var reorderedCollection = new[]
        {
            thirdOrder,
            firstOrder,
            secondOrder
        };

        var originalResult = _calculator.Calculate(
            originalCollection);

        var reorderedResult = _calculator.Calculate(
            reorderedCollection);

        Assert.Equal(
            originalResult.OrderedOrders.Select(order => order.Id),
            reorderedResult.OrderedOrders.Select(order => order.Id));

        Assert.Equal(
            originalResult.TotalDistanceKm,
            reorderedResult.TotalDistanceKm,
            precision: 10);
    }
}