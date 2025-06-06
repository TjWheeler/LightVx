using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LightVx.Validators;
using LightVx.Validators.File;

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
        private List<IValidator> _validators = new List<IValidator>();
        private readonly string _fieldName = ValidatorBase.DefaultFieldName;
        private string _fieldDisplayName = null;

        private bool? _isValid;

        public string FieldName => _fieldName;
        public List<IValidator> Validators => _validators;

        public bool IsValid
        {
            get
            {
                Validate();
                return _isValid.HasValue && _isValid.Value;
            }
            private set
            {
                _isValid = value;
            }
        }
        /// <summary>
        /// Calls the Validate method and returns true if validation succeeded
        /// </summary>
        public bool Apply
        {
            get
            {
                Validate();
                return _isValid.HasValue && _isValid.Value;
            }
        }
        public List<string> ErrorMessages { get; private set; } = new List<string>();

        public ValidatorFluent()
        {
        }

        public ValidatorFluent(ValidatorFluent validationDefinition) 
        {
            _validators = new List<IValidator>(validationDefinition._validators);
        }
        public ValidatorFluent(object input, List<IValidator> validators = null) 
        {
            _input = input;
            if (validators != null)
            {
                _validators = validators;
            }

        }

        public ValidatorFluent(object input, string fieldName, ValidatorFluent validationSet = null) : this(input)
        {
            _fieldName = fieldName;
            if (validationSet != null)
            {
                _validators = new List<IValidator>(validationSet._validators);
            }
        }
        public ValidatorFluent(object input, string fieldName, string fieldDisplayName, ValidatorFluent validationSet = null) : this(input, fieldName)
        {
            _fieldDisplayName = fieldDisplayName;
            if (validationSet != null)
            {
                _validators = new List<IValidator>(validationSet._validators);
            }
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
        /// Ensure that Failed and/or Success is called last in the fluent api chain.
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
        /// <summary>
        /// Checks for blacklisted characters that might be used in canonical attacks to traverse directory structures.
        /// This validation does not allow any directory information such as / or \ or .., however a single . is allowed.
        /// </summary>
        /// <returns></returns>
        public ValidatorFluent DoesNotTraverse()
        {
            return DoesNotContain(new[] { "..", "/", "%2f", "\\", "%5c", "%2e%2e", "%2e%2e", "%2e%2e", "%c1%1c", "%c0%af" }, true);
        }

        /// <summary>
        /// Checks to ensure the specified content exists within the input
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public ValidatorFluent Contains(string content, bool ignoreCase = false)
        {
            _validators.Add(new ContainsValidator(content, ignoreCase));
            return this;
        }
        /// <summary>
        /// Checks to ensure the specified content exists within the input
        /// </summary>
        /// <param name="content"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public ValidatorFluent Contains(string[] content, bool ignoreCase = false)
        {
            _validators.Add(new ContainsValidator(content, ignoreCase));
            return this;
        }
        /// <summary>
        /// Checks to ensure the specified content (array items) do not exist within the input
        /// </summary>
        /// <param name="content"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public ValidatorFluent DoesNotContain(string content, bool ignoreCase = false)
        {
            _validators.Add(new NotContainsValidator(content, ignoreCase));
            return this;
        }
        /// <summary>
        /// Checks to ensure the specified all the content (array items) do not exist within the input
        /// </summary>
        /// <param name="content"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public ValidatorFluent DoesNotContain(string[] content, bool ignoreCase = false)
        {
            _validators.Add(new NotContainsValidator(content, ignoreCase));
            return this;
        }
        /// <summary>
        /// Checks for a string length
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public ValidatorFluent HasLength(int min, int? max)
        {
            _validators.Add(new LengthValidator(min, max));
            return this;
        }
        /// <summary>
        /// checks alpha and digits only
        /// </summary>
        /// <returns></returns>
        public ValidatorFluent IsAlphaNumeric()
        {
            _validators.Add(new AlphaNumericValidator());
            return this;
        }
        /// <summary>
        /// checks alpha,digits hyphens and spaces.
        /// </summary>
        /// <returns></returns>
        public ValidatorFluent IsAlphaNumericHyphen()
        {
            _validators.Add(new AlphaNumericHyphenValidator());
            return this;
        }
        /// <summary>
        /// Validates the input is a date and after specified date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public ValidatorFluent IsAfter(DateTime date)
        {
            _validators.Add(new MinDateValidator(date.AddMilliseconds(1)));
            return this;
        }
        /// <summary>
        /// Validates the input is a date and before specified date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public ValidatorFluent IsBefore(DateTime date)
        {
            _validators.Add(new MaxDateValidator(date.AddMilliseconds(-1)));
            return this;
        }
        /// <summary>
        /// Validates the input is a date and between specified dates
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public ValidatorFluent IsBetweenDates(DateTime fromDate, DateTime toDate)
        {
            _validators.Add(new BetweenDateValidator(fromDate, toDate));
            return this;
        }
        /// <summary>
        /// Validates the input is a boolean value (true/false).  Can be a bool or string.
        /// Note: Yes/No does not validate as a boolean.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ValidatorFluent IsBool()
        {
            _validators.Add(new BoolValidator());
            return this;
        }
        /// <summary>
        /// Validates the input is a boolean value (true/false) and it matches the requested value.  Can be a bool or string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        public ValidatorFluent IsBoolEqual(bool expected = true)
        {
            _validators.Add(new BoolEqualsValidator(expected));
            return this;
        }

        /// <summary>
        /// Validates input is numberic
        /// </summary>
        /// <returns></returns>
        public ValidatorFluent IsNumeric()
        {
            _validators.Add(new NumericValidator());
            return this;
        }
        /// <summary>
        /// Validates input is alpha only
        /// </summary>
        /// <returns></returns>
        public ValidatorFluent IsAlphaText()
        {
            _validators.Add(new AlphaTextValidator());
            return this;
        }
        /// <summary>
        /// Validates input can be parsed to a decimal
        /// </summary>
        /// <returns></returns>
        public ValidatorFluent IsDecimal()
        {
            _validators.Add(new DecimalValidator());
            return this;
        }
        /// <summary>
        /// Validates input can be parsed to a double
        /// </summary>
        /// <returns></returns>
        public ValidatorFluent IsDouble()
        {
            _validators.Add(new DoubleValidator());
            return this;
        }
        public ValidatorFluent IsEmailAddress()
        {
            _validators.Add(new EmailValidator());
            return this;
        }
        /// <summary>
        /// Validates input is a phone number. Allows ( ) 0-9 and hyphen
        /// </summary>
        /// <returns></returns>
        public ValidatorFluent IsPhoneNumber()
        {
            _validators.Add(new PhoneNumberValidator());
            return this;
        }
        /// <summary>
        /// Uses the <see cref="XssSafeTextValidator"/> and the <see cref="SqlSafeTextValidator"/>
        /// </summary>
        /// <returns></returns>
        public ValidatorFluent IsSafeText()
        {
            _validators.Add(new SafeTextValidator());
            return this;
        }
        /// <summary>
        /// Checks for known XSS characters
        /// </summary>
        /// <returns></returns>
        public ValidatorFluent IsSafeForXss()
        {
            _validators.Add(new XssSafeTextValidator());
            return this;
        }
        /// <summary>
        /// Checks for known SQL injection characters
        /// </summary>
        /// <returns></returns>
        public ValidatorFluent IsSafeForSql()
        {
            _validators.Add(new SqlSafeTextValidator());
            return this;
        }

        public ValidatorFluent IsStringEqualTo(string expectedString, bool ignoreCase = false)
        {
            _validators.Add(new StringEqualsValidator(expectedString, ignoreCase));
            return this;
        }
        public ValidatorFluent IsStringNotEqualTo(string expectedString, bool ignoreCase = false)
        {
            _validators.Add(new StringNotEqualsValidator(expectedString, ignoreCase));
            return this;
        }

        /// <summary>
        /// Checks if a datetime or datetime? is within the valid SQL date range
        /// </summary>
        /// <returns></returns>
        public ValidatorFluent IsSqlDate()
        {
            _validators.Add(new SqlSafeDateValidator());
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
        public ValidatorFluent Min(DateTime value)
        {
            _validators.Add(new MinDateValidator(value));
            return this;
        }
        public ValidatorFluent Min(DateTime? value)
        {
            _validators.Add(new MinDateValidator(value));
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
        public ValidatorFluent Max(DateTime value)
        {
            _validators.Add(new MaxDateValidator(value));
            return this;
        }
        public ValidatorFluent Max(DateTime? value)
        {
            _validators.Add(new MaxDateValidator(value));
            return this;
        }
        public ValidatorFluent IsEmpty()
        {
            _validators.Add(new EmptyValidator());
            return this;
        }
        public ValidatorFluent IsGuid()
        {
            _validators.Add(new GuidValidator());
            return this;
        }
        public ValidatorFluent IsInt()
        {
            _validators.Add(new IntValidator());
            return this;
        }
        public ValidatorFluent IsIn(ICollection items, bool ignoreCase = false)
        {
            _validators.Add(new InCollectionValidator(items, ignoreCase));
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
        /// <summary>
        /// Allows Alpha, Spaces, Numbers and Hyphens
        /// </summary>
        /// <returns></returns>
        public ValidatorFluent IsTitleText()
        {
            _validators.Add(new AlphaNumericHyphenValidator());
            return this;
        }
        /// <summary>
        /// Allows Alpha, Hyphens and Apostrophies
        /// </summary>
        /// <returns></returns>
        public ValidatorFluent IsNameText()
        {
            _validators.Add(new NameTextValidator());
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
        /// Validates input can be parsed to a currency or optionally a double
        /// </summary>
        /// <returns></returns>
        public ValidatorFluent IsCurrency(bool requireCurrencySymbol = true)
        {
            _validators.Add(new CurrencyValidator(requireCurrencySymbol));
            return this;
        }

        /// <summary>
        /// Validates input is a valid ISO 8601 time component only. Eg; "15:30:45.123+02:00" or "23:59:59Z"
        /// If the value is an ISO 8601 date and time component then this validator will fail it.
        /// </summary>
        /// <returns></returns>
        public ValidatorFluent IsIsoTime()
        {
            _validators.Add(new IsoTimeValidator());
            return this;
        }

        /// <summary>
        /// Validates input is a valid ISO 8601 date component only. Eg; "2025-05-03" 
        /// If the value is an ISO 8601 date and time component then this validator will fail it.
        /// </summary>
        /// <returns></returns>
        public ValidatorFluent IsIsoDate()
        {
            _validators.Add(new IsoDateValidator());
            return this;
        }

        /// <summary>
        /// Validates input is a valid ISO 8601 date time. Eg; "1999-12-31T23:59:59.999+02:00" 
        /// If the value must match the ISO 8601 date and convert to a DateTime.
        /// </summary>
        /// <returns></returns>
        public ValidatorFluent IsIsoDateTime()
        {
            _validators.Add(new IsoDateTimeValidator());
            return this;
        }
        /// <summary>
        /// Validates that the input is a DateTime or a string can be parsed as a DateTime.
        /// </summary>
        /// <returns></returns>
        public ValidatorFluent IsDateTime()
        {
            _validators.Add(new DateTimeValidator());
            return this;
        }


        /// <summary>
        /// Looks for a Single Match
        /// </summary>
        /// <param name="regularExpression"></param>
        /// <returns></returns>
        public ValidatorFluent MatchesExpression(string regularExpression)
        {
            _validators.Add(new RegExValidator(regularExpression));
            return this;
        }

        /// <summary>
        /// Calls all the validators and runs the validate method.
        /// </summary>
        public ValidatorFluent Validate()
        {
            foreach (var validator in _validators)
            {
                validator.Validate(_input, _fieldName, _fieldDisplayName);
            }
            IsValid = _validators.All(t => t.IsValid);
            ErrorMessages = _validators.Where(t => !t.IsValid).Select(t => t.ErrorMessage).Where(t => t != string.Empty).ToList();
            return this;
        }
        /// <summary>
        /// Validates using the provided enumerable IValidator objects
        /// </summary>
        /// <param name="validators"></param>
        /// <returns></returns>
        public ValidatorFluent ValidateWith(IEnumerable<IValidator> validators)
        {
            _validators = new List<IValidator>(validators);
            return Validate();
        }
        public ValidatorFluent ValidateWith(ValidatorFluent validationDefintion)
        {
            _validators = new List<IValidator>(validationDefintion._validators);
            return Validate();
        }
        /// <summary>
        /// Checks starting and end bytes to see if the stream contains the JPG magic numbers.
        /// Note: The Input must be a Seekable Stream.
        /// </summary>
        public ValidatorFluent HasJpgImageSignature()
        {
            _validators.Add(new JpgSignatureValidator());
            return this;
        }
        /// <summary>
        /// Checks starting bytes to see if the stream contains the PNG magic numbers.
        /// </summary>
        public ValidatorFluent HasPngImageSignature()
        {
            _validators.Add(new PngSignatureValidator());
            return this;
        }
        /// <summary>
        /// Checks starting bytes to see if the stream contains the GIF (Gif87a/Gif89a) magic numbers.
        /// </summary>
        public ValidatorFluent HasGifImageSignature()
        {
            _validators.Add(new GifSignatureValidator());
            return this;
        }
    }
}