using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using LightVx.Tests.CustomValidator;
using LightVx.Tests.ObjectValidator;
using LightVx.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace LightVx.Tests
{
    [TestClass]
    public class ObjectValidatorTests : ValidatorUnitTestBase
    {
        [TestMethod]
        public void ValidateCustomer_Success()
        {
            var customer = new Customer();
            customer.Name = "Joe Someone";
            customer.DOB = DateTime.Now.AddYears(-25);

            var validator = new CustomerValidator(customer);
            Console.WriteLine(validator.ToString());
            Assert.IsTrue(validator.IsValid, $"Failed to validate: {validator.ToString()}" );
        }

        [TestMethod]
        public void ValidateCustomer_Fail()
        {
            var customer = new Customer();
            customer.Name = "@badchar";
            customer.DOB = DateTime.Now.AddYears(1);

            var validator = new CustomerValidator(customer);
            Console.WriteLine(validator.ToString());
            Assert.IsTrue(!validator.IsValid, $"Should have failed to validate: {validator.ToString()}");

            Assert.IsTrue(validator.ErrorMessages.Any(t => t == "Name does not contain correct values"), "Expected error for Name validation failure");
            Assert.IsTrue(validator.ErrorMessages.Any(t => t == "Date of Birth does not meet the maximum date range."), "Expected error for DOB validation failure");

        }
    }
}
