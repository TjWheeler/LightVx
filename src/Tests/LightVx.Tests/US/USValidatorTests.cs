using System;
using LightVx.Validators.US;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LightVx.Tests.US
{
    [TestClass]
    public class USValidatorTests : ValidatorUnitTestBase
    {
        [TestMethod]
        public void StateValidator_Ok()
        {
            var validator = new USStateValidator();
            TestValidatorForSuccess(validator, null);
            TestValidatorForSuccess(validator, string.Empty);
            TestValidatorForSuccess(validator, "AK");
            TestValidatorForSuccess(validator, "LA");
            TestValidatorForSuccess(validator, "WY");
            TestValidatorForSuccess(validator, "TX");
            TestValidatorForSuccess(validator, "UT");
        }
        [TestMethod]
        public void StateValidator_Fail()
        {
            var validator = new USStateValidator();
            TestValidatorForFailure(validator, "ak");
            TestValidatorForFailure(validator, "AKA");
            TestValidatorForFailure(validator, " AK");
            TestValidatorForFailure(validator, " AK ");
            TestValidatorForFailure(validator, "AK-");
            TestValidatorForFailure(validator, "abcdefg");
        }
        [TestMethod]
        public void StateValidatorFluent_Ok()
        {
            ExpectSuccess(Validator.Eval("AK").IsUSState());
            ExpectSuccess(Validator.Eval("LA").IsUSState());
            ExpectSuccess(Validator.Eval("WY").IsUSState());
            ExpectSuccess(Validator.Eval("TX").IsUSState());
            ExpectSuccess(Validator.Eval("UT").IsUSState());
        }
        [TestMethod]
        public void StateValidatorFluent_Fail()
        {
            ExpectFailure(Validator.Eval("aK").IsUSState());
            ExpectFailure(Validator.Eval("AKA").IsUSState());
            ExpectFailure(Validator.Eval(" AK").IsUSState());
            ExpectFailure(Validator.Eval(" AK ").IsUSState());
            ExpectFailure(Validator.Eval("AK-").IsUSState());
            ExpectFailure(Validator.Eval("abcdefg").IsUSState());
        }
    }
}
