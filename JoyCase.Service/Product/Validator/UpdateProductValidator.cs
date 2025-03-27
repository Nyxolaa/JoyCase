using FluentValidation;
using JoyCase.Application.Product.Command.UpdateProductCommand;

namespace JoyCase.Application.Product.Validator
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.CategoryId).GreaterThan(0);
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.UpdatedBy).NotEmpty();
        }
    }
}
