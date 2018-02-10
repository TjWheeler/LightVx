//   Contains:    User Input Validation - Reusable and Extendable classes
//   Description: A series of IInputValidators.  Provide the ability to validate
//             user input.  Validators can be combined using the AggregatedValidator.
//   Author: Tim Wheeler

//Examples:
//		Quick syntax example using Validator helper:
// 		string errorMessage;
//      if (Validator.IsNotValid<AlphaTextValidator>(input, nameof(account), out errorMessage))
//      {
//         return BadRequest($"The input is invalid: {errorMessage}");
//      }
//		 
//       private void Example1Numeric()
//       {
//           string input = "123ABC";
//           IInputValidator validator = new NumericValidator();
//           validator.Input = input;
//           validator.Validate();
//           if (!validator.IsValid)
//               Console.WriteLine("Input: " + input + " return error (" + validator.ErrorMessage + ")");
//       }
//       private void Example2ControlName()
//       {   //This example adds the Control Name to a validate function call which incorporates it
//           //in the user error message;
//           string input = "123ABC";
//           IInputValidator validator = new NumericValidator();
//           string userErrorMessage;
//           if(!validator.Validate(input,"Phone Number", out userErrorMessage))
//               Console.WriteLine(userErrorMessage);
//       }   

//Extending and Combining Validators:
//       //Uses the AggregatedValidator to combine to validation techniques into 1 validator
//       public class PhoneAndLengthValidator : AggregatedValidator
//       {
//           public PhoneAndLengthValidator(int minOccurance, int maxOccurance)
//           {
//               AddValidator(new LengthValidator(minOccurance, maxOccurance));
//               AddValidator(new PhoneNumberValidator());
//           }
//       }
//Notes: In most situations a validator should return success for null or empty string data.
//       This allows for validators like the length validator to be used or combined if the field
//       is required.  Like the ASP.Net RequiredFieldValidator.


using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LightVx
{
    #region IInputValidator Interface

    public interface IInputValidator
    {
        string FieldName { get; set; }

        /// <summary>
        ///     Accepts any object as input to be validated
        /// </summary>
        object Input { get; set; }

        /// <summary>
        ///     Indicates if the object is valid.  Must be called after the Validate()
        /// </summary>
        bool IsValid { get; set; }

        /// <summary>
        ///     Error message containing information about why validation has failed.
        /// </summary>
        string ErrorMessage { get; set; }

        /// <summary>
        ///     Performs validation and checks the result. If validation fails it outputs the error
        ///     message.
        /// </summary>
        /// <param name="input">Object to validate</param>
        /// <param name="fieldName">
        ///     Name of the field, used to generate an error that can
        ///     be shown to users
        /// </param>
        /// <param name="errorMessage">Empty string if no error, error message if failure</param>
        /// <returns>True if the input object is valid, false if a validation rule was broken</returns>
        bool Validate(object input, string fieldName, out string errorMessage);

        bool Validate(object input, string fieldName);

        bool Validate(object input);
    }

    #endregion

    public class ValidatorFluent
    {
        private readonly object _input;
        private readonly List<IInputValidator> _validators = new List<IInputValidator>();
        private readonly string _fieldName;

        public ValidatorFluent(object input)
        {
            _input = input;
        }

        public ValidatorFluent(object input, string fieldName) : this(input)
        {
            _fieldName = fieldName;
        }

        public bool Validated { get; private set; }
        public bool? IsValid { get; private set; }

        public ValidatorFluent Required()
        {
            _validators.Add(new LengthValidator(1));
            //_person.FirstName = firstName;
            return this;
        }

        public ValidatorFluent AddValidator(IInputValidator validator)
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

        private void Validate()
        {
            if (Validated) return;
            foreach (var validator in _validators) validator.Validate(_input, _fieldName);
            IsValid = _validators.All(t => t.IsValid);
            Validated = true;
        }

        /// <summary>
        ///     Ensure that Failed and/or Success is called last in the fluent api chain.
        /// </summary>
        /// <param name="onFailed"></param>
        /// <returns></returns>
        public ValidatorFluent Failed(Action<List<string>, List<IInputValidator>> onFailed)
        {
            Validate();
            var failedValidators = _validators.Where(t => !t.IsValid).ToList();
            var errorMessages = failedValidators.Where(t => !t.IsValid).Select(t => t.ErrorMessage).ToList();
            if (failedValidators.Count > 0) onFailed(errorMessages, failedValidators);
            return this;
        }
    }

    public static class Validator
    {
        public static ValidatorFluent Eval(this object input, string fieldName)
        {
            //, Func<IInputValidator> validator
            return new ValidatorFluent(input, fieldName);
        }
        //public static implicit operator WithValidators(ValidatorFluent)
        //{


        //    return new object();
        //}
        public static bool IsValid<T>(string input, string fieldName) where T : IInputValidator
        {
            var validator = (IInputValidator) Activator.CreateInstance(typeof(T));
            validator.Validate(input, fieldName);
            return validator.IsValid;
        }

        public static bool IsNotValid<T>(string input, string fieldName, out string errorMessage)
            where T : IInputValidator
        {
            return !IsValid<T>(input, fieldName, out errorMessage);
        }

        public static bool IsValid<T>(string input, string fieldName, out string errorMessage) where T : IInputValidator
        {
            var validator = (IInputValidator) Activator.CreateInstance(typeof(T));
            validator.Validate(input, fieldName);
            errorMessage = validator.IsValid ? string.Empty : validator.ErrorMessage;
            return validator.IsValid;
        }
    }

    #region InputValidator base class

    public abstract class InputValidatorBase : IInputValidator
    {
        #region private functions

        private void ResetFieldName()
        {
            _FieldName = _DefaultFieldName;
        }

        #endregion

        #region private and protected variables

        protected object _Input;
        protected string _ErrorMessage = string.Empty;
        protected bool _IsValid;
        protected string _FieldName = _DefaultFieldName;
        private const string _DefaultFieldName = "Input Data";

        #endregion

        #region Derived class helper functions

        protected void Fail(string message)
        {
            _IsValid = false;
            _ErrorMessage = _FieldName + " " + message;
        }

        protected void Succeed()
        {
            _IsValid = true;
            _ErrorMessage = string.Empty;
        }

        #endregion

        #region Abstract methods

        /// <summary>
        ///     For the derived class to specify its regex expression
        /// </summary>
        protected virtual string Expression { get; }

        protected abstract void Validate();

        #endregion

        #region public methods and properties

        public virtual bool Validate(object input, string fieldName, out string errorMessage)
        {
            _Input = input;
            _FieldName = fieldName;
            Validate();
            ResetFieldName();
            errorMessage = _ErrorMessage;
            return _IsValid;
        }

        public virtual bool Validate(object input, string fieldName)
        {
            _Input = input;
            _FieldName = fieldName;
            Validate();
            ResetFieldName();
            return _IsValid;
        }

        public virtual bool Validate(object input)
        {
            _Input = input;
            Validate();
            return _IsValid;
        }

        public object Input
        {
            get => _Input;
            set
            {
                _Input = value;
                _IsValid = false; //Reset the state
                _ErrorMessage = "Valdate() function not yet called";
            }
        }

        public bool IsValid
        {
            get => _IsValid;
            set => _IsValid = value;
        }

        public string ErrorMessage
        {
            get => _ErrorMessage;
            set => _ErrorMessage = value;
        }

        public string FieldName
        {
            get => _FieldName;
            set
            {
                if (value == null)
                    _FieldName = _DefaultFieldName;
                else
                    _FieldName = value;
            }
        }

        #endregion

        #region Regex helper methods

        protected bool HasMatch(string input)
        {
            return Regex.Matches(input, Expression).Count > 0;
        }

        protected bool SingleMatch(string input)
        {
            return Regex.Matches(input, Expression).Count == 1;
        }

        protected int MatchCount(string input)
        {
            return Regex.Matches(input, Expression).Count;
        }

        protected bool HasMatch(string input, string expression)
        {
            return Regex.Matches(input, expression).Count > 0;
        }

        protected bool SingleMatch(string input, string expression)
        {
            return Regex.Matches(input, expression).Count == 1;
        }

        protected int MatchCount(string input, string expression)
        {
            return Regex.Matches(input, expression).Count;
        }

        #endregion
    }

    #endregion

    #region AggregatedValidator - allows combining validators

    /// <summary>
    ///     Accepts other validators, and acts as an aggregate.  To combine validation techniques.
    ///     Eg a post code validator may be made up of the lengthvalidator and the numericvalidator.
    /// </summary>
    public class AggregatedValidator : InputValidatorBase
    {
        #region base implementation

        protected override string Expression => throw new NotImplementedException();

        #endregion

        #region private variables

        private readonly List<IInputValidator> _Validators = new List<IInputValidator>();

        #endregion

        #region Constructors

        public AggregatedValidator()
        {
        }

        public AggregatedValidator(IInputValidator[] validators)
        {
            foreach (var validator in validators) _Validators.Add(validator);
        }

        #endregion

        #region public methods and properties

        public void AddValidator(IInputValidator validator)
        {
            _Validators.Add(validator);
        }

        public StringCollection ErrorMessages { get; private set; } = new StringCollection();

        protected override void Validate()
        {
            var sbErrorMessage = new StringBuilder();
            ErrorMessages = new StringCollection();
            var foundError = false;
            foreach (var validator in _Validators)
            {
                //validator.Input = _Input;
                string outMessage;
                validator.Validate(_Input, FieldName, out outMessage);
                if (!validator.IsValid)
                {
                    foundError = true;
                    ErrorMessages.Add(validator.ErrorMessage);
                    sbErrorMessage.AppendLine(validator.ErrorMessage);
                }
            }

            _ErrorMessage = sbErrorMessage.ToString();
            IsValid = !foundError;
        }

        public override bool Validate(object input, string fieldName, out string errorMessage)
        {
            var foundError = false;
            ErrorMessages = new StringCollection();
            errorMessage = string.Empty;
            foreach (var validator in _Validators)
            {
                string error;
                var valid = validator.Validate(input, fieldName, out error);
                if (!valid)
                {
                    foundError = true;
                    ErrorMessages.Add(validator.ErrorMessage);
                    errorMessage = errorMessage + " " + error;
                }
            }

            if (foundError)
                return false;
            return true;
        }

        #endregion
    }

    #endregion

    #region AlphaText Validator

    /// <summary>
    ///     Validate text with length options
    /// </summary>
    public class AlphaNumericValidator : InputValidatorBase, IInputValidator
    {
        private const string _RegExpression = "^([a-zA-Z0-9\\s]{1,})$";

        #region base implementation

        protected override string Expression => _RegExpression;

        #endregion

        protected override void Validate()
        {
            if (_Input == null)
            {
                Succeed();
                return;
            }

            if (!(_Input is string))
            {
                Fail("is not a string value.");
                return;
            }

            if (!SingleMatch((string) _Input))
            {
                Fail("does not contain correct values");
                return;
            }

            Succeed();
        }
    }

    /// <summary>
    ///     Validate text with length options
    /// </summary>
    public class AlphaTextValidator : InputValidatorBase, IInputValidator
    {
        private const string _RegExpression = "^([a-z]{1,}|[A-Z]{1,})$";

        #region base implementation

        protected override string Expression => _RegExpression;

        #endregion

        protected override void Validate()
        {
            if (_Input == null)
            {
                Succeed();
                return;
            }

            if (!(_Input is string))
            {
                Fail("is not a string value.");
                return;
            }

            if (!SingleMatch((string) _Input))
            {
                Fail("does not contain correct values");
                return;
            }

            Succeed();
        }
    }

    #endregion

    #region Length Validator

    /// <summary>
    ///     Validate text with length options
    /// </summary>
    public class LengthValidator : InputValidatorBase, IInputValidator
    {
        private readonly int _MinOccurance;
        private readonly int _MaxOccurance;

        public LengthValidator(int minOccurance)
        {
            _MinOccurance = minOccurance;
        }

        public LengthValidator(int minOccurance, int maxOccurance)
        {
            _MinOccurance = minOccurance;
            _MaxOccurance = maxOccurance;
        }

        #region base implementation

        protected override string Expression { get; }

        #endregion

        protected override void Validate()
        {
            if (_Input == null && _MinOccurance == 0)
            {
                Succeed();
                return;
            }

            if (_Input == null && _MinOccurance > 0)
            {
                Fail(
                    string.Format("has no data (null) and is not valid. Must have a length of between {0} and {1}.",
                        _MinOccurance, _MaxOccurance));
                return;
            }

            if (_Input.ToString().Length >= _MinOccurance && _Input.ToString().Length <= _MaxOccurance)
            {
                Succeed();
            }
            else
            {
                string message;
                if (_MinOccurance == _MaxOccurance)
                    message =
                        string.Format("is not a valid length. Must have a length of {0}.", _MinOccurance);
                else
                    message =
                        string.Format("is not a valid length. Must have a length of between {0} and {1}.",
                            _MinOccurance,
                            _MaxOccurance);
                Fail(message);
            }
        }
    }

    #endregion

    #region Numeric Validator

    /// <summary>
    ///     Validate numeric numbers
    /// </summary>
    public class NumericValidator : InputValidatorBase, IInputValidator
    {
        private const string _RegExpression = @"^\d+$";

        #region base implementation

        protected override string Expression => _RegExpression;

        #endregion

        protected override void Validate()
        {
            if (_Input == null || _Input is string && (string) _Input == string.Empty)
            {
                Succeed();
                return;
            }

            if (SingleMatch(_Input.ToString()))
                Succeed();
            else
                Fail("is not a valid number. It can only contain numeric values.");
        }
    }

    #endregion

    #region PhoneNumber Validator

    /// <summary>
    ///     Validate phone numbers.  Allows ( ) 0-9 and hyphen
    /// </summary>
    public class PhoneNumberValidator : InputValidatorBase, IInputValidator
    {
        protected override void Validate()
        {
            if (_Input == null || (string) _Input == string.Empty)
            {
                Succeed();
                return;
            }

            if (!(_Input is string))
            {
                Fail("is not a string value.");
                return;
            }

            if (SingleMatch(_Input.ToString()))
                Succeed();
            else
                Fail(
                    "is not a valid phone number, is can only contain left and right brace, spaces, hyphens and digits.");
        }

        #region base implementation

        private const string _RegExpression = @"^(\d*\s?|\-?|\)?|\(?)*$";

        protected override string Expression => _RegExpression;

        #endregion
    }

    #endregion

    #region PhoneAndLengthValidator - Aggregated Validator

    public class PhoneAndLengthValidator : AggregatedValidator
    {
        public PhoneAndLengthValidator(int minOccurance, int maxOccurance)
        {
            AddValidator(new LengthValidator(minOccurance, maxOccurance));
            AddValidator(new PhoneNumberValidator());
        }
    }

    #endregion

    #region Hex Colour Validator

    /// <summary>
    ///     Validate text with length options
    /// </summary>
    public class HexColourValidator : InputValidatorBase, IInputValidator
    {
        private const string _RegExpression = @"^\#{1}[A-Fa-f0-9]{3}([A-Fa-f0-9]{3})?$";

        #region base implementation

        protected override string Expression => _RegExpression;

        #endregion

        protected override void Validate()
        {
            if (_Input == null)
            {
                Succeed();
                return;
            }

            if (!(_Input is string))
            {
                Fail("is not a hex colour value, eg #FFFFFF.");
                return;
            }

            if (!SingleMatch((string) _Input))
            {
                Fail("is not a hex colour value, eg #FFFFFF.");
                return;
            }

            Succeed();
        }
    }

    #endregion

    #region DecimalValidator

    /// <summary>
    ///     Validate decimal numbers.  digits and 1 period.  Also has maxium decimal
    ///     places restriction.
    /// </summary>
    public class DecimalValidator : InputValidatorBase, IInputValidator
    {
        #region Implementation

        protected override void Validate()
        {
            decimal result;
            ErrorMessage = string.Empty;
            if (_Input != null && _Input.ToString().Length > 0)
            {
                IsValid = decimal.TryParse(_Input.ToString(), out result);
                if (!IsValid) ErrorMessage = FieldName + " is not a valid decimal value";
            }
            else
            {
                Succeed();
            }
        }

        #endregion

        #region private variables and protected properties

        protected override string Expression { get; }

        #endregion
    }

    #endregion

    #region Email Validator

    /// <summary>
    ///     Validate Email Addresses
    /// </summary>
    public class EmailValidator : InputValidatorBase, IInputValidator
    {
        private const string _RegExpression =
            @"^([\&\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|" +
            @"(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        #region base implementation

        protected override string Expression => _RegExpression;

        #endregion

        protected override void Validate()
        {
            if (_Input == null || (string) _Input == string.Empty)
            {
                Succeed();
                return;
            }

            if (!(_Input is string))
            {
                Fail("is not a string value.");
                return;
            }

            if (SingleMatch((string) _Input))
                Succeed();
            else
                Fail("is not a valid email address.");
        }
    }

    #endregion

    #region Url Validator

    /// <summary>
    ///     Validate Url
    /// </summary>
    public class UrlValidator : InputValidatorBase, IInputValidator
    {
        private const string _RegExpression =
            @"^((((https?|ftps?|gopher|telnet|nntp)://)|(mailto:|news:))(%[0-9A-Fa-f]{2}|" +
            "[-()_.!~*';/?:@&=+$,A-Za-z0-9])+)([).!';/?:,]blank:)?$";

        #region base implementation

        protected override string Expression => _RegExpression;

        #endregion

        protected override void Validate()
        {
            if (_Input == null || (string) _Input == string.Empty)
            {
                Succeed();
                return;
            }

            if (!(_Input is string))
            {
                Fail("is not a string value.");
                return;
            }

            if (SingleMatch((string) _Input))
                Succeed();
            else
                Fail("is not a valid url.");
        }
    }

    #endregion

    #region Credit Card Validator

    [Obsolete("This validator is not working")]
    /// <summary>
    /// Validate Credit Card
    /// Note: This is not tested yet
    /// Validates null or empty string as Successful.  
    /// </summary>
    public class CreditCardValidator : InputValidatorBase, IInputValidator
    {
        private const string _RegExpression =
            @"^((4\d{3})|(5[1-5]\d{2})|(6011)|(7\d{3}))-?\d{4}-?\d{4}-?\d{4}|3[4,7]\d{13}$";

        #region base implementation

        protected override string Expression => _RegExpression;

        #endregion

        protected override void Validate()
        {
            if (_Input == null || (string) _Input == string.Empty)
            {
                Succeed();
                return;
            }

            if (!(_Input is string))
            {
                Fail("is not a string value.");
                return;
            }

            if (SingleMatch(_Input.ToString()))
                Succeed();
            else
                Fail("is not a valid credit card number.");
        }
    }

    #endregion

    #region Safe Text Validator

    /// <summary>
    ///     Safe Text matches Lower and upper case letters and all digits
    /// </summary>
    public class SafeTextValidator : InputValidatorBase, IInputValidator
    {
        private const string _RegExpression =
            @"^[a-zA-Z0-9\s.\-]+$";

        #region base implementation

        protected override string Expression => _RegExpression;

        #endregion

        protected override void Validate()
        {
            if (_Input == null || (string) _Input == string.Empty)
            {
                Succeed();
                return;
            }

            if (!(_Input is string))
            {
                Fail("is not a string value.");
                return;
            }

            if (SingleMatch(_Input.ToString()))
                Succeed();
            else
                Fail("is not valid.");
        }
    }

    #endregion

    #region Australian Business Number

    public class ABNValidator : AggregatedValidator
    {
        public ABNValidator()
        {
            AddValidator(new NumericValidator());
            AddValidator(new LengthValidator(11, 11));
            AddValidator(new ABNCheckSumValidator());
        }
    }

    /// <summary>
    ///     Validate Australian Company numbers
    /// </summary>
    public class ABNCheckSumValidator : InputValidatorBase, IInputValidator
    {
        #region base implementation

        protected override string Expression => string.Empty;

        #endregion

        protected override void Validate()
        {
            if (_Input == null || _Input is string && (string) _Input == string.Empty)
            {
                Succeed();
                return;
            }

            int runningTotal;
            if (IsValidABNCheckSum())
                Succeed();
            else
                Fail("has failed the ABN Checksum Validation.");
        }

        private bool IsValidABNCheckSum()
        {
            try
            {
                //1 Subtract 1 from the first (left) digit to give a new eleven digit number 
                //2 Multiply each of the digits in this new number by its weighting factor 
                //3 Sum the resulting 11 products 
                //4 Divide the total by 89, noting the remainder 
                //5 If the remainder is zero the number is valid
                var abnString = _Input.ToString();
                //1  Subtract 1 from the first (left) digit to give a new eleven digit number 
                abnString = ReduceFirstDigit(abnString);
                //2 and 3 - Multiply each of the digits in this new number by its weighting factor 
                //10,1,3,5,7,9,11,13,15,17,19
                int runningTotal;
                if (!SumWeightings(abnString, out runningTotal)) return false;
                //4 Divide by 89 - if remainder is 0 then its a valid ABN
                return decimal.Remainder(runningTotal, 89) == 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool SumWeightings(string abnString, out int runningTotal)
        {
            runningTotal = 0;
            for (var count = 0; count <= 10; count++)
            {
                var currentDigit = int.Parse(abnString.Substring(count, 1));
                switch (count + 1)
                {
                    case 1:
                        runningTotal = currentDigit * 10;
                        break;
                    case 2:
                        runningTotal += currentDigit * 1;
                        break;
                    case 3:
                        runningTotal += currentDigit * 3;
                        break;
                    case 4:
                        runningTotal += currentDigit * 5;
                        break;
                    case 5:
                        runningTotal += currentDigit * 7;
                        break;
                    case 6:
                        runningTotal += currentDigit * 9;
                        break;
                    case 7:
                        runningTotal += currentDigit * 11;
                        break;
                    case 8:
                        runningTotal += currentDigit * 13;
                        break;
                    case 9:
                        runningTotal += currentDigit * 15;
                        break;
                    case 10:
                        runningTotal += currentDigit * 17;
                        break;
                    case 11:
                        runningTotal += currentDigit * 19;
                        break;
                    default:
                        return false;
                }
            }

            return true;
        }

        private static string ReduceFirstDigit(string abnString)
        {
            var firstDigit = int.Parse(abnString.Substring(0, 1));
            if (firstDigit == 0)
                firstDigit = 9;
            else
                firstDigit = firstDigit - 1;
            abnString = firstDigit + abnString.Substring(1);
            return abnString;
        }
    }

    #endregion
}