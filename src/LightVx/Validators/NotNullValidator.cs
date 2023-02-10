using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class NotNullAttribute : AttributeValidator
    {
        public NotNullAttribute() : base(new NotNullValidator()) { }
    }
    public class NotNullValidator : ValidatorBase
    {
        public override void Validate()
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
