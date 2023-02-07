namespace Hilo.Domain;

using Hilo.Domain.Base;
using Hilo.Domain.Exceptions;

public sealed record HiLoGame
{
    private readonly Dictionary<Guid, (Player player, int magicNumber)> playerMagicNumbers;
    private bool gameStarted;
    private readonly GameRange gameRange;

    public static HiLoGame SetupNewGame() => new();

    public static HiLoGame RestoreGame(GameState gamestate) => new(gamestate.GameId, gamestate.GameRange, gamestate.PlayerState, gamestate.GameStarted);

    private HiLoGame()
    {
        this.GameId = Guid.NewGuid();
        this.playerMagicNumbers = new Dictionary<Guid, (Player player, int magicNumber)>();

        Random rand = new(Guid.NewGuid().GetHashCode());

        var end = rand.Next(600, 999);
        var start = rand.Next(0, 400);

        this.gameRange = new(start, end);
    }

    private HiLoGame(Guid gameId, GameRange gameRange, ICollection<(Player player, int magicNumber)> playerState, bool gameStarted)
    {
        this.GameId = gameId;
        this.playerMagicNumbers = playerState.ToDictionary(p => p.player.PlayerId, p => p);
        this.gameRange = gameRange;
        this.gameStarted = gameStarted;
    }

    public Guid GameId { get; init; }

    public PlayResult SelectNumber(Guid playerId, int number)
    {
        if (!this.gameStarted)
        {
            throw new ActionNotSupportedBeforeGameStartException(this.GameId);
        }

        return !this.playerMagicNumbers.TryGetValue(playerId, out var playerEntry)
            ? throw new PlayerNotFoundException(this.GameId, playerId)
            : number == playerEntry.magicNumber ? PlayResult.Win : playerEntry.magicNumber > number ? PlayResult.Higher : PlayResult.Lower;
    }

    public void AddNewPlayer(Player player)
    {
        if (this.gameStarted)
        {
            throw new ActionNotSupportedAfterGameStartException(this.GameId);
        }

        this.playerMagicNumbers.Add(player.PlayerId, (player, GenerateMagicNumber()));
    }

    public GameRange StartGame()
    {
        this.gameStarted = true;
        return this.gameRange;
    }

    public GameState ExportGameState() => new(this.GameId, this.gameRange, this.playerMagicNumbers.Values, this.gameStarted);

    private int GenerateMagicNumber() => new Random(Guid.NewGuid().GetHashCode()).Next(this.gameRange.Start, this.gameRange.End);

    public record GameState(Guid GameId, GameRange GameRange, ICollection<(Player player, int magicNumber)> PlayerState, bool GameStarted);

    public record GameRange(int Start, int End);

    public class PlayResult : Enumeration
    {
        public static readonly PlayResult Win = new(1, nameof(Win));
        public static readonly PlayResult Higher = new(2, nameof(Higher));
        public static readonly PlayResult Lower = new(3, nameof(Lower));

        private PlayResult(int id, string name) : base(id, name)
        {
        }
    }
}