using DroneDelivery.Domain.ValueObjects;

namespace DroneDelivery.Domain.Tests.ValueObjects;

public class CoordinateTests
{
    [Fact]
    public void Constructor_WithPositiveValues_ShouldCreateCoordinate()
    {
        var coordinate = new Coordinate(10.5, 20.5);

        Assert.Equal(10.5, coordinate.X);
        Assert.Equal(20.5, coordinate.Y);
    }

    [Fact]
    public void Constructor_WithNegativeValues_ShouldCreateCoordinate()
    {
        var coordinate = new Coordinate(-10.5, -20.5);

        Assert.Equal(-10.5, coordinate.X);
        Assert.Equal(-20.5, coordinate.Y);
    }

    [Fact]
    public void Constructor_WithZeroValues_ShouldCreateOriginCoordinate()
    {
        var coordinate = new Coordinate(0, 0);

        Assert.Equal(0, coordinate.X);
        Assert.Equal(0, coordinate.Y);
    }

    [Fact]
    public void Coordinates_WithSameValues_ShouldBeEqual()
    {
        var firstCoordinate = new Coordinate(10.5, -20.5);
        var secondCoordinate = new Coordinate(10.5, -20.5);

        Assert.Equal(firstCoordinate, secondCoordinate);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void Constructor_WithNonFiniteX_ShouldThrowArgumentOutOfRangeException(
        double invalidX)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => new Coordinate(invalidX, 10));

        Assert.Equal("x", exception.ParamName);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void Constructor_WithNonFiniteY_ShouldThrowArgumentOutOfRangeException(
        double invalidY)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => new Coordinate(10, invalidY));

        Assert.Equal("y", exception.ParamName);
    }
}