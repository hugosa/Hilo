namespace Hilo.Domain.Exceptions;
using System;

public class PlayerNotFoundException : DomainException
{
    internal PlayerNotFoundException(Guid gameId, Guid playerId)
        : base(gameId, "The player selected is not a part of this game.") => this.PlayerId = playerId;

    internal Guid PlayerId { get; }
}
