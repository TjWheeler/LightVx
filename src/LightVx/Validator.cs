//   Author: Tim Wheeler
//   GitHub: https://github.com/TjWheeler/LightVx
//   Contains:    User Input Validation - Reusable and Extendable classes
//   Description: A simple validation framework.  Intended to validate user input. 
//              : Also useful for validating webservice or webapi input.
//              : Validators can be combined using the AggregatedValidator.


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
using System.Diagnostics;

namespace LightVx
{
    /// <summary>
    /// Primary access to validation functions.
    /// </summary>
    [DebuggerStepThrough]
    public static class Validator
    {
        public static ValidatorFluent Eval(this object input, string fieldName)
        {
            return new ValidatorFluent(input, fieldName);
        }
        public static ValidatorFluent Eval(this object input)
        {
            return new ValidatorFluent(input);
        }
        public static bool IsValid<T>(string input, string fieldName) where T : IValidator
        {
            var validator = (IValidator) Activator.CreateInstance(typeof(T));
            validator.Validate(input, fieldName);
            return validator.IsValid;
        }

        public static bool IsNotValid<T>(string input, string fieldName, out string errorMessage)
            where T : IValidator
        {
            return !IsValid<T>(input, fieldName, out errorMessage);
        }

        public static bool IsValid<T>(string input, string fieldName, out string errorMessage) where T : IValidator
        {
            var validator = (IValidator) Activator.CreateInstance(typeof(T));
            validator.Validate(input, fieldName);
            errorMessage = validator.IsValid ? string.Empty : validator.ErrorMessage;
            return validator.IsValid;
        }
    }
}