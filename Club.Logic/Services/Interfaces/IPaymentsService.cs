using Club.Logic.Models;

namespace Club.Logic.Services.Interfaces;

public interface IPaymentsService
{
    Task AddPayment(Guid clientId, Guid subscriptionId, TimeSpan timeout);
    
    Task<LastPaymentsFullInfo> GetLastPaymentsFullInfo(int skip, int take, TimeSpan timeout);

    Task<PaymentFullInfo> GetClientLastPaymentFullInfo(Guid clientId, TimeSpan timeout);
}