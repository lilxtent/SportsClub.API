using System.ComponentModel.DataAnnotations;
using Clients.API.Extensions;
using Clients.API.Requests.Subscriptions;
using Clients.API.Responses.Subscriptions;
using Club.Models.Documents;
using Club.Repository.Models;
using Club.Repository.Repositories.Interfaces;
using Humanizer;
using Microsoft.AspNetCore.Mvc;

namespace Clients.API.Controllers;

[ApiController]
[Produces("application/json")]
[Route("subscriptions")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionsRepository _subscriptionsRepository;

    public SubscriptionsController(ISubscriptionsRepository subscriptionsRepository)
    {
        _subscriptionsRepository = subscriptionsRepository ?? throw new ArgumentNullException(nameof(subscriptionsRepository));
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ActionResult<Subscription>> GetSubscription([FromRoute, Required] Guid id)
    {
        var subscription = await _subscriptionsRepository.GetSubscriptionById(id, 30.Seconds());

        return Ok(subscription);
    }
    
    [HttpGet]
    [Route("all")]
    public async Task<ActionResult<IEnumerable<Subscription>>> GetAllSubscriptions()
    {
        var subscriptions = await _subscriptionsRepository.GetAllSubscriptions(30.Seconds());

        return Ok(subscriptions);
    }
    
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<GetSubscriptionsResult>> GetSubscription([FromQuery, Required] GetSubscriptionsRequest request)
    {
        var result = await _subscriptionsRepository.GetSubscriptions(request.Skip, request.Take, 30.Seconds());

        return Ok(new GetSubscriptionsResult
        {
            Subscriptions = result.Subscriptions,
            TotalCount = result.TotalCount
        });
    }
    
    [HttpPatch]
    [Route("update")]
    public async Task<ActionResult> UpdateSubscription([FromBody, Required] UpdateSubscriptionRequest request)
    {
        await _subscriptionsRepository.Update(
            request.MapToSubscription(),
            30.Seconds());

        return Ok();
    }
    
    [HttpPost]
    [Route("add")]
    public async Task<ActionResult> AddSubscription([FromBody, Required] Subscription subscription)
    {
        await _subscriptionsRepository.Add(subscription, 30.Seconds());

        return Ok();
    }
    
    [HttpGet]
    [Route("search")]
    public async Task<ActionResult<SearchSubscriptionsResponse>> SearchSubscriptions([FromQuery, Required] SearchSubscriptionsRequest request)
    {
        var result = await _subscriptionsRepository.Search(
            new SubscriptionSearchRules()
            {
                Like = request.Like
            },
            30.Seconds());

        return Ok(new SearchSubscriptionsResponse
        {
            Subscriptions = result.Subscriptions,
            TotalCount = result.TotalCount
        });
    }
}