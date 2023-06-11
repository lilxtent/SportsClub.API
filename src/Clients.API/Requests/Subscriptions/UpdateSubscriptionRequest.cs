namespace Clients.API.Requests.Subscriptions;

public class UpdateSubscriptionRequest
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public float Price { get; set; }

    public int DaysLong { get; set; }

    public string Description { get; set; }
}