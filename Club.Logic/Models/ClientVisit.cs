using Club.Models.Persons;

namespace Club.Logic.Models;

public class ClientVisit
{
    public int Id { get; set; }
    
    public Client Client { get; set; }
    
    public DateTime DateTime { get; set; }
}