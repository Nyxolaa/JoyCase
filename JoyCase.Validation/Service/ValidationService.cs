using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace JoyCase.Validation
{
    public class ValidationService : IValidationService
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ValidationResponse Validate<T>(T request)
        {
            var validator = _serviceProvider.GetService<IValidator<T>>();

            if (validator == null)
            {
                return ValidationResponse.Success();
            }

            var validationResult = validator.Validate(request);
            if (validationResult.IsValid)
            {
                return ValidationResponse.Success();
            }

            return ValidationResponse.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }
    }
}
