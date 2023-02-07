namespace Hilo.Application.Queries;

using System.Threading;
using System.Threading.Tasks;
using Hilo.Application.Model;
using Hilo.Application.Repositories;
using Hilo.Domain;
using MediatR;

public record GetOpenGamesQuery : IRequest<ICollection<HiLoGameDto>>;

public class GetOpenGamesQueryHandler : IRequestHandler<GetOpenGamesQuery, ICollection<HiLoGameDto>>
{
    private readonly IRepository<HiLoGame.GameState> repository;

    public GetOpenGamesQueryHandler(IRepository<HiLoGame.GameState> repository) => this.repository = repository;

    public async Task<ICollection<HiLoGameDto>> Handle(GetOpenGamesQuery request, CancellationToken cancellationToken)
    {
        ICollection<HiLoGame.GameState> gameStates = await this.repository.GetByAsync(c => !c.GameStarted);

        return gameStates.Select(g =>
            new HiLoGameDto(
                g.GameId,
                g.PlayerState.Select(p => new PlayerDto(p.player.PlayerId, p.player.PlayerName)).ToList())
            ).ToList();
    }
}