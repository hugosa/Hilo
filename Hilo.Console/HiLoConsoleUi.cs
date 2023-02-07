namespace Hilo.ConsoleApp;

using Hilo.Application.Commands;
using Hilo.Application.Model;
using Hilo.ConsoleApp.Wrappers.Console;
using Hilo.ConsoleApp.Wrappers.Environment;
using MediatR;

public class HiloConsoleUi
{
    private readonly IMediator mediator;
    private readonly IConsoleIO console;
    private readonly IRunningEnvironment environment;
    private readonly Queue<(Guid Id, string Name)> players;
    private Guid gameId;

    public HiloConsoleUi(IMediator mediator, IConsoleIO console, IRunningEnvironment environment)
    {
        this.mediator = mediator;
        this.console = console;
        this.environment = environment;
        this.players = new Queue<(Guid Id, string Name)>();
    }

    public async Task<bool> SetupGameAsync()
    {
        this.console.WriteLine("Welcome to Hi-LO from Gaming1.");
        this.console.WriteLine("Would you like to start a new game? (y/n)");

        ConsoleKeyInfo key = this.console.ReadKey(true);
        if (key.Key == ConsoleKey.Y)
        {
            try
            {
                this.gameId = await this.mediator.Send(new SetupNewGameCommand());
            }
            catch (ExpectedException ex)
            {
                HandleException(ex);
                EndGame();
            }
        }

        return this.gameId != default;
    }

    public async Task AddPlayersAsync()
    {
        this.console.WriteLine("Would you like to add a new player? (y/n)");
        try
        {
            await LocalAddPlayerAsync();
        }
        catch (ExpectedException ex)
        {
            HandleException(ex);
            EndGame();
        }

        async Task LocalAddPlayerAsync()
        {
            ConsoleKeyInfo key = this.console.ReadKey(true);
            if (key.Key == ConsoleKey.Y)
            {
                await LocalAddPlayerNameAsync();

                this.console.WriteLine("Would you like to add another player? (y/n)");
                await LocalAddPlayerAsync();
            }
            else if (!this.players.Any())
            {
                this.console.WriteLine("No players have been added to the game and you need at least one player registered to play. The game will end now.");
                EndGame();
            }
        }

        async Task LocalAddPlayerNameAsync()
        {
            this.console.WriteLine("Please enter the players's name.");

            var name = this.console.ReadLine();
            if (!string.IsNullOrEmpty(name))
            {
                Guid playerId = await this.mediator.Send(new RegisterNewPlayerCommand(this.gameId, name));
                this.players.Enqueue((playerId, name));

                this.console.WriteLine($"Welcome {name}");
            }
            else
            {
                this.console.WriteLine($"The player's name can't be empty.");
            }
        }
    }

    public async Task PlayAsync()
    {
        try
        {
            HiLoRangeDto gameRange = await this.mediator.Send(new StartNewGameCommand(this.gameId));

            this.console.WriteLine(@$"Welcome players! Your mistery number is comprised between {gameRange.Start} and {gameRange.End}. Good luck!");
            this.console.WriteLine(@"Press any key to start playing.");
            _ = this.console.ReadKey(true);
        }
        catch (ExpectedException ex)
        {
            HandleException(ex);
            goto Exit;
        }

        while (true)
        {
            (Guid Id, string Name) player = this.players.Peek();

            this.console.WriteLine($"Your turn {player.Name}. Please type in your guess number.");
            var input = this.console.ReadLine();

            if (!int.TryParse(input, out var number))
            {
                this.console.WriteLine("Your input is not valid. It must be a number. Please try again.");
                continue;
            }

            string? result;
            try
            {
                result = await this.mediator.Send(new SelectNumberCommand(this.gameId, player.Id, number));
            }
            catch (ExpectedException ex)
            {
                HandleException(ex);
                goto Exit;
            }

            if (result == "Win")
            {
                this.console.WriteLine(ParsePlayResult(result, player.Name, number));
                break;
            }

            this.console.WriteLine(ParsePlayResult(result, player.Name, number));
            _ = this.console.ReadKey(true);

            this.players.Enqueue(this.players.Dequeue());
        }

    Exit:
        EndGame();
    }

    private void EndGame()
    {
        this.console.WriteLine(@"Thank you from playing Hi-LO from Gaming1.");
        this.console.WriteLine(@"Press any key to exit.");
        _ = this.console.ReadKey(true);

        this.environment.Exit(0);
    }

    private string ParsePlayResult(string result, string player, int number)
    {
        var map = new Dictionary<string, string>
        {
            { "Win", $"Congratulations {player}. {number} is your Magic Number. Your are the winner!!!!" },
            { "Higher", @$"Better luck next time {player}. Your Magic Number is higher than {number}. Please hit any key to pass the turn." },
            { "Lower", @$"Better luck next time {player}. Your Magic Number is lower than {number}. Please hit any key to pass the turn." }
        };

        return map[result];
    }

    private void HandleException(ExpectedException ex)
    {
        this.console.WriteLine("Something went wrong with your action.");
        this.console.WriteLine($"{ex.Message}.");
    }
}