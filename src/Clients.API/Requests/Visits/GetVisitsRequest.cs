using System.ComponentModel.DataAnnotations;

namespace Clients.API.Requests.Visits;

public class GetVisitsRequest
{
    [Required]
    public int Skip { get; set; }
    
    [Required]
    public int Take { get; set; }
}