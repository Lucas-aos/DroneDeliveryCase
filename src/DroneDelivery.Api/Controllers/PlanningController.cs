using DroneDelivery.Api.Contracts.Requests;
using DroneDelivery.Api.Contracts.Responses;
using DroneDelivery.Api.Mapping;
using DroneDelivery.Domain.Planning;
using Microsoft.AspNetCore.Mvc;

namespace DroneDelivery.Api.Controllers;

[ApiController]
[Route("api/planning")]
public sealed class PlanningController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(PlanningResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public ActionResult<PlanningResponse> Plan(PlanningRequest request)
    {
        try
        {
            var drones = PlanningMapper.ToDomainDrones(request.Drones);
            var orders = PlanningMapper.ToDomainOrders(request.Orders);

            var planner = new TripPlanner();

            var planningResult = planner.Plan(drones, orders);

            var response = PlanningMapper.ToResponse(planningResult);

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(CreateErrorResponse(ex));
        }
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
}