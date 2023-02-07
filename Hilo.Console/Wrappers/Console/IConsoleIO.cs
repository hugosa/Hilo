namespace Hilo.ConsoleApp.Wrappers.Console;

public interface IConsoleIO
{
    void WriteLine(string line);
    string? ReadLine();
    ConsoleKeyInfo ReadKey(bool intercept);
    ConsoleKeyInfo ReadKey();
}