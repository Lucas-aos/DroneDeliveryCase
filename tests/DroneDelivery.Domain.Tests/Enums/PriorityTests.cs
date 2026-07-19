using DroneDelivery.Domain.Enums;

namespace DroneDelivery.Domain.Tests.Enums;

public class PriorityTests
{
    [Fact]
    public void Priority_ShouldContainOnlyApprovedValues()
    {
        var expectedValues = new HashSet<Priority>
        {
            Priority.High,
            Priority.Medium,
            Priority.Low
        };

        var actualValues = Enum
            .GetValues<Priority>()
            .ToHashSet();

        Assert.True(
            actualValues.SetEquals(expectedValues),
            "Priority must contain only High, Medium and Low.");
    }
}