using Club.Models.Statistics;

namespace Club.Repository.Repositories.Interfaces;

public interface IStatisticsRepository
{
    Task<IEnumerable<VisitStat>> GetVisitsStat(TimeSpan timeout);

    Task<IEnumerable<SubscriptionsStat>> GetSubscriptionsStat(TimeSpan timeout);

    Task<IEnumerable<PaymentStat>> GetPaymentsStat(TimeSpan timeout);
}