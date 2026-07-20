using System.Collections.Concurrent;

namespace DroneDelivery.Api.Storage;

public sealed class InMemoryPlanningStore
{
    private readonly ConcurrentDictionary<Guid, StoredPlanningScenario>
        _scenarios = new();

    public void Add(StoredPlanningScenario scenario)
    {
        _scenarios[scenario.PlanningId] = scenario;
    }

    public bool TryGet(
        Guid planningId,
        out StoredPlanningScenario? scenario)
    {
        var found = _scenarios.TryGetValue(
            planningId,
            out var storedScenario);

        scenario = storedScenario;

        return found;
    }
}