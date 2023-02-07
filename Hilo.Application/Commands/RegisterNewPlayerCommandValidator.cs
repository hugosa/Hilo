namespace Hilo.Application.Commands;
using FluentValidation;

public class RegisterNewPlayerCommandValidator : AbstractValidator<RegisterNewPlayerCommand>
{
    public RegisterNewPlayerCommandValidator()
    {
        _ = RuleFor(x => x.GameId).NotEmpty();
        _ = RuleFor(x => x.PlayerName).NotEmpty();
    }
}
