using Club.Logic.Models;
using Club.Logic.Services.Interfaces;
using Club.Repository.Repositories.Interfaces;

namespace Club.Logic.Services;

public class VisitsService : IVisitsService
{
    private readonly IVisitsRepository _visitsRepository;
    private readonly IClientsRepository _clientsRepository;

    public VisitsService(
        IVisitsRepository visitsRepository,
        IClientsRepository clientsRepository)
    {
        _visitsRepository = visitsRepository ?? throw new ArgumentNullException(nameof(visitsRepository));
        _clientsRepository = clientsRepository ?? throw new ArgumentNullException(nameof(clientsRepository));
    }
    
    public async Task<LastClientsVisits> GetLastClientsVisits(int skip, int take, TimeSpan timeout)
    {
        var lastVisits = await _visitsRepository.GetLastVisits(skip, take, timeout);

        var clients = (await _clientsRepository.GetClients(
            lastVisits.Visits.Select(x => x.ClientId).ToArray(),
            timeout)).ToDictionary(key => key.Id);

        return new LastClientsVisits
        {
            Visits = lastVisits.Visits.Select(x => new ClientVisit
            {
                Id = x.Id,
                Client = clients[x.ClientId],
                DateTime = x.DateTime
            }).ToArray(),
            TotalVisitsCount = lastVisits.TotalCount
        };
    }
}