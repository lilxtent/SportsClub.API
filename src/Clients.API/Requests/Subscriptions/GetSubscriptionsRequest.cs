using System.ComponentModel.DataAnnotations;

namespace Clients.API.Requests.Subscriptions;

public class GetSubscriptionsRequest
{
    [Required]
    [Range(0, 100)]
    public int Take { get; set; }
    
    [Required]
    public int Skip { get; set; }
}