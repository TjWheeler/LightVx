using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators
{
    public class NotEmptyValidator : ValidatorBase
    {
        protected override void Validate()
        {
            if (Input == null || string.IsNullOrEmpty(Input.ToString()))
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
