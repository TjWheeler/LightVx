using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators
{
    /// <summary>
    /// Validates string is empty
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class EmptyValidatorAttribute : AttributeValidator
    {
        public EmptyValidatorAttribute() : base(new EmptyValidator()) { }
    }
    /// <summary>
    /// Validates string is empty
    /// </summary>
    public class EmptyValidator : ValidatorBase
    {
        public override void Validate()
        {
            if (Input != null && Input is Array && ((Array)Input).Length == 0)
            {
                Succeed();
            }
            else if (Input == null || string.IsNullOrEmpty(Input.ToString()))
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
