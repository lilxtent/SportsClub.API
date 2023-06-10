using System.ComponentModel.DataAnnotations;
using Clients.API.Requests.Visits;
using Club.Logic.Models;
using Club.Logic.Services.Interfaces;
using Humanizer;
using Microsoft.AspNetCore.Mvc;

namespace Clients.API.Controllers;

[ApiController]
[Produces("application/json")]
[Route("visits")]
public class VisitsController: ControllerBase
{
    private readonly IVisitsService _visitsService;

    public VisitsController(IVisitsService visitsService)
    {
        _visitsService = visitsService ?? throw new ArgumentNullException(nameof(visitsService));
    }

    [HttpGet]
    [Route("last")]
    public async Task<ActionResult<LastClientsVisits>> GetVisits([FromQuery, Required] GetVisitsRequest request)
    {
        var result = await _visitsService.GetLastClientsVisits(request.Skip, request.Take, 30.Seconds());

        return Ok(result);
    }
}