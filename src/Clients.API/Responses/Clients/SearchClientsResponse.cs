using Club.Models.Persons;

namespace Clients.API.Responses.Clients;

public class SearchClientsResponse
{
    public Client[] Clients { get; set; }
    
    public int TotalCount { get; set; }
}