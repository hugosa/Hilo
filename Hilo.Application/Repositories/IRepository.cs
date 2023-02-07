namespace Hilo.Application.Repositories;
using System.Collections.Immutable;

public interface IRepository<T>
{
    Task<T> GetByIdAsync(Guid entityId);
    Task<ImmutableList<T>> GetByAsync(Func<T, bool> specification);
    Task<Guid> PersistAsync(T entity);
}