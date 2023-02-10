using System;

namespace LightVx.Validators
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class GuidAttribute : AttributeValidator
    {
        public GuidAttribute() : base(new GuidValidator()) { }
    }

    public class GuidValidator : ValidatorBase
    {
        public override void Validate()
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
