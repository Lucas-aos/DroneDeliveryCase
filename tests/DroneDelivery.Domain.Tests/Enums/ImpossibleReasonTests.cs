using DroneDelivery.Domain.Enums;

namespace DroneDelivery.Domain.Tests.Enums;

public class ImpossibleReasonTests
{
    [Fact]
    public void ImpossibleReason_ShouldContainOnlyApprovedValues()
    {
        var expectedValues = new HashSet<ImpossibleReason>
        {
            ImpossibleReason.WeightExceeded,
            ImpossibleReason.RangeExceeded,
            ImpossibleReason.WeightAndRangeExceeded
        };

        var actualValues = Enum
            .GetValues<ImpossibleReason>()
            .ToHashSet();

        Assert.True(
            actualValues.SetEquals(expectedValues),
            "ImpossibleReason must contain only the approved reasons.");
    }
}