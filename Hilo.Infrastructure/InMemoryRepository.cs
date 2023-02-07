namespace Hilo.Infrastructure;

using System.Collections.Immutable;
using System.Linq;
using Hilo.Application.Repositories;
using Hilo.Domain;

public class InMemoryRepository : IRepository<HiLoGame.GameState>
{
    private readonly ICollection<HiLoGame.GameState> store;

    public InMemoryRepository() => this.store = new List<HiLoGame.GameState>();

    public Task<ImmutableList<HiLoGame.GameState>> GetByAsync(Func<HiLoGame.GameState, bool> specification) => Task.FromResult(this.store.Where(specification).ToImmutableList());

    public Task<HiLoGame.GameState> GetByIdAsync(Guid gameId)
    {
        HiLoGame.GameState? gameState = this.store.FirstOrDefault(g => g.GameId == gameId);

        return gameState is null
            ? throw new KeyNotFoundException($"Could not find game with id {gameId}")
            : Task.FromResult(gameState);
    }

    public Task<Guid> PersistAsync(HiLoGame.GameState gameState)
    {
        HiLoGame.GameState? dbGameState = this.store.FirstOrDefault(g => g.GameId == gameState.GameId);

        if (dbGameState != null)
        {
            _ = this.store.Remove(dbGameState);
        }

        this.store.Add(gameState);

        return Task.FromResult(gameState.GameId);
    }
}