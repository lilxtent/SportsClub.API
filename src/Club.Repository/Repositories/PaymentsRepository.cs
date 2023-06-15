using System.Data;
using Club.Models.Documents;
using Club.Repository.Repositories.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Club.Repository.Repositories;

public class PaymentsRepository : IPaymentsRepository
{
    private readonly string _connectionString;
    private readonly ILogger _logger;
    
    public PaymentsRepository(
        string connectionString,
        ILogger logger)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task Add(Guid clientId, Subscription subscription, TimeSpan timeout)
    {
        var sql =
            """
            INSERT INTO "Payments"
                (clientid, subscriptionid, paymentdate, subscriptionstarttime, subscriptionendtime, paymentamount)
            VALUES
                (:clientid, :subscriptionid, :paymentdate, :subscriptionstarttime, :subscriptionendtime, :paymentamount);
            """;
        
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            await connection.OpenAsync();
            
            var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            await using var command = new NpgsqlCommand(sql, connection, transaction);
            
            await connection.ExecuteAsync(
                sql,
                new
                {
                    clientId,
                    subscriptionId = subscription.Id,
                    paymentDate = DateTime.Now,
                    subscriptionStartTime = DateTime.Now,
                    subscriptionEndTime = DateTime.Now.AddDays(subscription.DaysLong),
                    paymentAmount = subscription.Price
                },
                transaction,
                (int)timeout.TotalSeconds,
                CommandType.Text);

            await transaction.CommitAsync();

        }
        catch (NpgsqlException exception)
        {
            _logger.LogError("Ошибка при попытке добавить платеж. Error: {Message}", exception.Message);

            throw new Exception("Ошибка при попытке добавить платеж.");
        }
    }
    
    public async Task<Payment> GetClientLastPayment(Guid clientId, TimeSpan timeout)
    {
        var sql = 
            """
            SELECT
                *
            FROM
                "Payments"
            WHERE
                clientid = :clientId
            ORDER BY 
                paymentdate DESC
            LIMIT 1;
            """;

        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            await connection.OpenAsync();
            
            var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            await using var command = new NpgsqlCommand(sql, connection, transaction);
            
            var result = await connection.QuerySingleOrDefaultAsync<Payment>(
                sql,
                new {clientId},
                transaction,
                (int)timeout.TotalSeconds);

            return result;
        }
        catch (NpgsqlException exception)
        {
            _logger.LogError("Ошибка при попытке получить платеж. Error: {Message}", exception.Message);

            throw new Exception("Ошибка при попытке получить платеж.");
        }
    }
    
    public async Task<(Payment[] Payments, int TotalCount)> GetLastPayments(int skip, int take, TimeSpan timeout)
    {
        var sql = 
            """
            SELECT
                *
            FROM
                "Payments"
            ORDER BY  
                paymentdate DESC
            LIMIT :take OFFSET :skip;
            
            SELECT COUNT(*) FROM "Payments";
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
                throw new Exception("Ошибка при попытке получить платеж.");
            }
            
            return (result.Read<Payment>().ToArray(), result.ReadSingle<int>());
        }
        catch (NpgsqlException exception)
        {
            _logger.LogError("Ошибка при попытке получить платеж. Error: {Message}", exception.Message);

            throw new Exception("Ошибка при попытке получить платеж.");
        }
    }
}