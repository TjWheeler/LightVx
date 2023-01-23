using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators
{
    /// <summary>
    /// If string type, validates not null or empty string.
    /// If array type, validates not null and length is greater than 0
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class NotEmptyAttribute : AttributeValidator
    {
        public NotEmptyAttribute() : base(new NotEmptyValidator()) { }
    }
    /// <summary>
    /// If string type, validates not null or empty string.
    /// If array type, validates not null and length is greater than 0
    /// </summary>
    public class NotEmptyValidator : ValidatorBase
    {
        public override void Validate()
        {
            if(Input != null && Input is Array && ((Array) Input).Length == 0)
            {
                Fail("requires a value");
            }
            else if (Input == null || string.IsNullOrEmpty(Input.ToString()))
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
