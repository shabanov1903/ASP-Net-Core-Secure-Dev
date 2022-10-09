using CardStorageService.Core.Models.DTO;
using FluentValidation;

namespace CardStorageService.Services
{
    public class AuthenticationValidator : AbstractValidator<AuthenticationRequestDTO>
    {
        public AuthenticationValidator()
        {
            RuleFor(x => x.Login)
                .NotNull()
                .Length(5, 255)
                .EmailAddress();


            RuleFor(x => x.Password)
                .NotNull()
                .Length(5, 50);

        }
    }
}
