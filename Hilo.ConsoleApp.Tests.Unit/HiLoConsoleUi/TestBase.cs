namespace Hilo.ConsoleApp.Tests.Unit.HiLoConsoleUi;
using Hilo.ConsoleApp.Wrappers.Console;
using Hilo.ConsoleApp.Wrappers.Environment;
using MediatR;
using Moq;

public abstract class TestBase
{
    protected readonly Mock<IMediator> mediator;
    protected readonly Mock<IConsoleIO> console;
    protected readonly Mock<IRunningEnvironment> environment;

    protected TestBase()
    {
        this.mediator = new Mock<IMediator>();
        this.console = new Mock<IConsoleIO>();
        this.environment = new Mock<IRunningEnvironment>();
    }

    protected void SetupGameEnding()
    {
        _ = this.console.Setup(c => c.WriteLine("Thank you from playing Hi-LO from Gaming1."));
        _ = this.console.Setup(c => c.WriteLine("Press any key to exit."));
    }

    protected void VerifyGameEnding()
    {
        this.console.Verify(c => c.WriteLine("Thank you from playing Hi-LO from Gaming1."));
        this.console.Verify(c => c.WriteLine("Press any key to exit."));
        this.environment.Verify(e => e.Exit(0));
    }

    protected void SetupExceptionHandling(string exceptionMessage)
    {
        _ = this.console.Setup(c => c.WriteLine("Something went wrong with your action."));
        _ = this.console.Setup(c => c.WriteLine(exceptionMessage));
    }

    protected void VerifyExceptionHandling(string exceptionMessage)
    {
        this.console.Verify(c => c.WriteLine("Something went wrong with your action."));
        this.console.Verify(c => c.WriteLine(exceptionMessage));
    }
}
