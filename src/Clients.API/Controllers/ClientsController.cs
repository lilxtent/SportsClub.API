using System.ComponentModel.DataAnnotations;
using Clients.API.Extensions;
using Clients.API.Requests.Clients;
using Clients.API.Responses.Clients;
using Club.Models.Persons;
using Club.Repository.Models;
using Club.Repository.Repositories.Interfaces;
using Humanizer;
using Microsoft.AspNetCore.Mvc;

namespace Clients.API.Controllers;

[ApiController]
[Produces("application/json")]
[Route("clients")]
public class ClientsController : ControllerBase
{
    private readonly IClientsRepository _clientsRepository;

    public ClientsController(IClientsRepository clientsRepository)
    {
        _clientsRepository = clientsRepository ?? throw new ArgumentNullException(nameof(clientsRepository));
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ActionResult<Client>> GetClient([FromRoute, Required] Guid id)
    {
        var client = await _clientsRepository.GetClientById(id, 30.Seconds());

        return Ok(client);
    }

    [HttpGet]
    [Route("")]
    public async Task<ActionResult<GetClientsResponse>> GetClients([FromQuery, Required] GetClientsRequest request)
    {
        var result = await _clientsRepository.GetClients(request.Skip, request.Take, 30.Seconds());

        return Ok(new GetClientsResponse
        {
            Clients = result.Clients,
            TotalCount = result.TotalCount
        });
    }
    
    [HttpGet]
    [Route("search")]
    public async Task<ActionResult<SearchClientsResponse>> SearchClients([FromQuery, Required] SearchClientsRequest request)
    {
        var result = await _clientsRepository.Search(
            new ClientsSearchRules
            {
                Like = request.Like
            },
            30.Seconds());

        return Ok(new SearchClientsResponse
        {
            Clients = result.Clients,
            TotalCount = result.TotalCount
        });
    }
    
    [HttpPost]
    [Route("add")]
    public async Task<ActionResult> AddClient([FromBody, Required] Client client)
    {
        await _clientsRepository.Add(client, 30.Seconds());

        return Ok();
    }
    
    [HttpPatch]
    [Route("update")]
    public async Task<ActionResult> UpdateClient([FromBody, Required] UpdateClientRequest request)
    {
        await _clientsRepository.Update(
           request.MapToClient(),
            30.Seconds());

        return Ok();
    }
}