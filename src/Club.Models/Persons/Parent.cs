namespace Club.Models.Persons;

public record Parent
{
    public Guid Id { get; init; }
    
    public string Surname { get; init; }
    
    public string Name { get; init; }
    
    public string Patronymic { get; init; }
    
    public List<string> Phones { get; init; }
}