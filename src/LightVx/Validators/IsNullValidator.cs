using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators
{
    public class IsNullValidator : ValidatorBase
    {
        protected override void Validate()
        {
            if (Input == null)
            {
                Succeed();
            }
            else
            {
                Fail("should be null");
            }
        }
    }
}
