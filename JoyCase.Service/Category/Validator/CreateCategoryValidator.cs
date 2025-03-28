using FluentValidation;
using JoyCase.Application.Category.Command.CreateCategoryCommand;

namespace JoyCase.Application.Category.Validator
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(26);
        }
    }
}
