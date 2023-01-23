namespace LightVx.Validators
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class DoubleValidatorAttribute : AttributeValidator
    {
        public DoubleValidatorAttribute() : base(new DoubleValidator()) { }
    }
    public class DoubleValidator : ValidatorBase
    {
        public override void Validate()
        {
            if (_Input == null || _Input is string && (string)_Input == string.Empty)
            {
                Succeed();
                return;
            }

            if (double.TryParse(_Input.ToString(), out var _))
            {
                Succeed();
            }
            else
            {
                Fail("is not a valid Decimal.");
            }
        }
    }
}
