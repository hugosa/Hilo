namespace Hilo.ConsoleApp.Wrappers.Console;
using System;
public class ConsoleIO : IConsoleIO
{
    public void WriteLine(string line) => Console.WriteLine(line);
    public string? ReadLine() => Console.ReadLine();
    public ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(intercept);
    public ConsoleKeyInfo ReadKey() => Console.ReadKey();
}
