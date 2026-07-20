using System.Net;
using System.Net.Http.Json;
using DroneDelivery.Api.Contracts.Responses;
using Microsoft.AspNetCore.Mvc.Testing;

public sealed class PlanningControllerTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PlanningControllerTests(
        WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Plan_WithValidRequest_ReturnsCreated()
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
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);

        var created =
            await response.Content
                .ReadFromJsonAsync<PlanningCreatedResponse>();

        Assert.NotNull(created);
        Assert.NotEqual(Guid.Empty, created!.PlanningId);
        Assert.True(created.CreatedAtUtc <= DateTimeOffset.UtcNow);

        Assert.EndsWith(
            $"/api/planning/{created.PlanningId}",
            response.Headers.Location!.AbsoluteUri);

        var planning = created.Planning;

        Assert.Single(planning.Trips);
        Assert.Empty(planning.ImpossibleOrders);

        var trip = planning.Trips[0];

        Assert.Equal("DRONE-01", trip.DroneId);
        Assert.Single(trip.Orders);
        Assert.Equal("ORDER-01", trip.Orders[0].Id);
        Assert.Equal(1, trip.Orders[0].Sequence);
        Assert.Equal(2, trip.TotalWeightKg);
        Assert.Equal(10, trip.TotalDistanceKm);
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
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);

        var error =
            await response.Content
                .ReadFromJsonAsync<ErrorResponse>();

        Assert.NotNull(error);
        Assert.Equal(
            "The request is invalid.",
            error!.Message);

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
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);

        var error =
            await response.Content
                .ReadFromJsonAsync<ErrorResponse>();

        Assert.NotNull(error);
        Assert.Equal(
            "The request is invalid.",
            error!.Message);

        Assert.Single(error.Errors);

        Assert.Contains(
            "weight",
            error.Errors[0],
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Plan_WithImpossibleOrder_ReturnsCreatedWithImpossibleOrder()
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
        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var created =
            await response.Content
                .ReadFromJsonAsync<PlanningCreatedResponse>();

        Assert.NotNull(created);

        var planning = created!.Planning;

        Assert.Empty(planning.Trips);
        Assert.Single(planning.ImpossibleOrders);

        var impossibleOrder =
            planning.ImpossibleOrders[0];

        Assert.Equal(
            "ORDER-01",
            impossibleOrder.OrderId);

        Assert.Equal(
            "WeightExceeded",
            impossibleOrder.Reason);
    }

    [Fact]
    public async Task GetPlanning_WithExistingPlanningId_ReturnsPlanning()
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

        var createResponse =
            await _client.PostAsJsonAsync(
                "/api/planning",
                request);

        Assert.Equal(
            HttpStatusCode.Created,
            createResponse.StatusCode);

        var created =
            await createResponse.Content
                .ReadFromJsonAsync<PlanningCreatedResponse>();

        Assert.NotNull(created);

        // Act
        var response = await _client.GetAsync(
            $"/api/planning/{created!.PlanningId}");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var planning =
            await response.Content
                .ReadFromJsonAsync<PlanningResponse>();

        Assert.NotNull(planning);
        Assert.Single(planning!.Trips);
        Assert.Empty(planning.ImpossibleOrders);

        var trip = planning.Trips[0];

        Assert.Equal("DRONE-01", trip.DroneId);
        Assert.Single(trip.Orders);
        Assert.Equal("ORDER-01", trip.Orders[0].Id);
        Assert.Equal(1, trip.Orders[0].Sequence);
        Assert.Equal(2, trip.TotalWeightKg);
        Assert.Equal(10, trip.TotalDistanceKm);
    }

    [Fact]
    public async Task GetPlanning_WithUnknownPlanningId_ReturnsNotFound()
    {
        // Arrange
        var planningId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync(
            $"/api/planning/{planningId}");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}