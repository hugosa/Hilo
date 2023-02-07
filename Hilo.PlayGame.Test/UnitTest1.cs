namespace Hilo.PlayGame.Test;

using MediatR;
using Microsoft.Extensions.Hosting;
using Xunit;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Host.CreateDefaultBuilder().ConfigureServices((hostContext, services) =>
        {
            _ = services.AddApplication()
                        .AddInfrastructure()
                        .AddScoped<HiloConsoleUi>()
                        .AddSingleton<IConsoleIO, ConsoleIO>()
                        .AddSingleton<IRunningEnvironment, RunningEnvironment>();
        }
        var mediator = new Mediator(new ServiceFactory())
    }
}