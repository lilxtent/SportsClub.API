using Club.Logic.Services;
using Club.Logic.Services.Interfaces;
using Club.Repository.Repositories;
using Club.Repository.Repositories.Interfaces;
using Newtonsoft.Json;

namespace Clients.API.Extensions;

public static class IServiceCollectionExtensions
{
    public static void AddDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSettings()
            .AddRepositories()
            .AddSingleton<IVisitsService, VisitsService>();
    }

    private static IServiceCollection AddSettings(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton(
            _ => JsonConvert.DeserializeObject<Settings.Settings>(File.ReadAllText("settings.json")));
    }

    private static IClientsRepository CreateClientsRepository(IServiceProvider serviceProvider)
    {
        var settings = serviceProvider.GetService<Settings.Settings>();
        var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<ClientsRepository>();

        return new ClientsRepository(
            settings.ClientsConnectionString,
            logger
        );
    }
    
    private static IVisitsRepository CreateVisitsRepository(IServiceProvider serviceProvider)
    {
        var settings = serviceProvider.GetService<Settings.Settings>();
        var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<VisitsRepository>();

        return new VisitsRepository(
            settings.ClientsConnectionString,
            logger
        );
    }

    private static ISubscriptionsRepository CreateSubscriptionsRepository(IServiceProvider serviceProvider)
    {
        var settings = serviceProvider.GetService<Settings.Settings>();
        var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<SubscriptionsRepository>();

        return new SubscriptionsRepository(
            settings.ClientsConnectionString,
            logger
        );
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton(CreateClientsRepository)
            .AddSingleton(CreateVisitsRepository)
            .AddSingleton(CreateSubscriptionsRepository);
    }
}