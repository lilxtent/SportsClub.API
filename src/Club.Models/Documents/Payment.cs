namespace Club.Models.Documents;

public record Payment
{
    public Guid Id { get; init; }
    
    public Guid PayerId { get; init; }
    
    public DateTime Date { get; init; }
    
    public int Amount { get; init; }
}