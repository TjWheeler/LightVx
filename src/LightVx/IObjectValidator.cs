namespace LightVx
{
    public interface IObjectValidator<T>
    {
        string[] ErrorMessages { get; set; }
        T Input { get; set; }
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