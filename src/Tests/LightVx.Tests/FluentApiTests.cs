using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Net.NetworkInformation;
using LightVx.Tests.CustomValidator;
using LightVx.Tests.Files;
using LightVx.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LightVx.Tests
{
    [TestClass]
    public class FluentApiTests : ValidatorUnitTestBase
    {
        [TestMethod]
        public void IsAfterDateTests()
        {
            var date = new DateTime(2020, 1, 1);
            ExpectSuccess(Validator.Eval((string) null).IsAfter(date));
            ExpectSuccess(Validator.Eval(date.AddMilliseconds(1)).IsAfter(date));
            ExpectSuccess(Validator.Eval(date.AddDays(1)).IsAfter(date));

            ExpectFailure(Validator.Eval(date).IsAfter(date));
            ExpectFailure(Validator.Eval(date.AddSeconds(-1)).IsAfter(date));
        }

        [TestMethod]
        public void IsBeforeDateTests()
        {
            var date = new DateTime(2020, 1, 1);
            ExpectSuccess(Validator.Eval((string) null).IsBefore(date));
            ExpectSuccess(Validator.Eval(date.AddMilliseconds(-1)).IsBefore(date));
            ExpectSuccess(Validator.Eval(date.AddDays(-1)).IsBefore(date));

            ExpectFailure(Validator.Eval(date).IsBefore(date));
            ExpectFailure(Validator.Eval(date.AddSeconds(1)).IsBefore(date));
        }

        [TestMethod]
        public void BetweenDateTest_Ok()
        {
            var startDate = new DateTime(2020, 1, 1);
            var endDate = new DateTime(2020, 2, 1);
            ExpectSuccess(Validator.Eval((string) null).IsBetweenDates(startDate, endDate));
            ExpectSuccess(Validator.Eval(startDate.AddDays(1)).IsBetweenDates(startDate, endDate));
            ExpectSuccess(Validator.Eval(endDate.AddSeconds(-1)).IsBetweenDates(startDate, endDate));
            ExpectSuccess(Validator.Eval(startDate).IsBetweenDates(startDate, endDate));
            ExpectSuccess(Validator.Eval(endDate).IsBetweenDates(startDate, endDate));
        }

        [TestMethod]
        public void BetweenDateTest_Fail()
        {
            var startDate = new DateTime(2020, 1, 1);
            var endDate = new DateTime(2020, 2, 1);
            ExpectFailure(Validator.Eval(startDate.AddDays(-1)).IsBetweenDates(startDate, endDate));
            ExpectFailure(Validator.Eval(endDate.AddSeconds(1)).IsBetweenDates(startDate, endDate));
        }

        [TestMethod]
        public void MinDateTest_Ok()
        {
            var date = new DateTime(2020, 1, 1);
            ExpectSuccess(Validator.Eval((string) null).Min(date));
            ExpectSuccess(Validator.Eval(date).Min(date));
            ExpectSuccess(Validator.Eval(date.AddSeconds(1)).Min(date));
            ExpectSuccess(Validator.Eval(date.AddDays(1)).Min(date));
        }

        [TestMethod]
        public void MinDateTest_Fail()
        {
            var date = new DateTime(2020, 1, 1);
            ExpectFailure(Validator.Eval(string.Empty).Min(date));
            ExpectFailure(Validator.Eval(date.AddSeconds(-1)).Min(date));
            ExpectFailure(Validator.Eval(date.AddDays(-1)).Min(date));
        }

        [TestMethod]
        public void MaxDateTest_Ok()
        {
            var date = new DateTime(2020, 1, 1);
            ExpectSuccess(Validator.Eval((string) null));
            ExpectSuccess(Validator.Eval(date).Max(date));
            ExpectSuccess(Validator.Eval(date.AddSeconds(-1)).Max(date));
            ExpectSuccess(Validator.Eval(date.AddDays(-11)).Max(date));
        }

        [TestMethod]
        public void MaxDateTest_Fail()
        {
            var date = new DateTime(2020, 1, 1);
            ExpectFailure(Validator.Eval(string.Empty).Max(date));
            ExpectFailure(Validator.Eval(date.AddSeconds(1)).Max(date));
            ExpectFailure(Validator.Eval(date.AddDays(1)).Max(date));
        }

        [TestMethod]
        public void IsDoubleTest_Ok()
        {
            ExpectSuccess(Validator.Eval((string) null).IsDouble());
            ExpectSuccess(Validator.Eval(string.Empty).IsDouble());
            ExpectSuccess(Validator.Eval("1").IsDouble());
            ExpectSuccess(Validator.Eval(1).IsDouble());
            ExpectSuccess(Validator.Eval(1D).IsDouble());
            ExpectSuccess(Validator.Eval(1M).IsDouble());
        }

        [TestMethod]
        public void IsDoubleTest_Fail()
        {
            ExpectFailure(Validator.Eval("ABC").IsDouble());
        }
        [TestMethod]
        public void IsCurrencyTest_Ok()
        {
            string[] testValues = { "$1.00", "$1", "$1,234.56", "€1.234,56", "£1234.56", "¥1234", "₹1,234.56", "$1,000,000.00" };
            foreach (var value in testValues)
            {
                ExpectSuccess(Validator.Eval(value).IsCurrency());
            }
            ExpectSuccess(Validator.Eval((string)null).IsCurrency(false));
            ExpectSuccess(Validator.Eval(string.Empty).IsCurrency(false));
            ExpectSuccess(Validator.Eval("1").IsCurrency(false));
            ExpectSuccess(Validator.Eval(1).IsCurrency(false));
            ExpectSuccess(Validator.Eval(1D).IsCurrency(false));
            ExpectSuccess(Validator.Eval(1M).IsCurrency(false));
        }

        [TestMethod]
        public void IsCurrencyTest_Fail()
        {
            ExpectFailure(Validator.Eval("ABC").IsCurrency());
            ExpectFailure(Validator.Eval("ABC").IsCurrency());
            ExpectFailure(Validator.Eval("1").IsCurrency());
            ExpectFailure(Validator.Eval(1).IsCurrency());
            ExpectFailure(Validator.Eval(1D).IsCurrency());
            ExpectFailure(Validator.Eval(1M).IsCurrency());
        }
        [TestMethod]
        public void IsIsoTimeTest_Ok()
        {
            string[] testValues = { "23:59:59Z",
                "15:30:45.123+02:00",
                "08:15:00-05:00",
            };
            foreach (var value in testValues)
            {
                ExpectSuccess(Validator.Eval(value).IsIsoTime());
            }
        }

        [TestMethod]
        public void IsIsoTimeTest_Fail()
        {
            object[] testValues = {
                "2025-02-30", // Invalid (Feb 30 doesn't exist)
                "2025-13-01", // Invalid (Month 13)
                "2025-04-31",  // Invalid (April has only 30 days)
                "25:30:45.123+02:00",
                "24:00:00",
                "12:60:00",
                "ABC",
                "12pm",
                "13:00",
                1,
                new object(),
                DateTime.Now
            };
            foreach (var value in testValues)
            {
                ExpectFailure(Validator.Eval(value).IsIsoTime());
            }
        }
        [TestMethod]
        public void IsIsoDateTest_Ok()
        {
            string[] testValues = {
                "2025-05-03",
                "1999-12-31",
                "0001-01-01",
            };
            foreach (var value in testValues)
            {
                ExpectSuccess(Validator.Eval(value).IsIsoDate());
            }
        }

        [TestMethod]
        public void IsIsoDateTest_Fail()
        {
            object[] testValues = {
                "2025-02-30", // Invalid (Feb 30 doesn't exist)
                "2025-13-01", // Invalid (Month 13)
                "2025-04-31",  // Invalid (April has only 30 days)
                "25:30:45.123+02:00",
                "24:00:00",
                "12:60:00",
                "ABC",
                "12pm",
                "13:00",
                1,
                new object(),
                DateTime.Now,
            };
            foreach (var value in testValues)
            {
                ExpectFailure(Validator.Eval(value).IsIsoDate());
            }
        }
        [TestMethod]
        public void IsIsoDateTimeTest_Ok()
        {
            string[] testValues = {
                "2025-05-03T14:30:15Z",        // Valid UTC time
                "1999-12-31T23:59:59.999+02:00", // Valid with milliseconds and offset
            };
            foreach (var value in testValues)
            {
                ExpectSuccess(Validator.Eval(value).IsIsoDateTime());
            }
        }

        [TestMethod]
        public void IsIsoDateTimeTest_Fail()
        {
            object[] testValues = {
                "2025-02-30T12:00:00Z",        // Invalid (Feb 30 doesn't exist)
                "2025-13-01T08:15:00-05:00",   // Invalid (Month 13)
                "2025-04-31T10:20:45",         // Invalid (April only has 30 days, missing time zone)
                "24:00:00",
                "12:60:00",
                "ABC",
                "12pm",
                "13:00",
                1,
                new object(),
                DateTime.Now,
            };
            foreach (var value in testValues)
            {
                ExpectFailure(Validator.Eval(value).IsIsoDateTime());
            }
        }

        [TestMethod]
        public void IsDecimalTest_Ok()
        {
            ExpectSuccess(Validator.Eval((string) null).IsDecimal());
            ExpectSuccess(Validator.Eval(string.Empty).IsDecimal());
            ExpectSuccess(Validator.Eval("1").IsDecimal());
            ExpectSuccess(Validator.Eval(1).IsDecimal());
            ExpectSuccess(Validator.Eval(1D).IsDecimal());
            ExpectSuccess(Validator.Eval(1M).IsDecimal());
        }

        [TestMethod]
        public void IsDecimalTest_Fail()
        {
            ExpectFailure(Validator.Eval("ABC").IsDecimal());
        }

        [TestMethod]
        public void IsIntTest_Ok()
        {
            ExpectSuccess(Validator.Eval((string) null).IsInt());
            ExpectSuccess(Validator.Eval(string.Empty).IsInt());
            ExpectSuccess(Validator.Eval("1").IsInt());
            ExpectSuccess(Validator.Eval(1).IsInt());
            ExpectSuccess(Validator.Eval(1D).IsInt());
            ExpectSuccess(Validator.Eval(1M).IsInt());
        }

        [TestMethod]
        public void IsIntTest_Fail()
        {
            ExpectFailure(Validator.Eval("ABC").IsInt());
        }

        [TestMethod]
        public void RequiredValidatorTest_Ok()
        {
            ExpectSuccess(Validator.Eval("abc").Required());
            ExpectSuccess(Validator.Eval("1").Required());
            ExpectSuccess(Validator.Eval("123").Required());
        }

        [TestMethod]
        public void RequiredValidatorTest_Fail()
        {
            ExpectFailure(Validator.Eval("").Required());
            ExpectFailure(Validator.Eval((string) null).Required());
            var result = Validator.Eval((string) null).Required().Validate();
            Assert.IsTrue(result.ErrorMessages.Count == 1);
            Assert.AreEqual("The Field is required", result.ErrorMessages[0]);
        }

        [TestMethod]
        public void ValidatorFieldNameTest_Ok()
        {
            bool hasFoundName = false;
            var onFail = new Action<List<string>, List<IValidator>>((list, validators) =>
            {
                foreach (var validator in validators)
                {
                    Assert.AreEqual("Name", validator.FieldName, "The Field Name value has not been returned");
                    hasFoundName = true;
                }
            });
            Validator.Eval((string) null, "Name").Required().Fail(onFail);
            Assert.IsTrue(hasFoundName, "Validation did not trigger");
        }

        [TestMethod]
        public void ValidatorFieldDisplayNameTest_Ok()
        {
            bool hasFoundName = false;
            var onFail = new Action<List<string>, List<IValidator>>((list, validators) =>
            {
                foreach (var validator in validators)
                {
                    Assert.AreEqual("Name", validator.FieldName, "The Field Name value has not been returned");
                    Assert.AreEqual("First Name", validator.FieldDisplayName, "The Field Display Name value has not been returned");
                    Assert.IsTrue(validator.ErrorMessage.Contains("First Name"));
                    hasFoundName = true;
                }
            });
            Validator.Eval((string) null, "Name", "First Name").Required().Fail(onFail);
            Assert.IsTrue(hasFoundName, "Validation did not trigger");
        }

        [TestMethod]
        public void AlphaNumbericValidatorTest_Ok()
        {
            ExpectSuccess(Validator.Eval("123").IsAlphaNumeric());
            ExpectSuccess(Validator.Eval(123).IsAlphaNumeric());
            ExpectSuccess(Validator.Eval("ABC123").IsAlphaNumeric());
        }

        [TestMethod]
        public void AlphaNumbericValidatorTest_Fail()
        {
            ExpectFailure(Validator.Eval("123.34").IsAlphaNumeric());
            ExpectFailure(Validator.Eval(123.34).IsAlphaNumeric());
        }

        [TestMethod]
        public void LengthValidatorTest_Ok()
        {
            ExpectSuccess(Validator.Eval("123").HasLength(0, 3));
            ExpectSuccess(Validator.Eval("123").HasLength(1, 3));
            ExpectSuccess(Validator.Eval("123").HasLength(3, 3));
            ExpectSuccess(Validator.Eval("123456789").HasLength(9, 9));
            ExpectSuccess(Validator.Eval("123456789").HasLength(0, 9));
        }

        [TestMethod]
        public void LengthValidatorTest_Fail()
        {
            ExpectFailure(Validator.Eval("12345").HasLength(6, 7));
            ExpectFailure(Validator.Eval(123.34).HasLength(0, 3));
        }

        [TestMethod]
        public void AlphaTextValidatorTest_Ok()
        {
            ExpectSuccess(Validator.Eval("ABC DEF").IsAlphaText());
        }

        [TestMethod]
        public void AlphaTextValidatorTest_Fail()
        {
            ExpectFailure(Validator.Eval("123.34").IsAlphaText());
            ExpectFailure(Validator.Eval("ABC-DEF").IsAlphaText());
        }

        [TestMethod]
        public void PhoneNumberValidatorTest_Ok()
        {
            ExpectSuccess(Validator.Eval("(555) 1234 5678").IsPhoneNumber());
            ExpectSuccess(Validator.Eval("555-1234-5678").IsPhoneNumber());
        }

        [TestMethod]
        public void PhoneNumberValidatorTest_Fail()
        {
            ExpectFailure(Validator.Eval("ABC").IsPhoneNumber());
            ExpectFailure(Validator.Eval("(04) 1234-abcd").IsPhoneNumber());
        }

        [TestMethod]
        public void SafeTextValidatorTest_Ok()
        {
            ExpectSuccess(Validator.Eval("ABCDE DEF@$%^&*").IsSafeText());
        }

        [TestMethod]
        public void SafeTextValidatorTest_Fail()
        {
            ExpectFailure(Validator.Eval("<&").IsSafeText());
            ExpectFailure(Validator.Eval(">").IsSafeText());
            ExpectFailure(Validator.Eval("--").IsSafeText());
            ExpectFailure(Validator.Eval("%27").IsSafeText());
            ExpectFailure(Validator.Eval("'").IsSafeText());
        }

        [TestMethod]
        public void WebSafeTextValidatorTest_Ok()
        {
            ExpectSuccess(Validator.Eval("ABCdef0123 !@#$&*()-=:-'").IsSafeForXss());
        }

        [TestMethod]
        public void WebSafeTextValidatorTest_Fail()
        {
            ExpectFailure(Validator.Eval("<script type='text/javascript'>alert('text')</script>").IsSafeText());
            ExpectFailure(Validator.Eval(">").IsSafeText());
            ExpectFailure(Validator.Eval(">").IsSafeText());
        }

        [TestMethod]
        public void SqlSafeTextValidatorTest_Ok()
        {
            ExpectSuccess(Validator.Eval("ABCdef0123 !@$&*()-:-").IsSafeForXss());
        }

        [TestMethod]
        public void SqlSafeTextValidatorTest_Fail()
        {
            ExpectFailure(Validator.Eval("' or 'a' = 'a'").IsSafeText());
            ExpectFailure(Validator.Eval("--").IsSafeText());
            ExpectFailure(Validator.Eval("%23").IsSafeText());
        }

        [TestMethod]
        public void UrlValidatorTest_Ok()
        {
            ExpectSuccess(Validator.Eval("http://abc.com").IsUrl());
            ExpectSuccess(Validator.Eval("https://abc.com").IsUrl());
            ExpectSuccess(Validator.Eval("https://abc.com:1234").IsUrl());
        }

        [TestMethod]
        public void UrlValidatorTest_Fail()
        {
            ExpectFailure(Validator.Eval("www.someaddress.com").IsUrl());
            ExpectFailure(Validator.Eval("http://www.some address.com").IsUrl());
        }

        [TestMethod]
        public void MinValidatorTest_Ok()
        {
            ExpectSuccess(Validator.Eval("10").Min(10));
            ExpectSuccess(Validator.Eval("10").Min(5));
            ExpectSuccess(Validator.Eval("10").Min(9));
            ExpectSuccess(Validator.Eval("10").Min(10));
            ExpectSuccess(Validator.Eval(10).Min(5));
            ExpectSuccess(Validator.Eval(10).Min(9));
            ExpectSuccess(Validator.Eval(10).Min(10));
            ExpectSuccess(Validator.Eval(10D).Min(10D));
            ExpectSuccess(Validator.Eval(10M).Min(10M));

            ExpectSuccess(Validator.Eval("10").Min(10));
            ExpectSuccess(Validator.Eval("10.5").Min(10D));
            ExpectSuccess(Validator.Eval("10.5").Min(10M));

            ExpectSuccess(Validator.Eval((string) null).Min(10));
            ExpectSuccess(Validator.Eval(new[] { "first", "second", "third" }).Min(1));
            ExpectSuccess(Validator.Eval(new[] { "first", "second", "third" }).Min(2));
            ExpectSuccess(Validator.Eval(new[] { "first", "second", "third" }).Min(3));
            var collection = new List<string>();
            collection.AddRange(new[] { "first", "second", "third" });
            ExpectSuccess(Validator.Eval(collection).Min(1));
            ExpectSuccess(Validator.Eval(collection).Min(2));
            ExpectSuccess(Validator.Eval(collection).Min(3));
        }

        [TestMethod]
        public void MinValidatorTest_Fail()
        {
            ExpectFailure(Validator.Eval("10").Min(11));
            ExpectFailure(Validator.Eval("10").Min(100));
            ExpectFailure(Validator.Eval("10").Min(11));
            ExpectFailure(Validator.Eval("10.5").Min(11D));
            ExpectFailure(Validator.Eval("10.5").Min(11M));
            ExpectFailure(Validator.Eval("10.5A").Min(11M));
            ExpectFailure(Validator.Eval(10).Min(11));
            ExpectFailure(Validator.Eval(10).Min(100));
            ExpectFailure(Validator.Eval(new object()).Min(11));
            ExpectFailure(Validator.Eval(new[] { "first", "second", "third" }).Min(4));
            var collection = new List<string>();
            collection.AddRange(new[] { "first", "second", "third" });
            ExpectFailure(Validator.Eval(collection).Min(4));
        }

        [TestMethod]
        public void MaxValidatorTest_Ok()
        {
            ExpectSuccess(Validator.Eval("10").Max(10));
            ExpectSuccess(Validator.Eval("10").Max(11));
            ExpectSuccess(Validator.Eval("10").Max(100));
            ExpectSuccess(Validator.Eval(10).Max(10));
            ExpectSuccess(Validator.Eval(10).Max(11));
            ExpectSuccess(Validator.Eval(10).Max(100));
            ExpectSuccess(Validator.Eval(10D).Max(10D));
            ExpectSuccess(Validator.Eval(10M).Max(10M));
            ExpectSuccess(Validator.Eval(10D).Max(10));
            ExpectSuccess(Validator.Eval(10M).Max(10));
            ExpectSuccess(Validator.Eval((string) null).Max(10));
            ExpectSuccess(Validator.Eval(new[] { "first", "second", "third" }).Max(3));
            ExpectSuccess(Validator.Eval(new[] { "first", "second", "third" }).Max(4));
            var collection = new List<string>();
            collection.AddRange(new[] { "first", "second", "third" });
            ExpectSuccess(Validator.Eval(collection).Max(3));
            ExpectSuccess(Validator.Eval(collection).Max(4));
        }

        [TestMethod]
        public void MaxValidatorTest_Fail()
        {
            ExpectFailure(Validator.Eval("10").Max(9));
            ExpectFailure(Validator.Eval("10").Max(0));
            ExpectFailure(Validator.Eval(10).Max(9));
            ExpectFailure(Validator.Eval(10).Max(1));
            ExpectFailure(Validator.Eval(10).Max(0));
            ExpectFailure(Validator.Eval(new[] { "first", "second", "third" }).Max(2));
            ExpectFailure(Validator.Eval(new[] { "first", "second", "third" }).Max(1));
            ExpectFailure(Validator.Eval(new[] { "first", "second", "third" }).Max(0));
            var collection = new List<string>();
            collection.AddRange(new[] { "first", "second", "third" });
            ExpectFailure(Validator.Eval(collection).Max(2));
            ExpectFailure(Validator.Eval(collection).Max(1));
            ExpectFailure(Validator.Eval(collection).Max(0));
        }

        [TestMethod]
        public void EmptyValidatorTest_Ok()
        {
            ExpectSuccess(Validator.Eval((string) null).IsEmpty());
            ExpectSuccess(Validator.Eval(string.Empty).IsEmpty());
        }

        [TestMethod]
        public void EmptyValidatorTest_Fail()
        {
            ExpectFailure(Validator.Eval("A").IsEmpty());
            ExpectFailure(Validator.Eval(1).IsEmpty());
            ExpectFailure(Validator.Eval(100M).IsEmpty());
            ExpectFailure(Validator.Eval(100D).IsEmpty());
        }

        [TestMethod]
        public void NotEmptyValidatorTest_Ok()
        {
            ExpectSuccess(Validator.Eval("A").IsNotEmpty());
            ExpectSuccess(Validator.Eval(1).IsNotEmpty());
            ExpectSuccess(Validator.Eval(1D).IsNotEmpty());
            ExpectSuccess(Validator.Eval(1M).IsNotEmpty());
        }

        [TestMethod]
        public void NotEmptyValidatorTest_Fail()
        {
            ExpectFailure(Validator.Eval((string) null).IsNotEmpty());
            ExpectFailure(Validator.Eval(string.Empty).IsNotEmpty());
        }

        [TestMethod]
        public void NullValidatorTest_Ok()
        {
            ExpectSuccess(Validator.Eval((string) null).IsNull());
        }

        [TestMethod]
        public void NullValidatorTest_Fail()
        {
            ExpectFailure(Validator.Eval(string.Empty).IsNull());
            ExpectFailure(Validator.Eval("A").IsNull());
        }

        [TestMethod]
        public void NotNullValidatorTest_Ok()
        {
            ExpectSuccess(Validator.Eval("A").IsNotNull());
            ExpectSuccess(Validator.Eval(string.Empty).IsNotNull());
            ExpectSuccess(Validator.Eval(1).IsNotNull());
            ExpectSuccess(Validator.Eval(1D).IsNotNull());
            ExpectSuccess(Validator.Eval(1M).IsNotNull());
        }

        [TestMethod]
        public void NotNullValidatorTest_Fail()
        {
            ExpectFailure(Validator.Eval((string) null).IsNotNull());
        }

        [TestMethod]
        public void MinLengthValidatorTest_Ok()
        {
            ExpectSuccess(Validator.Eval("A").HasMinLength(1));
            ExpectSuccess(Validator.Eval("1233456").HasMinLength(1));
            ExpectSuccess(Validator.Eval("1233456").HasMinLength(6));
            ExpectSuccess(Validator.Eval((string) null).HasMinLength(5));
            ExpectSuccess(Validator.Eval(1).HasMinLength(1));
            ExpectSuccess(Validator.Eval(1D).HasMinLength(1));
            ExpectSuccess(Validator.Eval(1M).HasMinLength(1));
            ExpectSuccess(Validator.Eval(1111).HasMinLength(4));
        }

        [TestMethod]
        public void MinLengthValidatorTest_Fail()
        {
            ExpectFailure(Validator.Eval("A").HasMinLength(2));
            ExpectFailure(Validator.Eval("AB").HasMinLength(3));
            ExpectFailure(Validator.Eval(12).HasMinLength(3));
        }

        [TestMethod]
        public void MaxLengthValidatorTest_Ok()
        {
            ExpectSuccess(Validator.Eval("A").HasMaxLength(1));
            ExpectSuccess(Validator.Eval("A").HasMaxLength(2));
            ExpectSuccess(Validator.Eval("123456").HasMaxLength(6));
            ExpectSuccess(Validator.Eval((string) null).HasMaxLength(5));
            ExpectSuccess(Validator.Eval(1).HasMaxLength(1));
            ExpectSuccess(Validator.Eval(1D).HasMaxLength(1));
            ExpectSuccess(Validator.Eval(1M).HasMaxLength(1));
            ExpectSuccess(Validator.Eval(1111).HasMaxLength(4));
        }

        [TestMethod]
        public void MaxLengthValidatorTest_Fail()
        {
            ExpectFailure(Validator.Eval("A").HasMaxLength(0));
            ExpectFailure(Validator.Eval("AB").HasMaxLength(1));
            ExpectFailure(Validator.Eval(12).HasMaxLength(1));
        }

        [TestMethod]
        public void TestCustomValidator()
        {
            ExpectSuccess(Validator.Eval("0123").IsPostCode());
            ExpectSuccess(Validator.Eval("0000").IsPostCode());
            ExpectSuccess(Validator.Eval("9999").IsPostCode());
            ExpectFailure(Validator.Eval("A999").IsPostCode());
            ExpectFailure(Validator.Eval("999").IsPostCode());
            ExpectSuccess(Validator.Eval(1234).IsPostCode());
            ExpectFailure(Validator.Eval(123).IsPostCode());
        }

        [TestMethod]
        public void IsSqlDateTests()
        {
            var minDate = SqlDateTime.MinValue.Value;
            var maxDate = SqlDateTime.MaxValue.Value;
            var noDate = new DateTime?();

            ExpectSuccess(Validator.Eval((string) null).IsSqlDate());
            ExpectSuccess(Validator.Eval(minDate).IsSqlDate());
            ExpectSuccess(Validator.Eval(maxDate).IsSqlDate());
            ExpectSuccess(Validator.Eval(DateTime.Now).IsSqlDate());
            ExpectSuccess(Validator.Eval(noDate).IsSqlDate());

            ExpectFailure(Validator.Eval("").IsSqlDate());
            ExpectFailure(Validator.Eval("abc").IsSqlDate());
            ExpectFailure(Validator.Eval("123").IsSqlDate());
            ExpectFailure(Validator.Eval("21/12/2018").IsSqlDate()); //Must be datetime or datetime? datatype

        }

        [TestMethod]
        public void IsIn()
        {
            string[] arrayItems = { "One", "Two", "Three", "Four", "Five" };
            int[] arrayIntItems = { 1, 2, 3, 4, 5 };
            List<string> listItems = new List<string>(arrayItems);

            ExpectSuccess(Validator.Eval((string) null).IsIn(listItems, false));
            ExpectSuccess(Validator.Eval("One").IsIn(listItems, false));
            ExpectSuccess(Validator.Eval("One").IsIn(arrayItems, false));
            ExpectSuccess(Validator.Eval("one").IsIn(listItems, true));
            ExpectFailure(Validator.Eval("one").IsIn(listItems, false));
            ExpectSuccess(Validator.Eval("One").IsIn(listItems, false));

            ExpectSuccess(Validator.Eval(1).IsIn(arrayIntItems, false));
            ExpectSuccess(Validator.Eval(1).IsIn(arrayIntItems, true));

            ExpectFailure(Validator.Eval(6).IsIn(arrayIntItems, false));
            ExpectFailure(Validator.Eval(6).IsIn(arrayIntItems, true));
            ExpectFailure(Validator.Eval("Six").IsIn(listItems, false));
            ExpectFailure(Validator.Eval("Six").IsIn(listItems, true));

            //Collections
            string[] stringArrayMaster = { "One", "Two", "Three", "Four", "Five" };
            string[] stringArraySubset = { "One", "Two", "Three" };
            ExpectSuccess(Validator.Eval(stringArraySubset).IsIn(stringArrayMaster, false));
            ExpectFailure(Validator.Eval(new[] { "OneTwo", "Two" }).IsIn(stringArrayMaster, false));
            ExpectSuccess(Validator.Eval(Array.Empty<string>()).IsIn(stringArrayMaster, false));
        }

        [TestMethod]
        public void ContainsTests()
        {
            ExpectSuccess(Validator.Eval((string) null).Contains("abc", false));
            ExpectSuccess(Validator.Eval("").Contains("abc", false));
            ExpectSuccess(Validator.Eval("abcdef").Contains("abc", false));
            ExpectSuccess(Validator.Eval("aaa abc ccc").Contains("abc", false));
            ExpectSuccess(Validator.Eval("aaa abc ccc").Contains("abc", true));
            ExpectSuccess(Validator.Eval("aaa aBc ccc").Contains("abc", true));
            ExpectSuccess(Validator.Eval("aaa aBc def").Contains(new[] { "abc", "def" }, true));
            ExpectSuccess(Validator.Eval("aaa abc def").Contains(new[] { "abc", "def" }, false));

            ExpectFailure(Validator.Eval("a ab acb").Contains("abc", false));
            ExpectFailure(Validator.Eval("aaa aBc ccc").Contains("abc", false));
        }

        [TestMethod]
        public void DoesNotContainTests()
        {
            ExpectSuccess(Validator.Eval((string) null).DoesNotContain("abc", false));
            ExpectSuccess(Validator.Eval("").DoesNotContain("abc", false));
            ExpectSuccess(Validator.Eval("a ab acb").DoesNotContain("abc", false));
            ExpectSuccess(Validator.Eval("aaa aBc ccc").DoesNotContain("abc", false));

            ExpectFailure(Validator.Eval("abcdef").DoesNotContain("abc", false));
            ExpectFailure(Validator.Eval("aaa abc ccc").DoesNotContain("abc", false));
            ExpectFailure(Validator.Eval("aaa abc ccc").DoesNotContain("abc", true));
            ExpectFailure(Validator.Eval("aaa aBc ccc").DoesNotContain("abc", true));
            ExpectFailure(Validator.Eval("aaa aBc def").DoesNotContain(new[] { "abc", "def" }, true));
            ExpectFailure(Validator.Eval("aaa abc def").DoesNotContain(new[] { "abc", "def" }, false));
        }
        
        [TestMethod]
        public void MatchesExpressionTests()
        {
            ExpectSuccess(Validator.Eval((string) null).MatchesExpression("^[a-zA-Z]*$"));
            ExpectSuccess(Validator.Eval("abc").MatchesExpression("^[a-zA-Z]*$"));
            ExpectSuccess(Validator.Eval("abcDEF").MatchesExpression("^[a-zA-Z]*$"));

            ExpectFailure(Validator.Eval("abc DEF").MatchesExpression("^[a-zA-Z]*$"));
            ExpectFailure(Validator.Eval("abc#DEF").MatchesExpression("^[a-zA-Z]*$"));
            ExpectFailure(Validator.Eval("123").MatchesExpression("^[a-zA-Z]*$"));
        }

        [TestMethod]
        public void ValidationSetTests()
        {
            var numberTo5Set = Validator.Define().IsNumeric().Max(5);
            ExpectSuccess(Validator.Eval(1,"Number", numberTo5Set));
            ExpectSuccess(Validator.Eval("1", "Number", numberTo5Set));
            ExpectFailure(Validator.Eval(6, "Number", numberTo5Set));
            ExpectFailure(Validator.Eval("6", "Number", numberTo5Set));

            ExpectSuccess(Validator.Eval("1", "Number", numberTo5Set));
            ExpectSuccess(Validator.Eval(1, "Number", "FieldName", numberTo5Set));
            ExpectFailure(Validator.Eval("6", "Number", numberTo5Set));
            ExpectFailure(Validator.Eval(6, "FieldName", "FieldDisplayName", numberTo5Set));

            var nameValidationSet = Validator.Define().Required().IsNameText().HasMaxLength(15);
            ExpectSuccess(Validator.Eval("Joe Smith").ValidateWith(nameValidationSet));
            ExpectSuccess(Validator.Eval("Mark O'Malley").ValidateWith(nameValidationSet));
            ExpectFailure(Validator.Eval("Joe#Smith").ValidateWith(nameValidationSet));
            ExpectFailure(Validator.Eval(new string('A',16)).ValidateWith(nameValidationSet));
        }

        [TestMethod]
        public void ImageSignatureTests()
        {
            using (var stream = FileProvider.GetFile(TestFileEnum.SmallGif))
            {
                ExpectSuccess(Validator.Eval(stream).HasGifImageSignature());
            }
            using (var stream = FileProvider.GetFile(TestFileEnum.SmallJpg))
            {
                ExpectSuccess(Validator.Eval(stream).HasJpgImageSignature());
            }
            using (var stream = FileProvider.GetFile(TestFileEnum.SmallPng))
            {
                ExpectSuccess(Validator.Eval(stream).HasPngImageSignature());
            }
            
            //Failures
            using (var stream = FileProvider.GetFile(TestFileEnum.SmallGif))
            {
                ExpectFailure(Validator.Eval(stream).HasPngImageSignature());
            }
            using (var stream = FileProvider.GetFile(TestFileEnum.SmallJpg))
            {
                ExpectFailure(Validator.Eval(stream).HasGifImageSignature());
            }
            using (var stream = FileProvider.GetFile(TestFileEnum.SmallPng))
            {
                ExpectFailure(Validator.Eval(stream).HasJpgImageSignature());
            }
        }
        [TestMethod]
        public void ErrorMessagesCountTest()
        {
            var result = Validator.Eval("abcdef").Contains("abc", false).DoesNotContain("zxy");
            Assert.IsTrue(result.ErrorMessages.Count == 0, "Error messages should be 0");
        }

    }
}