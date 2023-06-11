using Clients.API.Mappers;
using Clients.API.Requests.Clients;
using Club.Models.Persons;

namespace Clients.API.Extensions;

public static class Map
{
    private static readonly Mapperly Mapperly = new Mapperly();
    
    public static Client MapToClient(this UpdateClientRequest request) => Mapperly.MapToClient(request);
}