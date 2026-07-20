using DroneDelivery.Api.Configuration;
using DroneDelivery.Api.Models;
using DroneDelivery.Api.Storage;
using Microsoft.Extensions.Options;

namespace DroneDelivery.Api.Analysis;

public sealed class FleetAdvisor
{
    private readonly FleetAnalysisOptions _options;

    public FleetAdvisor(
        IOptions<FleetAnalysisOptions> options)
    {
        _options = options.Value;

        if (_options.DroneSpeedKmPerHour <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(FleetAnalysisOptions.DroneSpeedKmPerHour),
                _options.DroneSpeedKmPerHour,
                "Drone speed must be greater than zero.");
        }
    }

    public FleetAnalysis Analyze(
        StoredPlanningScenario scenario)
    {
        ArgumentNullException.ThrowIfNull(scenario);

        var trips = scenario.Result.Trips;
        var drones = scenario.Drones;

        var tripsByDrone = trips
            .GroupBy(
                trip => trip.Drone.Id,
                StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                group => group.Key,
                group => group.ToArray(),
                StringComparer.OrdinalIgnoreCase);

        var droneAnalyses = drones
            .Select(drone =>
            {
                tripsByDrone.TryGetValue(
                    drone.Id,
                    out var droneTrips);

                droneTrips ??= [];

                var tripCount = droneTrips.Length;

                var deliveredOrders = droneTrips.Sum(
                    trip => trip.Orders.Count);

                var deliveredWeight = droneTrips.Sum(
                    trip => Convert.ToDouble(
                        trip.TotalWeightKg));

                var distance = droneTrips.Sum(
                    trip => Convert.ToDouble(
                        trip.TotalDistanceKm));

                var efficiency = CalculateRatioSafe(
                    deliveredWeight,
                    distance);

                var averageLoadFactor = tripCount == 0
                    ? 0
                    : droneTrips.Average(
                        trip => CalculatePercentageSafe(
                            Convert.ToDouble(
                                trip.TotalWeightKg),
                            Convert.ToDouble(
                                drone.CapacityKg)));

                var batteryUsageValues = droneTrips
                    .Select(
                        trip => CalculatePercentageSafe(
                            Convert.ToDouble(
                                trip.TotalDistanceKm),
                            Convert.ToDouble(
                                drone.RangeKm)))
                    .ToArray();

                var averageBatteryUsage =
                    batteryUsageValues.Length == 0
                        ? 0
                        : batteryUsageValues.Average();

                var maximumBatteryUsage =
                    batteryUsageValues.Length == 0
                        ? 0
                        : batteryUsageValues.Max();

                var estimatedTimeMinutes =
                    CalculateEstimatedTimeMinutes(distance);

                return new DroneAnalysis
                {
                    DroneId = drone.Id,
                    WasUsed = tripCount > 0,
                    TripCount = tripCount,
                    DeliveredOrders = deliveredOrders,
                    DeliveredWeightKg = Round(deliveredWeight),
                    DistanceKm = Round(distance),
                    EfficiencyKgPerKm = Round(efficiency),
                    AverageLoadFactorPercentage =
                        Round(averageLoadFactor),
                    AverageBatteryUsagePerTripPercentage =
                        Round(averageBatteryUsage),
                    MaximumBatteryUsagePerTripPercentage =
                        Round(maximumBatteryUsage),
                    EstimatedTimeMinutes =
                        Round(estimatedTimeMinutes)
                };
            })
            .ToArray();

        var totalDrones = drones.Count;

        var usedDrones = droneAnalyses.Count(
            drone => drone.WasUsed);

        var totalTrips = trips.Count;

        var deliveredOrders = trips.Sum(
            trip => trip.Orders.Count);

        var impossibleOrders =
            scenario.Result.ImpossibleOrders.Count;

        var totalDeliveredWeight = trips.Sum(
            trip => Convert.ToDouble(
                trip.TotalWeightKg));

        var totalDistance = trips.Sum(
            trip => Convert.ToDouble(
                trip.TotalDistanceKm));

        var fleetParticipation =
            CalculatePercentageSafe(
                usedDrones,
                totalDrones);

        var allTripLoadFactors = trips
            .Select(trip =>
            {
                var drone = drones.First(
                    candidate => string.Equals(
                        candidate.Id,
                        trip.Drone.Id,
                        StringComparison.OrdinalIgnoreCase));

                return CalculatePercentageSafe(
                    Convert.ToDouble(
                        trip.TotalWeightKg),
                    Convert.ToDouble(
                        drone.CapacityKg));
            })
            .ToArray();

        var averageLoadFactor =
            allTripLoadFactors.Length == 0
                ? 0
                : allTripLoadFactors.Average();

        var fleetEfficiency =
             CalculateRatioSafe(
                 totalDeliveredWeight,
                 totalDistance);

        var estimatedTotalTime =
            CalculateEstimatedTimeMinutes(
                totalDistance);

        var recommendations = BuildRecommendations(
            scenario,
            droneAnalyses,
            fleetParticipation,
            averageLoadFactor);

        return new FleetAnalysis
        {
            TotalDrones = totalDrones,
            UsedDrones = usedDrones,
            TotalTrips = totalTrips,
            DeliveredOrders = deliveredOrders,
            ImpossibleOrders = impossibleOrders,
            TotalDeliveredWeightKg =
                Round(totalDeliveredWeight),
            TotalDistanceKm = Round(totalDistance),
            FleetParticipationPercentage =
                Round(fleetParticipation),
            AverageLoadFactorPercentage =
                Round(averageLoadFactor),
            FleetEfficiencyKgPerKm =
                Round(fleetEfficiency),
            EstimatedTotalTimeMinutes =
                Round(estimatedTotalTime),
            Drones = droneAnalyses,
            Recommendations = recommendations
        };
    }

    private IReadOnlyList<FleetRecommendation>
        BuildRecommendations(
            StoredPlanningScenario scenario,
            IReadOnlyList<DroneAnalysis> droneAnalyses,
            double fleetParticipation,
            double averageLoadFactor)
    {
        var recommendations =
            new List<FleetRecommendation>();

        if (scenario.Result.ImpossibleOrders.Count > 0)
        {
            recommendations.Add(
                CreateImpossibleOrdersRecommendation(
                    scenario));
        }

        var hasTripNearRangeLimit =
            droneAnalyses.Any(
                drone =>
                    drone.MaximumBatteryUsagePerTripPercentage
                    >= _options
                        .HighRangeUsageThresholdPercentage);

        if (averageLoadFactor
                >= _options
                    .HighAverageLoadThresholdPercentage
            || hasTripNearRangeLimit)
        {
            recommendations.Add(
                new FleetRecommendation
                {
                    Type = "Warning",
                    Title = "Fleet Near Capacity",
                    Description =
                        "The fleet has trips with high load or range usage. " +
                        "Consider reviewing drone capacity and operational range."
                });
        }

        if (scenario.Drones.Count > 0
            && fleetParticipation
                < _options
                    .UnderutilizedFleetThresholdPercentage)
        {
            recommendations.Add(
                new FleetRecommendation
                {
                    Type = "Information",
                    Title = "Fleet Underutilized",
                    Description =
                        "Only a small portion of the available fleet participated " +
                        "in this planning scenario."
                });
        }

        if (recommendations.Count == 0)
        {
            recommendations.Add(
                new FleetRecommendation
                {
                    Type = "Success",
                    Title = "Fleet Well Balanced",
                    Description =
                        "No relevant capacity, range or participation issues " +
                        "were identified in this planning scenario."
                });
        }

        return recommendations;
    }

    private static FleetRecommendation
        CreateImpossibleOrdersRecommendation(
            StoredPlanningScenario scenario)
    {
        var impossibleOrderIds =
            scenario.Result.ImpossibleOrders
                .Select(order => order.Order.Id)
                .ToHashSet(
                    StringComparer.OrdinalIgnoreCase);

        var impossibleOrders = scenario.Orders
            .Where(order =>
                impossibleOrderIds.Contains(order.Id))
            .ToArray();

        var minimumCapacity =
            impossibleOrders.Length == 0
                ? 0
                : impossibleOrders.Max(
                    order => Convert.ToDouble(
                        order.WeightKg));

        var minimumRange =
            impossibleOrders.Length == 0
                ? 0
                : impossibleOrders.Max(
                    order =>
                    {
                        var x = Convert.ToDouble(order.Destination.X);
                        var y = Convert.ToDouble(order.Destination.Y);

                        var oneWayDistance =
                            Math.Sqrt((x * x) + (y * y));

                        return oneWayDistance * 2;
                    });

        return new FleetRecommendation
        {
            Type = "Critical",
            Title = "Impossible Orders Detected",
            Description =
                $"{scenario.Result.ImpossibleOrders.Count} order(s) " +
                "could not be assigned. Consider adding a drone " +
                "with greater capacity or range.",
            SuggestedMinimumCapacityKg =
                Round(minimumCapacity),
            SuggestedMinimumRangeKm =
                Round(minimumRange)
        };
    }

    private double CalculateEstimatedTimeMinutes(
        double distanceKm)
    {
        if (distanceKm <= 0)
        {
            return 0;
        }

        var hours =
            distanceKm / _options.DroneSpeedKmPerHour;

        return hours * 60;
    }

    private static double CalculatePercentageSafe(
        double numerator,
        double denominator)
    {
        if (denominator <= 0)
        {
            return 0;
        }

        return numerator / denominator * 100;
    }

    private static double Round(double value)
    {
        return Math.Round(
            value,
            2,
            MidpointRounding.AwayFromZero);
    }
    private static double CalculateRatioSafe(
    double numerator,
    double denominator)
    {
        if (denominator <= 0)
        {
            return 0;
        }

        return numerator / denominator;
    }
}