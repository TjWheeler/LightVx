using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators
{
    public class NotNullValidator : ValidatorBase
    {
        protected override void Validate()
        {
            if (Input == null)
            {
                Fail("requires a value");
            }
            else
            {
                Succeed();
            }
        }
    }
}
