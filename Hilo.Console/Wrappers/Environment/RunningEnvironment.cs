namespace Hilo.ConsoleApp.Wrappers.Environment;
using System;

public class RunningEnvironment : IRunningEnvironment
{
    public void Exit(int exitCode) => Environment.Exit(exitCode);
}
