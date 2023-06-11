using Club.Models.Documents;

namespace Clients.API.Responses.Subscriptions;

public class SearchSubscriptionsResponse
{
    public Subscription[] Subscriptions { get; set; }
    
    public int TotalCount { get; set; }
}