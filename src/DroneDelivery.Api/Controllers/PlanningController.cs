using DroneDelivery.Api.Contracts.Requests;
using DroneDelivery.Api.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DroneDelivery.Api.Controllers;

[ApiController]
[Route("api/planning")]
public sealed class PlanningController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(PlanningResponse), StatusCodes.Status200OK)]
    public ActionResult<PlanningResponse> Plan(PlanningRequest request)
    {
        var response = new PlanningResponse();

        return Ok(response);
    }
}