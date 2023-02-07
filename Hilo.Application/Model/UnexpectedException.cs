namespace Hilo.Application.Model;
using System;

public class UnexpectedException : Exception
{
    public UnexpectedException(string message, Exception ex)
        : base(message, ex)
    {
    }
}
