using Hilo.Application;
using Hilo.Application.Commands;
using Hilo.Application.Model;
using Hilo.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class Program
{
    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
        {
            _ = services.AddApplication().AddInfrastructure();
        });
    }

    private static async Task Main(string[] args)
    {
        IHost host = CreateHostBuilder(args).Build();
        IMediator mediator = ActivatorUtilities.GetServiceOrCreateInstance<IMediator>(host.Services);

        for (var i = 1; i <= 500; i++)
        {
            Guid gameId = await mediator.Send(new SetupNewGameCommand());
            Guid playerId = await mediator.Send(new RegisterNewPlayerCommand(gameId, "Hugo Sá"));
            HiLoRangeDto range = await mediator.Send(new StartNewGameCommand(gameId));

            var array = Enumerable.Range(range.Start, range.End - range.Start + 1).ToArray();
            
            var guess = array[array.Length / 2];
            var response = await mediator.Send(new SelectNumberCommand(gameId, playerId, guess));
            var attempts = 1;

            while (response != "Win")
            {
                var middle = array.Length / 2;
                array = response == "Higher" ? array.Skip(middle).ToArray() : array.Take(middle).ToArray();

                guess = array[array.Length / 2];
                response = await mediator.Send(new SelectNumberCommand(gameId, playerId, guess));
                attempts++;
            }

            Console.WriteLine($"Round {i}: You have beaten the game in {attempts} attempts. Your Magic Number was the number {guess}");
        }

        _ = Console.ReadKey();
    }
}