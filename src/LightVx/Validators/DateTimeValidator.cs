using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators
{
    /// <summary>
    /// Validates that a string can be parsed as a DateTime.
    /// Note: No specific format is enforced and relies on DateTime.TryParse.
    /// </summary>
    public class DateTimeValidator : ValidatorBase
    {
        public override void Validate()
        {
            if (_Input == null)
            {
                Succeed();
                return;
            }
            if (_Input is DateTime)
            {
                Succeed();
                return;
            }
            var isDate = DateTime.TryParse(_Input.ToString(), out _);
            if (!isDate)
            {
                Fail("is not a valid date.");
                return;
            }
            Succeed();
        }
    }
}
