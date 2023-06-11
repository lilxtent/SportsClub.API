namespace Clients.API.Requests.Clients;

public class UpdateClientRequest
{
    public Guid Id { get; init; }
    
    public string Surname { get; init; }
    
    public string Name { get; init; }
    
    public string Patronymic { get; init; }
    
    public DateTime BirthDate { get; init; }
    
    public string Phone { get; set; }
}