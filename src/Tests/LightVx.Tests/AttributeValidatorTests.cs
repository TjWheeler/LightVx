﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using LightVx;
using LightVx.Tests.AttributeValidation;
using LightVx.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace LightVx.Tests
{
    [TestClass]
    public class AttributeValidatorTests : ValidatorUnitTestBase
    {
        [TestMethod]
        public void PersonAttributeTest()
        {
            var person = new Person()
            {
                Id = string.Empty,
                FirstName = "Joe",
                LastName = "Smith"
            };
            Assert.IsTrue(Validator.Validate(person).IsValid);
            person.FirstName = new string('X', 11);
            Assert.IsFalse(Validator.Validate(person).IsValid);
            var result = Validator.Validate(person);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetFieldResult(nameof(Person.FirstName)));
            Assert.AreEqual(nameof(Person.FirstName), result.GetFieldResult(nameof(Person.FirstName)).FieldName);
            Assert.AreEqual(2, result.ValidatorResults.Count);
            Assert.AreEqual(1, result.ValidatorResults.Count(t => t.FieldName == nameof(Person.FirstName)));
            var firstNameResult = result.GetFieldResult(nameof(Person.FirstName));
            Assert.IsFalse(firstNameResult.IsValid);
            var lastNameResult = result.GetFieldResult(nameof(Person.LastName));
            Assert.IsTrue(lastNameResult.IsValid);
            Assert.IsTrue(result.FieldErrorMessages.Count == 1);

            person.FirstName = "Joe";
            person.LastName = "ABC $%";
            result = Validator.Validate(person);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.FieldErrorMessages.ContainsKey(nameof(Person.LastName)));
            Assert.AreEqual(1, result.FieldErrorMessages.Count);

            person.LastName = null;
            result = Validator.Validate(person);
            Assert.IsTrue(result.IsValid);

            person.FirstName = null;
            result = Validator.Validate(person);
            Assert.IsFalse(result.IsValid);

            person.FirstName = string.Empty;
            result = Validator.Validate(person);
            Assert.IsFalse(result.IsValid);
        }
    }
}