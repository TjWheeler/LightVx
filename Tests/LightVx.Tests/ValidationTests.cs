using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Validation.LightVx.Tests
{
    [TestClass]
    public class ValidationTests
    {

        private void Example2()
        {
            string input = "123ABC";
            IInputValidator validator = new NumericValidator();
            string userErrorMessage;
            validator.Validate(input);
            if (!validator.IsValid)
            {
                Console.WriteLine(validator.ErrorMessage);
            }

        }
    
        private void Example2Compressed()
        {
            string input = "123ABC";
            IInputValidator validator = new NumericValidator();
            string userErrorMessage;
            if(!validator.Validate(input,"First Name", out userErrorMessage))
                Console.WriteLine(userErrorMessage);
        }

        
        
        
        private void TestValidatorForSuccess(IInputValidator validator, object input)
        {
            validator.Validate(input);
            if(!validator.IsValid)
                Console.WriteLine("Input: " + input + " return error (" + validator.ErrorMessage + ")");
            EvaluateTruePart(validator);
            string errorMessage;
            Assert.IsTrue(validator.Validate(input, "Input Field", out errorMessage));
        }

        private static void EvaluateTruePart(IInputValidator validator)
        {
            Assert.IsTrue(validator.IsValid,
                          "Validator " + validator.GetType().Name +
                          " returned IsValid = false, but should have been true.");
            Assert.IsTrue(validator.ErrorMessage == string.Empty, "Error Message was not empty");
        }

        private void TestValidatorForFailure(IInputValidator validator, object input)
        {
            validator.Validate(input);
            if(validator.IsValid)
                Console.WriteLine(validator.GetType().Name + " failed to identify a validation issue with (" + input + ")");
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
            string input = "29002589460";
            ABNValidator validator = new ABNValidator();
            TestValidatorForSuccess(validator, input);
            TestValidatorForFailure(validator, null);
            TestValidatorForFailure(validator, "3004085616");
            TestValidatorForFailure(validator, "");
            TestValidatorForFailure(validator, null);
            TestValidatorForFailure(validator, "23005085616");
        }

        [TestMethod]
        public void AlphaTextValidator()
        {
            string data = "abc";
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
            string data = "123abc";
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
            string data = "04 976 1234";
            TestValidatorForSuccess(new PhoneNumberValidator(), data);
            data = null;
            TestValidatorForSuccess(new PhoneNumberValidator(), data);
            data = string.Empty;
            TestValidatorForSuccess(new PhoneNumberValidator(), data);
        }

        [TestMethod]
        public void PhoneValidationBad()
        {
            string data = "a04 9b76 1c234";
            TestValidatorForFailure(new PhoneNumberValidator(), data);
            PhoneNumberValidator phone = new PhoneNumberValidator();
            string error;
            phone.Validate(data, "My Test Field", out error);
            Console.WriteLine("Received Error (expected): " + error);
            
        }

        [TestMethod]
        public void PhoneValidationAlternateChar()
        {
            string data = "(04) 976-1234";
            TestValidatorForSuccess(new PhoneNumberValidator(), data);
        }

        [TestMethod]
        public void PhoneAndLengthAggregateValidator()
        {
            string data = "(04) 976 1234";
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
            EmailValidator validator = new EmailValidator();
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
            UrlValidator validator = new UrlValidator();
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
        /// Credit card validator is not currently working.
        /// </summary>
        //[TestMethod]
        public void CreditCardValidatorTests()
        {
            //This tests dont' work.  need to check CC formats.
            CreditCardValidator validator = new CreditCardValidator();
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
            IInputValidator validator = new NumericValidator();
            TestValidatorForSuccess(validator, 123);
            TestValidatorForSuccess(validator,"123");
            TestValidatorForSuccess(validator,123M);
            TestValidatorForSuccess(validator,"");
            TestValidatorForSuccess(validator,null);
            TestValidatorForFailure(validator, "abc");
            TestValidatorForFailure(validator, "1v3");
            TestValidatorForFailure(validator, "32.45");
            TestValidatorForFailure(validator, 12.25M);
        }
        //Length min max, postcode is 4
    }
}