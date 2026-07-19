using DroneDelivery.Domain.Calculations;
using DroneDelivery.Domain.ValueObjects;

namespace DroneDelivery.Domain.Tests.Calculations;

public class DistanceCalculatorTests
{
    [Fact]
    public void Calculate_WithThreeFourFiveTriangle_ShouldReturnFive()
    {
        var origin = new Coordinate(0, 0);
        var destination = new Coordinate(3, 4);

        var distance = DistanceCalculator.Calculate(
            origin,
            destination);

        Assert.Equal(5, distance, precision: 10);
    }

    [Fact]
    public void Calculate_WithSameCoordinates_ShouldReturnZero()
    {
        var coordinate = new Coordinate(10.5, -20.5);

        var distance = DistanceCalculator.Calculate(
            coordinate,
            coordinate);

        Assert.Equal(0, distance, precision: 10);
    }

    [Fact]
    public void Calculate_FromBaseToDestination_ShouldReturnExpectedDistance()
    {
        var baseCoordinate = new Coordinate(0, 0);
        var destination = new Coordinate(6, 8);

        var distance = DistanceCalculator.Calculate(
            baseCoordinate,
            destination);

        Assert.Equal(10, distance, precision: 10);
    }

    [Fact]
    public void Calculate_WhenCoordinatesAreReversed_ShouldReturnSameDistance()
    {
        var firstCoordinate = new Coordinate(2, 3);
        var secondCoordinate = new Coordinate(8, 11);

        var forwardDistance = DistanceCalculator.Calculate(
            firstCoordinate,
            secondCoordinate);

        var reverseDistance = DistanceCalculator.Calculate(
            secondCoordinate,
            firstCoordinate);

        Assert.Equal(
            forwardDistance,
            reverseDistance,
            precision: 10);
    }

    [Fact]
    public void Calculate_WithNegativeCoordinates_ShouldReturnExpectedDistance()
    {
        var origin = new Coordinate(-2, -3);
        var destination = new Coordinate(1, 1);

        var distance = DistanceCalculator.Calculate(
            origin,
            destination);

        Assert.Equal(5, distance, precision: 10);
    }
}