using Club.Models.Documents;

namespace Clients.API.Responses.Payments;

public class GetPaymentsResult
{
    public Payment[] Payments { get; set; }
    
    public int TotalCount { get; set; }
}