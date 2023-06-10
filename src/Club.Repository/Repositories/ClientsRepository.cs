using System.Data;
using Club.Models.Persons;
using Club.Repository.Models;
using Club.Repository.Repositories.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using Exception = System.Exception;

namespace Club.Repository.Repositories;

public class ClientsRepository: IClientsRepository
{
    private readonly string _connectionString;
    private readonly ILogger _logger;

    public ClientsRepository(
        string connectionString,
        ILogger logger)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Client> GetClientById(Guid id, TimeSpan timeout)
    {
        var sql = 
            """
            SELECT
                *
            FROM
                "Clients"
            WHERE
                Id = :id
            """;

        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            await connection.OpenAsync();
            
            var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            
            var result = await connection.QuerySingleAsync<Client>(
                sql,
                new {id},
                transaction,
                (int)timeout.TotalSeconds);

            if (result is null)
            {
                throw new Exception($"Ошибка при попытке получить клиента с id {id}.");
            }
            
            return result;
        }
        catch (NpgsqlException exception)
        {
            _logger.LogError("Ошибка при попытке получить клиента. Error: {Message}", exception.Message);

            throw new Exception($"Ошибка при попытке получить клиента с id {id}.");
        }
    }

    public async Task<IEnumerable<Client>> GetClients(ICollection<Guid> ids, TimeSpan timeout)
    {
        var sql = 
            $"""
            SELECT
                *
            FROM
                "Clients"
            WHERE
                Id IN ({string.Join(',', ids.Select(x => $"'{x}'"))})
            """;

        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            await connection.OpenAsync();
            
            var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            
            var result = await connection.QueryAsync<Client>(
                sql,
                //new {ids/* = string.Join(',', ids.Select(x => $"'{x}'"))*/},
                transaction: transaction,
                commandTimeout: (int)timeout.TotalSeconds);

            if (result is null)
            {
                throw new Exception($"Ошибка при попытке получить клиента с id {string.Join(',', ids)}.");
            }
            
            return result;
        }
        catch (NpgsqlException exception)
        {
            _logger.LogError("Ошибка при попытке получить клиента. Error: {Message}", exception.Message);

            throw new Exception($"Ошибка при попытке получить клиента с id {string.Join(',', ids)}.");
        }
    }
    
    public async Task<(Client[] Clients, int TotalCount)> GetClients(int skip, int take, TimeSpan timeout)
    {
        var sql = 
            """
            SELECT
                *
            FROM
                "Clients"
            ORDER BY 
                Surname, 
                Name,
                Patronymic
            LIMIT :take OFFSET :skip;
            
            SELECT COUNT(*) FROM "Clients";
            """;

        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            await connection.OpenAsync();
            
            var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            await using var command = new NpgsqlCommand(sql, connection, transaction);
            
            var result = await connection.QueryMultipleAsync(
                sql,
                new {skip, take},
                transaction,
                (int)timeout.TotalSeconds);

            if (result is null)
            {
                throw new Exception("Ошибка при попытке получить клиентов.");
            }
            
            return (result.Read<Client>().ToArray(), result.ReadSingle<int>());
        }
        catch (NpgsqlException exception)
        {
            _logger.LogError("Ошибка при попытке получить клиента. Error: {Message}", exception.Message);

            throw new Exception("Ошибка при попытке получить клиентов.");
        }
    }

    public async Task<(Client[] Clients, int TotalCount)> Search(ClientsSearchRules searchRules, TimeSpan timeout)
    {
        var sql = 
            $"""
            SELECT
                *
            FROM
                "Clients"
            WHERE
                concat(surname, ' ', name, ' ', patronymic) LIKE '%{searchRules.Like}%'
            ORDER BY
                surname,
                name,
                patronymic;
                
            SELECT
                COUNT(*)
            FROM
                "Clients"
            WHERE
                concat(surname, ' ', name, ' ', patronymic) LIKE '%{searchRules.Like}%';
            """;

        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            await connection.OpenAsync();
            
            var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            
            var result = await connection.QueryMultipleAsync(
                sql,
                transaction: transaction,
                commandTimeout: (int)timeout.TotalSeconds);

            if (result is null)
            {
                throw new Exception("Ошибка при попытке получить клиентов.");
            }
            
            return (result.Read<Client>().ToArray(), result.ReadSingle<int>());
        }
        catch (NpgsqlException exception)
        {
            _logger.LogError("Ошибка при попытке получить клиента. Error: {Message}", exception.Message);

            throw new Exception("Ошибка при попытке получить клиентов.");
        }
    }

    public async Task Update(Client client, TimeSpan timeout)
    {
        const string sql = """
            UPDATE
                "Clients"
            SET
                surname = :surname,
                name = :name,
                patronymic = :patronymic,
                birthdate = :birthdate
            WHERE
                id = :id
            """;

        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            await connection.OpenAsync();
            
            var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            
            var result = await connection.ExecuteAsync(
                sql,
                new
                {
                    client.Id,
                    client.Surname,
                    client.Name,
                    client.Patronymic,
                    BirthDate = client.BirthDate
                },
                transaction,
                (int)timeout.TotalSeconds,
                CommandType.Text);

            await transaction.CommitAsync();
        }
        catch (NpgsqlException exception)
        {
            _logger.LogError("Ошибка при попытке обновить клиента. Error: {Message}", exception.Message);

            throw new Exception($"Ошибка при попытке обновить клиента с id {client.Id}.");
        }
    }
}