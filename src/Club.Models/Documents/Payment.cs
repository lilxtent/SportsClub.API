﻿namespace Club.Models.Documents;

public class Payment
{
    public int Id { get; set; }
    
    public Guid ClientId { get; set; }
    
    public Guid SubscriptionId { get; set; }

    public DateTime PaymentDate { get; set; }

    public DateTime SubscriptionStartTime { get; set; }

    public DateTime SubscriptionEndTime { get; set; }

    public decimal PaymentAmount { get; set; }
}