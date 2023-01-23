namespace LightVx.Validators
{
    /// <summary>
    /// Blacklist approach to detects the use of characters or escaped values used in XSS.
    /// Warning: Ensure other defence factors are in use to protect your system.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class XssSafeTextValidatorAttribute : AttributeValidator
    {
        public XssSafeTextValidatorAttribute() : base(new XssSafeTextValidator()) { }
    }
    /// <summary>
    /// Blacklist approach to detects the use of characters or escaped values used in XSS.
    /// Warning: Ensure other defence factors are in use to protect your system.
    /// </summary>
    public class XssSafeTextValidator : ValidatorBase
    {
        protected override string Expression => @"(?i)((\%3C)|<)|((\%3E)|>)";

        public override void Validate()
        {
            if (_Input == null || (string)_Input == string.Empty)
            {
                Succeed();
                return;
            }
            if (HasMatch((string)_Input))
                Fail("contains invalid characters."); 
            else
                Succeed();
        }
    }
}
