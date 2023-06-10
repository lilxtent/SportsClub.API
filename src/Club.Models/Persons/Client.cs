namespace Club.Models.Persons;

public record Client
{
    public Guid Id { get; init; }
    
    public string Surname { get; init; }
    
    public string Name { get; init; }
    
    public string Patronymic { get; init; }
    
    public DateTime BirthDate { get; init; }
    
    /*//TODO: перенести в отдельную таблицу?
    public List<string> Phones { get; init; }
    
    //TODO: перенести в отдельную таблицу
    public DateTime LastPaymentDate { get; set; }
    
    public string Section { get; init; }

    //TODO: перенести в отдельную таблицу
    public DateTime LastVisitDate { get; set; }*/
}