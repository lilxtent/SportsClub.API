using Club.Logic.Models;
using Club.Logic.Services.Interfaces;
using Club.Repository.Repositories.Interfaces;

namespace Club.Logic.Services;

public class PaymentsService : IPaymentsService
{
    private readonly IPaymentsRepository _paymentsRepository;
    private readonly IClientsRepository _clientsRepository;
    private readonly ISubscriptionsRepository _subscriptionsRepository;
    
    public PaymentsService(
        IPaymentsRepository paymentsRepository,
        IClientsRepository clientsRepository,
        ISubscriptionsRepository subscriptionsRepository)
    {
        _paymentsRepository = paymentsRepository ?? throw new ArgumentNullException(nameof(paymentsRepository));
        _clientsRepository = clientsRepository ?? throw new ArgumentNullException(nameof(clientsRepository));
        _subscriptionsRepository = subscriptionsRepository ?? throw new ArgumentNullException(nameof(subscriptionsRepository));
    }

    public async Task AddPayment(Guid clientId, Guid subscriptionId, TimeSpan timeout)
    {
        var subscription = await _subscriptionsRepository.GetSubscriptionById(subscriptionId, timeout);

        await _paymentsRepository.Add(clientId, subscription, timeout);
    }
    
    public async Task<LastPaymentsFullInfo> GetLastPaymentsFullInfo(int skip, int take, TimeSpan timeout)
    {
        var (lastPayments, lastPaymentsTotalCount) = await _paymentsRepository.GetLastPayments(skip, take, timeout);
        
        var clients = (await _clientsRepository.GetClients(
            lastPayments.Select(x => x.ClientId).ToArray(),
            timeout)).ToDictionary(key => key.Id);
        
        var subscriptions = (await _subscriptionsRepository.GetSubscriptions(
            lastPayments.Select(x => x.SubscriptionId).ToArray(),
            timeout)).ToDictionary(key => key.Id);

        return new LastPaymentsFullInfo
        {
            PaymentFullInfo = lastPayments.Select(x => new PaymentFullInfo
            {
                Id = x.Id,
                Client = clients[x.ClientId],
                Subscription = subscriptions[x.SubscriptionId],
                PaymentDate = x.PaymentDate,
                SubscriptionStartTime = x.SubscriptionStartTime,
                SubscriptionEndTime = x.SubscriptionEndTime,
                PaymentAmount = x.PaymentAmount
            }).ToArray(),
            TotalCount = lastPaymentsTotalCount
        };
    }
    
    public async Task<PaymentFullInfo> GetClientLastPaymentFullInfo(Guid clientId, TimeSpan timeout)
    {
        var lastPayment = await _paymentsRepository.GetClientLastPayment(clientId, timeout);

        if (lastPayment is null)
        {
            return null;
        }
        
        var client = await _clientsRepository.GetClientById(clientId, timeout);
        
        var subscription = await _subscriptionsRepository.GetSubscriptionById(lastPayment.SubscriptionId, timeout);

        return new PaymentFullInfo
        {
            Id = lastPayment.Id,
            Client = client,
            Subscription = subscription,
            PaymentDate = lastPayment.PaymentDate,
            SubscriptionStartTime = lastPayment.SubscriptionStartTime,
            SubscriptionEndTime = lastPayment.SubscriptionEndTime,
            PaymentAmount = lastPayment.PaymentAmount

        };
    }
}