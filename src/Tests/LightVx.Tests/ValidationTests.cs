using System;
using System.Linq;
using LightVx;
using LightVx.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Validation.LightVx.Tests
{
    [TestClass]
    public class ValidationTests
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
                 .HasLength(2,null)
                 .Fail((errors, validators) =>
                 {
                     Console.WriteLine(string.Join(",",errors));
                     // Validation failed, put your failure logic here
                 });

        }
        private void TestValidatorForSuccess(IValidator validator, object input)
        {
            validator.Validate(input);
            if (!validator.IsValid)
                Console.WriteLine("Input: " + input + " return error (" + validator.ErrorMessage + ")");
            EvaluateTruePart(validator);
            string errorMessage;
            Assert.IsTrue(validator.Validate(input, "Input Field", out errorMessage));
        }

        private static void EvaluateTruePart(IValidator validator)
        {
            Assert.IsTrue(validator.IsValid,
                "Validator " + validator.GetType().Name +
                " returned IsValid = false, but should have been true.");
            Assert.IsTrue(validator.ErrorMessage == string.Empty, "Error Message was not empty");
        }

        private void TestValidatorForFailure(IValidator validator, object input)
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

        [TestMethod]
        public void ABNTest()
        {
            var input = "29002589460";
            var validator = new AbnValidator();
            TestValidatorForSuccess(validator, input);
            TestValidatorForFailure(validator, null);
            TestValidatorForFailure(validator, "3004085616");
            TestValidatorForFailure(validator, "");
            TestValidatorForFailure(validator, null);
            TestValidatorForFailure(validator, "23005085616");
        }
        [TestMethod]
        public void HexColorTest()
        {
            
            var validator = new HexColorValidator();
            TestValidatorForSuccess(validator, "#ffffff");
            TestValidatorForSuccess(validator, "#fff");
            TestValidatorForSuccess(validator, null);
            TestValidatorForFailure(validator, "3004085616");
            TestValidatorForFailure(validator, "");
        }

        [TestMethod]
        public void AlphaTextValidator()
        {
            var data = "abc";
            TestValidatorForSuccess(new AlphaTextValidator(), data);

            data = "abc1";
            TestValidatorForFailure(new AlphaTextValidator(), data);

            data = "a1c";
            TestValidatorForFailure(new AlphaTextValidator(), data);

            data = "1bc";
            TestValidatorForFailure(new AlphaTextValidator(), data);

            data = "1";
            TestValidatorForFailure(new AlphaTextValidator(), data);

            data = "!c";
            TestValidatorForFailure(new AlphaTextValidator(), data);
        }


        [TestMethod]
        public void LengthValidator()
        {
            var data = "123abc";
            TestValidatorForSuccess(new LengthValidator(3, 6), data);
            TestValidatorForSuccess(new LengthValidator(6, 6), data);
            TestValidatorForFailure(new LengthValidator(3, 5), data);
            TestValidatorForFailure(new LengthValidator(7, 7), data);
            data = null;
            TestValidatorForSuccess(new LengthValidator(0, 6), data);
            data = string.Empty;
            TestValidatorForSuccess(new LengthValidator(0, 6), data);

            data = "Thanks for the feedback.  Here's my text.\n\tTjw";
            TestValidatorForSuccess(new LengthValidator(1, 500), data);
        }


        [TestMethod]
        public void PhoneValidationGood()
        {
            var data = "04 976 1234";
            TestValidatorForSuccess(new PhoneNumberValidator(), data);
            data = null;
            TestValidatorForSuccess(new PhoneNumberValidator(), data);
            data = string.Empty;
            TestValidatorForSuccess(new PhoneNumberValidator(), data);
        }

        [TestMethod]
        public void PhoneValidationBad()
        {
            var data = "a04 9b76 1c234";
            TestValidatorForFailure(new PhoneNumberValidator(), data);
            var phone = new PhoneNumberValidator();
            string error;
            phone.Validate(data, "My Test Field", out error);
            Console.WriteLine("Received Error (expected): " + error);
        }

        [TestMethod]
        public void PhoneValidationAlternateChar()
        {
            var data = "(04) 976-1234";
            TestValidatorForSuccess(new PhoneNumberValidator(), data);
        }

        [TestMethod]
        public void PhoneAndLengthAggregateValidator()
        {
            var data = "(04) 976 1234";
            TestValidatorForSuccess(new PhoneAndLengthValidator(3, 13), data);
            TestValidatorForFailure(new PhoneAndLengthValidator(3, 12), data);

            data = "(04) 976-1234";
            TestValidatorForSuccess(new PhoneAndLengthValidator(6, 13), data);
            TestValidatorForFailure(new PhoneAndLengthValidator(3, 5), data);

            data = "(04)";
            TestValidatorForFailure(new PhoneAndLengthValidator(6, 13), data);
        }

        [TestMethod]
        public void EmailValidatorTests()
        {
            var validator = new EmailValidator();
            TestValidatorForSuccess(validator, "abc@def.com");
            TestValidatorForSuccess(validator, "abc@def.co.nz");
            TestValidatorForSuccess(validator, "abc@def.net.au");
            TestValidatorForSuccess(validator, "abcdefghijklmnopqrstuvwxy.z-j@def.com");
            TestValidatorForFailure(validator, " a @b");
            TestValidatorForFailure(validator, "ab.co.nz");
            TestValidatorForFailure(validator, "frank.anne@com");
            TestValidatorForFailure(validator, "joe. @bcd.net.au");
            TestValidatorForFailure(validator, " a @b");
            TestValidatorForSuccess(validator, null);
            TestValidatorForSuccess(validator, "");
        }

        [TestMethod]
        public void UrlValidatorTests()
        {
            var validator = new UrlValidator();
            TestValidatorForSuccess(validator, "http://www.purecoding.net");
            TestValidatorForSuccess(validator, "http://www.abc.co.nz/about/default.aspx");
            TestValidatorForSuccess(validator, "http://www.abc.net.au");
            TestValidatorForSuccess(validator, "https://www.secure.com/default.htm");
            TestValidatorForFailure(validator, "http:// a @b");
            TestValidatorForFailure(validator, "http:/www.abc.com");
            TestValidatorForFailure(validator, "zhttp://www.abc.com");
            TestValidatorForSuccess(validator, "");
            TestValidatorForSuccess(validator, null);
        }

        /// <summary>
        ///     Credit card validator is not currently working.
        /// </summary>
        //[TestMethod]
        public void CreditCardValidatorTests()
        {
            //This tests dont' work.  need to check CC formats.
            var validator = new CreditCardValidator();
            TestValidatorForSuccess(validator, "1234 5789 3214 4567");
            TestValidatorForSuccess(validator, "1234-5789-3214-4567");
            TestValidatorForSuccess(validator, "1234578932144567");
            TestValidatorForFailure(validator, "12345789a2144567");
            TestValidatorForFailure(validator, "1234578932144567234324");
            TestValidatorForSuccess(validator, null);
            TestValidatorForSuccess(validator, "");
        }

        [TestMethod]
        public void DecimalValidatorTests()
        {
            TestValidatorForSuccess(new DecimalValidator(), 4100.25M);
            TestValidatorForSuccess(new DecimalValidator(), "4100.25");
            TestValidatorForSuccess(new DecimalValidator(), 4100M);
            TestValidatorForFailure(new DecimalValidator(), "4100.25X");
        }

        [TestMethod]
        public void NumericValidatorTests()
        {
            IValidator validator = new NumericValidator();
            TestValidatorForSuccess(validator, 123);
            TestValidatorForSuccess(validator, "123");
            TestValidatorForSuccess(validator, 123M);
            TestValidatorForSuccess(validator, "");
            TestValidatorForSuccess(validator, null);
            TestValidatorForFailure(validator, "abc");
            TestValidatorForFailure(validator, "1v3");
            TestValidatorForFailure(validator, "32.45");
            TestValidatorForFailure(validator, 12.25M);
        }

        //Length min max, postcode is 4
    }
}