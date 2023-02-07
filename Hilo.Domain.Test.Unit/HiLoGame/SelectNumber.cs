namespace Hilo.Domain.Tests.Unit.HiLoGame;

using FluentAssertions;
using Hilo.Domain.Exceptions;
using Xunit;

using HiLoGame = Domain.HiLoGame;

public class SelectNumber : TestBase
{
    [Fact]
    public void Should_Return_Win_When_Magic_Number_Equals_Selected_Number()
    {
        var selectedNumber = 5;
        var gameState = new HiLoGame.GameState(Guid.NewGuid(), this.range, this.playerState, true);

        var uut = HiLoGame.RestoreGame(gameState);
        HiLoGame.PlayResult result = uut.SelectNumber(this.player.PlayerId, selectedNumber);

        _ = result.Should().Be(HiLoGame.PlayResult.Win);
    }

    [Fact]
    public void Should_Return_Higher_When_Magic_Number_Higher_Than_Selected_Number()
    {
        var selectedNumber = 4;
        var gameState = new HiLoGame.GameState(Guid.NewGuid(), this.range, this.playerState, true);

        var uut = HiLoGame.RestoreGame(gameState);
        HiLoGame.PlayResult result = uut.SelectNumber(this.player.PlayerId, selectedNumber);

        _ = result.Should().Be(HiLoGame.PlayResult.Higher);
    }

    [Fact]
    public void Should_Return_Lower_When_Magic_Number_Lower_Than_Selected_Number()
    {
        var selectedNumber = 6;
        var gameState = new HiLoGame.GameState(Guid.NewGuid(), this.range, this.playerState, true);

        var uut = HiLoGame.RestoreGame(gameState);
        HiLoGame.PlayResult result = uut.SelectNumber(this.player.PlayerId, selectedNumber);

        _ = result.Should().Be(HiLoGame.PlayResult.Lower);
    }

    [Fact]
    public void Should_Throw_ActionNotSupportedBeforeGameStartException_If_Game_Not_Started()
    {
        var selectedNumber = 5;
        var gameState = new HiLoGame.GameState(Guid.NewGuid(), this.range, this.playerState, false);

        var uut = HiLoGame.RestoreGame(gameState);
        Func<HiLoGame.PlayResult> action = () => uut.SelectNumber(this.player.PlayerId, selectedNumber);

        _ = action.Should().Throw<ActionNotSupportedBeforeGameStartException>();
    }

    [Fact]
    public void Should_Throw_PlayerNotFoundException_If_Unregistered_Player_Attempts_To_Play()
    {
        var selectedNumber = 5;
        var gameState = new HiLoGame.GameState(Guid.NewGuid(), this.range, this.playerState, true);

        var uut = HiLoGame.RestoreGame(gameState);
        Func<HiLoGame.PlayResult> action = () => uut.SelectNumber(Guid.NewGuid(), selectedNumber);

        _ = action.Should().Throw<PlayerNotFoundException>();
    }
}