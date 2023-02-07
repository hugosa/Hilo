namespace Hilo.Domain.Tests.Unit.HiLoGame;

using FluentAssertions;
using Hilo.Domain.Exceptions;
using Xunit;

using HiLoGame = Domain.HiLoGame;

public class AddNewPlayer : TestBase
{
    [Fact]
    public void Should_Throw_ActionNotSupportedAfterGameStartException_If_Trying_To_Register_Player_After_Game_Starts()
    {
        var gameState = new HiLoGame.GameState(Guid.NewGuid(), this.range, this.playerState, true);

        var uut = HiLoGame.RestoreGame(gameState);
        Action action = () => uut.AddNewPlayer(new Player("New Player"));

        _ = action.Should().Throw<ActionNotSupportedAfterGameStartException>();
    }
}
