namespace Hilo.Application.Commands;
using Hilo.Application.Repositories;
using Hilo.Domain;

using MediatR;

public record SetupNewGameCommand : IRequest<Guid>;

public class SetupNewGameCommandHandler : IRequestHandler<SetupNewGameCommand, Guid>
{
    private readonly IRepository<HiLoGame.GameState> gameRepository;

    public SetupNewGameCommandHandler(IRepository<HiLoGame.GameState> gameRepository) => this.gameRepository = gameRepository;

    public async Task<Guid> Handle(SetupNewGameCommand request, CancellationToken cancellationToken)
    {
        var game = HiLoGame.SetupNewGame();

        return await this.gameRepository.PersistAsync(game.ExportGameState());
    }
}
