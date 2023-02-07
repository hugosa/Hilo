namespace Hilo.Domain.Exceptions;
using System;

public abstract class DomainException : Exception
{
    protected DomainException(Guid entityId, string message)
        : base(message) => this.EntityId = entityId;

    public Guid EntityId { get; }
}
