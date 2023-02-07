namespace Hilo.ConsoleApp;

using Hilo.Application;
using Hilo.ConsoleApp.Wrappers.Console;
using Hilo.ConsoleApp.Wrappers.Environment;
using Hilo.Infrastructure;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class Program
{
    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
        {
            _ = services.AddApplication()
                        .AddInfrastructure()
                        .AddScoped<HiloConsoleUi>()
                        .AddSingleton<IConsoleIO, ConsoleIO>()
                        .AddSingleton<IRunningEnvironment, RunningEnvironment>();
        });
    }

    private static async Task Main(string[] args)
    {
        IHost host = CreateHostBuilder(args).Build();

        HiloConsoleUi gameUi = ActivatorUtilities.GetServiceOrCreateInstance<HiloConsoleUi>(host.Services);

        if (await gameUi.SetupGameAsync())
        {
            await gameUi.AddPlayersAsync();
            await gameUi.PlayAsync();
        }
    }
}