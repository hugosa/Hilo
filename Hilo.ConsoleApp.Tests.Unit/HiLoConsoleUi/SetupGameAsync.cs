namespace Hilo.ConsoleApp.Tests.Unit.HiLoConsoleUi;

using FluentAssertions;
using Hilo.Application.Commands;
using Hilo.Application.Model;
using Moq;
using Xunit;

public class SetupGameAsync : TestBase
{
    [Fact]
    public async void Should_Return_True_If_Game_Id_Is_Not_Default()
    {
        var gameId = Guid.NewGuid();

        _ = this.mediator.Setup(m => m.Send(It.IsAny<SetupNewGameCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(gameId);

        _ = this.console.Setup(c => c.ReadKey(It.IsAny<bool>()))
                        .Returns(new ConsoleKeyInfo('y', ConsoleKey.Y, false, false, false));

        var sut = new HiloConsoleUi(this.mediator.Object, this.console.Object, this.environment.Object);
        var result = await sut.SetupGameAsync();

        _ = result.Should().Be(true);
    }

    [Fact]
    public async void Should_Return_False_If_Game_Id_Is_Default()
    {
        Guid gameId = default;

        _ = this.mediator.Setup(m => m.Send(It.IsAny<SetupNewGameCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(gameId);

        _ = this.console.Setup(c => c.ReadKey(It.IsAny<bool>()))
                        .Returns(new ConsoleKeyInfo('y', ConsoleKey.Y, false, false, false));

        var sut = new HiloConsoleUi(this.mediator.Object, this.console.Object, this.environment.Object);
        var result = await sut.SetupGameAsync();

        _ = result.Should().Be(false);
    }

    [Fact]
    public async void Should_Exit_Application_If_Exception_Was_Thrown()
    {
        _ = this.mediator.Setup(m => m.Send(It.IsAny<SetupNewGameCommand>(), It.IsAny<CancellationToken>()))
                         .Throws(new ExpectedException("Test Message", It.IsAny<Exception>()));

        _ = this.console.Setup(c => c.ReadKey(It.IsAny<bool>()))
                        .Returns(new ConsoleKeyInfo('y', ConsoleKey.Y, false, false, false));

        var sut = new HiloConsoleUi(this.mediator.Object, this.console.Object, this.environment.Object);
        _ = await sut.SetupGameAsync();

        VerifyExceptionHandling(@"Test Message.");
        VerifyGameEnding();
    }
}