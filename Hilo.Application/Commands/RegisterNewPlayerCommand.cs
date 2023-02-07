namespace Hilo.Application.Commands;

using Hilo.Application.Repositories;
using Hilo.Domain;

using MediatR;

public record RegisterNewPlayerCommand(Guid GameId, string PlayerName) : IRequest<Guid>;

public class RegisterNewPlayerCommandHandler : IRequestHandler<RegisterNewPlayerCommand, Guid>
{
    private readonly IRepository<HiLoGame.GameState> gameRepository;

    public RegisterNewPlayerCommandHandler(IRepository<HiLoGame.GameState> gameRepository) => this.gameRepository = gameRepository;

    public async Task<Guid> Handle(RegisterNewPlayerCommand request, CancellationToken cancellationToken)
    {
        Player player = new(request.PlayerName);

        HiLoGame.GameState gameState = await this.gameRepository.GetByIdAsync(request.GameId);

        var game = HiLoGame.RestoreGame(gameState);
        game.AddNewPlayer(player);

        _ = await this.gameRepository.PersistAsync(game.ExportGameState());

        return player.PlayerId;
    }
}