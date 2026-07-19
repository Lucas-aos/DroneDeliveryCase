namespace DroneDelivery.Domain.Entities;

public class Drone
{
    public string Id { get; }

    public double CapacityKg { get; }

    public double RangeKm { get; }

    public int InputOrder { get; }

    public Drone(
        string id,
        double capacityKg,
        double rangeKm,
        int inputOrder)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException(
                "The drone ID is required.",
                nameof(id));
        }

        if (!double.IsFinite(capacityKg) || capacityKg <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(capacityKg),
                capacityKg,
                "The drone capacity must be a finite number greater than zero.");
        }

        if (!double.IsFinite(rangeKm) || rangeKm <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(rangeKm),
                rangeKm,
                "The drone range must be a finite number greater than zero.");
        }

        if (inputOrder < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(inputOrder),
                inputOrder,
                "The input order cannot be negative.");
        }

        Id = id;
        CapacityKg = capacityKg;
        RangeKm = rangeKm;
        InputOrder = inputOrder;
    }
}