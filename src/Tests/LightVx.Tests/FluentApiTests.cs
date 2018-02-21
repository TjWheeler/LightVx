﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LightVx.Tests.CustomValidator;

namespace LightVx.Tests
{
    [TestClass]
    public class FluentApiTests : ValidatorUnitTestBase
    {
        

        [TestMethod]
        public void RequiredValidatorTest_Ok()
        {
            ExpectSuccess("abc".Eval().Required());
            ExpectSuccess("1".Eval().Required());
            ExpectSuccess("123".Eval().Required());
        }

        [TestMethod]
        public void RequiredValidatorTest_Fail()
        {
            ExpectFailure("".Eval().Required());
            ExpectFailure(Validator.Eval(null).Required());
        }

        [TestMethod]
        public void AlphaNumbericValidatorTest_Ok()
        {
            ExpectSuccess("123".Eval().IsAlphaNumeric());
            ExpectSuccess(123.Eval().IsAlphaNumeric());
            ExpectSuccess("ABC123".Eval().IsAlphaNumeric());
        }

        [TestMethod]
        public void AlphaNumbericValidatorTest_Fail()
        {
            ExpectFailure("123.34".Eval().IsAlphaNumeric());
            ExpectFailure(123.34.Eval().IsAlphaNumeric());
        }

        [TestMethod]
        public void LengthValidatorTest_Ok()
        {
            ExpectSuccess("123".Eval().HasLength(0, 3));
            ExpectSuccess("123".Eval().HasLength(1, 3));
            ExpectSuccess("123".Eval().HasLength(3, 3));
            ExpectSuccess("123456789".Eval().HasLength(9, 9));
            ExpectSuccess("123456789".Eval().HasLength(0, 9));
        }

        [TestMethod]
        public void LengthValidatorTest_Fail()
        {
            ExpectFailure("12345".Eval().HasLength(6, 7));
            ExpectFailure(123.34.Eval().HasLength(0, 3));
        }

        [TestMethod]
        public void AlphaTextValidatorTest_Ok()
        {
            ExpectSuccess("ABC DEF".Eval().IsAlphaText());
        }

        [TestMethod]
        public void AlphaTextValidatorTest_Fail()
        {
            ExpectFailure("123.34".Eval().IsAlphaText());
            ExpectFailure("ABC-DEF".Eval().IsAlphaText());
        }

        [TestMethod]
        public void PhoneNumberValidatorTest_Ok()
        {
            ExpectSuccess("(555) 1234 5678".Eval().IsPhoneNumber());
            ExpectSuccess("555-1234-5678".Eval().IsPhoneNumber());
        }

        [TestMethod]
        public void PhoneNumberValidatorTest_Fail()
        {
            ExpectFailure("ABC".Eval().IsPhoneNumber());
            ExpectFailure("(04) 1234-abcd".Eval().IsPhoneNumber());
        }

        [TestMethod]
        public void SafeTextValidatorTest_Ok()
        {
            ExpectSuccess("ABCDE DEF@$%^&*".Eval().IsSafeText());
        }

        [TestMethod]
        public void SafeTextValidatorTest_Fail()
        {
            ExpectFailure("<&".Eval().IsSafeText());
            ExpectFailure(">".Eval().IsSafeText());
            ExpectFailure("--".Eval().IsSafeText());
            ExpectFailure("%27".Eval().IsSafeText());
            ExpectFailure("'".Eval().IsSafeText());
        }

        [TestMethod]
        public void WebSafeTextValidatorTest_Ok()
        {
            ExpectSuccess("ABCdef0123 !@#$&*()-=:-'".Eval().IsSafeForXss());
        }

        [TestMethod]
        public void WebSafeTextValidatorTest_Fail()
        {
            ExpectFailure("<script type='text/javascript'>alert('text')</script>".Eval().IsSafeText());
            ExpectFailure(">".Eval().IsSafeText());
            ExpectFailure(">".Eval().IsSafeText());
        }
        [TestMethod]
        public void SqlSafeTextValidatorTest_Ok()
        {
            ExpectSuccess("ABCdef0123 !@$&*()-:-".Eval().IsSafeForXss());
        }

        [TestMethod]
        public void SqlSafeTextValidatorTest_Fail()
        {
            ExpectFailure("' or 'a' = 'a'".Eval().IsSafeText());
            ExpectFailure("--".Eval().IsSafeText());
            ExpectFailure("%23".Eval().IsSafeText());
        }

        [TestMethod]
        public void UrlValidatorTest_Ok()
        {
            ExpectSuccess("http://abc.com".Eval().IsUrl());
            ExpectSuccess("https://abc.com".Eval().IsUrl());
            ExpectSuccess("https://abc.com:1234".Eval().IsUrl());
        }

        [TestMethod]
        public void UrlValidatorTest_Fail()
        {
            ExpectFailure("www.someaddress.com".Eval().IsUrl());
            ExpectFailure("http://www.some address.com".Eval().IsUrl());
        }

        [TestMethod]
        public void MinValidatorTest_Ok()
        {
            ExpectSuccess("10".Eval().Min(10));
            ExpectSuccess("10".Eval().Min(5));
            ExpectSuccess("10".Eval().Min(9));
            ExpectSuccess("10".Eval().Min(10));
            ExpectSuccess(10.Eval().Min(5));
            ExpectSuccess(10.Eval().Min(9));
            ExpectSuccess(10.Eval().Min(10));
            ExpectSuccess(10D.Eval().Min(10D));
            ExpectSuccess(10M.Eval().Min(10M));

            ExpectSuccess(Validator.Eval("10").Min(10));
            ExpectSuccess(Validator.Eval("10.5").Min(10D));
            ExpectSuccess(Validator.Eval("10.5").Min(10M));

            ExpectSuccess(Validator.Eval(null).Min(10));
            ExpectSuccess(Validator.Eval(new[] { "first", "second", "third" }).Min(1));
            ExpectSuccess(Validator.Eval(new [] {"first", "second", "third"}).Min(2));
            ExpectSuccess(Validator.Eval(new[] { "first", "second", "third" }).Min(3));
            List<string> collection = new List<string>();
            collection.AddRange(new[] { "first", "second", "third" });
            ExpectSuccess(Validator.Eval(collection).Min(1));
            ExpectSuccess(Validator.Eval(collection).Min(2));
            ExpectSuccess(Validator.Eval(collection).Min(3));
        }

        [TestMethod]
        public void MinValidatorTest_Fail()
        {
            ExpectFailure("10".Eval().Min(11));
            ExpectFailure("10".Eval().Min(100));
            ExpectFailure(Validator.Eval("10").Min(11));
            ExpectFailure(Validator.Eval("10.5").Min(11D));
            ExpectFailure(Validator.Eval("10.5").Min(11M));
            ExpectFailure(Validator.Eval("10.5A").Min(11M));
            ExpectFailure(10.Eval().Min(11));
            ExpectFailure(10.Eval().Min(100));
            ExpectFailure(Validator.Eval(new object()).Min(11));
            ExpectFailure(Validator.Eval(new[] { "first", "second", "third" }).Min(4));
            List<string> collection = new List<string>();
            collection.AddRange(new[] { "first", "second", "third" });
            ExpectFailure(Validator.Eval(collection).Min(4));
        }

        [TestMethod]
        public void MaxValidatorTest_Ok()
        {
            ExpectSuccess("10".Eval().Max(10));
            ExpectSuccess("10".Eval().Max(11));
            ExpectSuccess("10".Eval().Max(100));
            ExpectSuccess(10.Eval().Max(10));
            ExpectSuccess(10.Eval().Max(11));
            ExpectSuccess(10.Eval().Max(100));
            ExpectSuccess(10D.Eval().Max(10D));
            ExpectSuccess(10M.Eval().Max(10M));
            ExpectSuccess(10D.Eval().Max(10));
            ExpectSuccess(10M.Eval().Max(10));
            ExpectSuccess(Validator.Eval(null).Max(10));
            ExpectSuccess(Validator.Eval(new[] { "first", "second", "third" }).Max(3));
            ExpectSuccess(Validator.Eval(new[] { "first", "second", "third" }).Max(4));
            List<string> collection = new List<string>();
            collection.AddRange(new[] { "first", "second", "third" });
            ExpectSuccess(Validator.Eval(collection).Max(3));
            ExpectSuccess(Validator.Eval(collection).Max(4));
        }

        [TestMethod]
        public void MaxValidatorTest_Fail()
        {
            ExpectFailure("10".Eval().Max(9));
            ExpectFailure("10".Eval().Max(0));
            ExpectFailure(10.Eval().Max(9));
            ExpectFailure(10.Eval().Max(1));
            ExpectFailure(10.Eval().Max(0));
            ExpectFailure(Validator.Eval(new[] { "first", "second", "third" }).Max(2));
            ExpectFailure(Validator.Eval(new[] { "first", "second", "third" }).Max(1));
            ExpectFailure(Validator.Eval(new[] { "first", "second", "third" }).Max(0));
            List<string> collection = new List<string>();
            collection.AddRange(new[] { "first", "second", "third" });
            ExpectFailure(Validator.Eval(collection).Max(2));
            ExpectFailure(Validator.Eval(collection).Max(1));
            ExpectFailure(Validator.Eval(collection).Max(0));
        }

        [TestMethod]
        public void EmptyValidatorTest_Ok()
        {
            ExpectSuccess(Validator.Eval(null).IsEmpty());
            ExpectSuccess(Validator.Eval(string.Empty).IsEmpty());
        }

        [TestMethod]
        public void EmptyValidatorTest_Fail()
        {
            ExpectFailure("A".Eval().IsEmpty());
            ExpectFailure(1.Eval().IsEmpty());
            ExpectFailure(100M.Eval().IsEmpty());
            ExpectFailure(100D.Eval().IsEmpty());
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
            ExpectFailure(Validator.Eval(null).IsNotEmpty());
            ExpectFailure(Validator.Eval(string.Empty).IsNotEmpty());
        }

        [TestMethod]
        public void NullValidatorTest_Ok()
        {
            ExpectSuccess(Validator.Eval(null).IsNull());
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
            ExpectFailure(Validator.Eval(null).IsNotNull());
        }

        [TestMethod]
        public void MinLengthValidatorTest_Ok()
        {
            ExpectSuccess(Validator.Eval("A").HasMinLength(1));
            ExpectSuccess(Validator.Eval("1233456").HasMinLength(1));
            ExpectSuccess(Validator.Eval("1233456").HasMinLength(6));
            ExpectSuccess(Validator.Eval(null).HasMinLength(5));
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
            ExpectSuccess(Validator.Eval(null).HasMaxLength(5));
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
    }
}