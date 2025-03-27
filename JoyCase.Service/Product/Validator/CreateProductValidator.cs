using FluentValidation;
using JoyCase.Application.Product.Command.CreateProductCommand;

namespace JoyCase.Application.Product.Validator
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.CategoryId).GreaterThan(0);
            RuleFor(x => x.Price).GreaterThan(0);
        }
    }
}
