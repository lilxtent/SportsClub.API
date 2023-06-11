using Clients.API.Requests.Clients;
using Clients.API.Requests.Subscriptions;
using Club.Models.Documents;
using Club.Models.Persons;
using Riok.Mapperly.Abstractions;

namespace Clients.API.Mappers;

[Mapper]
public partial class Mapperly
{
    public partial Client MapToClient(UpdateClientRequest request);

    public partial Subscription MapToSubscription(UpdateSubscriptionRequest request);
}