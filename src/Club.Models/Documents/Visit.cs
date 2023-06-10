namespace Club.Models.Documents;

public class Visit
{
    public int Id { get; set; }
    
    public Guid ClientId { get; set; }
    
    public DateTime DateTime { get; set; }
}