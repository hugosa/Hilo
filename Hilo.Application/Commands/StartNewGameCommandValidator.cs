namespace Hilo.Application.Commands;

using FluentValidation;

public class StartNewGameCommandValidator : AbstractValidator<StartNewGameCommand>
{
    public StartNewGameCommandValidator() => _ = RuleFor(x => x.GameId).NotEmpty();
}
