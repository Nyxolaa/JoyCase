using FluentValidation;
using JoyCase.Application.User.Query.LoginUserQuery;

namespace JoyCase.Application.User.Validator
{
    public class LoginUserValidator : AbstractValidator<LoginUserQuery>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Kullanici adi zorunludur ((: ");
        }
    }
}
