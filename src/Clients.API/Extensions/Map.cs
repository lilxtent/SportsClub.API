using Clients.API.Mappers;
using Clients.API.Requests.Clients;
using Clients.API.Requests.Subscriptions;
using Club.Models.Documents;
using Club.Models.Persons;

namespace Clients.API.Extensions;

public static class Map
{
    private static readonly Mapperly Mapperly = new Mapperly();
    
    public static Client MapToClient(this UpdateClientRequest request) => Mapperly.MapToClient(request);
    
    public static Subscription MapToSubscription(this UpdateSubscriptionRequest request) => Mapperly.MapToSubscription(request);
}