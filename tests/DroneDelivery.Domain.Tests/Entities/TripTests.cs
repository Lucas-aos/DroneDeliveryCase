using DroneDelivery.Domain.Entities;
using DroneDelivery.Domain.Enums;
using DroneDelivery.Domain.ValueObjects;

namespace DroneDelivery.Domain.Tests.Entities;

public class TripTests
{
    private static Drone CreateDrone() =>
        new(
            "DRONE-01",
            20,
            100,
            0);

    private static Order CreateOrder(
        string id = "ORDER-01")
        => new(
            id,
            5,
            new Coordinate(10, 5),
            Priority.High,
            0);

    [Fact]
    public void Constructor_WithValidArguments_ShouldCreateTrip()
    {
        var drone = CreateDrone();

        var orders = new[]
        {
            CreateOrder()
        };

        var trip = new Trip(
            drone,
            orders,
            5,
            22.5);

        Assert.Same(drone, trip.Drone);
        Assert.Single(trip.Orders);
        Assert.Equal(5, trip.TotalWeightKg);
        Assert.Equal(22.5, trip.TotalDistanceKm);
    }

    [Fact]
    public void Constructor_WithNullDrone_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new Trip(
                null!,
                new[] { CreateOrder() },
                5,
                10));
    }

    [Fact]
    public void Constructor_WithNullOrders_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new Trip(
                CreateDrone(),
                null!,
                5,
                10));
    }

    [Fact]
    public void Constructor_WithEmptyOrders_ShouldThrowArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new Trip(
                CreateDrone(),
                Enumerable.Empty<Order>(),
                5,
                10));

        Assert.Equal("orders", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithDuplicateOrders_ShouldThrowArgumentException()
    {
        var order = CreateOrder();

        var exception = Assert.Throws<ArgumentException>(() =>
            new Trip(
                CreateDrone(),
                new[]
                {
                    order,
                    new Order(
                        order.Id,
                        5,
                        new Coordinate(20,20),
                        Priority.Low,
                        1)
                },
                10,
                30));

        Assert.Equal("orders", exception.ParamName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void Constructor_WithInvalidTotalWeight_ShouldThrowArgumentOutOfRangeException(
        double weight)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Trip(
                CreateDrone(),
                new[] { CreateOrder() },
                weight,
                10));

        Assert.Equal("totalWeightKg", exception.ParamName);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void Constructor_WithInvalidTotalDistance_ShouldThrowArgumentOutOfRangeException(
        double distance)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Trip(
                CreateDrone(),
                new[] { CreateOrder() },
                5,
                distance));

        Assert.Equal("totalDistanceKm", exception.ParamName);
    }

    [Fact]
    public void Constructor_ShouldCreateInternalCopyOfOrders()
    {
        var list = new List<Order>
        {
            CreateOrder()
        };

        var trip = new Trip(
            CreateDrone(),
            list,
            5,
            10);

        list.Clear();

        Assert.Single(trip.Orders);
    }

    [Fact]
    public void Orders_ShouldBeReadOnly()
    {
        var trip = new Trip(
            CreateDrone(),
            new[] { CreateOrder() },
            5,
            10);

        Assert.IsAssignableFrom<IReadOnlyList<Order>>(trip.Orders);
    }
}