using Club.Logic.Models;

namespace Club.Logic.Services.Interfaces;

public interface IVisitsService
{
    Task<LastClientsVisits> GetLastClientsVisits(int skip, int take, TimeSpan timeout);
}