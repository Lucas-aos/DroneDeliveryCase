using DroneDelivery.Api.Contracts.Requests;
using DroneDelivery.Api.Contracts.Responses;
using DroneDelivery.Api.Mapping;
using DroneDelivery.Api.Storage;
using DroneDelivery.Domain.Planning;
using Microsoft.AspNetCore.Mvc;

namespace DroneDelivery.Api.Controllers;

[ApiController]
[Route("api/planning")]
public sealed class PlanningController : ControllerBase
{
    private readonly InMemoryPlanningStore _store;

    public PlanningController(InMemoryPlanningStore store)
    {
        _store = store;
    }

    [HttpPost]
    [ProducesResponseType(
        typeof(PlanningCreatedResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        typeof(ErrorResponse),
        StatusCodes.Status400BadRequest)]
    public ActionResult<PlanningCreatedResponse> Plan(
        PlanningRequest request)
    {
        try
        {
            var drones = PlanningMapper.ToDomainDrones(request.Drones);
            var orders = PlanningMapper.ToDomainOrders(request.Orders);

            var planner = new TripPlanner();

            var planningResult = planner.Plan(drones, orders);

            var planningResponse =
                PlanningMapper.ToResponse(planningResult);

            var planningId = Guid.NewGuid();
            var createdAtUtc = DateTimeOffset.UtcNow;

            var scenario = new StoredPlanningScenario
            {
                PlanningId = planningId,
                CreatedAtUtc = createdAtUtc,
                Drones = drones.ToArray(),
                Orders = orders.ToArray(),
                Result = planningResult
            };

            _store.Add(scenario);

            var createdResponse = new PlanningCreatedResponse
            {
                PlanningId = planningId,
                CreatedAtUtc = createdAtUtc,
                Planning = planningResponse
            };

            return CreatedAtAction(
                nameof(GetPlanning),
                new { planningId },
                createdResponse);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(CreateErrorResponse(ex));
        }
    }

    [HttpGet("{planningId:guid}")]
    [ProducesResponseType(
        typeof(PlanningResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<PlanningResponse> GetPlanning(
        Guid planningId)
    {
        if (!_store.TryGet(planningId, out var scenario))
        {
            return NotFound();
        }

        var response =
            PlanningMapper.ToResponse(scenario!.Result);

        return Ok(response);
    }

    private static ErrorResponse CreateErrorResponse(
        ArgumentException exception)
    {
        return new ErrorResponse
        {
            Message = "The request is invalid.",
            Errors =
            [
                exception.Message
            ]
        };
    }

    [HttpGet("{planningId:guid}/routes")]
    [ProducesResponseType(
    typeof(IReadOnlyList<RouteResponse>),
    StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IReadOnlyList<RouteResponse>> GetRoutes(
    Guid planningId)
    {
        if (!_store.TryGet(planningId, out var scenario))
        {
            return NotFound();
        }

        var routes =
            PlanningMapper.ToRouteResponses(scenario!);

        return Ok(routes);
    }

    [HttpGet("{planningId:guid}/drones")]
    [ProducesResponseType(
    typeof(IReadOnlyList<DroneSummaryResponse>),
    StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IReadOnlyList<DroneSummaryResponse>> GetDrones(
    Guid planningId)
    {
        if (!_store.TryGet(planningId, out var scenario))
        {
            return NotFound();
        }

        var drones =
            PlanningMapper.ToDroneSummaryResponses(scenario!);

        return Ok(drones);
    }
}