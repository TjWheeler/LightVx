using System;
using LightVx.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LightVx.Tests
{
    [TestClass]
    public class ValidatorTests
    {
        [TestMethod]
        public void IsValidTests()
        {
            string errorMessage;
            Assert.IsTrue(Validator.IsValid<AlphaNumericValidator>("ABC123", "TestField", out errorMessage), errorMessage);
            Assert.IsFalse(Validator.IsValid<AlphaNumericValidator>("ABC123-", "TestField", out errorMessage), errorMessage);

            Assert.IsFalse(Validator.IsNotValid<AlphaNumericValidator>("ABC123", "TestField", out errorMessage), errorMessage);
            Assert.IsTrue(Validator.IsNotValid<AlphaNumericValidator>("ABC123-", "TestField", out errorMessage), errorMessage);

            Assert.IsTrue(Validator.IsValid<AlphaNumericValidator>("ABC123", "TestField"));
            Assert.IsFalse(Validator.IsValid<AlphaNumericValidator>("ABC123-", "TestField"));

            Assert.IsFalse(Validator.IsNotValid<AlphaNumericValidator>("ABC123", "TestField"));
            Assert.IsTrue(Validator.IsNotValid<AlphaNumericValidator>("ABC123-", "TestField"));

        }
    }
}
