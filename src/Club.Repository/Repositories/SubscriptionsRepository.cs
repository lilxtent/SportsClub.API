using System.Data;
using Club.Models.Documents;
using Club.Repository.Models;
using Club.Repository.Repositories.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Club.Repository.Repositories;

public class SubscriptionsRepository : ISubscriptionsRepository
{
    private readonly string _connectionString;
    private readonly ILogger _logger;
    
    public SubscriptionsRepository(
        string connectionString,
        ILogger logger)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Add(Subscription subscription, TimeSpan timeout)
    {
        const string sql = """
            INSERT INTO "Subscriptions" 
                (id, name, price, dayslong, description)
            VALUES
                (:id, :name, :price, :dayslong, :description)
            """;

        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            await connection.OpenAsync();

            var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            await connection.ExecuteAsync(
                sql,
                new
                {
                    subscription.Id,
                    subscription.Name,
                    subscription.Price,
                    subscription.DaysLong,
                    subscription.Description
                },
                transaction,
                (int)timeout.TotalSeconds,
                CommandType.Text);

            await transaction.CommitAsync();
        }
        catch (NpgsqlException exception)
        {
            _logger.LogError("Ошибка при попытке обновить подписку. Error: {Message}", exception.Message);

            throw new Exception($"Ошибка при попытке обновить подписку с id {subscription.Id}.");
        }
    }

    public async Task<IEnumerable<Subscription>> GetAllSubscriptions(TimeSpan timeout)
    {
        var sql =
            """
            SELECT
                *
            FROM
                "Subscriptions"
            """;

        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            await connection.OpenAsync();

            var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            var result = await connection.QueryAsync<Subscription>(
                sql,
                new {},
                transaction,
                (int)timeout.TotalSeconds);

            if (result is null)
            {
                throw new Exception("Ошибка при попытке получить абонементы.");
            }

            return result;
        }
        catch (NpgsqlException exception)
        {
            _logger.LogError("Ошибка при попытке получить абонементы. Error: {Message}", exception.Message);

            throw new Exception("Ошибка при попытке получить абонементы.");
        }
    }
    
    public async Task<Subscription> GetSubscriptionById(Guid id, TimeSpan timeout)
    {
        var sql =
            """
            SELECT
                *
            FROM
                "Subscriptions"
            WHERE
                Id = :id
            """;

        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            await connection.OpenAsync();

            var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            var result = await connection.QuerySingleAsync<Subscription>(
                sql,
                new { id },
                transaction,
                (int)timeout.TotalSeconds);

            if (result is null)
            {
                throw new Exception($"Ошибка при попытке получить подписку с id {id}.");
            }

            return result;
        }
        catch (NpgsqlException exception)
        {
            _logger.LogError("Ошибка при попытке получить подписку. Error: {Message}", exception.Message);

            throw new Exception($"Ошибка при попытке получить подписку с id {id}.");
        }
    }
    
    public async Task<(Subscription[] Subscriptions, int TotalCount)> GetSubscriptions(int skip, int take, TimeSpan timeout)
    {
        var sql =
            """
            SELECT
                *
            FROM
                "Subscriptions"
            ORDER BY 
                name
            LIMIT :take OFFSET :skip;
            
            SELECT COUNT(*) FROM "Subscriptions";
            """;

        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            await connection.OpenAsync();

            var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            await using var command = new NpgsqlCommand(sql, connection, transaction);

            var result = await connection.QueryMultipleAsync(
                sql,
                new { skip, take },
                transaction,
                (int)timeout.TotalSeconds);

            if (result is null)
            {
                throw new Exception("Ошибка при попытке получить подписки.");
            }

            return (result.Read<Subscription>().ToArray(), result.ReadSingle<int>());
        }
        catch (NpgsqlException exception)
        {
            _logger.LogError("Ошибка при попытке получить подписки. Error: {Message}", exception.Message);

            throw new Exception("Ошибка при попытке получить подписки.");
        }
    }
    
    public async Task<IEnumerable<Subscription>> GetSubscriptions(ICollection<Guid> ids, TimeSpan timeout)
    {
        var sql =
            $"""
            SELECT
                *
            FROM
                "Subscriptions"
            WHERE
                Id IN ({string.Join(',', ids.Select(x => $"'{x}'"))})
            """;

        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            await connection.OpenAsync();

            var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            var result = await connection.QueryAsync<Subscription>(
                sql,
                transaction: transaction,
                commandTimeout: (int)timeout.TotalSeconds);

            if (result is null)
            {
                throw new Exception($"Ошибка при попытке получить абонемент с id {string.Join(',', ids)}.");
            }

            return result;
        }
        catch (NpgsqlException exception)
        {
            _logger.LogError("Ошибка при попытке получить абонемент. Error: {Message}", exception.Message);

            throw new Exception($"Ошибка при попытке получить абонемент с id {string.Join(',', ids)}.");
        }
    }
    
    public async Task Update(Subscription subscription, TimeSpan timeout)
    {
        const string sql = """
            UPDATE
                "Subscriptions"
            SET
                name = :name,
                price = :price,
                dayslong = :dayslong,
                description = :description
            WHERE
                id = :id
            """;

        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            await connection.OpenAsync();

            var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            await connection.ExecuteAsync(
                sql,
                new
                {
                    subscription.Id,
                    subscription.Name,
                    subscription.Price,
                    subscription.DaysLong,
                    subscription.Description
                },
                transaction,
                (int)timeout.TotalSeconds,
                CommandType.Text);

            await transaction.CommitAsync();
        }
        catch (NpgsqlException exception)
        {
            _logger.LogError("Ошибка при попытке обновить абонемент. Error: {Message}", exception.Message);

            throw new Exception($"Ошибка при попытке обновить абонемент с id {subscription.Id}.");
        }
    }
    
    public async Task<(Subscription[] Subscriptions, int TotalCount)> Search(SubscriptionSearchRules searchRules, TimeSpan timeout)
    {
        var sql =
            $"""
            SELECT
                *
            FROM
                "Subscriptions"
            WHERE
                name LIKE '%{searchRules.Like}%'
            ORDER BY
                name;
                
            SELECT
                COUNT(*)
            FROM
                "Subscriptions"
            WHERE
                name LIKE '%{searchRules.Like}%';
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
                throw new Exception("Ошибка при попытке получить абонементы.");
            }

            return (result.Read<Subscription>().ToArray(), result.ReadSingle<int>());
        }
        catch (NpgsqlException exception)
        {
            _logger.LogError("Ошибка при попытке получить абонементы. Error: {Message}", exception.Message);

            throw new Exception("Ошибка при попытке получить абонементы.");
        }
    }
}