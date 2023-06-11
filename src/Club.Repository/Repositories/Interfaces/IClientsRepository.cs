using Club.Models.Persons;
using Club.Repository.Models;

namespace Club.Repository.Repositories.Interfaces;

public interface IClientsRepository
{
    Task<Client> GetClientById(Guid id, TimeSpan timeout);

    Task<IEnumerable<Client>> GetClients(ICollection<Guid> ids, TimeSpan timeout);

    Task<(Client[] Clients, int TotalCount)> GetClients(int skip, int take, TimeSpan timeout);

    Task<(Client[] Clients, int TotalCount)> Search(ClientsSearchRules searchRules, TimeSpan timeout);

    Task Add(Client client, TimeSpan timeout);
    
    Task Update(Client client, TimeSpan timeout);
}