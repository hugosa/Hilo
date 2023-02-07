namespace Hilo.Application.Commands;

using Hilo.Application.Repositories;
using Hilo.Domain;

using MediatR;

using static Hilo.Domain.HiLoGame;

public record SelectNumberCommand(Guid GameId, Guid PlayerId, int Number) : IRequest<string>;

public class SelectNumberCommandHandler : IRequestHandler<SelectNumberCommand, string>
{
    private readonly IRepository<HiLoGame.GameState> gameRepository;

    public SelectNumberCommandHandler(IRepository<HiLoGame.GameState> gameRepository) => this.gameRepository = gameRepository;

    public async Task<string> Handle(SelectNumberCommand request, CancellationToken cancellationToken)
    {
        HiLoGame.GameState gameState = await this.gameRepository.GetByIdAsync(request.GameId);

        var game = HiLoGame.RestoreGame(gameState);
        PlayResult selectionResult = game.SelectNumber(request.PlayerId, request.Number);

        _ = await this.gameRepository.PersistAsync(game.ExportGameState());

        return selectionResult.Name;
    }
}
