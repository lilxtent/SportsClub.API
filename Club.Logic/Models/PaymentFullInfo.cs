using Club.Models.Documents;
using Club.Models.Persons;

namespace Club.Logic.Models;

public class PaymentFullInfo
{
    public int Id { get; set; }
    
    public Client Client { get; set; }
    
    public Subscription Subscription { get; set; }

    public DateTime PaymentDate { get; set; }

    public DateTime SubscriptionStartTime { get; set; }

    public DateTime SubscriptionEndTime { get; set; }

    public decimal PaymentAmount { get; set; }
}