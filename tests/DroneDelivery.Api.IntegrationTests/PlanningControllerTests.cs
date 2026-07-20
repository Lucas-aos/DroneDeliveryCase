using System.Net;
using System.Net.Http.Json;
using DroneDelivery.Api.Contracts.Responses;
using Microsoft.AspNetCore.Mvc.Testing;

public sealed class PlanningControllerTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PlanningControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Plan_WithValidRequest_ReturnsOk()
    {
        // Arrange
        var request = new
        {
            drones = new[]
            {
                new
                {
                    id = "DRONE-01",
                    capacityKg = 10,
                    rangeKm = 30
                }
            },
            orders = new[]
            {
                new
                {
                    id = "ORDER-01",
                    weightKg = 2,
                    priority = "High",
                    x = 3,
                    y = 4
                }
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/planning",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var planningResponse =
            await response.Content.ReadFromJsonAsync<PlanningResponse>();

        Assert.NotNull(planningResponse);

        var trip = planningResponse.Trips[0];

        Assert.Equal("DRONE-01", trip.DroneId);
        Assert.Single(trip.Orders);
        Assert.Equal("ORDER-01", trip.Orders[0].Id);
        Assert.Equal(1, trip.Orders[0].Sequence);
        Assert.Equal(2, trip.TotalWeightKg);
        Assert.Equal(10, trip.TotalDistanceKm);

        Assert.Empty(planningResponse.ImpossibleOrders);
    }
    [Fact]
    public async Task Plan_WithInvalidPriority_ReturnsBadRequest()
    {
        // Arrange
        var request = new
        {
            drones = new[]
            {
            new
            {
                id = "DRONE-01",
                capacityKg = 10,
                rangeKm = 30
            }
        },
            orders = new[]
            {
            new
            {
                id = "ORDER-01",
                weightKg = 2,
                priority = "Urgent",
                x = 3,
                y = 4
            }
        }
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/planning",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var error =
            await response.Content.ReadFromJsonAsync<ErrorResponse>();

        Assert.NotNull(error);

        Assert.Equal("The request is invalid.", error!.Message);

        Assert.Single(error.Errors);

        Assert.Contains(
            "Invalid priority",
            error.Errors[0]);
    }
    [Fact]
    public async Task Plan_WithInvalidWeight_ReturnsBadRequest()
    {
        // Arrange
        var request = new
        {
            drones = new[]
            {
            new
            {
                id = "DRONE-01",
                capacityKg = 10,
                rangeKm = 30
            }
        },
            orders = new[]
            {
            new
            {
                id = "ORDER-01",
                weightKg = 0,
                priority = "High",
                x = 3,
                y = 4
            }
        }
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/planning",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var error =
            await response.Content.ReadFromJsonAsync<ErrorResponse>();

        Assert.NotNull(error);
        Assert.Equal("The request is invalid.", error!.Message);
        Assert.Single(error.Errors);

        Assert.Contains(
            "weight",
            error.Errors[0],
            StringComparison.OrdinalIgnoreCase);
    }
    [Fact]
    public async Task Plan_WithImpossibleOrder_ReturnsOkWithImpossibleOrder()
    {
        // Arrange
        var request = new
        {
            drones = new[]
            {
            new
            {
                id = "DRONE-01",
                capacityKg = 5,
                rangeKm = 30
            }
        },
            orders = new[]
            {
            new
            {
                id = "ORDER-01",
                weightKg = 10,
                priority = "High",
                x = 3,
                y = 4
            }
        }
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/planning",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var planningResponse =
            await response.Content.ReadFromJsonAsync<PlanningResponse>();

        Assert.NotNull(planningResponse);
        Assert.Empty(planningResponse!.Trips);
        Assert.Single(planningResponse.ImpossibleOrders);

        var impossibleOrder = planningResponse.ImpossibleOrders[0];

        Assert.Equal("ORDER-01", impossibleOrder.OrderId);
        Assert.Equal("WeightExceeded", impossibleOrder.Reason);
    }
}