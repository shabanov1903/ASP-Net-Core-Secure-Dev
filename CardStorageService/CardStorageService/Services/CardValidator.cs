using CardStorageService.Core.Models.DTO;
using FluentValidation;

namespace CardStorageService.Services
{
    public class CardValidator : AbstractValidator<CardDTO>
    {
        public CardValidator()
        {
            RuleFor(x => x.ClientId)
                .NotNull();
            
            RuleFor(x => x.Name)
                .Length(5, 255);
            
            RuleFor(x => x.CardNo)
                .Length(16);
            
            RuleFor(x => x.CVV2)
                .Length(3);
            
            RuleFor(x => x.ExpDate)
                .NotNull()
                .LessThan(DateTime.Now);
        }
    }
}
