namespace Hilo.Application.Model;

public record HiLoGameDto(Guid GameId, ICollection<PlayerDto> players);