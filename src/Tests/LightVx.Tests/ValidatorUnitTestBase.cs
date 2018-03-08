using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LightVx.Tests
{
    public class ValidatorUnitTestBase
    {
        protected void TestValidatorForSuccess(IValidator validator, object input)
        {
            validator.Validate(input);
            if (!validator.IsValid)
                Console.WriteLine("Input: " + input + " return error (" + validator.ErrorMessage + ")");
            EvaluateTruePart(validator);
            string errorMessage;
            Assert.IsTrue(validator.Validate(input, "Input Field", out errorMessage));
        }

        protected static void EvaluateTruePart(IValidator validator)
        {
            Assert.IsTrue(validator.IsValid,
                "Validator " + validator.GetType().Name +
                " returned IsValid = false, but should have been true.");
            Assert.IsTrue(validator.ErrorMessage == string.Empty, "Error Message was not empty");
        }

        protected void TestValidatorForFailure(IValidator validator, object input)
        {
            validator.Validate(input);
            if (validator.IsValid)
                Console.WriteLine(validator.GetType().Name + " failed to identify a validation issue with (" + input +
                                  ")");
            Assert.IsFalse(validator.IsValid,
                "Validator " + validator.GetType().Name +
                " returned IsValid = true, but should have been false.");
            Assert.IsFalse(validator.ErrorMessage == string.Empty, "Error Message was empty");
            string errorMessage;
            validator.Validate(input, "Input Field", out errorMessage);
            Console.WriteLine("Expected Error: " + validator.GetType().Name + " : " + errorMessage);
            Assert.IsFalse(validator.Validate(input, "Input Field", out errorMessage));
        }

        protected void ExpectSuccess(ValidatorFluent validation)
        {
            bool? success = null;
            validation
                .Success(() => { success = true; })
                .Fail((errors, validators) =>
                {
                    Console.WriteLine(string.Join(",", errors));
                    success = false;
                });
            if (!success.HasValue)
            {
                Assert.Fail("Received neither success or failure.");
                return;
            }

            if (success == false) Assert.Fail("Expected success, but received failure.");
        }

        protected void ExpectFailure(ValidatorFluent validation)
        {
            bool? success = null;
            validation
                .Success(() => { success = true; })
                .Fail((errors, validators) => { success = false; });
            if (!success.HasValue) Assert.Fail("Received neither success or failure.");
            if (success == true) Assert.Fail("Expected failure but received success.");
        }
    }
}
