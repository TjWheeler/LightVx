using System;

namespace LightVx.Validators
{
    public class GuidValidator : ValidatorBase
    {
        protected override void Validate()
        {
            if (_Input == null || _Input is string && (string)_Input == string.Empty)
            {
                Succeed();
                return;
            }

            if (_Input is Guid)
            {
                Succeed();
            }
            else if (Guid.TryParse(_Input.ToString(), out var _))
            {
                Succeed();
            }
            else
            {
                Fail("is not a valid GUID.");
            }
        }
    }
}
