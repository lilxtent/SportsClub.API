using Club.Models.Persons;

namespace Clients.API.Responses.Clients;

public class GetClientsResponse
{
    public Client[] Clients { get; set; }
    
    public int TotalCount { get; set; }
}