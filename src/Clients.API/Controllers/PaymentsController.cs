using System.ComponentModel.DataAnnotations;
using Clients.API.Requests.Payments;
using Club.Logic.Models;
using Club.Logic.Services.Interfaces;
using Club.Repository.Repositories.Interfaces;
using Humanizer;
using Microsoft.AspNetCore.Mvc;

namespace Clients.API.Controllers;

[ApiController]
[Produces("application/json")]
[Route("payments")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentsService _paymentsService;
    private readonly IPaymentsRepository _paymentsRepository;

    public PaymentsController(
        IPaymentsService paymentsService,
        IPaymentsRepository paymentsRepository)
    {
        _paymentsService = paymentsService ?? throw new ArgumentNullException(nameof(paymentsService));
        _paymentsRepository = paymentsRepository ?? throw new ArgumentNullException(nameof(paymentsRepository));
    }
    
    [HttpPost]
    [Route("add")]
    public async Task<ActionResult> AddPayment(
        [FromQuery, Required] Guid clientId,
        [FromQuery, Required] Guid subscriptionId)
    {
       await _paymentsService.AddPayment(clientId, subscriptionId, 30.Seconds());

        return Ok();
    }
    
    [HttpGet]
    [Route("last")]
    public async Task<ActionResult<LastPaymentsFullInfo>> GetLastPayments([FromQuery, Required] GetPaymentsRequest request)
    {
        var result = await _paymentsService.GetLastPaymentsFullInfo(request.Skip, request.Take, 30.Seconds());

        return Ok(result);
    }
    
    [HttpGet]
    [Route("client-last")]
    public async Task<ActionResult<LastPaymentsFullInfo>> GetClientLastPayment([FromQuery, Required] Guid clientId)
    {
        var result = await _paymentsService.GetClientLastPaymentFullInfo(clientId, 30.Seconds());

        return Ok(result);
    }
}