using Club.Models.Persons;

namespace Club.Models.Group;

public record Group
{
    public Trainer Trainer { get; init; }
    
    public List<string> Members { get; init; }
}