using Club.Models.Documents;

namespace Club.Repository.Repositories.Interfaces;

public interface IVisitsRepository
{
    Task Add(Guid clientId, TimeSpan timeout);
    
    Task<(Visit[] Visits, int TotalCount)> GetLastVisits(int skip, int take, TimeSpan timeout);
}