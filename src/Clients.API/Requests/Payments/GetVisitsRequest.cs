using System.ComponentModel.DataAnnotations;

namespace Clients.API.Requests.Payments;

public class GetPaymentsRequest
{
    [Required]
    public int Skip { get; set; }
    
    [Required]
    public int Take { get; set; }
}