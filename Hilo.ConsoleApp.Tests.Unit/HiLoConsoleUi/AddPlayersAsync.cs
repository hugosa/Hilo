namespace Hilo.ConsoleApp.Tests.Unit.HiLoConsoleUi;

using Hilo.Application.Commands;
using Hilo.Application.Model;
using Moq;
using Xunit;

public class AddPlayersAsync : TestBase
{
    [Fact]
    public async void Should_Write_Warning_Message_And_Ask_For_New_Player_If_User_Name_IsNullOrEmpty_String()
    {
        var msg1 = "The player's name can't be empty.";
        var msg2 = "Would you like to add another player? (y/n)";

        _ = this.console.SetupSequence(c => c.ReadKey(It.IsAny<bool>()))
                        .Returns(new ConsoleKeyInfo('y', ConsoleKey.Y, false, false, false))
                        .Returns(new ConsoleKeyInfo('n', ConsoleKey.N, false, false, false));
        _ = this.console.Setup(c => c.ReadLine())
                        .Returns(string.Empty);
        _ = this.console.Setup(c => c.WriteLine(msg1));
        _ = this.console.Setup(c => c.WriteLine(msg2));

        var sut = new HiloConsoleUi(this.mediator.Object, this.console.Object, this.environment.Object);
        await sut.AddPlayersAsync();

        this.console.Verify(c => c.WriteLine(msg1));
        this.console.Verify(c => c.WriteLine(msg2));
    }

    [Fact]
    public async void Should_Write_Warning_Message_And_Exit_Game_If_No_User_Has_Been_Registered()
    {
        var warningMessage = "No players have been added to the game and you need at least one player registered to play. The game will end now.";

        SetupGameEnding();

        _ = this.console.Setup(c => c.WriteLine(warningMessage));
        _ = this.console.Setup(c => c.ReadKey(It.IsAny<bool>()))
                        .Returns(new ConsoleKeyInfo('n', ConsoleKey.N, false, false, false));

        var sut = new HiloConsoleUi(this.mediator.Object, this.console.Object, this.environment.Object);
        await sut.AddPlayersAsync();

        this.console.Verify(c => c.WriteLine(warningMessage));
        VerifyGameEnding();
    }

    [Fact]
    public async void Should_Write_Exit_Message_And_Exit_Application_If_Exception_Was_Thrown()
    {
        var exceptionMessage = @"Test Message.";

        SetupExceptionHandling(exceptionMessage);
        SetupGameEnding();

        _ = this.mediator.Setup(m => m.Send(It.IsAny<RegisterNewPlayerCommand>(), It.IsAny<CancellationToken>()))
                         .Throws(new ExpectedException("Test Message", It.IsAny<Exception>()));

        _ = this.console.Setup(c => c.ReadKey(It.IsAny<bool>()))
                        .Returns(new ConsoleKeyInfo('y', ConsoleKey.Y, false, false, false));
        _ = this.console.Setup(c => c.ReadLine())
                        .Returns("Test Player");

        var sut = new HiloConsoleUi(this.mediator.Object, this.console.Object, this.environment.Object);
        await sut.AddPlayersAsync();

        VerifyExceptionHandling(exceptionMessage);
        VerifyGameEnding();
    }
}