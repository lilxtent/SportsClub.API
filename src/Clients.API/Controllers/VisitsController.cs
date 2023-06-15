using System.ComponentModel.DataAnnotations;
using Clients.API.Requests.Visits;
using Club.Logic.Models;
using Club.Logic.Services.Interfaces;
using Club.Repository.Repositories.Interfaces;
using Humanizer;
using Microsoft.AspNetCore.Mvc;

namespace Clients.API.Controllers;

[ApiController]
[Produces("application/json")]
[Route("visits")]
public class VisitsController: ControllerBase
{
    private readonly IVisitsService _visitsService;
    private readonly IVisitsRepository _visitsRepository;

    public VisitsController(
        IVisitsService visitsService,
        IVisitsRepository visitsRepository)
    {
        _visitsService = visitsService ?? throw new ArgumentNullException(nameof(visitsService));
        _visitsRepository = visitsRepository ?? throw new ArgumentNullException(nameof(visitsRepository));
    }

    [HttpPost]
    [Route("add")]
    public async Task<ActionResult> GetVisits([FromQuery, Required] Guid clientId)
    {
        await _visitsRepository.Add(clientId, 30.Seconds());

        return Ok();
    }
    
    [HttpGet]
    [Route("last")]
    public async Task<ActionResult<LastClientsVisits>> GetVisits([FromQuery, Required] GetVisitsRequest request)
    {
        var result = await _visitsService.GetLastClientsVisits(request.Skip, request.Take, 30.Seconds());

        return Ok(result);
    }
}