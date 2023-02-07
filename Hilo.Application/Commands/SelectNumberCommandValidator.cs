namespace Hilo.Application.Commands;
using FluentValidation;

public class SelectNumberCommandValidator : AbstractValidator<SelectNumberCommand>
{
    public SelectNumberCommandValidator()
    {
        _ = RuleFor(x => x.GameId).NotEmpty();
        _ = RuleFor(x => x.PlayerId).NotEmpty();
        _ = RuleFor(x => x.Number).NotEmpty();
    }
}
