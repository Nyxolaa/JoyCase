namespace JoyCase.Validation
{
    public class ValidationResponse
    {
        public bool IsValid { get; set; }
        public List<string> ValidationResult { get; set; } = new List<string>();

        private ValidationResponse(bool isValid, List<string> errors)
        {
            IsValid = isValid;
            ValidationResult = errors ?? new List<string>();
        }

        public static ValidationResponse Success() => new ValidationResponse(true, new List<string>());

        public static ValidationResponse Failure(List<string> errors) => new ValidationResponse(false, errors);
    }
}
