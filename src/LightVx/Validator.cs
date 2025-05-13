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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace LightVx
{
    /// <summary>
    /// Primary access to validation functions.
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Evaluate values directly
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="fieldName"></param>
        /// <param name="validationDefinition"></param>
        /// <returns></returns>
        public static ValidatorFluent Eval<T>(T input, string fieldName, ValidatorFluent validationDefinition = null)
        {
            return new ValidatorFluent(input, fieldName, validationDefinition);
        }
        /// <summary>
        /// Evaluate values directly
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldDisplayName"></param>
        /// <param name="validationDefinition"></param>
        /// <returns></returns>
        public static ValidatorFluent Eval<T>(T input, string fieldName, string fieldDisplayName, ValidatorFluent validationDefinition = null)
        {
            return new ValidatorFluent(input, fieldName, fieldDisplayName, validationDefinition);
        }
        public static ValidatorFluent Eval<T>(T input)
        {
            return new ValidatorFluent(input);
        }

        /// <summary>
        /// Create a Validation Definition which can be supplied later and reused
        /// </summary>
        /// <returns></returns>
        public static ValidatorFluent Define()
        {
            return new ValidatorFluent();
        }
        /// <summary>
        /// Validates an Object based on its Property Attributes.
        /// Attributes must be of type <see cref="IAttributeValidator"/>.
        /// Optionally restrict validation to a subset of fields
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="validateFields">t => new { t.FirstName, t.LastName } will restrict validation to specified fields only</param>
        /// <returns></returns>
        public static ValidationResult Validate<T>(T input, Expression<Func<T, object>> validateFields = null) where T : class
        {
            var validators = GetAttributeValidators(input);
            var result = new ValidationResult();
            var propNames = validateFields != null ? GetPropNames(validateFields) : new List<string>();
            foreach (var validator in validators)
            {
                if(propNames.Count > 0 && !propNames.Contains(validator.FieldName)) continue;
                validator.Validate();
                if (result.FieldResults.ContainsKey(validator.FieldName))
                {
                    var fieldResult = result.FieldResults[validator.FieldName];
                    fieldResult.Validators.Add(validator);
                }
                else
                {
                    result.FieldResults.Add(validator.FieldName, new ValidatorResult()
                    {
                        FieldName = validator.FieldName,
                        FieldDisplayName = validator.FieldDisplayName,
                        Validators = new List<IValidator>(new []{validator}),
                    });
                }
            }
            return result;
        }
        private static List<string> GetPropNames<T>(Expression<Func<T, object>> selectedProperties)
        {
            var result = new List<string>();
            var expression = selectedProperties.Body as NewExpression;
            if (expression != null)
            {
                foreach (var member in expression.Members)
                {
                    result.Add(member.Name);
                }
            }
            return result;
        }
        public static bool IsValid<T>(string input, string fieldName) where T : IValidator
        {
            var validator = (IValidator)Activator.CreateInstance(typeof(T));
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
            var validator = (IValidator)Activator.CreateInstance(typeof(T));
            validator.Validate(input, fieldName);
            errorMessage = validator.IsValid ? string.Empty : validator.ErrorMessage;
            return validator.IsValid;
        }
        private static void AddCustomAttributeValidators<T>(T o, ValidatorFluent fluent)
        {
            var validators = GetAttributeValidators(o);
            if (validators.Count > 0)
            {
                fluent.Validators.AddRange(validators);
            }
        }
        private static List<IValidator> GetAttributeValidators<T>(T o)
        {
            var validators = new List<IValidator>();
            PropertyInfo[] props = typeof(T).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    if (attr is IAttributeValidator)
                    {
                        object value = prop.GetValue(o, null);
                        IValidator validator = ((IAttributeValidator)attr).Validator;
                        validator.FieldName = prop.Name;
                        validator.Input = value;
                        validator.FieldDisplayName = GetDisplayNameFromAttribute(prop);
                        validators.Add(validator);
                    }
                }
            }
            return validators;
        }
        
        private static string GetDisplayNameFromAttribute(PropertyInfo pInfo)
        {
            object[] attrs = pInfo.GetCustomAttributes(true);
            foreach (object attr in attrs)
            {
                if (attr is DisplayNameAttribute)
                {
                    return ((DisplayNameAttribute)attr).DisplayName;
                }
            }
            //No display name, so lets assume pascal case and add spaces.
            return Regex.Replace(pInfo.Name, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
        }
    }
}