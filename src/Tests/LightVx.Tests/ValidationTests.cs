using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using LightVx;
using LightVx.Tests;
using LightVx.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Validation.LightVx.Tests
{
    [TestClass]
    public class ValidationTests : ValidatorUnitTestBase
    {
        [TestMethod]
        public void MinDate()
        {
            var date = new DateTime(2020, 1, 1);
            var validator = new MinDateValidator(date);
            TestValidatorForSuccess(validator, null);
            TestValidatorForSuccess(validator, date);
            TestValidatorForSuccess(validator, date.AddSeconds(1));
            TestValidatorForSuccess(validator, date.AddDays(1));
            TestValidatorForSuccess(validator, date.AddMonths(1));
            TestValidatorForSuccess(validator, date.AddYears(1));
            TestValidatorForFailure(validator, "0");
            TestValidatorForFailure(validator, 1);
            TestValidatorForFailure(validator, 1D);
            TestValidatorForFailure(validator, date.AddSeconds(-1));
            TestValidatorForFailure(validator, date.AddDays(-1));
        }

        [TestMethod]
        public void MaxDate()
        {
            var date = new DateTime(2020, 1, 1);
            var validator = new MaxDateValidator(date);
            TestValidatorForSuccess(validator, null);
            TestValidatorForSuccess(validator, date);
            TestValidatorForSuccess(validator, date.AddSeconds(-1));
            TestValidatorForSuccess(validator, date.AddDays(-1));
            TestValidatorForSuccess(validator, date.AddMonths(-1));
            TestValidatorForSuccess(validator, date.AddYears(-1));
            TestValidatorForFailure(validator, "0");
            TestValidatorForFailure(validator, 1);
            TestValidatorForFailure(validator, 1D);
            TestValidatorForFailure(validator, date.AddSeconds(1));
            TestValidatorForFailure(validator, date.AddDays(1));
        }

        [TestMethod]
        public void IsBool_Ok()
        {
            var validator = new BoolValidator();
            TestValidatorForSuccess(validator, "true");
            TestValidatorForSuccess(validator, "True");
            TestValidatorForSuccess(validator, "TRUE");
            TestValidatorForSuccess(validator, "false");
            TestValidatorForSuccess(validator, "False");
            TestValidatorForSuccess(validator, "False");
            TestValidatorForSuccess(validator, false);
            TestValidatorForSuccess(validator, true);
        }

        [TestMethod]
        public void IsBool_Fail()
        {
            var validator = new BoolValidator();
            TestValidatorForFailure(validator, "0");
            TestValidatorForFailure(validator, "1");
            TestValidatorForFailure(validator, "ABC");
            TestValidatorForFailure(validator, 1);
            TestValidatorForFailure(validator, 1D);
            TestValidatorForFailure(validator, 1M);
            TestValidatorForFailure(validator, "Yes");
            TestValidatorForFailure(validator, "No");
        }

        [TestMethod]
        public void IsDouble_Ok()
        {
            var validator = new DoubleValidator();
            TestValidatorForSuccess(validator, "1");
            TestValidatorForSuccess(validator, 1D);
            TestValidatorForSuccess(validator, "1.1");
            TestValidatorForSuccess(validator, 1.1);
            TestValidatorForSuccess(validator, 1.1M);
            TestValidatorForSuccess(validator, 1.1D);
        }

        [TestMethod]
        public void IsDouble_Fail()
        {
            var validator = new DoubleValidator();
            TestValidatorForFailure(validator, "ABC");
        }

        [TestMethod]
        public void IsDecimal_Ok()
        {
            var validator = new DecimalValidator();
            TestValidatorForSuccess(validator, "1");
            TestValidatorForSuccess(validator, 1D);
            TestValidatorForSuccess(validator, "1.1");
            TestValidatorForSuccess(validator, 1.1);
            TestValidatorForSuccess(validator, 1.1M);
            TestValidatorForSuccess(validator, 1.1D);
        }

        [TestMethod]
        public void IsDecimal_Fail()
        {
            var validator = new DecimalValidator();
            TestValidatorForFailure(validator, "ABC");
        }

        [TestMethod]
        public void WebSafeTextTest()
        {
            var validator = new XssSafeTextValidator();
            TestValidatorForSuccess(validator, "ABCdef0123 !@#$&*()-=:-'");
            TestValidatorForSuccess(validator, null);
            TestValidatorForSuccess(validator, string.Empty);
            TestValidatorForFailure(validator, "<script type='text/javascript'>alert('text')</script>");
            TestValidatorForFailure(validator, "<");
            TestValidatorForFailure(validator, ">");
            TestValidatorForFailure(validator, "%3C");
            TestValidatorForFailure(validator, "%3c");
            TestValidatorForFailure(validator, "%3E");
            TestValidatorForFailure(validator, "%3e");
            TestValidatorForFailure(validator, "abc<def");
            TestValidatorForFailure(validator, "<script");
        }

        [TestMethod]
        public void SqlSafeTextTest()
        {
            var validator = new SqlSafeTextValidator();
            TestValidatorForSuccess(validator, "ABCdef0123 !@$&*()-:-");
            TestValidatorForSuccess(validator, null);
            TestValidatorForSuccess(validator, string.Empty);
            TestValidatorForFailure(validator, "--");
            TestValidatorForFailure(validator, "'");
            TestValidatorForFailure(validator, "exec sp_help");
            TestValidatorForFailure(validator, "eXec sp_help");
            TestValidatorForFailure(validator, "exec xp_help");
            TestValidatorForFailure(validator, "%23");
            TestValidatorForFailure(validator, "%27");
            TestValidatorForFailure(validator, "ABC%3bDEF");
            TestValidatorForFailure(validator, "ABC %3B DEF");
        }

        [TestMethod]
        public void SafeTextTest()
        {
            var validator = new SafeTextValidator();
            TestValidatorForSuccess(validator, "ABCdef0123 !@$&*()-:-");
            TestValidatorForSuccess(validator, null);
            TestValidatorForSuccess(validator, string.Empty);
            TestValidatorForSuccess(validator, "ABCdef0123 !@$&*()-:-");
            TestValidatorForSuccess(validator, null);
            TestValidatorForSuccess(validator, string.Empty);


            TestValidatorForFailure(validator, "--");
            TestValidatorForFailure(validator, "'");
            TestValidatorForFailure(validator, "exec sp_help");
            TestValidatorForFailure(validator, "eXec sp_help");
            TestValidatorForFailure(validator, "exec xp_help");
            TestValidatorForFailure(validator, "%23");
            TestValidatorForFailure(validator, "%27");
            TestValidatorForFailure(validator, "ABC%3bDEF");
            TestValidatorForFailure(validator, "ABC %3B DEF");

            TestValidatorForFailure(validator, "<script type='text/javascript'>alert('text')</script>");
            TestValidatorForFailure(validator, "<");
            TestValidatorForFailure(validator, ">");
            TestValidatorForFailure(validator, "%3C");
            TestValidatorForFailure(validator, "%3c");
            TestValidatorForFailure(validator, "%3E");
            TestValidatorForFailure(validator, "%3e");
            TestValidatorForFailure(validator, "abc<def");
            TestValidatorForFailure(validator, "<script");
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
        public void ArrayLengthValidator()
        {
            TestValidatorForSuccess(new LengthValidator(0, 50), new int[] { });
            TestValidatorForSuccess(new LengthValidator(0, 50), new[] { 1, 2, 3 });
            TestValidatorForSuccess(new LengthValidator(0, 3), new[] { 1, 2, 3 });
            TestValidatorForSuccess(new LengthValidator(0, 3), null);
            TestValidatorForFailure(new LengthValidator(0, 1), new[] { 1, 2, 3 });
        }

        [TestMethod]
        public void AlphaTextValidator()
        {
            TestValidatorForSuccess(new AlphaTextValidator(), null);
            TestValidatorForSuccess(new AlphaTextValidator(), string.Empty);

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
            TestValidatorForSuccess(validator, "AbcTest1800@gmail.com".ToLower());
            TestValidatorForSuccess(validator, "AbcTest1800@gmail.com");
            TestValidatorForSuccess(validator, "joe.smith@smith.io");
            TestValidatorForSuccess(validator, "abc@def.com");
            TestValidatorForSuccess(validator, "abc@def.co.nz");
            TestValidatorForSuccess(validator, "abc@def.net.au");
            TestValidatorForSuccess(validator, "abc@freds.technology");
            TestValidatorForSuccess(validator, "o'connel@freds.technology");
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

        [TestMethod]
        public void SqlDateValidatorTests()
        {
            IValidator validator = new SqlSafeDateValidator();
            var minDate = SqlDateTime.MinValue.Value;
            var maxDate = SqlDateTime.MaxValue.Value;
            var noDate = new DateTime?();

            TestValidatorForSuccess(validator, minDate);
            TestValidatorForSuccess(validator, maxDate);
            TestValidatorForSuccess(validator, DateTime.Now);
            TestValidatorForSuccess(validator, noDate);
            TestValidatorForSuccess(validator, null);
            TestValidatorForFailure(validator, minDate.AddSeconds(-1));
            TestValidatorForFailure(validator, "abc");
            TestValidatorForFailure(validator, "21/12/2018"); //Must be datetime or datetime? datatype
            TestValidatorForFailure(validator, "");
        }

        [TestMethod]
        public void InCollectionValidatorTests()
        {
            string[] arrayItems = { "One", "Two", "Three", "Four", "Five" };
            int[] arrayIntItems = { 1, 2, 3, 4, 5 };
            var listItems = new List<string>(arrayItems);
            IValidator arrayValidator = new InCollectionValidator(arrayItems);
            IValidator listValidator = new InCollectionValidator(listItems);
            IValidator arrayIntValidator = new InCollectionValidator(arrayIntItems);

            TestValidatorForSuccess(arrayValidator, "One");
            TestValidatorForSuccess(listValidator, "One");
            TestValidatorForSuccess(arrayIntValidator, 1);

            TestValidatorForSuccess(arrayValidator, "Two");
            TestValidatorForSuccess(listValidator, "Two");
            TestValidatorForSuccess(arrayIntValidator, 2);

            TestValidatorForSuccess(arrayValidator, "Three");
            TestValidatorForSuccess(listValidator, "Three");
            TestValidatorForSuccess(arrayIntValidator, 3);

            TestValidatorForSuccess(arrayValidator, "Four");
            TestValidatorForSuccess(listValidator, "Four");
            TestValidatorForSuccess(arrayIntValidator, 4);

            TestValidatorForSuccess(arrayValidator, "Five");
            TestValidatorForSuccess(listValidator, "Five");
            TestValidatorForSuccess(arrayIntValidator, 5);

            TestValidatorForSuccess(arrayValidator, "");
            TestValidatorForSuccess(arrayValidator, null);

            TestValidatorForSuccess(new InCollectionValidator(arrayItems, true), "one");

            TestValidatorForFailure(arrayValidator, "one");
            TestValidatorForFailure(arrayValidator, "Six");
            TestValidatorForFailure(listValidator, "Six");
            TestValidatorForFailure(arrayIntValidator, 6);
            TestValidatorForFailure(arrayValidator, "Six");
            TestValidatorForFailure(arrayValidator, "Seven");
            TestValidatorForFailure(arrayValidator, "12345");
            TestValidatorForFailure(arrayValidator, DateTime.Today);
        }

        [TestMethod]
        public void ContainsValidatorTests()
        {
            IValidator validator = new ContainsValidator("abcde");
            TestValidatorForSuccess(validator, "x abcde x");
            TestValidatorForSuccess(validator, "xabcdex");
            TestValidatorForSuccess(validator, "abcde");
            TestValidatorForSuccess(validator, "");
            TestValidatorForSuccess(validator, null);

            TestValidatorForFailure(validator, ".");
            TestValidatorForFailure(validator, "h");
            TestValidatorForFailure(validator, "1");
            TestValidatorForFailure(validator, "!@#$");

            validator = new ContainsValidator(new[] { "abcde", "x" });
            TestValidatorForSuccess(validator, "abcde x");
            TestValidatorForSuccess(validator, "abxde abcde");
            TestValidatorForSuccess(validator, "abcde aaa x");
            TestValidatorForFailure(validator, "1");
            TestValidatorForFailure(validator, "x");
            TestValidatorForFailure(validator, "abcde");
        }

        [TestMethod]
        public void NotContainsValidatorTests()
        {
            IValidator validator = new NotContainsValidator("abcde");
            TestValidatorForSuccess(validator, "");
            TestValidatorForSuccess(validator, null);
            TestValidatorForSuccess(validator, ".");
            TestValidatorForSuccess(validator, "h");
            TestValidatorForSuccess(validator, "1");
            TestValidatorForSuccess(validator, "!@#$");

            TestValidatorForFailure(validator, "x abcde x");
            TestValidatorForFailure(validator, "xabcdex");
            TestValidatorForFailure(validator, "abcde");

            validator = new NotContainsValidator(new[] { "abcde", " " });
            validator.FieldName = "My Field";
            Assert.IsFalse(validator.Validate("abcde"));
            Assert.IsTrue(validator.ErrorMessage.Contains("My Field"));
            TestValidatorForFailure(validator, "abcde");
            TestValidatorForFailure(validator, "x a");
            TestValidatorForFailure(validator, " ");
            TestValidatorForSuccess(validator, "1");
        }
    }
}