using System.Data;
using Club.Models.Documents;
using Club.Repository.Repositories.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Club.Repository.Repositories;

public class VisitsRepository : IVisitsRepository
{
    private readonly string _connectionString;
    private readonly ILogger _logger;
    
    public VisitsRepository(
        string connectionString,
        ILogger logger)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<(Visit[] Visits, int TotalCount)> GetLastVisits(int skip, int take, TimeSpan timeout)
    {
        var sql = 
            """
            SELECT
                *
            FROM
                "Visits"
            ORDER BY 
                datetime
            LIMIT :take OFFSET :skip;
            
            SELECT COUNT(*) FROM "Visits";
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
            
            return (result.Read<Visit>().ToArray(), result.ReadSingle<int>());
        }
        catch (NpgsqlException exception)
        {
            _logger.LogError("Ошибка при попытке получить клиента. Error: {Message}", exception.Message);

            throw new Exception("Ошибка при попытке получить клиентов.");
        }
    }
}