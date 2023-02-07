namespace Hilo.Domain.Exceptions;
using System;

public class ActionNotSupportedAfterGameStartException : DomainException
{
    internal ActionNotSupportedAfterGameStartException(Guid gameId)
        : base(gameId, "This action cannot be performed. The game has already started.")
    {
    }
}