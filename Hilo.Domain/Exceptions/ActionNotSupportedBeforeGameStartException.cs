namespace Hilo.Domain.Exceptions;
using System;

public class ActionNotSupportedBeforeGameStartException : DomainException
{
    internal ActionNotSupportedBeforeGameStartException(Guid gameId)
        : base(gameId, "This action cannot be performed. The game hasn't started.")
    {
    }
}