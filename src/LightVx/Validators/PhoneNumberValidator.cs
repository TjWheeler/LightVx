namespace LightVx.Validators
{
    /// <summary>
    ///     Validate phone numbers.  Allows ( ) 0-9 and hyphen
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class PhoneNumberAttribute : AttributeValidator
    {
        public PhoneNumberAttribute() : base(new PhoneNumberValidator()) { }
    }
    /// <summary>
    ///     Validate phone numbers.  Allows ( ) 0-9 and hyphen
    /// </summary>
    public class PhoneNumberValidator : ValidatorBase
    {
        public override void Validate()
        {
            if (_Input == null || (string) _Input == string.Empty)
            {
                Succeed();
                return;
            }

            if (SingleMatch(_Input.ToString()))
                Succeed();
            else
                Fail(
                    "is not a valid phone number, it can only contain left and right brace, spaces, hyphens and digits.");
        }

        #region base implementation

        private const string RegExpression = @"^(\d*\s?|\-?|\)?|\(?)*$";

        protected override string Expression => RegExpression;

        #endregion
    }
}