using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validation.LightVx;

namespace LightVx.Tests
{
    [TestClass]
    public class FluentApiTests
    {
      

        [TestMethod]
        public void RequiredValidatorTest_Ok()
        {
            ExpectSuccess(Validator.Eval("abc").Required());
            ExpectSuccess(Validator.Eval("1").Required());
            ExpectSuccess(Validator.Eval("123").Required());
            ExpectSuccess(Validator.Eval(1).Required());
            ExpectSuccess(Validator.Eval(123).Required());
            ExpectSuccess(Validator.Eval(100M).Required());
            ExpectSuccess(Validator.Eval(100D).Required());
        }
        [TestMethod]
        public void RequiredValidatorTest_Fail()
        {
            ExpectFailure(Validator.Eval("").Required());
            ExpectFailure(Validator.Eval(null).Required());
        }

        private void ExpectSuccess(ValidatorFluent validation)
        {
            bool? success = null;
            validation
                .Success(() => { success = true; })
                .Failed((list, validators) =>
                {
                    success = false;
                });
            if (!success.HasValue)
            {
                Assert.Fail("Received neither success or failure.");
                return;
            }
            if (success == false)
            {
                Assert.Fail("Expected success, but received failure.");
                return;
            }
        }
        private void ExpectFailure(ValidatorFluent validation)
        {
            bool? success = null;
            validation
                .Success(() => { success = true; })
                .Failed((list, validators) =>
                {
                    success = false;
                });
            if (!success.HasValue)
            {
                Assert.Fail("Received neither success or failure.");
            }
            if (success == true)
            {
                Assert.Fail("Expected failure but received success.");
            }
        }
    }
}
