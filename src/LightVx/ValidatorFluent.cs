using System;
using System.Collections.Generic;
using System.Linq;
using LightVx.Validators;

namespace LightVx
{

    /// <summary>
    /// This is a Fluent API over the Validation Framework.
    /// Ensure that Fail and/or Success are the last in the chain.
    /// Eg;
    /// Validator.Eval("abc", "Customer First Name")
    ///     .Required()
    ///     .IsAlphaText()
    ///     .Fail((errors, validators) =>
    ///     {
    ///             ... // Validation failed, put your failure logic here
    ///     });
    /// </summary>
    public class ValidatorFluent
    {
        private readonly object _input;
        private readonly List<IValidator> _validators = new List<IValidator>();
        private readonly string _fieldName = ValidatorBase.DefaultFieldName;
        private bool? _isValid;
        public bool Validated { get; private set; }

        public bool? IsValid
        {
            get
            {
                if (!Validated)
                {
                    Validate();
                }
                return _isValid;
            }
            private set => _isValid = value;
        }

        public List<string> ErrorMessages { get; private set; }
        public ValidatorFluent(object input)
        {
            _input = input;
        }

        public ValidatorFluent(object input, string fieldName) : this(input)
        {
            _fieldName = fieldName;
        }

        
        public ValidatorFluent Required()
        {
            _validators.Add(new LengthValidator(1));
            return this;
        }

        public ValidatorFluent AddValidator(IValidator validator)
        {
            _validators.Add(validator);
            return this;
        }

        
        /// <summary>
        ///     Ensure that Failed and/or Success is called last in the fluent api chain.
        /// </summary>
        /// <param name="onSuccess"></param>
        /// <returns></returns>
        public ValidatorFluent Success(Action onSuccess)
        {
            Validate();
            var failedValidators = _validators.Where(t => !t.IsValid).ToList();
            if (failedValidators.Count == 0) onSuccess();
            return this;
        }

        /// <summary>
        ///     Ensure that Failed and/or Success is called last in the fluent api chain.
        /// </summary>
        /// <param name="onFailed"></param>
        /// <returns></returns>
        public ValidatorFluent Fail(Action<List<string>, List<IValidator>> onFailed)
        {
            Validate();
            var failedValidators = _validators.Where(t => !t.IsValid).ToList();
            var errorMessages = failedValidators.Where(t => !t.IsValid).Select(t => t.ErrorMessage).ToList();
            if (failedValidators.Count > 0) onFailed(errorMessages, failedValidators);
            return this;
        }

        public ValidatorFluent HasLength(int min, int? max)
        {
            _validators.Add(new LengthValidator(min, max));
            return this;
        }
        public ValidatorFluent IsAlphaNumeric()
        {
            _validators.Add(new AlphaNumericValidator());
            return this;
        }
        public ValidatorFluent IsAlphaText()
        {
            _validators.Add(new AlphaTextValidator());
            return this;
        }
        public ValidatorFluent IsDecimal()
        {
            _validators.Add(new DecimalValidator());
            return this;
        }
        public ValidatorFluent IsEmailAddress()
        {
            _validators.Add(new EmailValidator());
            return this;
        }

        public ValidatorFluent IsPhoneNumber()
        {
            _validators.Add(new PhoneNumberValidator());
            return this;
        }
        public ValidatorFluent IsSafeText()
        {
            _validators.Add(new SafeTextValidator());
            return this;
        }
        public ValidatorFluent IsUrl()
        {
            _validators.Add(new UrlValidator());
            return this;
        }
        public ValidatorFluent Min(int value)
        {
            _validators.Add(new MinValidator(value));
            return this;
        }
        public ValidatorFluent Min(double value)
        {
            _validators.Add(new MinValidator(value));
            return this;
        }
        public ValidatorFluent Min(decimal value)
        {
            _validators.Add(new MinValidator(value));
            return this;
        }
        public ValidatorFluent Max(int value)
        {
            _validators.Add(new MaxValidator(value));
            return this;
        }
        public ValidatorFluent Max(double value)
        {
            _validators.Add(new MaxValidator(value));
            return this;
        }
        public ValidatorFluent Max(decimal value)
        {
            _validators.Add(new MaxValidator(value));
            return this;
        }
        public ValidatorFluent IsEmpty()
        {
            _validators.Add(new EmptyValidator());
            return this;
        }
        public ValidatorFluent IsNotEmpty()
        {
            _validators.Add(new NotEmptyValidator());
            return this;
        }
        public ValidatorFluent IsNull()
        {
            _validators.Add(new IsNullValidator());
            return this;
        }
        public ValidatorFluent IsNotNull()
        {
            _validators.Add(new NotNullValidator());
            return this;
        }
        public ValidatorFluent HasMinLength(int minLength)
        {
            _validators.Add(new MinLengthValidator(minLength));
            return this;
        }
        public ValidatorFluent HasMaxLength(int maxLength)
        {
            _validators.Add(new MaxLengthValidator(maxLength));
            return this;
        }

        /// <summary>
        /// Calls all the validators and runs the validate method.
        /// </summary>
        public ValidatorFluent Validate()
        {
            if (Validated) return this;
            foreach (var validator in _validators)
            {
                validator.Validate(_input, _fieldName);
            }
            IsValid = _validators.All(t => t.IsValid);
            ErrorMessages = _validators.Where(t => !t.IsValid).Select(t => t.ErrorMessage).ToList();
            Validated = true;
            return this;
        }
    }
}