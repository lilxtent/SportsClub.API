using System.Data;
using Club.Models.Statistics;
using Club.Repository.Repositories.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Club.Repository.Repositories;

public class StatisticsRepository : IStatisticsRepository
{
    private readonly string _connectionString;
    private readonly ILogger _logger;

    public StatisticsRepository(
        string connectionString,
        ILogger logger)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<VisitStat>> GetVisitsStat(TimeSpan timeout)
    {
        var sql =
            """
            SELECT
                date_part('day', datetime) as day,
                COUNT(*) as "VisitsCount"
            FROM
                "Visits"
            WHERE
                datetime > (CURRENT_TIMESTAMP - INTERVAL '7 DAY')
            GROUP BY
                day
            ORDER BY
                day
            """;

        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            await connection.OpenAsync();

            var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            var result = await connection.QueryAsync<VisitStat>(
                sql,
                new { },
                transaction,
                (int)timeout.TotalSeconds);

            if (result is null)
            {
                throw new Exception("Ошибка при попытке получить статистику.");
            }

            return result;
        }
        catch (NpgsqlException exception)
        {
            _logger.LogError("Ошибка при попытке получить статистику. Error: {Message}", exception.Message);

            throw new Exception("Ошибка при попытке получить статистику.");
        }
    }
    
    public async Task<IEnumerable<SubscriptionsStat>> GetSubscriptionsStat(TimeSpan timeout)
    {
        var sql =
            """
            SELECT
                (SELECT name FROM "Subscriptions" WHERE id = subscriptionid) as "SubscriptionName",
                COUNT(subscriptionid) as "Count"
            FROM
                "Payments"
            WHERE
                current_timestamp < subscriptionendtime
            GROUP BY
                "SubscriptionName"
            """;

        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            await connection.OpenAsync();

            var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            var result = await connection.QueryAsync<SubscriptionsStat>(
                sql,
                new { },
                transaction,
                (int)timeout.TotalSeconds);

            if (result is null)
            {
                throw new Exception("Ошибка при попытке получить статистику.");
            }

            return result;
        }
        catch (NpgsqlException exception)
        {
            _logger.LogError("Ошибка при попытке получить статистику. Error: {Message}", exception.Message);

            throw new Exception("Ошибка при попытке получить статистику.");
        }
    }
    
    public async Task<IEnumerable<PaymentStat>> GetPaymentsStat(TimeSpan timeout)
    {
        var sql =
            """
            SELECT
                sum(paymentamount) as "SumTotal",
                date_part('month', paymentdate) as "Month"
            FROM
                "Payments"
            WHERE
                "Payments".paymentdate > (CURRENT_TIMESTAMP - INTERVAL '12 YEAR')
            GROUP BY
                "Month"
            ORDER BY
                "Month"
            """;

        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            await connection.OpenAsync();

            var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            var result = await connection.QueryAsync<PaymentStat>(
                sql,
                new { },
                transaction,
                (int)timeout.TotalSeconds);

            if (result is null)
            {
                throw new Exception("Ошибка при попытке получить статистику.");
            }

            return result;
        }
        catch (NpgsqlException exception)
        {
            _logger.LogError("Ошибка при попытке получить статистику. Error: {Message}", exception.Message);

            throw new Exception("Ошибка при попытке получить статистику.");
        }
    }
}