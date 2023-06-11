using Club.Models.Documents;

namespace Clients.API.Responses.Subscriptions;

public class GetSubscriptionsResult
{
    public Subscription[] Subscriptions { get; set; }
    
    public int TotalCount { get; set; }
}