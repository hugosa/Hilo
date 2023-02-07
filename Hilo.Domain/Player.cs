namespace Hilo.Domain;

public sealed record Player
{
    public Player(string playerName)
    {
        this.PlayerId = Guid.NewGuid();
        this.PlayerName = playerName;
    }

    public Guid PlayerId { get; }
    public string PlayerName { get; }
}