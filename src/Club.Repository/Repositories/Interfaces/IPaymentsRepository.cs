using Club.Models.Documents;

namespace Club.Repository.Repositories.Interfaces;

public interface IPaymentsRepository
{
    Task Add(Guid clientId, Subscription subscription, TimeSpan timeout);
    
    Task<Payment> GetClientLastPayment(Guid clientId, TimeSpan timeout);
    
    Task<(Payment[] Payments, int TotalCount)> GetLastPayments(int skip, int take, TimeSpan timeout);
}