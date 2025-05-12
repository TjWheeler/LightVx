namespace LightVx
{
    public interface IObjectValidator<T> : IObjectValidator
    {
        T Input { get; set; }
    }

    public interface IObjectValidator
    {
        /// <summary>
        /// List of error messages, joined with a comma.
        /// </summary>
        string ErrorMessage { get; }
        string[] ErrorMessages { get; set; }
        bool IsValid { get; }
        string Name { get; set; }
        void AddValidator(IValidator validator);
        void AddValidator(ValidatorFluent fluentValidator);
        ValidatorFluent Eval(object input);
        ValidatorFluent Eval(object input, string fieldName);
        void Reset();
        string ToString();
        bool Validate();
    }
}