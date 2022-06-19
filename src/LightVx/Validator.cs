//   Author: Tim Wheeler
//   GitHub: https://github.com/TjWheeler/LightVx
//   Purpose: User Input Validation - Web Service / Web API Input Validation
//   Description: A simple validation framework.  Intended to validate user input. 
//              : Also useful for validating webservice or webapi input.
//              : Validators can be combined using the AggregatedValidator.
//  Note: Validators generally return Success if the data is null.  The Required validator and NotNull validators are the exceptions.

//Examples:
/*
		Example 1: Quick syntax example using Validator helper:
 		string errorMessage;
        if (Validator.IsNotValid<AlphaTextValidator>(input, nameof(account), out errorMessage))
        {
            return BadRequest($"The input is invalid: {errorMessage}");
        }

		Example 2: Fluent API example:		 
        public void Example2Test()
        {
            string input = "ABCD";
            var isValid = Validator.Eval(input, "MyFieldName")
                    .Required()
                    .HasLength(0, 3)
                    .IsAlphaText()
                    .Fail(((errors, validators) =>
                    {
                        Console.WriteLine("Example failure: " + string.Join(";", errors));
                        
                    })).IsValid;
            if (isValid != false)
            {
                Assert.Fail("This validator should have failed");
            }
        }
 */
/*   These examples use the validators directly.  For more convenience use the Validator helper.
       private void Example1Numeric()
       {
           string input = "123ABC";
           IValidator validator = new NumericValidator();
           validator.Input = input;
           validator.Validate();
           if (!validator.IsValid)
               Console.WriteLine("Input: " + input + " return error (" + validator.ErrorMessage + ")");
       }
       private void Example2ControlName()
       {   //This example adds the Control Name to a validate function call which incorporates it
           //in the user error message;
           string input = "123ABC";
           IValidator validator = new NumericValidator();
           string userErrorMessage;
           if(!validator.Validate(input,"Phone Number", out userErrorMessage))
               Console.WriteLine(userErrorMessage);
       }   
*/

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

//Adding custom validator to the Fluent API
//Step 1: Add the custom validator
/* public class PostCodeValidator : AggregatedValidator
    {
        public PostCodeValidator()
        {
            AddValidator(new LengthValidator(4, 4));
            AddValidator(new NumericValidator());
        }
    }
*/
//Step 2: Add an extension method
/*  public static class PostCodeValidatorExtension
    {
        public static ValidatorFluent IsPostCode(this ValidatorFluent fluentApi)
        {
            fluentApi.AddValidator(new PostCodeValidator());
            return fluentApi;
        }
    }
*/
//Step 3: Call it to validate input
/*
    public void ExampleCustomValidator()
    {
        string input = "...";
        var isValid = Validator.Eval(input, "MyFieldName")
            .Required()
            .IsPostCode()
            .Fail(((errors, validators) =>
            {
                Console.WriteLine("Example failure: " + string.Join(";", errors));
            })).IsValid;
    }
 *
 */
//Notes: In most situations a validator should return success for null or empty string data.
//       This allows for validators like the length validator to be used or combined if the field
//       is required.  
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

        public static bool IsNotValid<T>(string input, string fieldName)
            where T : IValidator
        {
            return !IsValid<T>(input, fieldName);
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