using DroneDelivery.Domain.Entities;
using DroneDelivery.Domain.Enums;
using DroneDelivery.Domain.Planning;
using DroneDelivery.Domain.ValueObjects;

namespace DroneDelivery.Domain.Tests.Planning;

public class RouteCalculationResultTests
{
    private static Order CreateOrder(
        string id = "ORDER-01",
        int inputOrder = 0)
        => new(
            id,
            weightKg: 5,
            destination: new Coordinate(1, 1),
            priority: Priority.High,
            inputOrder);

    [Fact]
    public void Constructor_WithValidArguments_ShouldCreateResult()
    {
        var order = CreateOrder();

        var result = new RouteCalculationResult(
            new[] { order },
            totalDistanceKm: 10.5);

        Assert.Single(result.OrderedOrders);
        Assert.Same(order, result.OrderedOrders[0]);
        Assert.Equal(10.5, result.TotalDistanceKm);
    }

    [Fact]
    public void Constructor_WithEmptyCollectionAndZeroDistance_ShouldCreateResult()
    {
        var result = new RouteCalculationResult(
            Array.Empty<Order>(),
            totalDistanceKm: 0);

        Assert.Empty(result.OrderedOrders);
        Assert.Equal(0, result.TotalDistanceKm);
    }

    [Fact]
    public void Constructor_WithNullOrders_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new RouteCalculationResult(
                null!,
                totalDistanceKm: 0));

        Assert.Equal("orderedOrders", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullItem_ShouldThrowArgumentException()
    {
        var orders = new Order[]
        {
            CreateOrder(),
            null!
        };

        var exception = Assert.Throws<ArgumentException>(
            () => new RouteCalculationResult(
                orders,
                totalDistanceKm: 10));

        Assert.Equal("orderedOrders", exception.ParamName);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void Constructor_WithInvalidDistance_ShouldThrowArgumentOutOfRangeException(
        double invalidDistance)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => new RouteCalculationResult(
                new[] { CreateOrder() },
                invalidDistance));

        Assert.Equal("totalDistanceKm", exception.ParamName);
    }

    [Fact]
    public void Constructor_ShouldCreateInternalCopyOfOrders()
    {
        var orders = new List<Order>
        {
            CreateOrder()
        };

        var result = new RouteCalculationResult(
            orders,
            totalDistanceKm: 10);

        orders.Clear();

        Assert.Single(result.OrderedOrders);
    }

    [Fact]
    public void OrderedOrders_ShouldBeExposedAsReadOnlyList()
    {
        var result = new RouteCalculationResult(
            new[] { CreateOrder() },
            totalDistanceKm: 10);

        Assert.IsAssignableFrom<IReadOnlyList<Order>>(
            result.OrderedOrders);
    }
}