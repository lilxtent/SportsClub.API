using Clients.API.Requests.Clients;
using Club.Models.Persons;
using Riok.Mapperly.Abstractions;

namespace Clients.API.Mappers;

[Mapper]
public partial class Mapperly
{
    public partial Client MapToClient(UpdateClientRequest request);
}