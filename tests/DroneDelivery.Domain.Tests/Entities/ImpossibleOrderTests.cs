using DroneDelivery.Domain.Entities;
using DroneDelivery.Domain.Enums;
using DroneDelivery.Domain.ValueObjects;

namespace DroneDelivery.Domain.Tests.Entities;

public class ImpossibleOrderTests
{
    private static Order CreateOrder() =>
        new(
            "ORDER-01",
            5,
            new Coordinate(10, 20),
            Priority.High,
            0);

    [Fact]
    public void Constructor_WithValidArguments_ShouldCreateImpossibleOrder()
    {
        var order = CreateOrder();

        var impossibleOrder = new ImpossibleOrder(
            order,
            ImpossibleReason.WeightExceeded);

        Assert.Same(order, impossibleOrder.Order);
        Assert.Equal(
            ImpossibleReason.WeightExceeded,
            impossibleOrder.Reason);
    }

    [Fact]
    public void Constructor_WithNullOrder_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ImpossibleOrder(
                null!,
                ImpossibleReason.RangeExceeded));
    }

    [Fact]
    public void Constructor_WithInvalidReason_ShouldThrowArgumentOutOfRangeException()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ImpossibleOrder(
                CreateOrder(),
                (ImpossibleReason)999));

        Assert.Equal("reason", exception.ParamName);
    }

    [Theory]
    [InlineData(ImpossibleReason.WeightExceeded)]
    [InlineData(ImpossibleReason.RangeExceeded)]
    [InlineData(ImpossibleReason.WeightAndRangeExceeded)]
    public void Constructor_WithAllValidReasons_ShouldCreateImpossibleOrder(
        ImpossibleReason reason)
    {
        var impossibleOrder = new ImpossibleOrder(
            CreateOrder(),
            reason);

        Assert.Equal(reason, impossibleOrder.Reason);
    }
}