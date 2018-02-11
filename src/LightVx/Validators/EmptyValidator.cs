using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators
{
    /// <summary>
    /// Validates string is empty
    /// </summary>
    public class EmptyValidator : ValidatorBase
    {
        protected override void Validate()
        {
            if (Input == null || string.IsNullOrEmpty(Input.ToString()))
            {
                Succeed();
            }
            else
            {
                Fail("is not empty");
            }
        }
    }
}
