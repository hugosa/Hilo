namespace Hilo.Infrastructure;

using Hilo.Application.Repositories;
using Hilo.Domain;

using Microsoft.Extensions.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services.AddSingleton<IRepository<HiLoGame.GameState>, InMemoryRepository>();
}

