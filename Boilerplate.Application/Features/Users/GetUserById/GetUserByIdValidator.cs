using FluentValidation;

namespace Boilerplate.Application.Features.Users.GetUserById
{
    public sealed class GetUserByIdValidator : AbstractValidator<GetUserByIdQuery>
    {
        public GetUserByIdValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("O Id do utilizador deve ser um número positivo.");
        }
    }
}
