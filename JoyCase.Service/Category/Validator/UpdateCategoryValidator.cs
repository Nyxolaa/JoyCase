using FluentValidation;
using JoyCase.Application.Category.Command.UpdateCategoryCommand;

namespace JoyCase.Application.Category.Validator
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.UpdatedBy).NotEmpty();
        }
    }
}
