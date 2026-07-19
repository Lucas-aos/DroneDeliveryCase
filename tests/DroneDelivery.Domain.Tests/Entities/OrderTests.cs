using DroneDelivery.Domain.Entities;
using DroneDelivery.Domain.Enums;
using DroneDelivery.Domain.ValueObjects;

namespace DroneDelivery.Domain.Tests.Entities;

public class OrderTests
{
    [Fact]
    public void Constructor_WithValidArguments_ShouldCreateOrder()
    {
        var destination = new Coordinate(10.5, -3.2);

        var order = new Order(
            id: "ORDER-01",
            weightKg: 4.5,
            destination: destination,
            priority: Priority.High,
            inputOrder: 0);

        Assert.Equal("ORDER-01", order.Id);
        Assert.Equal(4.5, order.WeightKg);
        Assert.Equal(destination, order.Destination);
        Assert.Equal(Priority.High, order.Priority);
        Assert.Equal(0, order.InputOrder);
    }

    [Fact]
    public void Constructor_WithDestinationAtBase_ShouldCreateOrder()
    {
        var order = new Order(
            id: "ORDER-01",
            weightKg: 4.5,
            destination: new Coordinate(0, 0),
            priority: Priority.Medium,
            inputOrder: 0);

        Assert.Equal(new Coordinate(0, 0), order.Destination);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidId_ShouldThrowArgumentException(
        string? invalidId)
    {
        var exception = Assert.Throws<ArgumentException>(
            () => new Order(
                id: invalidId!,
                weightKg: 5,
                destination: new Coordinate(1, 1),
                priority: Priority.High,
                inputOrder: 0));

        Assert.Equal("id", exception.ParamName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void Constructor_WithInvalidWeight_ShouldThrowArgumentOutOfRangeException(
        double invalidWeight)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => new Order(
                id: "ORDER-01",
                weightKg: invalidWeight,
                destination: new Coordinate(1, 1),
                priority: Priority.High,
                inputOrder: 0));

        Assert.Equal("weightKg", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithInvalidPriority_ShouldThrowArgumentOutOfRangeException()
    {
        var invalidPriority = (Priority)999;

        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => new Order(
                id: "ORDER-01",
                weightKg: 5,
                destination: new Coordinate(1, 1),
                priority: invalidPriority,
                inputOrder: 0));

        Assert.Equal("priority", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNegativeInputOrder_ShouldThrowArgumentOutOfRangeException()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => new Order(
                id: "ORDER-01",
                weightKg: 5,
                destination: new Coordinate(1, 1),
                priority: Priority.Low,
                inputOrder: -1));

        Assert.Equal("inputOrder", exception.ParamName);
    }
}