using DroneDelivery.Domain.Enums;
using DroneDelivery.Domain.ValueObjects;

namespace DroneDelivery.Domain.Entities;

public class Order
{
    public string Id { get; }

    public double WeightKg { get; }

    public Coordinate Destination { get; }

    public Priority Priority { get; }

    public int InputOrder { get; }

    public Order(
        string id,
        double weightKg,
        Coordinate destination,
        Priority priority,
        int inputOrder)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException(
                "The order ID is required.",
                nameof(id));
        }

        if (!double.IsFinite(weightKg) || weightKg <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(weightKg),
                weightKg,
                "The order weight must be a finite number greater than zero.");
        }

        if (!Enum.IsDefined(priority))
        {
            throw new ArgumentOutOfRangeException(
                nameof(priority),
                priority,
                "The order priority is invalid.");
        }

        if (inputOrder < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(inputOrder),
                inputOrder,
                "The input order cannot be negative.");
        }

        Id = id;
        WeightKg = weightKg;
        Destination = destination;
        Priority = priority;
        InputOrder = inputOrder;
    }
}