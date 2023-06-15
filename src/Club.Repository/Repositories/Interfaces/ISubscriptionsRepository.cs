using Club.Models.Documents;
using Club.Repository.Models;

namespace Club.Repository.Repositories.Interfaces;

public interface ISubscriptionsRepository
{
    Task Add(Subscription subscription, TimeSpan timeout);

    Task<IEnumerable<Subscription>> GetAllSubscriptions(TimeSpan timeout);

    Task<IEnumerable<Subscription>> GetSubscriptions(ICollection<Guid> ids, TimeSpan timeout);
    
    Task<Subscription> GetSubscriptionById(Guid id, TimeSpan timeout);
    
    Task<(Subscription[] Subscriptions, int TotalCount)> GetSubscriptions(int skip, int take, TimeSpan timeout);

    Task Update(Subscription subscription, TimeSpan timeout);

    Task<(Subscription[] Subscriptions, int TotalCount)> Search(SubscriptionSearchRules searchRules, TimeSpan timeout);
}