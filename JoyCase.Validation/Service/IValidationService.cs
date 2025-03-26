namespace JoyCase.Validation
{
    public interface IValidationService
    {
        ValidationResponse Validate<T>(T request);
    }
}
