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
    [Fact]
    public async Task GetRoutes_WithExistingPlanning_ReturnsRoutes()
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

        var createResponse = await _client.PostAsJsonAsync(
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
            $"/api/planning/{created!.PlanningId}/routes");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var routes =
            await response.Content
                .ReadFromJsonAsync<List<RouteResponse>>();

        Assert.NotNull(routes);
        Assert.Single(routes);

        var route = routes[0];

        Assert.Equal(1, route.TripSequence);
        Assert.Equal("DRONE-01", route.DroneId);
        Assert.Equal(2, route.TotalWeightKg);
        Assert.Equal(10, route.TotalDistanceKm);

        Assert.Single(route.Stops);

        var stop = route.Stops[0];

        Assert.Equal(1, stop.Sequence);
        Assert.Equal("ORDER-01", stop.OrderId);
        Assert.Equal(3, stop.X);
        Assert.Equal(4, stop.Y);
    }
    [Fact]
    public async Task GetRoutes_WithUnknownPlanningId_ReturnsNotFound()
    {
        // Arrange
        var planningId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync(
            $"/api/planning/{planningId}/routes");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
    [Fact]
    public async Task GetDrones_WithExistingPlanning_ReturnsDroneSummaries()
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
            },
            new
            {
                id = "DRONE-02",
                capacityKg = 20,
                rangeKm = 50
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

        var createResponse = await _client.PostAsJsonAsync(
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
            $"/api/planning/{created!.PlanningId}/drones");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var drones =
            await response.Content
                .ReadFromJsonAsync<List<DroneSummaryResponse>>();

        Assert.NotNull(drones);
        Assert.Equal(2, drones.Count);

        var usedDrone = Assert.Single(
        drones,
        drone => drone.WasUsed);

        Assert.Equal(1, usedDrone.TripCount);
        Assert.Equal(1, usedDrone.DeliveredOrders);
        Assert.Equal(2, usedDrone.TotalDeliveredWeightKg);
        Assert.Equal(10, usedDrone.TotalDistanceKm);

        var unusedDrone = Assert.Single(
            drones,
            drone => !drone.WasUsed);

        Assert.Equal(0, unusedDrone.TripCount);
        Assert.Equal(0, unusedDrone.DeliveredOrders);
        Assert.Equal(0, unusedDrone.TotalDeliveredWeightKg);
        Assert.Equal(0, unusedDrone.TotalDistanceKm);
    }
    [Fact]
    public async Task GetDrones_WithUnknownPlanningId_ReturnsNotFound()
    {
        // Arrange
        var planningId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync(
            $"/api/planning/{planningId}/drones");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
    [Fact]
    public async Task GetFleetAnalysis_WithExistingPlanning_ReturnsAnalysis()
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
            },
            new
            {
                id = "DRONE-02",
                capacityKg = 20,
                rangeKm = 50
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
            $"/api/planning/{created!.PlanningId}/fleet-analysis");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var analysis =
            await response.Content
                .ReadFromJsonAsync<FleetAnalysisResponse>();

        Assert.NotNull(analysis);

        Assert.Equal(2, analysis!.TotalDrones);
        Assert.Equal(1, analysis.UsedDrones);
        Assert.Equal(1, analysis.TotalTrips);
        Assert.Equal(1, analysis.DeliveredOrders);
        Assert.Equal(0, analysis.ImpossibleOrders);

        Assert.Equal(2, analysis.TotalDeliveredWeightKg);
        Assert.Equal(10, analysis.TotalDistanceKm);

        Assert.Equal(
            50,
            analysis.FleetParticipationPercentage);

        Assert.Equal(
            0.2,
            analysis.FleetEfficiencyKgPerKm);

        Assert.Equal(
            15,
            analysis.EstimatedTotalTimeMinutes);

        Assert.Equal(2, analysis.Drones.Count);
        Assert.NotEmpty(analysis.Recommendations);
    }
    [Fact]
    public async Task GetFleetAnalysis_ReturnsUsedDroneMetrics()
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
                rangeKm = 20
            }
        },
            orders = new[]
            {
            new
            {
                id = "ORDER-01",
                weightKg = 5,
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

        var created =
            await createResponse.Content
                .ReadFromJsonAsync<PlanningCreatedResponse>();

        Assert.NotNull(created);

        // Act
        var response = await _client.GetAsync(
            $"/api/planning/{created!.PlanningId}/fleet-analysis");

        // Assert
        var analysis =
            await response.Content
                .ReadFromJsonAsync<FleetAnalysisResponse>();

        Assert.NotNull(analysis);

        var drone = Assert.Single(
            analysis!.Drones);

        Assert.Equal("DRONE-01", drone.DroneId);
        Assert.True(drone.WasUsed);
        Assert.Equal(1, drone.TripCount);
        Assert.Equal(1, drone.DeliveredOrders);
        Assert.Equal(5, drone.DeliveredWeightKg);
        Assert.Equal(10, drone.DistanceKm);

        Assert.Equal(
            0.5,
            drone.EfficiencyKgPerKm);

        Assert.Equal(
            50,
            drone.AverageLoadFactorPercentage);

        Assert.Equal(
            50,
            drone.AverageBatteryUsagePerTripPercentage);

        Assert.Equal(
            50,
            drone.MaximumBatteryUsagePerTripPercentage);

        Assert.Equal(
            15,
            drone.EstimatedTimeMinutes);
    }
    [Fact]
    public async Task GetFleetAnalysis_WithImpossibleOrder_ReturnsDroneSuggestion()
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
                rangeKm = 8
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

        var createResponse =
            await _client.PostAsJsonAsync(
                "/api/planning",
                request);

        var created =
            await createResponse.Content
                .ReadFromJsonAsync<PlanningCreatedResponse>();

        Assert.NotNull(created);

        // Act
        var response = await _client.GetAsync(
            $"/api/planning/{created!.PlanningId}/fleet-analysis");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var analysis =
            await response.Content
                .ReadFromJsonAsync<FleetAnalysisResponse>();

        Assert.NotNull(analysis);
        Assert.Equal(1, analysis!.ImpossibleOrders);
        Assert.Equal(0, analysis.TotalTrips);

        var recommendation = Assert.Single(
            analysis.Recommendations,
            item => item.Title ==
                "Impossible Orders Detected");

        Assert.Equal(
            "ImpossibleOrders",
            recommendation.Type);

        Assert.Equal(
            "Critical",
            recommendation.Severity);

        Assert.Equal(
            10,
            recommendation.SuggestedMinimumCapacityKg);

        Assert.Equal(
            10,
            recommendation.SuggestedMinimumRangeKm);
    }
    [Fact]
    public async Task GetFleetAnalysis_WithUnknownPlanningId_ReturnsNotFound()
    {
        // Arrange
        var planningId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync(
            $"/api/planning/{planningId}/fleet-analysis");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}