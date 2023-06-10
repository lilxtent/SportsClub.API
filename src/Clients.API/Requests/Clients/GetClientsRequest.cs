using System.ComponentModel.DataAnnotations;

namespace Clients.API.Requests.Clients;

public class GetClientsRequest
{
    [Required]
    [Range(0, 100)]
    public int Take { get; set; }
    
    [Required]
    public int Skip { get; set; }
}