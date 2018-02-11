using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightVx.Tests.CustomValidator;
using LightVx.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LightVx.Tests
{
    [TestClass]
    public class ExampleMethodTests
    {
        private void Example2()
        {
            var input = "123ABC";
            IValidator validator = new NumericValidator();
            validator.Validate(input);
            if (!validator.IsValid) Console.WriteLine(validator.ErrorMessage);
        }

        private void Example2Compressed()
        {
            var input = "123ABC";
            IValidator validator = new NumericValidator();
            string userErrorMessage;
            if (!validator.Validate(input, "First Name", out userErrorMessage))
                Console.WriteLine(userErrorMessage);
        }
        [TestMethod]
        public void Example3FluentApi()
        {
            var input = "Joe";
            Validator.Eval(input, "Customer First Name")
                .Required()
                .IsAlphaText()
                .HasLength(2, null)
                .Fail((errors, validators) =>
                {
                    Console.WriteLine(string.Join(",", errors));
                     // Validation failed, put your failure logic here
                 });

        }
        [TestMethod]
        public void ExampleFluentApi()
        {
            var input = "123ABC";
            Validator.Eval(input, "Customer ID")
                .Required()
                .IsAlphaNumeric()
                .HasLength(6, 6)
                .Success((() =>
                {
                    Console.WriteLine("Validation succeeded");
                }))
                .Fail((errors, validators) =>
                {
                    Console.WriteLine(string.Join(",", errors));
                    // Validation failed, put your failure logic here
                });

        }
        [TestMethod]
        public void Example4FluentApi()
        {
            var input = 100; //user input to be validated
            var result = Validator.Eval(input, "Quantity Requested")
                .Required()
                .Min(50)
                .Max(100)
                .Validate();
            if (result.IsValid == false)
            {
                Console.WriteLine(string.Join(";", result.ErrorMessages));
                //... add failure logic here
            }
        }
        [TestMethod]
        public void Example5FluentApi()
        {
            var input = "https://github.com/TjWheeler/LightVx"; //user input to be validated
            Validator.Eval(input, "Source Url")
                .Required()
                .IsUrl()
                .Success((() =>
                {
                    Console.WriteLine("Validation succeeded");
                }))
                .Fail((errors, validators) =>
                {
                    Console.WriteLine(string.Join(",", errors));
                    // Validation failed, put your failure logic here
                });
        }

        [TestMethod]
        public void ExampleDirect()
        {
            //Validate numberic
            string input = "123ABC";
            IValidator validator = new NumericValidator();
            validator.Validate(input, "My Field Name");
            if (!validator.IsValid)
            {
                Console.WriteLine("Input: " + input + " return error (" + validator.ErrorMessage + ")");
            }
                
        }
        [TestMethod]
        public void Example2Test()
        {
            string input = "ABCD";
            bool? isValid = Validator.Eval(input, "MyFieldName")
                .Required()
                .HasLength(0, 3)
                .IsAlphaText()
                .Fail(((errors, validators) =>
                {
                    Console.WriteLine("Example failure: " + string.Join(";", errors));
                    return;
                })).IsValid;
            if (isValid != false)
            {
                Assert.Fail("This validator should have failed");
            }
        }
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

    }
}
