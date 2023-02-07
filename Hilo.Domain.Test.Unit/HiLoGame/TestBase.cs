namespace Hilo.Domain.Tests.Unit.HiLoGame;
using HiLoGame = Domain.HiLoGame;

public class TestBase
{
    protected readonly Player player;
    protected readonly ICollection<(Player player, int magicNumber)> playerState;
    protected readonly HiLoGame.GameRange range;

    public TestBase()
    {
        this.player = new Player("Test Player");
        this.range = new HiLoGame.GameRange(1, 10);
        this.playerState = new List<(Player player, int magicNumber)>()
        {
           new (this.player, 5)
        };
    }
}