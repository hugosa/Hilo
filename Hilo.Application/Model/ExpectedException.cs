namespace Hilo.Application.Model;
using System;

public class ExpectedException : Exception
{
    public ExpectedException(string message, Exception ex)
        : base(message, ex)
    {
    }
}
