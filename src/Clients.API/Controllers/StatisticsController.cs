using Club.Models.Statistics;
using Club.Repository.Repositories.Interfaces;
using Humanizer;
using Microsoft.AspNetCore.Mvc;

namespace Clients.API.Controllers;

[ApiController]
[Produces("application/json")]
[Route("statistics")]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsRepository _statisticsRepository;

    public StatisticsController(IStatisticsRepository statisticsRepository)
    {
        _statisticsRepository = statisticsRepository ?? throw new ArgumentNullException(nameof(statisticsRepository));
    }

    [HttpGet]
    [Route("last-visits")]
    public async Task<ActionResult<IEnumerable<VisitStat>>> GetSubscription()
    {
        var visit = await _statisticsRepository.GetVisitsStat(30.Seconds());

        return Ok(visit);
    }
    
    [HttpGet]
    [Route("subscriptions")]
    public async Task<ActionResult<IEnumerable<SubscriptionsStat>>> GetSubscriptions()
    {
        var subscription = await _statisticsRepository.GetSubscriptionsStat(30.Seconds());

        return Ok(subscription);
    }
    
    [HttpGet]
    [Route("payments")]
    public async Task<ActionResult<IEnumerable<PaymentStat>>> GetPayments()
    {
        var paymentsStat = await _statisticsRepository.GetPaymentsStat(30.Seconds());

        return Ok(paymentsStat);
    }
}