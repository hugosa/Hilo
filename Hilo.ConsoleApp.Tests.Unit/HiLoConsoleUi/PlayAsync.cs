namespace Hilo.ConsoleApp.Tests.Unit.HiLoConsoleUi;

using Hilo.Application.Commands;
using Hilo.Application.Model;
using Moq;
using Xunit;

public class PlayAsync : TestBase
{
    [Fact]
    public async void Should_Write_Exit_Message_And_Exit_Application_If_Exception_Was_Thrown_Executing_StartNewGameCommand()
    {
        var exceptionMessage = @"Test Message.";

        SetupExceptionHandling(exceptionMessage);
        SetupGameEnding();

        _ = this.mediator.Setup(m => m.Send(It.IsAny<StartNewGameCommand>(), It.IsAny<CancellationToken>()))
                         .Throws(new ExpectedException("Test Message", It.IsAny<Exception>()));

        var sut = new HiloConsoleUi(this.mediator.Object, this.console.Object, this.environment.Object);
        await sut.PlayAsync();

        VerifyExceptionHandling(exceptionMessage);
        VerifyGameEnding();
    }

    [Fact]
    public async void Should_Write_Exit_Message_And_Exit_Application_If_Exception_Was_Thrown_Executing_SelectNumberCommand()
    {
        var exceptionMessage = @"Test Message.";

        SetupExceptionHandling(exceptionMessage);
        SetupGameEnding();

        _ = this.mediator.Setup(m => m.Send(It.IsAny<StartNewGameCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new HiLoRangeDto(1, 10));
        _ = this.mediator.Setup(m => m.Send(It.IsAny<RegisterNewPlayerCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(It.IsAny<Guid>());
        _ = this.mediator.Setup(m => m.Send(It.IsAny<SelectNumberCommand>(), It.IsAny<CancellationToken>()))
                          .Throws(new ExpectedException("Test Message", It.IsAny<Exception>()));

        _ = this.console.SetupSequence(c => c.ReadKey(It.IsAny<bool>()))
                        .Returns(new ConsoleKeyInfo('y', ConsoleKey.Y, false, false, false))
                        .Returns(new ConsoleKeyInfo('n', ConsoleKey.N, false, false, false));
        _ = this.console.SetupSequence(c => c.ReadLine())
                        .Returns("Test Player")
                        .Returns("5");

        var sut = new HiloConsoleUi(this.mediator.Object, this.console.Object, this.environment.Object);
        await sut.AddPlayersAsync();
        await sut.PlayAsync();

        VerifyExceptionHandling(exceptionMessage);
        VerifyGameEnding();
    }

    [Fact]
    public async void Should_Write_Warning_Message_And_Retry_If_Magic_Number_Input_Is_Not_Integer()
    {
        _ = this.mediator.Setup(m => m.Send(It.IsAny<StartNewGameCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new HiLoRangeDto(1, 10));
        _ = this.mediator.Setup(m => m.Send(It.IsAny<RegisterNewPlayerCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(It.IsAny<Guid>());
        _ = this.mediator.Setup(m => m.Send(It.IsAny<SelectNumberCommand>(), It.IsAny<CancellationToken>()))
                          .Throws(new ExpectedException(It.IsAny<string>(), It.IsAny<Exception>()));

        _ = this.console.SetupSequence(c => c.ReadKey(It.IsAny<bool>()))
                        .Returns(new ConsoleKeyInfo('y', ConsoleKey.Y, false, false, false))
                        .Returns(new ConsoleKeyInfo('n', ConsoleKey.N, false, false, false));
        _ = this.console.SetupSequence(c => c.ReadLine())
                        .Returns("Test Player")
                        .Returns("Not a number")
                        .Returns("5");

        var sut = new HiloConsoleUi(this.mediator.Object, this.console.Object, this.environment.Object);
        await sut.AddPlayersAsync();
        await sut.PlayAsync();

        this.console.Verify(c => c.WriteLine("Your input is not valid. It must be a number. Please try again."));
        this.console.Verify(c => c.WriteLine("Your turn Test Player. Please type in your guess number."));
    }
}