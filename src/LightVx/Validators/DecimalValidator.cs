namespace LightVx.Validators
{
    /// <summary>
    ///     Validate decimal numbers.  digits and 1 period.  Also has maximum decimal
    ///     places restriction.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class DecimalValidatorAttribute : AttributeValidator
    {
        public DecimalValidatorAttribute() : base(new DecimalValidator()) { }
    }
    /// <summary>
    ///     Validate decimal numbers.  digits and 1 period.  Also has maximum decimal
    ///     places restriction.
    /// </summary>
    public class DecimalValidator : ValidatorBase
    {

        public override void Validate()
        {
            if (_Input == null || _Input is string && (string)_Input == string.Empty)
            {
                Succeed();
                return;
            }
            if (_Input is decimal)
            {
                Succeed();
            }
            else if (decimal.TryParse(_Input.ToString(), out var _))
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