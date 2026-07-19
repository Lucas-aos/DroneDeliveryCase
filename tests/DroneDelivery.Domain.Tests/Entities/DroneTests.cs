using DroneDelivery.Domain.Entities;

namespace DroneDelivery.Domain.Tests.Entities;

public class DroneTests
{
    [Fact]
    public void Constructor_WithValidArguments_ShouldCreateDrone()
    {
        var drone = new Drone(
            id: "DRONE-01",
            capacityKg: 15.5,
            rangeKm: 120,
            inputOrder: 0);

        Assert.Equal("DRONE-01", drone.Id);
        Assert.Equal(15.5, drone.CapacityKg);
        Assert.Equal(120, drone.RangeKm);
        Assert.Equal(0, drone.InputOrder);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidId_ShouldThrowArgumentException(
        string? invalidId)
    {
        var exception = Assert.Throws<ArgumentException>(
            () => new Drone(
                invalidId!,
                capacityKg: 10,
                rangeKm: 100,
                inputOrder: 0));

        Assert.Equal("id", exception.ParamName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void Constructor_WithInvalidCapacity_ShouldThrowArgumentOutOfRangeException(
        double invalidCapacity)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => new Drone(
                id: "DRONE-01",
                capacityKg: invalidCapacity,
                rangeKm: 100,
                inputOrder: 0));

        Assert.Equal("capacityKg", exception.ParamName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void Constructor_WithInvalidRange_ShouldThrowArgumentOutOfRangeException(
        double invalidRange)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => new Drone(
                id: "DRONE-01",
                capacityKg: 10,
                rangeKm: invalidRange,
                inputOrder: 0));

        Assert.Equal("rangeKm", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNegativeInputOrder_ShouldThrowArgumentOutOfRangeException()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => new Drone(
                id: "DRONE-01",
                capacityKg: 10,
                rangeKm: 100,
                inputOrder: -1));

        Assert.Equal("inputOrder", exception.ParamName);
    }
}