using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx
{
    /// <summary>
    /// Allows for advanced object validation to be contained to a separate class.
    /// Use the Eval methods to access the FluentAPI to validate within your derived class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectValidator<T> : IObjectValidator<T>
    {
        public ObjectValidator() { }
        public ObjectValidator(T input)
        {
            Input = input;
            Name = typeof(T).Name;
        }
        public ObjectValidator(T input, string name) : this(input)
        {
            Name = name;
        }

        protected IList<IValidator> Validators = new List<IValidator>();
        protected IList<ValidatorFluent> FluentValidators = new List<ValidatorFluent>();
        protected bool RequiresRevalidation = true;
        public virtual string Name { get; set; }
        public T Input { get; set; }
        private bool _isValid = false;
        public bool IsValid
        {
            get
            {
                if (RequiresRevalidation)
                {
                    Validate();
                }
                return _isValid;
            }
            protected set
            {
                _isValid = value;
            }
        }
        public string[] ErrorMessages { get; set; }
        public override string ToString()
        {
            return $"{Name} is {(IsValid ? "Valid" : "Invalid")}{(!IsValid ? " Errors: " + string.Join(",", ErrorMessages) : "")}";
        }
        public void Reset()
        {
            RequiresRevalidation = true;
            ErrorMessages = new string[] { };
            Validators.Clear();
            FluentValidators.Clear();
            IsValid = false;
        }
        public virtual bool Validate()
        {
            var errors = new List<string>();
            foreach (var validator in Validators)
            {
                var tempValidator = Activator.CreateInstance(validator.GetType()) as IValidator;
                tempValidator.Validate(validator.Input, validator.FieldName);
                if (!validator.IsValid)
                {
                    errors.Add(tempValidator.ErrorMessage);
                }
            }
            foreach (var validator in FluentValidators)
            {
                var valid = validator.IsValid;
                if (!valid.Value)
                {
                    errors.AddRange(validator.ErrorMessages);
                }
            }
            ErrorMessages = errors.ToArray();
            IsValid = ErrorMessages.Length == 0;
            RequiresRevalidation = false;
            return IsValid;
        }
        public virtual void AddValidator(IValidator validator)
        {
            RequiresRevalidation = true;
            Validators.Add(validator);
        }
        public virtual void AddValidator(ValidatorFluent fluentValidator)
        {
            RequiresRevalidation = true;
            FluentValidators.Add(fluentValidator);
        }
        public ValidatorFluent Eval(object input, string fieldName)
        {
            RequiresRevalidation = true;
            var validator = new ValidatorFluent(input, fieldName);
            FluentValidators.Add(validator);
            return validator;

        }
        public ValidatorFluent Eval(object input)
        {
            RequiresRevalidation = true;
            var validator = new ValidatorFluent(input);
            FluentValidators.Add(validator);
            return validator;
        }
    }
}
