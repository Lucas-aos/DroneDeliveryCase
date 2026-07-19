using DroneDelivery.Domain.ValueObjects;

namespace DroneDelivery.Domain.Calculations;

public static class DistanceCalculator
{
    public static double Calculate(
        Coordinate origin,
        Coordinate destination)
    {
        var deltaX = destination.X - origin.X;
        var deltaY = destination.Y - origin.Y;

        return Math.Sqrt(
            Math.Pow(deltaX, 2) +
            Math.Pow(deltaY, 2));
    }
}