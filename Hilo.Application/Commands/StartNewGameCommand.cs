namespace Hilo.Application.Commands;

using System;

using Hilo.Application.Model;
using Hilo.Application.Repositories;
using Hilo.Domain;

using MediatR;

public record StartNewGameCommand(Guid GameId) : IRequest<HiLoRangeDto>;

public class StartNewGameCommandHandler : IRequestHandler<StartNewGameCommand, HiLoRangeDto>
{
    private readonly IRepository<HiLoGame.GameState> gameRepository;

    public StartNewGameCommandHandler(IRepository<HiLoGame.GameState> gameRepository) => this.gameRepository = gameRepository;

    public async Task<HiLoRangeDto> Handle(StartNewGameCommand request, CancellationToken cancellationToken)
    {
        HiLoGame.GameState gameState = await this.gameRepository.GetByIdAsync(request.GameId);

        var game = HiLoGame.RestoreGame(gameState);
        HiLoGame.GameRange range = game.StartGame();

        _ = await this.gameRepository.PersistAsync(game.ExportGameState());

        return new HiLoRangeDto(range.Start, range.End);
    }
}
