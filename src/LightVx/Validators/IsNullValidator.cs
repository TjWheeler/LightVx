using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class IsNullAttribute : AttributeValidator
    {
        public IsNullAttribute() : base(new IsNullValidator()) { }
    }
    public class IsNullValidator : ValidatorBase
    {
        public override void Validate()
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
