namespace DroneDelivery.Domain.ValueObjects;

public readonly record struct Coordinate
{
    public double X { get; }

    public double Y { get; }

    public Coordinate(double x, double y)
    {
        if (!double.IsFinite(x))
        {
            throw new ArgumentOutOfRangeException(
                nameof(x),
                x,
                "The X coordinate must be a finite number.");
        }

        if (!double.IsFinite(y))
        {
            throw new ArgumentOutOfRangeException(
                nameof(y),
                y,
                "The Y coordinate must be a finite number.");
        }

        X = x;
        Y = y;
    }
}